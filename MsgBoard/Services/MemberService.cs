using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using Dapper;
using HashUtility.Interface;
using HashUtility.Services;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Models.Interface;
using MsgBoard.ViewModel.Admin;
using MsgBoard.ViewModel.Member;

namespace MsgBoard.Services
{
    public class MemberService : IMemberService
    {
        protected HashService HashService;
        private IUserRepository _userRepo;
        private IPasswordRepository _passwordRepo;

        public MemberService()
        {
            HashService = new HashService();
            _userRepo = new UserRepository();
            _passwordRepo = new PasswordRepository();
        }

        public void SetHashTool(HashService hashService)
        {
            HashService = hashService;
        }

        /// <summary>
        /// 新增會員資料
        /// </summary>
        /// <param name="conn">Connection</param>
        /// <param name="entity">會員Entity</param>
        /// <returns>
        /// 回傳該筆會員的Id
        /// </returns>
        public int CreateUser(IDbConnection conn, User entity)
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

        /// <summary>
        /// 儲存會員大頭照
        /// </summary>
        /// <param name="file">上傳之圖片檔案</param>
        /// <param name="path">圖片上傳實體路徑</param>
        /// <returns>大頭照實際儲存完整路徑</returns>
        internal string SaveMemberPic(HttpPostedFileBase file, string path)
        {
            if (file == null) return string.Empty;
            if (file.ContentLength <= 0) return string.Empty;
            var savePath = GetSavePath(path);
            file.SaveAs(savePath.Item1);
            return savePath.Item2;
        }

        /// <summary>
        /// 產生圖片存檔路徑
        /// </summary>
        /// <param name="mapPath">上傳目錄實體路徑</param>
        /// <returns>
        /// Tuple Item1:實體路徑包含檔名
        /// Tuple Item2:檔名
        /// </returns>
        private static Tuple<string, string> GetSavePath(string mapPath)
        {
            var fileName = $"{Guid.NewGuid():N}.jpg";
            var savePath = Path.Combine(mapPath, fileName);
            while (File.Exists(savePath))
            {
                fileName = $"{Guid.NewGuid():N}.jpg";
                savePath = Path.Combine(mapPath, fileName);
            }
            return new Tuple<string, string>(savePath, fileName);
        }

        /// <summary>
        /// 從ViewModel轉為User Entity
        /// </summary>
        /// <param name="model">新增會員ViewModel</param>
        /// <param name="picPath">The pic path.</param>
        /// <returns>User Entity</returns>
        public User ConvertToUserEntity(MemberCreateViewModel model, string picPath)
        {
            return new User
            {
                Guid = Guid.NewGuid().ToString(),
                IsAdmin = false,
                Mail = model.Mail,
                Name = model.Name,
                Pic = picPath,
                IsDel = false
            };
        }

        public bool CreatePassword(IDbConnection conn, Password entity)
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

        /// <summary>
        /// 取得會員Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        internal User GetUser(IDbConnection connection, int id)
        {
            return _userRepo.GetUserById(connection, id);
        }

        /// <summary>
        /// 取得會員密碼Entity
        /// </summary>
        /// <param name="userId">會員Id</param>
        /// <param name="guid">會員Guid</param>
        /// <param name="userPass">會員密碼</param>
        /// <returns></returns>
        public Password ConvertToPassEntity(int userId, string guid, string userPass)
        {
            return new Password
            {
                UserId = userId,
                HashPw = HashService.GetMemberHashPw(guid, userPass)
            };
        }

        /// <summary>
        /// 登入帳密檢查
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="email">會員帳號(email)</param>
        /// <param name="userPass">會員密碼</param>
        /// <returns>返回會員登入結果</returns>
        public virtual UserLoginResult CheckUserPassword(IDbConnection connection, string email, string userPass)
        {
            var result = new UserLoginResult();

            var user = _userRepo.GetUserByMail(connection, email);
            if (user == null) return result;

            var password = _passwordRepo.FindPasswordByUserId(connection, user.Id);
            if (password == null) return result;

            var hashPassword = HashService.GetMemberHashPw(user.Guid, userPass);
            result.Auth = password.HashPw == hashPassword && user.IsDel.Equals(false);
            if (result.Auth)
            {
                result.User = user;
            }
            return result;
        }

        /// <summary>
        /// 檢查系統是否已經存在相同使用者帳號
        /// </summary>
        /// <param name="connection">The Connection</param>
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckUserExist(IDbConnection connection, string email)
        {
            var sqlCmd = GetCheckUserExistSqlCmd();
            return connection.QueryFirstOrDefault<bool>(sqlCmd, new { email });
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

        /// <summary>
        /// 測試注入用的方法，用來設定UserRepository
        /// </summary>
        /// <param name="userRepo">The user repo.</param>
        [Conditional("DEBUG")]
        public void SetUserRepository(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// 測試注入用的方法，用來設定PasswordRepository
        /// </summary>
        /// <param name="passRepo">The pass repo.</param>
        [Conditional("DEBUG")]
        public void SetPasswordRepository(IPasswordRepository passRepo)
        {
            _passwordRepo = passRepo;
        }

        /// <summary>
        /// 會員資料修改
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="user">會員資料entity</param>
        public void UpdateUser(IDbConnection connection, User user)
        {
            _userRepo.Update(connection, user);
        }

        /// <summary>
        /// 取得歷史密碼資料
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="userId">會員Id</param>
        /// <returns></returns>
        public IEnumerable<Password> GetHistroyPasswords(IDbConnection connection, int userId)
        {
            var sqlCmd = GetHistroyPasswordsSqlCmd();
            return connection.Query<Password>(sqlCmd, new { userId });
        }

        private string GetHistroyPasswordsSqlCmd()
        {
            return "select * from [dbo].[Password] (nolock) where UserId = @userId";
        }

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public IQueryable<AdminIndexViewModel> GetUserCollection(IDbConnection connection)
        {
            var sqlCmd = GetUserCollectionAllSqlCmd();
            return connection.Query<AdminIndexViewModel>(sqlCmd).AsQueryable();
        }

        //public IEnumerable<AdminIndexViewModel> GetUserCollection(IDbConnection connection, int page, int pageSize)
        //{
        //    var sqlCmd = GetUserCollectionSqlCmd();
        //    var start = (page - 1) * pageSize + 1;
        //    var end = page * pageSize;
        //    return connection.Query<AdminIndexViewModel>(sqlCmd, new { start, end });
        //}

        private string GetUserCollectionAllSqlCmd()
        {
            return @"
	select ROW_NUMBER() OVER(ORDER BY Id) AS RowId, Id, Pic, Name, Mail, IsAdmin, IsDel
    from [dbo].[user] (nolock)";
        }

        //        private string GetUserCollectionSqlCmd()
        //        {
        //            return @"
        //select * from (
        //	select ROW_NUMBER() OVER(ORDER BY Id) AS RowId, Id, Pic, Name, Mail, IsAdmin, IsDel
        //    from [dbo].[user] (nolock)
        //) r
        //where RowId between @start and @end
        //";
        //        }

        /// <summary>
        /// 取得會員文章、回復數量 (未刪除)
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">會員Id</param>
        /// <returns></returns>
        public UserArticleCount GetUserArticleCount(IDbConnection conn, int id)
        {
            var sqlCmd = GetUserArticleCountSqlCmd();
            return conn.QueryFirstOrDefault<UserArticleCount>(sqlCmd, new { id });
        }

        private string GetUserArticleCountSqlCmd()
        {
            return @"
--declare @id int
--set @id=18
select
(select count(*) from [dbo].[Post] (nolock) where CreateUserId = @Id and IsDel=0) as [PostCount],
(select count(*) from [dbo].[Reply] (nolock) where CreateUserId = @Id and IsDel=0) as [ReplyCount]
";
        }
    }
}