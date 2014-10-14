using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace X3Platform.Web
{
    /// <summary>虚拟路径 工具类</summary>
    public class UrlHelper
    {
        #region 静态函数:UrlEncode(string virtualPath)
        /// <summary>获取物理路径</summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value).Replace("(", "%28").Replace(")", "%29");
        }
        #endregion
    }
}
