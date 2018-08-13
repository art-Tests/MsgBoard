using System;
using System.Diagnostics;
using System.Text;

namespace HashUtility.Services
{
    /// <summary>
    /// Hash工具
    /// </summary>
    public class HashTool
    {
        public HashTool()
        {
            _hashType = new Sha512HashTool();
        }

        /// <summary>
        /// 金鑰
        /// </summary>
        private string _hashKey = "@ehsn";

        private readonly Sha512HashTool _hashType;

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
            return _hashType.GetHash(hashStr);
        }
    }
}