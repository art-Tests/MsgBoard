namespace MsgBoard.DataModel.Dto
{
    /// <summary>
    /// 作者
    /// </summary>
    public class Author
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 會員頭像路徑
        /// </summary>
        public string Pic { get; set; }
    }
}