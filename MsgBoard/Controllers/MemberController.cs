using System.Web.Mvc;
using MsgBoard.Services;
using MsgBoard.ViewModel.Member;
using System.Transactions;

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

            var connection = ConnectionFactory.GetConnection();
            var loginResult = _memberService.CheckUserPassword(connection, model.Account, model.Password);
            if (loginResult.Auth.Equals(false))
            {
                ModelState.AddModelError("LoginError", "帳號或密碼錯誤");
                return View(model);
            }

            // 登入成功
            Session["auth"] = loginResult.Auth;
            Session["memberAreaData"] = loginResult;
            return RedirectToAction("Index", "Post");
        }

        /// <summary>
        /// 會員登出
        /// </summary>
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Post");
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

            var isExistSameUser = _memberService.CheckUserExist(ConnectionFactory.GetConnection(), model.Mail);
            if (isExistSameUser)
            {
                ModelState.AddModelError("SameUser", $"{model.Mail} 已被註冊，若忘記密碼請洽系統管理員。");
                return View(model);
            }

            using (var tranScope = new TransactionScope())
            {
                using (var connection = ConnectionFactory.GetConnection())
                {
                    // Table User
                    var fileName = _memberService.SaveMemberPic(model, Server.MapPath(FileUploadPath));
                    var user = _memberService.ConvertToUserEntity(model, $"{FileUploadPath}/{fileName}");
                    var userId = _memberService.CreateUser(connection, user);

                    // Table Password
                    var password = _memberService.ConvertToPassEntity(userId, user.Guid, model.Password);
                    _memberService.CreatePassword(connection, password);
                }

                tranScope.Complete();
            }

            return RedirectToAction("Index", "Post");
        }
    }
}