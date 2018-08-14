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
            var dbPost = _postService.GetPostById(Conn, id.Value);
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
            return RedirectToAction("Index", "Post");
        }
    }
}