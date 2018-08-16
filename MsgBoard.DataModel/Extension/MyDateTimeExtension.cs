using System;
using System.Globalization;

namespace MsgBoard.DataModel.Extension
{
    public static class MyDateTimeExtension
    {
        /// <summary>
        /// 轉換為民國年顯示
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static string ConvertToChinese(this DateTime dt)
        {
            var tw = new TaiwanCalendar();
            return $"民國 {tw.GetYear(dt)} 年 {dt.Month} 月 {dt.Day} 日";
        }
    }
}