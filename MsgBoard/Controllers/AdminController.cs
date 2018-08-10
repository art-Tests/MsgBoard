using System.Data;
using System.Web.Mvc;
using MsgBoard.Filter;
using MsgBoard.Services;

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
            if (CheckIsAdmin().Equals(false))
            {
                return RedirectToAction("Index", "Post");
            }

            var model = _memberService.GetUserCollection(_connection, page, pageSize);
            return View(model);
        }

        private bool CheckIsAdmin()
        {
            return _memberService.CheckIsAdmin(Session["memberAreaData"]);
        }
    }
}