using System.Web;
using MsgBoard.Models.Entity;

namespace MsgBoard.Models.Dto
{
    /// <summary>
    /// 登入使用者
    /// </summary>
    public static class SignInUser
    {
        /// <summary>
        /// 目前登入使用者資訊
        /// </summary>
        public static User User
        {
            get
            {
                if (HttpContext.Current.Session["memberAreaData"] is User user)
                {
                    return user;
                }
                return null;
            }
        }

        /// <summary>
        /// 目前使用者登入狀態
        /// </summary>
        public static bool Auth
        {
            get
            {
                if (HttpContext.Current.Session["auth"] is bool isAuth)
                {
                    return isAuth;
                }
                return false;
            }
        }

        public static void UserLogin(bool isAuth, User user)
        {
            HttpContext.Current.Session["auth"] = isAuth;
            HttpContext.Current.Session["memberAreaData"] = user;
        }
    }
}