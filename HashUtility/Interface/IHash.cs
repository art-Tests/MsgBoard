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
        /// <param name="inputValue">要雜湊的字串</param>
        /// <returns>雜湊完畢的字串</returns>
        string GetHash(string inputValue);
    }
}