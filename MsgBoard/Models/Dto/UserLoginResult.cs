namespace MsgBoard.Models.Dto
{
    /// <summary>
    ///  會員登入結果
    /// </summary>
    public class UserLoginResult
    {
        /// <summary>
        /// 是否通過登入驗證
        /// </summary>
        public bool Auth { get; set; } = false;

        /// <summary>
        /// 是否為管理者
        /// </summary>
        public bool IsAdmin { get; set; } = false;
    }
}