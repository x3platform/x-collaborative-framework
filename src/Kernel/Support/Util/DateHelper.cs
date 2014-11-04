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
        /// <summary>获取当前时间的时间戳 UNIX 时间戳, 即从 1970年1月1日 00:00:00 到当前时间的秒数之和.</summary>
        public static long GetTimestamp()
        {
            return GetTimestamp(DateTime.Now);
        }

        /// <summary>获取某个时间的 UNIX 时间戳, 即从 1970年1月1日 00:00:00 到某个时间的秒数之和.</summary>
        public static long GetTimestamp(DateTime datetime)
        {
            return (long)(datetime - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds;
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
}
