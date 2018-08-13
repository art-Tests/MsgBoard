using System.Web.Mvc;

namespace MsgBoard.Helper
{
    public static class TagHelper
    {
        public static string CreateTag(this HtmlHelper helper, bool isEnable, string text)
        {
            return isEnable
                ? $"<label class=\"label label-warning\">{text}</label>"
                : string.Empty;
        }
    }
}