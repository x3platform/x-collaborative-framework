namespace X3Platform.Web
{
    using System;
    using System.Web;

    /// <summary>�û�������Ϣ</summary>
    [Obsolete("����ʹ��X3Platform.Web.UserAgnets.Parser����")]
    public sealed class HttpUserAgnetHelper
    {
        private HttpUserAgnetHelper() { }

        #region ��̬����:GetHardwareType()
        /// <summary>��ȡӲ������ PC Pad Moblie</summary>
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
