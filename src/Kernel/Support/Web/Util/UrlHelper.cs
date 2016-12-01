using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace X3Platform.Web
{
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
            return HttpUtility.UrlEncode(value).Replace("(", "%28").Replace(")", "%29");
        }
        #endregion
    }
}
