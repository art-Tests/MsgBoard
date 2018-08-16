using MsgBoard.DataModel.Dto;

namespace Services.Interface
{
    public interface IMemberService
    {
        /// <summary>
        /// 登入帳密檢查
        /// </summary>
        /// <param name="email">會員帳號(email)</param>
        /// <param name="userPass">會員密碼</param>
        /// <returns>返回會員登入結果</returns>
        UserLoginResult CheckUserPassword(string email, string userPass);
    }
}