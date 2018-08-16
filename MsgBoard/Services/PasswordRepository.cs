using System.Collections.Generic;
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
        /// <param name="conn">The conn.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Password FindPasswordByUserId(IDbConnection conn, int userId)
        {
            var sqlCmd = GetFindPasswordByUserIdSqlCmd();
            return conn.QueryFirstOrDefault<Password>(sqlCmd, new { userId });
        }

        public IEnumerable<Password> GetUserHistroyPasswords(IDbConnection conn, int userId)
        {
            var sqlCmd = GetHistroyPasswordsSqlCmd();
            return conn.Query<Password>(sqlCmd, new { userId });
        }

        public bool Create(IDbConnection conn, Password entity)
        {
            var sqlCmd = GetCreatePasswordSqlCmd();
            return conn.Execute(sqlCmd, entity) == 1;
        }

        private string GetCreatePasswordSqlCmd()
        {
            return @"
INSERT INTO [dbo].[Password] ([HashPw] ,[UserId])
     VALUES (@HashPw ,@UserId)";
        }

        private string GetHistroyPasswordsSqlCmd()
        {
            return "select * from [dbo].[Password] (nolock) where UserId = @userId";
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
        /// <param name="conn">The conn.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Password FindPasswordByUserId(IDbConnection conn, int userId);

        /// <summary>
        /// 取得歷史密碼資料
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="userId">會員Id</param>
        /// <returns></returns>
        IEnumerable<Password> GetUserHistroyPasswords(IDbConnection conn, int userId);

        /// <summary>
        /// 新增密碼
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="entity">密碼entity</param>
        /// <returns></returns>
        bool Create(IDbConnection conn, Password entity);
    }
}