using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MsgBoard.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        [Route("~/")]
        [Route("Home")]
        [Route("Home/Index")]
        [Route("Post")]
        public ActionResult Index()
        {
            return View();
        }
    }
}