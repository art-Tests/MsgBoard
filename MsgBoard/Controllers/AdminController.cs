using System.Web.Mvc;
using DataAccess.Interface;
using DataAccess.Services;
using MsgBoard.DataModel.Dto;
using MsgBoard.Filter;
using MsgBoard.Services;
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