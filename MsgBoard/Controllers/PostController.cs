﻿using System.Net;
using System.Web.Mvc;
using MsgBoard.BL.Services;
using MsgBoard.DataModel.Dto;
using MsgBoard.DataModel.ViewModel.Post;
using MsgBoard.Filter;
using PagedList;

namespace MsgBoard.Controllers
{
    [AuthorizePlus]
    public class PostController : Controller
    {
        private readonly PostService _postService = new PostService();

        /// <summary>
        /// 取得文章列表
        /// </summary>
        /// <param name="id">會員編號(選填)，需與查詢項目一併使用</param>
        /// <param name="queryItem">要查詢文章或回覆(選填)，需與會員編號一併使用</param>
        /// <param name="page">第幾頁</param>
        /// <param name="pageSize">分頁筆數</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index(int? id, string queryItem = "", int page = 1, int pageSize = 5)
        {
            var model = _postService
                .GetPostCollection(id, queryItem)
                .ToPagedList(page, pageSize);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create() => View();

        [HttpPost]
        public ActionResult Create(PostViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            _postService.CreatePost(model);
            return RedirectToAction("Index", "Post");
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _postService.GetPostById(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(int? id, PostViewModel model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid) return View(model);

            var dbPost = _postService.GetPostById(id.Value);
            if (dbPost == null)
            {
                return View(model);
            }

            if (SignInUser.User.IsAdmin || SignInUser.User.Id == dbPost.CreateUserId)
            {
                _postService.UpdatePost(model);
                return RedirectToAction("Index", "Post");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _postService.GetPostById(id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }

            if (SignInUser.User.IsAdmin || model.CreateUserId == SignInUser.User.Id)
            {
                _postService.DeletePostAndReply(id.Value);
                return RedirectToAction("Index", "Post");
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
    }
}