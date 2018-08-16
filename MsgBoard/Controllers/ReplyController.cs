using System.Net;
using System.Web.Mvc;
using MsgBoard.Filter;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Services;

namespace MsgBoard.Controllers
{
    public class ReplyController : BaseController
    {
        private readonly PostService _postService = new PostService();
        private readonly ReplyService _replyService = new ReplyService();

        [HttpGet, AuthorizePlus]
        public ActionResult Create(int? id)
        {
            //參數錯誤
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //查無文章
            var dbPost = _postService.GetPostById(id.Value);
            if (dbPost == null)
            {
                return HttpNotFound();
            }
            //已刪除文章不允許回覆
            if (dbPost.IsDel)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.postId = id;
            return View();
        }

        [HttpPost, AuthorizePlus]
        public ActionResult Create(Reply model)
        {
            if (!ModelState.IsValid) return View(model);

            model.CreateUserId = SignInUser.User.Id;
            model.UpdateUserId = SignInUser.User.Id;
            _replyService.Create(Conn, model);
            SignInUser.AdjustReplyCnt(1);
            return RedirectToAction("Index", "Post");
        }

        [HttpGet, AuthorizePlus]
        public ActionResult Update(int? id)
        {
            //參數錯誤
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //查無文章
            var model = _replyService.GetReplyById(Conn, id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }
            //已刪除文章不允許回覆
            if (model.IsDel)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(model);
        }

        public ActionResult Update(int? id, Reply model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid) return View(model);

            var reply = _replyService.GetReplyById(Conn, id.Value);
            if (reply == null)
            {
                return View(model);
            }

            if (SignInUser.User.IsAdmin || SignInUser.User.Id == reply.CreateUserId)
            {
                reply.Content = model.Content;
                reply.UpdateUserId = SignInUser.User.Id;
                _replyService.Update(Conn, reply);

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
            var model = _replyService.GetReplyById(Conn, id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }
            if (SignInUser.User.IsAdmin || model.CreateUserId == SignInUser.User.Id)
            {
                _replyService.Delete(Conn, id.Value);
                if (model.CreateUserId == SignInUser.User.Id)
                {
                    SignInUser.AdjustReplyCnt(-1);
                }

                return RedirectToAction("Index", "Post");
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
    }
}