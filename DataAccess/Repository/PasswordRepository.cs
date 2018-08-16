using System.Collections.Generic;
using System.Data;
using Dapper;
using DataAccess.Repository.Interface;
using DataModel.Entity;

namespace DataAccess.Repository
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
}