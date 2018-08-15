using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MsgBoard.Services;
using MsgBoard.ViewModel.Member;
using System.Transactions;
using MsgBoard.Filter;
using MsgBoard.Models.Dto;

namespace MsgBoard.Controllers
{
    public class MemberController : BaseController
    {
        private readonly MemberService _memberService;

        public MemberController()
        {
            _memberService = new MemberService();
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Title = "會員登入";
            return View();
        }

        [HttpPost]
        public ActionResult Login(MemberLoginViewModel model)
        {
            ViewBag.Title = "會員登入";

            if (!ModelState.IsValid) return View(model);

            var loginResult = _memberService.CheckUserPassword(Conn, model.Account, model.Password);
            if (loginResult.Auth.Equals(false))
            {
                ModelState.AddModelError("LoginError", "帳號或密碼錯誤");
                return View(model);
            }

            var artCnt = _memberService.GetUserArticleCount(Conn, loginResult.User.Id);
            SignInUser.UserLogin(true, loginResult.User, artCnt);
            return RedirectToAction("Index", "Post");
        }

        /// <summary>
        /// 會員登出
        /// </summary>
        public ActionResult LogOut()
        {
            RemoveUserSession();
            return RedirectToAction("Index", "Post");
        }

        private void RemoveUserSession()
        {
            Session.RemoveAll();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "新增會員";
            return View();
        }

        [HttpPost]
        public ActionResult Create(MemberCreateViewModel model)
        {
            ViewBag.Title = "新增會員";

            if (!ModelState.IsValid) return View(model);

            var isExistSameUser = _memberService.CheckUserExist(ConnFactory.GetConnection(), model.Mail);
            if (isExistSameUser)
            {
                ModelState.AddModelError("SameUser", $"{model.Mail} 已被註冊，若忘記密碼請洽系統管理員。");
                return View(model);
            }

            using (var tranScope = new TransactionScope())
            {
                using (var connection = ConnFactory.GetConnection())
                {
                    // Table User
                    var fileName = _memberService.SaveMemberPic(model.File, Server.MapPath(FileUploadPath));
                    var user = _memberService.ConvertToUserEntity(model, $"{FileUploadPath}/{fileName}");
                    var userId = _memberService.CreateUser(connection, user);

                    // Table Password
                    var password = _memberService.ConvertToPassEntity(userId, user.Guid, model.Password);
                    _memberService.CreatePassword(connection, password);

                    // 註冊完直接給他登入
                    var artCnt = _memberService.GetUserArticleCount(Conn, user.Id);
                    SignInUser.UserLogin(true, user, artCnt);
                }

                tranScope.Complete();
            }

            return RedirectToAction("Index", "Post");
        }

        [HttpGet]
        [AuthorizePlus]
        public ActionResult Update(int? id, int? page)
        {
            var backController = page != null ? "Admin" : "Post";
            var backAction = "Index";

            ViewBag.Title = "修改會員資料";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var connection = ConnFactory.GetConnection();
            var user = _memberService.GetUser(connection, id.Value);
            if (user == null)
            {
                //return HttpNotFound("Member Not Found");
                return RedirectToAction(backAction, backController);
            }

            var isAllowEdit = CheckAllowEditMember(id);
            if (isAllowEdit.Equals(false))
            {
                return RedirectToAction(backAction, backController);
            }

            var model = new MemberUpdateViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Password = string.Empty,
                Pic = user.Pic,
                BackAction = backAction,
                BackController = backController,
                BackPage = page
            };
            return View(model);
        }

        private bool CheckAllowEditMember(int? id)
        {
            return SignInUser.User.IsAdmin || SignInUser.User.Id == id;
        }

        [HttpPost]
        [AuthorizePlus]
        public ActionResult Update(int id, MemberUpdateViewModel model)
        {
            var connection = ConnFactory.GetConnection();
            var user = _memberService.GetUser(connection, id);

            if (ModelState.IsValid.Equals(false))
            {
                model.Pic = user.Pic;
                return View(model);
            }

            // Update Table Password
            var newPassword = model.Password;
            if (string.IsNullOrEmpty(newPassword).Equals(false))
            {
                var newPassEntity = _memberService.ConvertToPassEntity(user.Id, user.Guid, newPassword);

                // 管理者可以強制變更密碼
                if (SignInUser.User.IsAdmin.Equals(false))
                {
                    var isSamePassword = CheckIsHistroyPassword(connection, user.Id, newPassEntity.HashPw);
                    if (isSamePassword)
                    {
                        ModelState.AddModelError("HistroyPassword", "新密碼不可跟使用過的舊密碼相同。");
                        model.Password = string.Empty;
                        return View(model);
                    }
                }

                _memberService.CreatePassword(connection, newPassEntity);
            }

            // 大頭照
            var fileName = _memberService.SaveMemberPic(model.File, Server.MapPath(FileUploadPath));
            if (string.IsNullOrEmpty(fileName).Equals(false))
            {
                user.Pic = $"{FileUploadPath}/{fileName}";
            }

            // Update Table User
            user.Name = model.Name;
            _memberService.UpdateUser(connection, user);

            // 修改自己的資料完畢之後也要更新Session
            if (SignInUser.User.Id == id)
            {
                var artCnt = _memberService.GetUserArticleCount(Conn, user.Id);
                SignInUser.UserLogin(true, user, artCnt);
            }
            return RedirectToAction(model.BackAction, model.BackController, new { page = model.BackPage });
        }

        /// <summary>
        /// 檢查歷史密碼中是否存在相同的密碼
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="userId">會員Id</param>
        /// <param name="newHashPass">要檢查的hash密碼</param>
        /// <returns>True表示歷史紀錄有相同密碼</returns>
        private bool CheckIsHistroyPassword(IDbConnection connection, int userId, string newHashPass)
        {
            var histroyPasswords = _memberService.GetHistroyPasswords(connection, userId);
            return histroyPasswords.Any(x => x.HashPw == newHashPass);
        }

        public ActionResult ChangeStat(int? id, bool newStat, int page = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (SignInUser.User.IsAdmin.Equals(false))
            {
                return RedirectToAction("Index", "Post");
            }

            var connection = ConnFactory.GetConnection();
            var user = _memberService.GetUser(connection, id.Value);
            if (user == null)
            {
                return HttpNotFound("Member Not Found");
                //return RedirectToAction("Index", "Post");
            }

            user.IsDel = !newStat;
            _memberService.UpdateUser(connection, user);
            return RedirectToAction("Index", "Admin", new { page });
        }
    }
}