using System.Configuration;
using System.Net;
using System.Web.Mvc;
using MsgBoard.BL.Services;
using MsgBoard.DataModel.Dto;
using MsgBoard.DataModel.ViewModel.Member;
using MsgBoard.Filter;

namespace MsgBoard.Controllers
{
    public class MemberController : Controller
    {
        protected string FileUploadPath = ConfigurationManager.AppSettings["uploadPath"];
        private readonly MemberService _memberService = new MemberService();

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

            var loginResult = _memberService.CheckUserPassword(model.Account, model.Password);
            if (loginResult.Auth.Equals(false))
            {
                ModelState.AddModelError("LoginError", "帳號或密碼錯誤");
                return View(model);
            }

            var artCnt = _memberService.GetUserArticleCount(loginResult.User.Id);
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

            var isExistSameUser = _memberService.CheckUserExist(model.Mail);
            if (isExistSameUser)
            {
                ModelState.AddModelError("SameUser", $"{model.Mail} 已被註冊，若忘記密碼請洽系統管理員。");
                return View(model);
            }
            var picPath = Server.MapPath(FileUploadPath);
            _memberService.CreateUser(model, picPath);

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

            var user = _memberService.GetUser(id.Value);
            if (user == null)
            {
                return RedirectToAction(backAction, backController);
            }

            if ((SignInUser.User.IsAdmin || SignInUser.User.Id == id).Equals(false))
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

        [HttpPost]
        [AuthorizePlus]
        public ActionResult Update(int id, MemberUpdateViewModel model)
        {
            var user = _memberService.GetUser(id);
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
                    var isSamePassword = _memberService.CheckIsHistroyPassword(user.Id, newPassEntity.HashPw);
                    if (isSamePassword)
                    {
                        ModelState.AddModelError("HistroyPassword", "新密碼不可跟使用過的舊密碼相同。");
                        model.Password = string.Empty;
                        return View(model);
                    }
                }

                _memberService.CreatePassword(newPassEntity);
            }

            // 大頭照
            var fileName = _memberService.SaveMemberPic(model.File, Server.MapPath(FileUploadPath));
            if (string.IsNullOrEmpty(fileName).Equals(false))
            {
                // 移除舊照片
                _memberService.RemoveMemberPic(Server.MapPath(user.Pic));
                user.Pic = $"{FileUploadPath}/{fileName}";
            }

            // Update Table User
            user.Name = model.Name;
            _memberService.UpdateUser(user);

            // 修改自己的資料完畢之後也要更新Session
            if (SignInUser.User.Id == id)
            {
                var artCnt = _memberService.GetUserArticleCount(user.Id);
                SignInUser.UserLogin(true, user, artCnt);
            }
            return RedirectToAction(model.BackAction, model.BackController, new { page = model.BackPage });
        }

        [AuthorizePlus]
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

            var user = _memberService.GetUser(id.Value);
            if (user == null)
            {
                return HttpNotFound("Member Not Found");
            }

            user.IsDel = !newStat;
            _memberService.UpdateUser(user);
            return RedirectToAction("Index", "Admin", new { page });
        }
    }
}