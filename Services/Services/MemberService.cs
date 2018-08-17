using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using DataAccess.Interface;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using DataAccess.Services;
using DataModel.Entity;
using HashUtility.Services;
using MsgBoard.DataModel.Dto;
using MsgBoard.DataModel.ViewModel.Member;
using MsgBoard.Services.Interface;

namespace MsgBoard.BL.Services
{
    public class MemberService : IMemberService
    {
        protected string FileUploadPath = ConfigurationManager.AppSettings["uploadPath"];

        protected HashService HashService = new HashService();
        private IUserRepository _userRepo = new UserRepository();
        private IPasswordRepository _passwordRepo = new PasswordRepository();

        private readonly IPostRepository _postRepo = new PostRepository();
        private readonly IReplyRepository _replyRepo = new ReplyRepository();
        private readonly IConnectionFactory _connFactory;
        private readonly IDbConnection _conn;

        public MemberService()
        {
            _conn = new ConnectionFactory().GetConnection();
        }

        public MemberService(IConnectionFactory factory)
        {
            _connFactory = factory;
            _conn = _connFactory.GetConnection();
        }

        /// <summary>
        /// 設定HashTool，單元測試用
        /// </summary>
        /// <param name="hashService">The hash service.</param>
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
        public string SaveMemberPic(HttpPostedFileBase file, string path)
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
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public bool CreatePassword(PasswordViewModel model)
        {
            var entity = ConvertToPasswordEntity(model);
            return _passwordRepo.Create(_conn, entity);
        }

        private Password ConvertToPasswordEntity(PasswordViewModel model)
        {
            if (model == null) return null;
            return new Password
            {
                Id = model.Id,
                UserId = model.UserId,
                CreateTime = model.CreateTime,
                HashPw = model.HashPw
            };
        }

        /// <summary>
        /// 取得會員Entity
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public UserViewModel GetUser(int id)
        {
            var entity = _userRepo.GetUserById(_conn, id);
            return ConvertToUserViewModel(entity);
        }

        /// <summary>
        /// 取得會員密碼Entity
        /// </summary>
        /// <param name="userId">會員Id</param>
        /// <param name="guid">會員Guid</param>
        /// <param name="userPass">會員密碼</param>
        /// <returns></returns>
        public PasswordViewModel ConvertToPassEntity(int userId, string guid, string userPass)
        {
            return new PasswordViewModel
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

            var user = _userRepo.GetUserByMail(_conn, email);
            if (user == null) return result;

            var password = _passwordRepo.FindPasswordByUserId(_conn, user.Id);
            if (password == null) return result;

            var hashPassword = HashService.GetMemberHashPw(user.Guid, userPass);
            result.Auth = password.HashPw == hashPassword && user.IsDel.Equals(false);
            if (result.Auth)
            {
                result.User = ConvertToUserViewModel(user);
            }
            return result;
        }

        /// <summary>
        /// 檢查系統是否已經存在相同使用者帳號
        /// </summary>
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        public bool CheckUserExist(string email) => _userRepo.CheckUserExist(_conn, email);

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
        public void UpdateUser(UserViewModel user)
        {
            var entity = ConvertVmToUserEntity(user);
            _userRepo.Update(_conn, entity);
        }

        private User ConvertVmToUserEntity(UserViewModel user)
        {
            if (user == null) return null;
            return new User
            {
                Id = user.Id,
                Guid = user.Guid,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                IsDel = user.IsDel,
                Mail = user.Mail,
                Pic = user.Pic
            };
        }

        /// <summary>
        /// 取得會員文章、回復數量 (未刪除)
        /// </summary>
        /// <param name="id">會員Id</param>
        /// <returns></returns>
        public UserArticleCount GetUserArticleCount(int id)
        {
            return new UserArticleCount()
            {
                PostCount = _postRepo.GetPostCountByUserId(_conn, id),
                ReplyCount = _replyRepo.GetReplyCountByUserId(_conn, id)
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
                using (var connection = _connFactory.GetConnection())
                {
                    // Table User
                    var fileName = SaveMemberPic(model.File, path);
                    var user = ConvertToUserEntity(model, $"{FileUploadPath}/{fileName}");
                    user.Id = _userRepo.Create(connection, user);
                    var userVm = ConvertToUserViewModel(user);
                    // Table Password
                    var password = ConvertToPassEntity(user.Id, user.Guid, model.Password);
                    CreatePassword(password);

                    // 註冊完直接給他登入-因為是新會員，所以文章count直接給預設0即可
                    SignInUser.UserLogin(true, userVm, new UserArticleCount());
                }

                tranScope.Complete();
            }
        }

        private UserViewModel ConvertToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Mail = user.Mail,
                Name = user.Name,
                Pic = user.Pic,
                IsAdmin = user.IsAdmin,
                IsDel = user.IsDel,
                Guid = user.Guid
            };
        }

        /// <summary>
        /// 檢查歷史密碼中是否存在相同的密碼
        /// </summary>
        /// <param name="userId">會員Id</param>
        /// <param name="newHashPass">要檢查的hash密碼</param>
        /// <returns>True表示歷史紀錄有相同密碼</returns>
        public bool CheckIsHistroyPassword(int userId, string newHashPass)
        {
            var histroyPasswords = _passwordRepo.GetUserHistroyPasswords(_conn, userId);
            return histroyPasswords.Any(x => x.HashPw == newHashPass);
        }

        /// <summary>
        /// 會員大頭照實體路徑
        /// </summary>
        /// <param name="path">檔案實體路徑</param>
        public void RemoveMemberPic(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}