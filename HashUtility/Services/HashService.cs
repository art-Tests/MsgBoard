using System;
using System.Diagnostics;
using HashUtility.Factory;

namespace HashUtility.Services
{
    /// <summary>
    /// Hash工具
    /// </summary>
    public class HashService
    {
        /// <summary>
        /// 金鑰
        /// </summary>
        private string _hashKey = "i_am_security_key";

        private string[] _algList;

        public HashService()
        {
            var configAlgList = Properties.Settings.Default.algList;
            _algList = configAlgList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 測試用：設定Hash演算法
        /// </summary>
        /// <param name="algName">指定演算法，例如:SHA512,SHA256</param>
        [Conditional("DEBUG")]
        public void SetAlgList(string algName)
        {
            _algList = algName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 測試用：將Hash Key手動變更
        /// </summary>
        /// <param name="testHashKey">要測試的HashKey</param>
        [Conditional("DEBUG")]
        public void SetHashKey(string testHashKey)
        {
            _hashKey = testHashKey;
        }

        /// <summary>
        /// 取得會員密碼雜湊值
        /// </summary>
        /// <param name="guid">會員的GUID</param>
        /// <param name="pass">要雜湊的密碼</param>
        /// <returns>該會員的密碼雜湊值</returns>
        public string GetMemberHashPw(string guid, string pass)
        {
            var hashStr = $"{pass}{guid}{_hashKey}";

            var result = hashStr;
            foreach (var alg in _algList)
            {
                var tmp = HashFactory.GetInstance(alg);
                if (tmp != null)
                {
                    result = tmp.GetHash(result);
                }
            }

            return result;
        }
    }
}