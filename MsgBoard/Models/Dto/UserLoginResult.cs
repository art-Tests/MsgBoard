using MsgBoard.Models.Entity;

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
        /// 會員資料
        /// </summary>
        public User User { get; set; } = null;
    }
}