using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace X3Platform.Web
{
    /// <summary>����·�� ������</summary>
    public class VirtualPathHelper
    {
        #region ��̬����:Combine(string url, string virtualPath)
        /// <summary>�ϲ�·��</summary>
        /// <param name="url">·��</param>
        /// <param name="virtualPath">�������·��</param>
        /// <returns></returns>
        public static string Combine(string url, string virtualPath)
        {
            // http:// | https://

            int startIndex = 1;

            if (string.IsNullOrEmpty(url))
            {
                // Ĭ�����õ���Ŀ¼
                url = string.Empty;
            }
            else
            {
                if (url.IndexOf(":") > -1)
                {
                    startIndex = url.IndexOf(":");
                }

                // �� "/" ����ȥ��.
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

        #region ��̬����:GetPhysicalPath(string virtualPath)
        /// <summary>��ȡ����·��</summary>
        /// <param name="virtualPath">����·��</param>
        /// <returns></returns>
        public static string GetPhysicalPath(string virtualPath)
        {
            return HttpContext.Current == null ? virtualPath : HttpContext.Current.Server.MapPath(virtualPath);
        }
        #endregion
    }
}
