namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.IO;
    using System.Text;
    #endregion

    /// <summary>时间处理辅助类</summary>
    public class DateHelper
    {
        private static DateTime baseTime = new DateTime(1970, 1, 1);

        /// <summary>默认时间</summary>
        public static DateTime DefaultTime { get { return baseTime; } }

        /// <summary>获取当前时间的时间戳 UNIX 时间戳, 即从 1970年1月1日 00:00:00 到当前时间的秒数之和.</summary>
        public static long GetTimestamp()
        {
            return GetTimestamp(DateTime.Now);
        }

        /// <summary>获取某个时间的 UNIX 时间戳, 即从 1970年1月1日 00:00:00 到某个时间的秒数之和.</summary>
        /// <param name="datetime">时间</param>
        public static long GetTimestamp(DateTime datetime)
        {
            // return (dateTime.Ticks - baseTime.Ticks) / 10000000 - 8 * 60 * 60;
            // return (long)(datetime - baseTime.ToLocalTime()).TotalSeconds;
            return (datetime.Ticks - baseTime.Ticks) / 10000000;
        }

        /// <summary>获取某个 UNIX 时间戳的时间格式.</summary>
        /// <param name="timestamp">从 1970年1月1日 00:00:00 到某个时间的秒数之和</param>
        public static DateTime ToDateTime(long timestamp)
        {
            // return new DateTime((timeStamp + 8 * 60 * 60) * 10000000 + BaseTime.Ticks);
            return new DateTime(timestamp * 10000000 + baseTime.Ticks).ToLocalTime();
        }

        /// <summary>获取某个时间到当前时间的时间间隔</summary>
        public static TimeSpan GetTimeSpan(DateTime beginTime)
        {
            return GetTimeSpan(beginTime, DateTime.Now);
        }

        /// <summary>获取某段时间的时间间隔</summary>
        public static TimeSpan GetTimeSpan(DateTime beginTime, DateTime endTime)
        {
            return (new TimeSpan(endTime.Ticks).Subtract(new TimeSpan(beginTime.Ticks)));
        }
    }

    /// <summary>日期扩展方法类</summary>
    public static class DateExtensions
    {
        /// <summary>获取某个时间的 UNIX 时间戳, 即从 1970年1月1日 00:00:00 到某个时间的秒数之和.</summary>
        public static long GetTimestamp(this DateTime date)
        {
            return DateHelper.GetTimestamp(date);
        }
    }
}
