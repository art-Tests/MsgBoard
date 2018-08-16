using System.Web.Mvc;
using MsgBoard.Filter;
using MsgBoard.Models.Dto;
using MsgBoard.Services;
using MsgBoard.Services.Factory;
using MsgBoard.Services.Interface;
using PagedList;

namespace MsgBoard.Controllers
{
    [AuthorizePlus]
    public class AdminController : Controller
    {
        private readonly MemberService _memberService;
        private readonly IConnectionFactory _connFactory = new ConnectionFactory();

        public AdminController()
        {
            _memberService = new MemberService(_connFactory);
        }

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            if (SignInUser.User.IsAdmin.Equals(false))
            {
                return RedirectToAction("Index", "Post");
            }

            ViewData["nowPage"] = page;

            var model = _memberService
                .GetUserCollection()
                .ToPagedList(page, pageSize);
            return View(model);
        }
    }
}