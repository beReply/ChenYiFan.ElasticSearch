using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchUseDemo.Extensions
{
    public static class RandomGeneratorExtension
    {
        /// <summary>
        /// 随机生成对应长度的字符串
        /// </summary>
        /// <param name="randomNumber"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string GeneratorRandom(this RandomNumberGenerator randomNumber, int digits)
        {
            byte[] bytes = new byte[digits];
            randomNumber.GetBytes(bytes);
            var randomString = BitConverter.ToString(bytes);
            var randomSplit = randomString.Split('-');

            return randomSplit.Aggregate("", (current, random) => current + random.Substring(0, 1));
        }

        /// <summary>
        /// 随机生成对应长度的数字
        /// </summary>
        /// <param name="randomNumber"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string GeneratorDigitalRandom(this RandomNumberGenerator randomNumber, int digits)
        {
            byte[] bytes = new byte[digits];
            randomNumber.GetBytes(bytes);
            var randomString = BitConverter.ToString(bytes);
            var randomSplit = randomString.Split('-');

            return randomSplit.Select(random =>
                    Encoding.ASCII.GetBytes(random.Substring(0, 1)))
                .Aggregate("", (current, res) => current + ("" + res.Last()).Last());
        }

        /// <summary>
        /// 随机产生常用汉字
        /// </summary>
        /// <param name="count">要产生汉字的个数</param>
        /// <returns>常用汉字</returns>
        public static string GenerateChineseWord(int count)
        {
            string chineseWords = "";
            System.Random rm = new System.Random();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var gb = Encoding.GetEncoding("gb2312");

            for (int i = 0; i < count; i++)
            {
                // 获取区码(常用汉字的区码范围为16-55)
                int regionCode = rm.Next(16, 56);

                // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)
                var positionCode = rm.Next(1, regionCode == 55 ? 90 : 95);

                // 转换区位码为机内码
                int regionCodeMachine = regionCode + 160;// 160即为十六进制的20H+80H=A0H
                int positionCodeMachine = positionCode + 160;// 160即为十六进制的20H+80H=A0H

                // 转换为汉字
                byte[] bytes = new byte[] { (byte)regionCodeMachine, (byte)positionCodeMachine };
                chineseWords += gb.GetString(bytes);
            }
            return chineseWords;
        }


        public static T RandomArray<T>(T[] ens)
        {
            var random = new Random();
            var enRandom = ens[random.Next(0, ens.Length)];
            return enRandom;
        }
    }
}
