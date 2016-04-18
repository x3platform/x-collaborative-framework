namespace X3Platform.Web
{
    using System.Web;

    /// <summary>内容类型 工具类</summary>
    public class HttpContentTypeHelper
    {
        private const string headerToken = "x-platform-content-type";

        /// <summary>内部前缀</summary>
        private const string prefix = "application/x-platform-";

        /// <summary>设置自定义内容类型的值</summary>
        public static void SetValue(string value)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[headerToken];

            if (cookie == null)
            {
                cookie = new HttpCookie(headerToken, prefix + value);

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = prefix + value;
            }
        }

        /// <summary>获取自定义内容类型的值</summary>
        /// <returns></returns>
        public static string GetValue(bool destroy)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[headerToken];

            if (cookie == null)
            {
                return HttpContext.Current.Response.ContentType;
            }
            else
            {
                string value = cookie.Value;

                if (destroy)
                {
                    HttpContext.Current.Response.Cookies.Remove(headerToken);
                }

                if (string.IsNullOrEmpty(value))
                {
                    return HttpContext.Current.Response.ContentType;
                }

                value = value.Replace(prefix, string.Empty).ToLower();

                switch (value)
                {
                    case "html":
                        return "text/html";
                    case "xml":
                        return "text/xml";
                    case "css":
                        return "text/css";
                    case "javascrtipt":
                        return "application/x-javascrtipt";
                    case "zip":
                        return "application/x-zip-compressed";
                    case "json":
                        return "application/json";
                    default:
                        return "text/plain";
                }
            }
        }
    }
}
