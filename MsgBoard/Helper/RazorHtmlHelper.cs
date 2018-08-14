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

        public static string CreateReplyLink(this HtmlHelper helper, int replyCount, int postId)
        {
            return replyCount > 0
                ? $"<a href=\"javascript:;\" class=\"reply-link\" data-postid=\"{postId}\">查看全部 {replyCount} 則留言</a><div class=\"reply-area\"></div>"
                : "<span>無回覆訊息</span>";
        }
    }
}