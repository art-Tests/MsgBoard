using System.Web;
using DataModel.Entity;

namespace MsgBoard.DataModel.Dto
{
    /// <summary>
    /// 登入使用者
    /// </summary>
    public static class SignInUser
    {
        /// <summary>
        /// 目前使用者文章數量
        /// </summary>
        public static UserArticleCount ArticleCount
        {
            get
            {
                if (HttpContext.Current.Session["memberArticleCount"] is UserArticleCount artCnt)
                {
                    return artCnt;
                }
                return default(UserArticleCount);
            }
        }

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

        /// <summary>
        /// 會員登入設定Session
        /// </summary>
        /// <param name="isAuth">設定登入狀態</param>
        /// <param name="user">設定登入會員資料</param>
        /// <param name="artCnt">設定登入會員文章數量</param>
        public static void UserLogin(bool isAuth, User user, UserArticleCount artCnt)
        {
            HttpContext.Current.Session["auth"] = isAuth;
            HttpContext.Current.Session["memberAreaData"] = user;
            SetArticleCount(artCnt);
        }

        /// <summary>
        /// 調整session的發文數量
        /// </summary>
        /// <param name="number">修正值(整數，允許負值)</param>
        public static void AdjustPostCnt(int number)
        {
            if (HttpContext.Current.Session["memberArticleCount"] is UserArticleCount artCnt)
            {
                artCnt.PostCount = artCnt.PostCount + number;
                SetArticleCount(artCnt);
            }
        }

        /// <summary>
        /// 調整session的回覆數量
        /// </summary>
        /// <param name="number">修正值(整數，允許負值)</param>
        public static void AdjustReplyCnt(int number)
        {
            if (HttpContext.Current.Session["memberArticleCount"] is UserArticleCount artCnt)
            {
                artCnt.ReplyCount = artCnt.ReplyCount + number;
                SetArticleCount(artCnt);
            }
        }

        /// <summary>
        /// 設定會員文章數量Session
        /// </summary>
        /// <param name="artCnt">文章數量entity</param>
        public static void SetArticleCount(UserArticleCount artCnt)
        {
            HttpContext.Current.Session["memberArticleCount"] = artCnt;
        }
    }
}