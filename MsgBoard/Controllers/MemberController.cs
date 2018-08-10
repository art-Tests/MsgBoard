using System.Net;
using System.Web.Mvc;
using MsgBoard.Services;
using MsgBoard.ViewModel.Member;
using System.Transactions;
using System.Web;
using MsgBoard.Filter;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;

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

            var connection = _connFactory.GetConnection();
            var loginResult = _memberService.CheckUserPassword(connection, model.Account, model.Password);
            if (loginResult.Auth.Equals(false))
            {
                ModelState.AddModelError("LoginError", "帳號或密碼錯誤");
                return View(model);
            }

            CreateOrUpdateUserSession(loginResult);
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

            var isExistSameUser = _memberService.CheckUserExist(_connFactory.GetConnection(), model.Mail);
            if (isExistSameUser)
            {
                ModelState.AddModelError("SameUser", $"{model.Mail} 已被註冊，若忘記密碼請洽系統管理員。");
                return View(model);
            }

            using (var tranScope = new TransactionScope())
            {
                using (var connection = _connFactory.GetConnection())
                {
                    // Table User
                    var fileName = _memberService.SaveMemberPic(model.File, Server.MapPath(FileUploadPath));
                    var user = _memberService.ConvertToUserEntity(model, $"{FileUploadPath}/{fileName}");
                    var userId = _memberService.CreateUser(connection, user);

                    // Table Password
                    var password = _memberService.ConvertToPassEntity(userId, user.Guid, model.Password);
                    _memberService.CreatePassword(connection, password);

                    // 註冊完直接給他登入
                    CreateOrUpdateUserSession(user, true);
                }

                tranScope.Complete();
            }

            return RedirectToAction("Index", "Post");
        }

        [HttpGet]
        [AuthorizePlus]
        public ActionResult Update(int? id)
        {
            ViewBag.Title = "修改會員資料";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var connection = _connFactory.GetConnection();
            var user = _memberService.GetUser(connection, id.Value);
            if (user == null)
            {
                return HttpNotFound("Member Not Found");
            }

            var model = new MemberUpdateViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Password = string.Empty,
                Pic = user.Pic
            };
            return View(model);
        }

        [HttpPost]
        [AuthorizePlus]
        public ActionResult Update(int id, FormCollection form, HttpPostedFileBase file)
        {
            var connection = _connFactory.GetConnection();
            var user = _memberService.GetUser(connection, id);

            if (TryUpdateModel(user, "", form.AllKeys, new[] { "Id" }))
            {
                var fileName = _memberService.SaveMemberPic(file, Server.MapPath(FileUploadPath));
                if (string.IsNullOrEmpty(fileName).Equals(false))
                {
                    user.Pic = $"{FileUploadPath}/{fileName}";
                }

                _memberService.UpdateUser(connection, user);

                // 修改資料完畢之後也要更新Session
                CreateOrUpdateUserSession(user, true);
                return RedirectToAction("Index", "Post");
            }

            var model = new MemberUpdateViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Password = string.Empty,
                Pic = user.Pic
            };
            return View(model);
        }

        /// <summary>
        /// 更新會員登入Session資料
        /// </summary>
        /// <param name="user">會員Entity</param>
        /// <param name="isAuth">是否登入</param>
        private void CreateOrUpdateUserSession(User user, bool isAuth)
        {
            var loginResult = new UserLoginResult
            {
                Auth = isAuth,
                User = user
            };
            CreateOrUpdateUserSession(loginResult);
        }

        /// <summary>
        /// 更新會員登入Session資料
        /// </summary>
        /// <param name="loginResult">會員登入結果</param>
        private void CreateOrUpdateUserSession(UserLoginResult loginResult)
        {
            Session["auth"] = loginResult.Auth;
            Session["memberAreaData"] = loginResult;
        }
    }
}