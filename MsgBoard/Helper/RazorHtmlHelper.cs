using System.Web.Mvc;

namespace MsgBoard.Helper
{
    public static class RazorHtmlHelper
    {
        /// <summary>
        /// 建立BootStrap3的Label
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="isEnable">是否顯示Label</param>
        /// <param name="text">Label文字</param>
        /// <param name="textClass">Label文字Class (預設warning)</param>
        /// <returns></returns>
        public static string CreateTag(this HtmlHelper helper, bool isEnable, string text, string textClass = "warning")
        {
            return isEnable
                ? $"<label class=\"label label-{textClass}\">{text}</label>"
                : string.Empty;
        }

        /// <summary>
        /// 取得會員文章、回覆連結
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="id">會員Id</param>
        /// <param name="count">要顯示的數量</param>
        /// <param name="linkType">連結類型(已發佈；已回覆)</param>
        /// <returns></returns>
        public static string GetCurUserArticleLink(this HtmlHelper helper, int id, int count, string linkType)
        {
            var text = linkType.ToUpper() == "POST" ? "已發佈" : "已回覆";
            var link = count > 0
                ? $@"<a href=""/Post/Index/{id}?queryItem={linkType}"">{text}：{count}</a>"
                : $@"<span class=""navbar-text"">{text}：0</span>";
            return link;
        }

        /// <summary>
        /// 建立查看回覆的連結
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="replyCount">reply數量</param>
        /// <param name="postId">文章Id</param>
        /// <returns></returns>
        public static string CreateReplyLink(this HtmlHelper helper, int replyCount, int postId)
        {
            if (replyCount > 0)
            {
                return
                    $@"
<a href=""javascript:;"" class=""reply-link"" data-postid=""{postId}"">
    查看全部 {replyCount} 則留言
</a>
<a href=""javascript:;"" class=""reply-close"" style=""display:none"">隱藏留言</a>
<div class=""reply-area""></div>
";
            }
            else
            {
                return "<span>無回覆訊息</span>";
            }
        }
    }
}