﻿using System.Text;
using HashUtility.Interface;

namespace HashUtility.Services
{
    public abstract class BaseHashTool : IHash
    {
        public string HashType { get; set; }
        public HashAlgorithmFactory AlgorithmFactory { get; set; }

        protected BaseHashTool()
        {
            AlgorithmFactory = new HashAlgorithmFactory();
        }

        protected static string ConvertToText(byte[] hashedInputBytes)
        {
            // Convert to text
            // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte
            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }

        /// <summary>
        /// 取得雜湊結果
        /// </summary>
        /// <param name="data">要雜湊的資料</param>
        /// <returns>雜湊完畢的字串</returns>
        public string GetHash(string data)
        {
            using (var shaM = AlgorithmFactory.GetInstance(HashType))
            {
                var byteData = Encoding.UTF8.GetBytes(data);
                return ConvertToText(shaM.ComputeHash(byteData));
            }
        }
    }
}