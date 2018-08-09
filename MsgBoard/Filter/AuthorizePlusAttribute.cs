using System;
using System.Web.Mvc;

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
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}