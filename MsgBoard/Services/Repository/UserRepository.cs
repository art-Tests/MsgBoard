using System.Data;
using System.Linq;
using Dapper;
using MsgBoard.Models.Entity;
using MsgBoard.Models.ViewModel.Admin;
using MsgBoard.Services.Interface;

namespace MsgBoard.Services.Repository
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

        /// <summary>
        /// 依據會員Id取得User Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="id">會員Id</param>
        /// <returns></returns>
        public User GetUserById(IDbConnection connection, int id)
        {
            var sqlCmd = "select top 1 * from [dbo].[User] (nolock) where Id=@Id";
            return connection.QueryFirstOrDefault<User>(sqlCmd, new { id });
        }

        /// <summary>
        /// 會員資料修改
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="user">會員資料entity</param>
        public void Update(IDbConnection connection, User user)
        {
            var sqlCmd = GetUpdateSqlCmd();
            connection.Execute(sqlCmd, user);
        }

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        public IQueryable<AdminIndexViewModel> GetUserCollection(IDbConnection conn)
        {
            var sqlCmd = GetUserCollectionAllSqlCmd();
            return conn.Query<AdminIndexViewModel>(sqlCmd).AsQueryable();
        }

        /// <summary>
        /// 檢查系統是否已經存在相同使用者帳號
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        public bool CheckUserExist(IDbConnection conn, string email)
        {
            var sqlCmd = GetCheckUserExistSqlCmd();
            return conn.QueryFirstOrDefault<bool>(sqlCmd, new { email });
        }

        public int Create(IDbConnection conn, User entity)
        {
            var sqlCmd = GetCreateUserSqlCmd();
            return conn.QueryFirstOrDefault<int>(sqlCmd, entity);
        }

        /// <summary>
        /// 新增User的SQL
        /// </summary>
        /// <returns>Sql語句</returns>
        private string GetCreateUserSqlCmd()
        {
            return @"
INSERT INTO [dbo].[User] ([Name], [Mail], [Pic], [Guid], [IsDel])
     VALUES (@Name, @Mail, @Pic, @Guid, 0)

SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]
";
        }

        private string GetCheckUserExistSqlCmd()
        {
            return @"
if exists(select top 1 * from [dbo].[User] (nolock) where Mail = @email)
select 'true'
else
select 'false'
";
        }

        private string GetUserCollectionAllSqlCmd()
        {
            return @"
	select ROW_NUMBER() OVER(ORDER BY Id) AS RowId, Id, Pic, Name, Mail, IsAdmin, IsDel
    from [dbo].[user] (nolock)";
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
}