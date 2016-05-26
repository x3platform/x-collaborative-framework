using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace X3Platform.Web
{
    /// <summary>����Ĺ����� ������</summary>
    public class RequestHelper
    {
        #region ��̬����:Dump(HttpRequestBase request)
        /// <summary>��ȡ�������ϸ��Ϣ</summary>
        /// <param name="request">������Ϣ</param>
        /// <returns></returns>
        public static string Dump(HttpRequestBase request)
        {
            return Dump(request, string.Empty);
        }
        #endregion

        #region ��̬����:Dump(HttpRequestBase request, string rawInput)
        /// <summary>��ȡ�������ϸ��Ϣ</summary>
        /// <param name="request">������Ϣ</param>
        /// <returns></returns>
        public static string Dump(HttpRequestBase request, string rawInput)
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("=== request common ===");

            outString.AppendLine("user agent :" + request.UserAgent);
            outString.AppendLine("content type :" + request.ContentType);

            if (request.Files.Count > 0)
            {
                outString.AppendLine("file count :" + request.Files.Count);
            }

            if (request.ContentType == "application/xml" || request.ContentType == "application/json")
            {
                if (!string.IsNullOrEmpty(rawInput))
                {
                    outString.AppendLine("=== request data ===");
                    outString.AppendLine(rawInput);
                }
            }
            else
            {
                if (request.QueryString.Count > 0)
                {
                    outString.AppendLine("=== request query string ===");

                    foreach (string key in request.QueryString.AllKeys)
                    {
                        outString.AppendLine(key + ":" + request.QueryString[key]);
                    }
                }

                if (request.Form.Count > 0)
                {
                    outString.AppendLine("=== request form ===");

                    foreach (string key in request.Form.AllKeys)
                    {
                        outString.AppendLine(key + ":" + (request.Form[key].Length < 4000 ? request.Form[key] : "[Long String] " + request.Form[key].Length));
                    }
                }
            }

            return outString.ToString();
        }
        #endregion

        /// <summary></summary>
        /// <returns></returns>
        public static string Fetch(string defaultName)
        {
            return Fetch(defaultName, new string[] { });
        }

        /// <summary></summary>
        /// <returns></returns>
        public static string Fetch(string defaultName, string alias)
        {
            return Fetch(defaultName, new string[] { alias });
        }

        /// <summary></summary>
        /// <returns></returns>
        public static string Fetch(string defaultName, string[] alias)
        {
            HttpRequestBase request = new HttpRequestWrapper(HttpContext.Current.Request);

            return Fetch(request, defaultName, alias);
        }

        /// <summary></summary>
        /// <returns></returns>
        public static string Fetch(HttpRequestBase request, string defaultName, string[] alias)
        {
            // GET ��ʽ
            string value = request.QueryString[defaultName];

            // POST ��ʽ
            if (string.IsNullOrEmpty(value))
            {
                value = request.Form[defaultName];
            }

            if (string.IsNullOrEmpty(value))
            {
                // �������ݵ�����
                foreach (string item in alias)
                {
                    if (!string.IsNullOrEmpty(request.QueryString[item]))
                    {
                        return request.QueryString[item];
                    }

                    if (!string.IsNullOrEmpty(request.Form[item]))
                    {
                        return request.Form[item];
                    }
                }

                return string.Empty;
            }
            else
            {
                return value;
            }
        }
    }
}
