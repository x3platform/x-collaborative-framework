using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace X3Platform.Web
{
    /// <summary>����·�� ������</summary>
    public class UrlHelper
    {
        #region ��̬����:UrlEncode(string virtualPath)
        /// <summary>��ȡ����·��</summary>
        /// <param name="virtualPath">����·��</param>
        /// <returns></returns>
        public static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value).Replace("(", "%28").Replace(")", "%29");
        }
        #endregion
    }
}
