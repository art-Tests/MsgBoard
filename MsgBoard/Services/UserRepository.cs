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
        public User GetUserByMail(IDbConnection connection, string email)
        {
            var sqlCmd = "select top 1 * from [dbo].[User] (nolock) where Mail=@email";
            return connection.QueryFirstOrDefault<User>(sqlCmd, new { email });
        }

        public User GetUserById(IDbConnection connection, int id)
        {
            var sqlCmd = "select top 1 * from [dbo].[User] (nolock) where Id=@Id";
            return connection.QueryFirstOrDefault<User>(sqlCmd, new { id });
        }

        public void Update(IDbConnection connection, User user)
        {
            var sqlCmd = GetUpdateSqlCmd();
            connection.Execute(sqlCmd, user);
        }

        private string GetUpdateSqlCmd()
        {
            return @"
UPDATE [dbo].[User]
   SET [Name] = @Name
      ,[Pic] = @Pic
      ,[IsDel] = @IsDel
 WHERE Id = @Id
";
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
        User GetUserByMail(IDbConnection connection, string email);

        /// <summary>
        /// 依據會員Id取得User Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="id">會員Id</param>
        /// <returns></returns>
        User GetUserById(IDbConnection connection, int id);

        /// <summary>
        /// 會員資料修改
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="user">會員資料entity</param>
        void Update(IDbConnection connection, User user);
    }
}