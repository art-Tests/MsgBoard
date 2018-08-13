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
        /// <param name="data">要雜湊的資料</param>
        /// <returns>雜湊完畢的字串</returns>
        public byte[] GetHash(byte[] data)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                return shaM.ComputeHash(data);
            }
        }
    }
}