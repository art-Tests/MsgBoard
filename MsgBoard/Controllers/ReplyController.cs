using System.Net;
using System.Web.Mvc;
using DataAccess.Interface;
using DataAccess.Services;
using DataModel.Entity;
using MsgBoard.DataModel.Dto;
using MsgBoard.Filter;
using MsgBoard.Services;

namespace MsgBoard.Controllers
{
    public class ReplyController : Controller
    {
        private readonly ReplyService _replyService;
        private readonly IConnectionFactory _connFactory = new ConnectionFactory();

        public ReplyController()
        {
            _replyService = new ReplyService(_connFactory);
        }

        [HttpGet, AuthorizePlus]
        public ActionResult Create(int? id)
        {
            //參數錯誤
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //查無文章
            var dbPost = _replyService.GetPostById(id.Value);
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
            _replyService.CreateReply(model);
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
            var model = _replyService.GetReplyById(id.Value);
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

            var dbReply = _replyService.GetReplyById(id.Value);
            if (dbReply == null)
            {
                return View(model);
            }

            if (SignInUser.User.IsAdmin || SignInUser.User.Id == dbReply.CreateUserId)
            {
                _replyService.UpdateReply(model, dbReply);
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
            var model = _replyService.GetReplyById(id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }
            if (SignInUser.User.IsAdmin || model.CreateUserId == SignInUser.User.Id)
            {
                _replyService.DeleteReply(model);
                return RedirectToAction("Index", "Post");
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
    }
}