using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace X3Platform.Web
{
    /// <summary>Url ������</summary>
    public class UrlHelper
    {
        #region ��̬����:Combine(string url, string args)
        /// <summary>�ϲ�·��</summary>
        /// <param name="url">·��</param>
        /// <param name="httpParams">����</param>
        /// <returns></returns>
        public static string Combine(string url, string httpParams)
        {
            return url + ((url.IndexOf("?") == -1) ? "?" : "&") + httpParams;
        }
        #endregion

        #region ��̬����:UrlEncode(string value)
        /// <summary>��ַ����</summary>
        /// <param name="value">����·��</param>
        /// <returns></returns>
        public static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value).Replace("(", "%28").Replace(")", "%29");
        }
        #endregion
    }
}
