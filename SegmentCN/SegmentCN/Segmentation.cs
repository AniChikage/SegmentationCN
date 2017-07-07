using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegmentCN
{
    /// <summary>
    /// 分词算法
    /// </summary>
    public class SegmentationCN
    {
        #region 单模式分词
        /// <summary>
        /// 用最大匹配算法进行分词，正反向均可。
        /// 为了节约内存，词典参数是引用传递
        /// </summary>
        /// <param name="inputStr">要进行分词的字符串</param>
        /// <param name="dictList">词典</param>
        /// <param name="segModel">0:为从左到右分词，1:为从右到左分词</param>
        /// <param name="maxLength">每个分词的最大长度</param>
        /// <returns>储存了分词结果的字符串数组</returns>
        public List<string> doSegSingalModel(string inputStr, ref List<string> dictList, int segModel, int maxLength)
        {
            if (dictList == null)
                return null;
            if (string.IsNullOrEmpty(inputStr))
                return null;
            if (!(maxLength > 0))
                return null;
            List<string> segWords = new List<string>();
            List<string> segWordsReverse = new List<string>();
            string word = "";
            int wordLength = maxLength;
            int position = 0;
            int segLength = 0;
            while (segLength < inputStr.Length)
            {
                if ((inputStr.Length - segLength) < maxLength)
                    wordLength = inputStr.Length - segLength;
                else
                    wordLength = maxLength;
                if (segModel == 0)
                    position = segLength;
                else
                    position = inputStr.Length - segLength - wordLength;
                word = inputStr.Substring(position, wordLength);
                while (!dictList.Contains(word))
                {
                    if (word.Length == 1)
                        break;
                    if (segModel == 0)
                        word = word.Substring(0, word.Length - 1);
                    else
                        word = word.Substring(1);
                }
                if (segModel == 0)
                    segWords.Add(word);
                else
                    segWordsReverse.Add(word);
                segLength += word.Length;

            }

            //逆向分词
            if (segModel == 1)
            {
                for (int i = 0; i < segWordsReverse.Count; i++)
                {
                    segWords.Add(segWordsReverse[segWordsReverse.Count - 1 - i]);
                }
            }
            return segWords;

        }
        #endregion

        #region  单模式：默认词典最大关键词长度为7
        public List<string> doSegSingalModel(string inputStr, ref List<string> dictList, int segModel)
        {
            return doSegSingalModel(inputStr, ref dictList, segModel, 7);
        }
        #endregion

        #region  单模式：默认正向,默认词典最大关键词长度为7
        public List<string> doSegSingalModel(string inputStr, ref List<string> dictList)
        {
            return doSegSingalModel(inputStr, ref dictList, 0, 7);
        }
        #endregion

        #region  单模式：逆向
        public List<string> SegMMRightToLeft(string inputStr, ref List<string> dictList)
        {
            return doSegSingalModel(inputStr, ref dictList, 1, 7);
        }
        #endregion

        #region 比较两个list是否相等
        private bool CompStringList(ref List<string> strList1, ref List<string> strList2)
        {
            if (strList1 == null || strList2 == null)
                return false;

            if (strList1.Count != strList2.Count)
                return false;
            else
            {
                for (int i = 0; i < strList1.Count; i++)
                {
                    if (strList1[i] != strList2[i])
                        return false;
                }
            }
            return true;
        }
        #endregion

        #region 双向最大匹配
        public List<string> doSegBidirectional(string inputStr, ref List<string> dictList)
        {
            List<string> segWordssegModel = new List<string>();
            List<string> segWordsRightToLeft = new List<string>();
            List<string> segWordsFinal = new List<string>();
            List<string> wordsFromLeft = new List<string>();
            List<string> wordsFromRight = new List<string>();
            List<string> wordsAtMiddle = new List<string>();
            segWordssegModel = doSegSingalModel(inputStr, ref dictList);

            //逆向
            segWordsRightToLeft = SegMMRightToLeft(inputStr, ref dictList);

            //判断两头的分词拼接，是否已经在输入字符串的中间交汇，只要没有交汇，就不停循环
            while ((segWordssegModel[0].Length + segWordsRightToLeft[segWordsRightToLeft.Count - 1].Length) < inputStr.Length)
            {
                if (CompStringList(ref segWordssegModel, ref segWordsRightToLeft))
                {
                    wordsAtMiddle = segWordssegModel.ToList<string>();
                    break;
                }
                if (segWordssegModel.Count < segWordsRightToLeft.Count)
                {
                    wordsAtMiddle = segWordssegModel.ToList<string>();
                    break;
                }
                else if (segWordssegModel.Count > segWordsRightToLeft.Count)
                {
                    wordsAtMiddle = segWordsRightToLeft.ToList<string>();
                    break;
                }

                {
                    int singleCharsegModel = 0;
                    for (int i = 0; i < segWordssegModel.Count; i++)
                    {
                        if (segWordssegModel[i].Length == 1)
                            singleCharsegModel++;
                    }

                    int singleCharRightToLeft = 0;
                    for (int j = 0; j < segWordsRightToLeft.Count; j++)
                    {
                        if (segWordsRightToLeft[j].Length == 1)
                            singleCharRightToLeft++;
                    }

                    if (singleCharsegModel < singleCharRightToLeft)
                    {
                        wordsAtMiddle = segWordssegModel.ToList<string>();
                        break;
                    }
                    else if (singleCharsegModel > singleCharRightToLeft)
                    {
                        wordsAtMiddle = segWordsRightToLeft.ToList<string>();
                        break;
                    }
                }

                wordsFromLeft.Add(segWordssegModel[0]);
                wordsFromRight.Add(segWordsRightToLeft[segWordsRightToLeft.Count - 1]);

                inputStr = inputStr.Substring(segWordssegModel[0].Length);
                inputStr = inputStr.Substring(0, inputStr.Length - segWordsRightToLeft[segWordsRightToLeft.Count - 1].Length);

                segWordssegModel.Clear();
                segWordsRightToLeft.Clear();

                segWordssegModel = doSegSingalModel(inputStr, ref dictList);

                //逆向
                segWordsRightToLeft = SegMMRightToLeft(inputStr, ref dictList);

            }

            if ((segWordssegModel[0].Length + segWordsRightToLeft[segWordsRightToLeft.Count - 1].Length) > inputStr.Length)
            {
                wordsAtMiddle = segWordssegModel.ToList<string>();
            }
            else if ((segWordssegModel[0].Length + segWordsRightToLeft[segWordsRightToLeft.Count - 1].Length) == inputStr.Length)
            {
                wordsAtMiddle.Add(segWordssegModel[0]);
                wordsAtMiddle.Add(segWordsRightToLeft[segWordsRightToLeft.Count - 1]);
            }

            foreach (string wordLeft in wordsFromLeft)
            {
                segWordsFinal.Add(wordLeft);
            }
            foreach (string wordMiddle in wordsAtMiddle)
            {
                segWordsFinal.Add(wordMiddle);
            }
            for (int i = 0; i < wordsFromRight.Count; i++)
            {
                segWordsFinal.Add(wordsFromRight[wordsFromRight.Count - 1 - i]);
            }

            return segWordsFinal;

        }
        #endregion
    }
}