using System;

namespace LComplete.Framework.Common
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// 将时间戳转换为时间类型
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(string timestamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timestamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 将JSON中的时间类型(示例：Tue May 31 17:46:55 +0800 2011)转换为C#时间类型
        /// </summary>
        /// <param name="jsonDate"></param>
        /// <returns></returns>
        public static DateTime ConvertJSONDateToDateTime(string jsonDate)
        {
            return DateTime.ParseExact(jsonDate, "ddd MMM dd HH:mm:ss K yyyy",
                                       new System.Globalization.CultureInfo("en-GB"));
        }

        /// <summary>
        /// 取某个时间的unix时间戳
        /// </summary>
        /// <returns></returns>
        public static string ToTimeStamp(this DateTime dt)
        {
            long jsBeginTick = new DateTime(1970, 1, 1).Ticks;

            long tick = (dt.ToUniversalTime().Ticks - jsBeginTick) / 10000;
            return tick.ToString();
        }
    }
}