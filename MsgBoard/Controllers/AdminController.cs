using System.Linq;
using System.Web.Mvc;
using DataAccess.Interface;
using DataAccess.Services;
using MsgBoard.DataModel.Dto;
using MsgBoard.Filter;
using PagedList;
using Services;

namespace MsgBoard.Controllers
{
    [AuthorizePlus]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly IConnectionFactory _connFactory = new ConnectionFactory();

        public AdminController()
        {
            _adminService = new AdminService(_connFactory);
        }

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            if (SignInUser.User.IsAdmin.Equals(false))
            {
                return RedirectToAction("Index", "Post");
            }

            ViewData["nowPage"] = page;

            var model = _adminService
                .GetUserCollection().AsQueryable()
                .ToPagedList(page, pageSize);
            return View(model);
        }
    }
}