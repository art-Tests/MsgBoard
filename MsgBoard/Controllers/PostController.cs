using System.Data;
using System.Net;
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

        [HttpGet, AuthorizePlus]
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _postService.GetPostById(_conn, id.Value);
            return View(model);
        }

        public ActionResult Update(int? id, Post model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid) return View(model);

            var dbPost = _postService.GetPostById(_conn, id.Value);
            if (dbPost == null)
            {
                return View(model);
            }

            dbPost.Content = model.Content;
            dbPost.UpdateUserId = SignInUser.User.Id;
            _postService.Update(_conn, dbPost);

            return RedirectToAction("Index","Post");
        }
    }
}