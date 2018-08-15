using System.Data;
using Dapper;
using MsgBoard.Models.Entity;

namespace MsgBoard.Services
{
    public class PasswordRepository : IPasswordRepository
    {
        /// <summary>
        /// 依據UserId取得Password Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Password FindPasswordByUserId(IDbConnection connection, int userId)
        {
            var sqlCmd = GetFindPasswordByUserIdSqlCmd();
            return connection.QueryFirstOrDefault<Password>(sqlCmd, new { userId });
        }

        private string GetFindPasswordByUserIdSqlCmd()
        {
            return "select top 1 * from [dbo].[Password] (nolock) where UserId=@userId order by CreateTime desc";
        }
    }

    public interface IPasswordRepository
    {
        /// <summary>
        /// 依據UserId取得Password Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Password FindPasswordByUserId(IDbConnection connection, int userId);
    }
}