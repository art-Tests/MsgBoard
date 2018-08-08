using System;
using System.Security.Cryptography;
using System.Text;
using HashUtility.Interface;

namespace HashUtility.Services
{
    /// <summary>
    /// SHA512雜湊
    /// </summary>
    public class Sha512HashTool : IHash
    {
        /// <summary>
        /// 取得雜湊結果
        /// </summary>
        /// <param name="inputValue">要雜湊的字串</param>
        /// <returns>雜湊完畢的字串</returns>
        public string GetHash(string inputValue)
        {
            var data = Encoding.UTF8.GetBytes(inputValue);
            using (SHA512 shaM = new SHA512Managed())
            {
                var hash = shaM.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }
    }
}