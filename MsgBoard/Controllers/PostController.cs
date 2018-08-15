using System.Net;
using System.Transactions;
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
        private readonly PostService _postService = new PostService();
        private readonly ReplyService _replyService = new ReplyService();
        private readonly MemberService _memberService = new MemberService();

        // GET: Post
        public ActionResult Index(int? id, string queryItem = "", int page = 1, int pageSize = 5)
        {
            var model = _postService
                .GetPostCollection(Conn, id, queryItem)
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

            SignInUser.AdjustPostCnt(1);
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
            var model = _postService.GetPostById(Conn, id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }

            if (SignInUser.User.IsAdmin || model.CreateUserId == SignInUser.User.Id)
            {
                using (var transScope = new TransactionScope())
                {
                    using (var connection = ConnFactory.GetConnection())
                    {
                        // Delete Post
                        _postService.Delete(connection, id.Value);
                        // Delete Reply
                        _replyService.DeleteByPostId(connection, id.Value);
                    }
                    transScope.Complete();
                }
                // 刪除文章、回復，有可能刪除到管理者或是其他人的資料，因此直接重新刷新目前User的文章數量資訊
                var artCnt = _memberService.GetUserArticleCount(Conn, SignInUser.User.Id);
                SignInUser.SetArticleCount(artCnt);

                return RedirectToAction("Index", "Post");
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
    }
}