using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MsgBoard.Filter
{
    public class AuthorizePlusAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Convert.ToBoolean(filterContext.HttpContext.Session["auth"]))
            {
                // success
            }
            else
            {
                // fail
                filterContext.Result = new RedirectResult("~/Member/Login");
                //base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}