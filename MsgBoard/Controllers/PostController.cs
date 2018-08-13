using System.Data;
using System.Web.Mvc;
using MsgBoard.Filter;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Services;

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
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AuthorizePlus]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AuthorizePlus]
        public ActionResult Create(Post model)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserId = SignInUser.User.Id;

                _postService.Create(_conn, model);
                return RedirectToAction("Index", "Post");
            }
            return View(model);
        }
    }
}