using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using HashUtility.Services;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Services.Common;
using MsgBoard.Services.Interface;
using MsgBoard.Services.Repository;
using MsgBoard.ViewModel.Admin;
using MsgBoard.ViewModel.Member;

namespace MsgBoard.Services
{
    public class MemberService : BaseService, IMemberService
    {
        protected HashService HashService = new HashService();
        private IUserRepository _userRepo = new UserRepository();
        private IPasswordRepository _passwordRepo = new PasswordRepository();

        private readonly IPostRepository _postRepo = new PostRepository();
        private readonly IReplyRepository _replyRepo = new ReplyRepository();

        [Conditional("DEBUG")]
        public void SetHashTool(HashService hashService)
        {
            HashService = hashService;
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

        /// <summary>
        /// 新增密碼
        /// </summary>
        /// <param name="entity">密碼entity</param>
        /// <returns></returns>
        public bool CreatePassword(Password entity) => _passwordRepo.Create(Conn, entity);

        /// <summary>
        /// 取得會員Entity
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        internal User GetUser(int id) => _userRepo.GetUserById(Conn, id);

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

        /// <inheritdoc />
        /// <summary>
        /// 登入帳密檢查
        /// </summary>
        /// <param name="email">會員帳號(email)</param>
        /// <param name="userPass">會員密碼</param>
        /// <returns>返回會員登入結果</returns>
        public virtual UserLoginResult CheckUserPassword(string email, string userPass)
        {
            var result = new UserLoginResult();

            var user = _userRepo.GetUserByMail(Conn, email);
            if (user == null) return result;

            var password = _passwordRepo.FindPasswordByUserId(Conn, user.Id);
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
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        public bool CheckUserExist(string email) => _userRepo.CheckUserExist(Conn, email);

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
        /// <param name="user">會員資料entity</param>
        public void UpdateUser(User user) => _userRepo.Update(Conn, user);

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <returns></returns>
        public IQueryable<AdminIndexViewModel> GetUserCollection() => _userRepo.GetUserCollection(Conn);

        /// <summary>
        /// 取得會員文章、回復數量 (未刪除)
        /// </summary>
        /// <param name="id">會員Id</param>
        /// <returns></returns>
        public UserArticleCount GetUserArticleCount(int id)
        {
            return new UserArticleCount()
            {
                PostCount = _postRepo.GetPostCountByUserId(Conn, id),
                ReplyCount = _replyRepo.GetReplyCountByUserId(Conn, id)
            };
        }

        /// <summary>
        /// 新增網站會員
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="path">存放大頭照的實體路徑</param>
        public void CreateUser(MemberCreateViewModel model, string path)
        {
            using (var tranScope = new TransactionScope())
            {
                using (var connection = ConnFactory.GetConnection())
                {
                    // Table User
                    var fileName = SaveMemberPic(model.File, path);
                    var user = ConvertToUserEntity(model, $"{FileUploadPath}/{fileName}");
                    user.Id = _userRepo.Create(connection, user);

                    // Table Password
                    var password = ConvertToPassEntity(user.Id, user.Guid, model.Password);
                    _passwordRepo.Create(Conn, password);

                    // 註冊完直接給他登入-因為是新會員，所以文章count直接給預設0即可
                    SignInUser.UserLogin(true, user, new UserArticleCount());
                }

                tranScope.Complete();
            }
        }

        /// <summary>
        /// 檢查歷史密碼中是否存在相同的密碼
        /// </summary>
        /// <param name="userId">會員Id</param>
        /// <param name="newHashPass">要檢查的hash密碼</param>
        /// <returns>True表示歷史紀錄有相同密碼</returns>
        public bool CheckIsHistroyPassword(int userId, string newHashPass)
        {
            var histroyPasswords = _passwordRepo.GetUserHistroyPasswords(Conn, userId);
            return histroyPasswords.Any(x => x.HashPw == newHashPass);
        }
    }
}