using System.Linq;
using System.Web.Mvc;
using MsgBoard.BL.Services;
using MsgBoard.DataModel.Dto;
using MsgBoard.Filter;
using PagedList;

namespace MsgBoard.Controllers
{
    [AuthorizePlus]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService = new AdminService();

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