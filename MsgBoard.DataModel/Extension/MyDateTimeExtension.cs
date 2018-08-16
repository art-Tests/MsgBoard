using System;
using System.Globalization;

namespace MsgBoard.DataModel.Extension
{
    public static class MyDateTimeExtension
    {
        public static string ConvertToChinese(this DateTime dt)
        {
            var tw = new TaiwanCalendar();
            return $"民國 {tw.GetYear(dt)} 年 {dt.Month} 月 {dt.Day} 日";
        }
    }
}