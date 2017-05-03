namespace X3Platform.Web
{
    using System;

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
#if NETCORE10
            return System.Net.WebUtility.UrlEncode(value).Replace("(", "%28").Replace(")", "%29");
#else
            return System.Web.HttpUtility.UrlEncode(value).Replace("(", "%28").Replace(")", "%29");
#endif
        }
        #endregion

        #region ��̬����:UrlDecode(string value)
        /// <summary>��ַ����</summary>
        /// <param name="value">����·��</param>
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
