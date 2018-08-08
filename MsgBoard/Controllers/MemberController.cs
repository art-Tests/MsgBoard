using System.IO;
using System.Web.Mvc;
using MsgBoard.Services;
using MsgBoard.ViewModel.Member;

namespace MsgBoard.Controllers
{
    public class MemberController : Controller
    {
        private readonly MemberService _service = new MemberService();

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

            var fileName = Path.GetFileName(model.File.FileName);
            var path = Path.Combine(Server.MapPath("~/FileUploads"), fileName);
            _service.CreateMember(model, path);

            return RedirectToAction("Create");
        }
    }
}