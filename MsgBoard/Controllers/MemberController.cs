using System.IO;
using System.Web;
using System.Web.Mvc;
using MsgBoard.ViewModel.Member;

namespace MsgBoard.Controllers
{
    public class MemberController : Controller
    {
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
            if (model.File.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.File.FileName);
                var path = Path.Combine(Server.MapPath("~/FileUploads"), fileName);
                model.File.SaveAs(path);
            }
            return RedirectToAction("Create");
        }
    }
}