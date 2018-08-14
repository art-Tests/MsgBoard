using System.Web.Mvc;
using MsgBoard.Filter;
using MsgBoard.Models.Dto;
using MsgBoard.Services;
using PagedList;

namespace MsgBoard.Controllers
{
    [AuthorizePlus]
    public class AdminController : BaseController
    {
        private readonly MemberService _memberService;

        public AdminController()
        {
            _memberService = new MemberService();
        }

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            if (SignInUser.User.IsAdmin.Equals(false))
            {
                return RedirectToAction("Index", "Post");
            }

            ViewData["nowPage"] = page;

            var model = _memberService
                .GetUserCollection(Conn)
                .ToPagedList(page, pageSize);
            return View(model);
        }
    }
}