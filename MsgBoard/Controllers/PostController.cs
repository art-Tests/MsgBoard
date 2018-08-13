using System.Data;
using System.Web.Mvc;
using MsgBoard.Filter;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Services;
using PagedList;

namespace MsgBoard.Controllers
{
    public class PostController : BaseController
    {
        private readonly IDbConnection _conn;
        private readonly PostService _postService;

        public PostController()
        {
            _conn = _connFactory.GetConnection();
            _postService = new PostService();
        }

        // GET: Post
        [Route("~/")]
        [Route("Home")]
        [Route("Home/Index")]
        [Route("Post")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = _postService
                .GetPostCollection(_conn)
                .ToPagedList(page, pageSize);
            return View(model);
        }

        [HttpGet]
        [AuthorizePlus]
        public ActionResult Create()
        {
            ViewBag.BtnName = "新增";

            return View();
        }

        [HttpPost]
        [AuthorizePlus]
        public ActionResult Create(Post model)
        {
            if (!ModelState.IsValid) return View(model);

            model.CreateUserId = SignInUser.User.Id;
            model.UpdateUserId = SignInUser.User.Id;
            _postService.Create(_conn, model);
            return RedirectToAction("Index", "Post");
        }
    }
}