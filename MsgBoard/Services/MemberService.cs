using System;
using System.Data;
using System.IO;
using Dapper;
using HashUtility.Services;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.ViewModel.Member;

namespace MsgBoard.Services
{
    internal class MemberService
    {
        private readonly HashTool _hashTool = new HashTool();

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
INSERT INTO [dbo].[User] ([Name], [Mail], [Pic], [Guid])
     VALUES (@Name, @Mail, @Pic, @Guid)

SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]
";
        }

        /// <summary>
        /// 儲存會員大頭照
        /// </summary>
        /// <param name="model">新增會員ViewModel</param>
        /// <param name="path">圖片上傳實體路徑</param>
        /// <returns>大頭照實際儲存完整路徑</returns>
        public string SaveMemberPic(MemberCreateViewModel model, string path)
        {
            if (model.File.ContentLength <= 0) return string.Empty;
            var savePath = GetSavePath(path);
            model.File.SaveAs(savePath.Item1);
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
                Pic = picPath
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
                HashPw = _hashTool.GetMemberHashPw(guid, userPass)
            };
        }

        /// <summary>
        /// 登入帳密檢查
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="email">會員帳號(email)</param>
        /// <param name="userPass">會員密碼</param>
        /// <returns>返回會員登入結果</returns>
        public UserLoginResult CheckUserPassword(IDbConnection connection, string email, string userPass)
        {
            var result = new UserLoginResult();

            var user = FindUserByMail(connection, email);
            if (user == null) return result;

            var password = FindPasswordByUserId(connection, user.Id);
            if (password == null) return result;

            var hashPassword = _hashTool.GetMemberHashPw(user.Guid, userPass);
            result.Auth = password.HashPw == hashPassword;
            result.User = user;
            return result;
        }

        /// <summary>
        /// 依據UserId取得Password Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        private Password FindPasswordByUserId(IDbConnection connection, int userId)
        {
            var sqlCmd = GetFindPasswordByUserIdSqlCmd();
            return connection.QueryFirstOrDefault<Password>(sqlCmd, new { userId });
        }

        private string GetFindPasswordByUserIdSqlCmd()
        {
            return "select top 1 * from [dbo].[Password] (nolock) where UserId=@userId order by CreateTime desc";
        }

        /// <summary>
        /// 依據會員Email取得User Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        private User FindUserByMail(IDbConnection connection, string email)
        {
            var sqlCmd = GetFindUserSqlCmd();
            return connection.QueryFirstOrDefault<User>(sqlCmd, new { email });
        }

        private string GetFindUserSqlCmd()
        {
            return @"select top 1 * from [dbo].[User] (nolock) where Mail=@email";
        }
    }
}