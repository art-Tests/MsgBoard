using System.Data;
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
        private readonly IDbConnection _connection;

        public AdminController()
        {
            _memberService = new MemberService();
            _connection = _connFactory.GetConnection();
        }

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            if (SignInUser.User.IsAdmin.Equals(false))
            {
                return RedirectToAction("Index", "Post");
            }

            ViewData["nowPage"] = page;

            var model = _memberService
                .GetUserCollection(_connection)
                .ToPagedList(page, pageSize);
            return View(model);
        }
    }
}