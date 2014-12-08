using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace X3Platform.Web
{
    /// <summary>虚拟路径 工具类</summary>
    public class VirtualPathHelper
    {
        #region 静态函数:Combine(string url, string virtualPath)
        /// <summary>合并路径</summary>
        /// <param name="url">路径</param>
        /// <param name="virtualPath">相对虚拟路径</param>
        /// <returns></returns>
        public static string Combine(string url, string virtualPath)
        {
            // http:// | https://

            int startIndex = 1;

            if (string.IsNullOrEmpty(url))
            {
                // 默认设置到跟目录
                url = string.Empty;
            }
            else
            {
                if (url.IndexOf(":") > -1)
                {
                    startIndex = url.IndexOf(":");
                }

                // 把 "/" 符号去掉.
                if (url[url.Length - 1] == '/')
                {
                    url = url.Substring(0, url.Length - 1);
                }
            }

            string protocol = (startIndex == 0) ? string.Empty : url.Substring(0, startIndex);

            string hostName = url.Substring((startIndex == 0 ? 0 : startIndex + 3));

            if (virtualPath.Substring(0, 1) != "/")
            {
                virtualPath = string.Concat("/", virtualPath);
            }

            protocol = string.IsNullOrEmpty(protocol) ? string.Empty : string.Format("{0}://", protocol);

            virtualPath.Replace("//", "/");

            return string.Concat(protocol, hostName, virtualPath);
        }
        #endregion

        #region 静态函数:GetPhysicalPath(string virtualPath)
        /// <summary>获取物理路径</summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static string GetPhysicalPath(string virtualPath)
        {
            return HttpContext.Current == null ? virtualPath : HttpContext.Current.Server.MapPath(virtualPath);
        }
        #endregion
    }
}
