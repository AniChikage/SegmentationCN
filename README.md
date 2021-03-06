## SegmentationCN，分词

## 简介
程序用于中文语句的分词，实现基于最大匹配算法的前向、后向和双向分词技术，并提供了接口，具体下面介绍

## 环境
>. 软件要求：Visual Studio 2013及以上版本
>. 语言：C#

## 接口说明
`List<string> doSegSingalModel(string inputStr, ref List<string> dictList, int segModel, int maxLength)`
* inputStr：要进行分词的字符串
* dictList：词典
* segModel：0为从左到右分词，1为从右到左分词，默认：正向
* maxLength：每个分词的最大长度，默认：7

`List<string> doSegBidirectional(string inputStr, ref List<string> dictList)`
* inputStr：要进行分词的字符串
* dictList：词典

## 使用说明
仓库包括2个工程文件和1个.dll文件，其中SegmentCN是用于生成.dll文件的程序，segmentation是测试程序

>1. 在使用前，将segmentation应用程序目录下的conf文件夹拷贝至自己的程序下面，conf中包含字典文件，可以根据自己的需要进行配置
>2. 将SegmentCN.dll导入自己的工程中
>3. 进行调用，定义`SegmentationCN segmentationCN = new SegmentationCN();`，然后对应接口进行调用即可，具体使用可以参照示例程序
