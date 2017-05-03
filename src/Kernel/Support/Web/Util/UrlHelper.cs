namespace X3Platform.Web
{
    using System;

    /// <summary>Url 工具类</summary>
    public class UrlHelper
    {
        #region 静态函数:Combine(string url, string args)
        /// <summary>合并路径</summary>
        /// <param name="url">路径</param>
        /// <param name="httpParams">参数</param>
        /// <returns></returns>
        public static string Combine(string url, string httpParams)
        {
            return url + ((url.IndexOf("?") == -1) ? "?" : "&") + httpParams;
        }
        #endregion

        #region 静态函数:UrlEncode(string value)
        /// <summary>地址编码</summary>
        /// <param name="value">虚拟路径</param>
        /// <returns></returns>
        public static string UrlEncode(string value)
        {
#if NETCORE10
            return System.Net.WebUtility.UrlEncode(value).Replace("(", "%28").Replace(")", "%29");
#else
            return System.Web.HttpUtility.UrlEncode(value).Replace("(", "%28").Replace(")", "%29");
#endif
        }
        #endregion

        #region 静态函数:UrlDecode(string value)
        /// <summary>地址编码</summary>
        /// <param name="value">虚拟路径</param>
        /// <returns></returns>
        public static string UrlDecode(string value)
        {
#if NETCORE10
            return System.Net.WebUtility.UrlDecode(value);
#else
            return System.Web.HttpUtility.UrlDecode(value);
#endif
        }
        #endregion
    }
}
