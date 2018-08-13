namespace HashUtility.Interface
{
    /// <summary>
    /// Hash介面
    /// </summary>
    public interface IHash
    {
        /// <summary>
        /// 取得Hash值
        /// </summary>
        /// <param name="data">要雜湊的資料</param>
        /// <returns>雜湊完畢的字串</returns>
        string GetHash(string data);
    }
}