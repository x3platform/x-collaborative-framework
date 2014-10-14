namespace X3Platform.Web
{
    using System;
    using System.Web;

    /// <summary>用户代理信息</summary>
    [Obsolete("建议使用X3Platform.Web.UserAgnets.Parser代替")]
    public sealed class HttpUserAgnetHelper
    {
        private HttpUserAgnetHelper() { }

        #region 静态函数:GetHardwareType()
        /// <summary>获取硬件类型 PC Pad Moblie</summary>
        /// <returns></returns>
        public static string GetHardwareType()
        {
            // http://user-agent-string.info/

            string hardwareType = string.Empty;

            string httpUserAgnet = HttpContext.Current.Request.UserAgent;

            string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };

            if (httpUserAgnet.Contains("Windows NT") || httpUserAgnet.Contains("Mac OS X") || httpUserAgnet.Contains("Macintosh"))
            {
                hardwareType = "PC";
            }
            else
            {
                foreach (string keyword in keywords)
                {
                    if (httpUserAgnet.Contains(keyword))
                    {
                        hardwareType = "Moblie";
                        break;
                    }
                }
            }

            return hardwareType;
        }
        #endregion
    }
}
