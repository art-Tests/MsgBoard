using System.Data;
using MsgBoard.Models.Dto;

namespace MsgBoard.Models.Interface
{
    public interface IMemberService
    {
        /// <summary>
        /// 登入帳密檢查
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="email">會員帳號(email)</param>
        /// <param name="userPass">會員密碼</param>
        /// <returns>返回會員登入結果</returns>
        UserLoginResult CheckUserPassword(IDbConnection connection, string email, string userPass);
    }
}