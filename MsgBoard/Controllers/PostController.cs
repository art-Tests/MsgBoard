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
        private readonly PostService _postService;

        public PostController()
        {
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
                .GetPostCollection(Conn)
                .ToPagedList(page, pageSize);
            return View(model);
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
            if (!ModelState.IsValid) return View(model);

            model.CreateUserId = SignInUser.User.Id;
            model.UpdateUserId = SignInUser.User.Id;
            _postService.Create(Conn, model);
            return RedirectToAction("Index", "Post");
        }

        [HttpGet, AuthorizePlus]
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _postService.GetPostById(Conn, id.Value);
            return View(model);
        }

        [HttpPost, AuthorizePlus]
        public ActionResult Update(int? id, Post model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid) return View(model);

            var dbPost = _postService.GetPostById(Conn, id.Value);
            if (dbPost == null)
            {
                return View(model);
            }

            if (SignInUser.User.IsAdmin || SignInUser.User.Id == dbPost.CreateUserId)
            {
                dbPost.Content = model.Content;
                dbPost.UpdateUserId = SignInUser.User.Id;
                _postService.Update(Conn, dbPost);

                return RedirectToAction("Index", "Post");
            }

            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbPost = _postService.GetPostById(Conn, id.Value);
            if (dbPost == null)
            {
                return HttpNotFound();
            }

            _postService.Delete(Conn, id.Value);
            return RedirectToAction("Index", "Post");
        }
    }
}