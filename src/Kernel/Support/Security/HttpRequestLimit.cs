namespace X3Platform.Security
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;

    using X3Platform.Security;
    using X3Platform.Util;
    using System.Web.Caching;
    using System.Web;
    using X3Platform.Security.Configuration;
    #endregion

    /// <summary>请求限制管理</summary>
    public static class HttpRequestLimit
    {
        /// <summary>限制IP</summary>
        public static bool LimitIP()
        {
            // 默认限制 一个IP 两小时 1000 次请求

            return LimitIP(SecurityConfigurationView.Instance.DefaultRequestLimitMinutes, SecurityConfigurationView.Instance.DefaultRequestLimitCount);
        }

        /// <summary>限制IP</summary>
        public static bool LimitIP(int minutes, int count)
        {
            HttpContext context = HttpContext.Current;

            string content = "IP-" + minutes + "-" + count + "-" + context.Request.UserHostAddress;

            // 默认限制 一个IP 两小时 1000 次请求

            return LimitContent(content, minutes, count);
        }

        /// <summary>限制IP</summary>
        public static bool LimitRawUrl()
        {
            // 默认限制 一个IP 两小时 1000 次请求

            return LimitRawUrl(SecurityConfigurationView.Instance.DefaultRequestLimitMinutes, SecurityConfigurationView.Instance.DefaultRequestLimitCount);
        }

        /// <summary>限制IP</summary>
        public static bool LimitRawUrl(int minutes, int count)
        {
            HttpContext context = HttpContext.Current;

            string content = "IP-" + minutes + "-" + count + "-" + context.Request.RawUrl;

            // 默认限制 一个IP 两小时 500 次请求

            return LimitContent(content, minutes, count);
        }

        /// <summary>限制内容</summary>
        /// <param name="content">内容</param>
        /// <param name="minutes">分钟数</param>
        /// <param name="count">次数</param>
        public static bool LimitContent(string content, int minutes, int count)
        {
            HttpContext context = HttpContext.Current;

            // 限制 IP 访问频次 两个小时 500 次
            string key = "HttpRequestLimit-" + content;

            int hit = (Int32)(context.Cache[key] ?? 0);

            if (hit > count)
            {
                return true;
            }
            else
            {
                hit++;
            }

            if (hit == 1)
            {
                context.Cache.Add(key, hit, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            else
            {
                context.Cache[key] = hit;
            }

            return false;
        }
    }
}
