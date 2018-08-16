namespace MsgBoard.DataModel.Dto
{
    /// <summary>
    /// 使用者文章數量
    /// </summary>
    public class UserArticleCount
    {
        /// <summary>
        /// 發文數量
        /// </summary>
        public int PostCount { get; set; }

        /// <summary>
        /// 回覆數量
        /// </summary>
        public int ReplyCount { get; set; }
    }
}