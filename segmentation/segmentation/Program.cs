using SegmentCN;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace segmentation
{
    class Program
    {
        static List<string> ListDict = new List<string>();
        static List<string> ListOutput = new List<string>();
        static Segmentation segmention = new Segmentation();
        static SegmentationCN segmentaionCN = new SegmentationCN();
        public static void Main(string[] args)
        {
            ListDict = ReadTextFileToList(System.Environment.CurrentDirectory + "//conf//dict.txt");
            //ListOutput = segmention.doSegBidirectional("我是一个医生",ref ListDict);
            ListOutput = segmentaionCN.doSegBidirectional("我是一个医生", ref ListDict);
            foreach (string e in ListOutput)
            {
                Console.WriteLine("{0}", e.ToString());
            }
            Console.Read();
        }

        //读取文本文件转换为List 
        static List<string> ReadTextFileToList(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            List<string> list = new List<string>();
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            //使用StreamReader类来读取文件 
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            // 从数据流中读取每一行，直到文件的最后一行
            string tmp = sr.ReadLine();
            while (tmp != null)
            {
                list.Add(tmp);
                tmp = sr.ReadLine();
            }
            //关闭此StreamReader对象 
            sr.Close();
            fs.Close();
            return list;
        }
    }
}
