using System.Data;
using Dapper;
using MsgBoard.Models.Entity;

namespace MsgBoard.Services
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// 依據會員Email取得User Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        public User FindUserByMail(IDbConnection connection, string email)
        {
            var sqlCmd = "select top 1 * from [dbo].[User] (nolock) where Mail=@email";
            return connection.QueryFirstOrDefault<User>(sqlCmd, new { email });
        }
    }

    public interface IUserRepository
    {
        /// <summary>
        /// 依據會員Email取得User Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        User FindUserByMail(IDbConnection connection, string email);
    }
}