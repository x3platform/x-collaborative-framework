namespace X3Platform.TestTools
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.Reflection;

    using Common.Logging;

    using X3Platform;
    using X3Platform.Ajax.Net;
    #endregion

    /// <summary>���Ը�������</summary>
    public sealed class TestSupport
    {
        private static string appKey = null;
        // Ӧ�ñ�ʶ
        public static string GetAppKey()
        {
            if (appKey == null)
            {
                appKey = ConfigurationManager.AppSettings["appKey"];
            }

            return appKey;
        }

        private static string appSecret = null;
        // Ӧ����Կ
        public static string GetAppSecret()
        {
            if (appSecret == null)
            {
                appSecret = ConfigurationManager.AppSettings["appSecret"];
            }

            return appSecret;
        }

        private static string apiHostPrefix = null;
        
        /// <summary>API����������ǰ׺</summary>
        /// <returns></returns>
        public static string GetApiHostPrefix()
        {
            if (apiHostPrefix == null)
            {
                apiHostPrefix = ConfigurationManager.AppSettings["apiHostPrefix"];
            }

            return apiHostPrefix;
        }

        /// <summary></summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Uri GetApiUri(string url)
        {
            return new Uri(TestSupport.GetApiHostPrefix() + url);
        }

        private static string accessToken = null;

        /// <summary></summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            if (accessToken == null)
            {
                accessToken = ConfigurationManager.AppSettings["accessToken"];
            }

            return accessToken;
        }

        /// <summary>
        /// XML HttpRequest
        /// </summary>
        /// <param name="reqeustData"></param>
        /// <returns></returns>
        public static string Xhr(AjaxRequestData reqeustData)
        {
            if (!reqeustData.Args.ContainsKey("accessToken"))
            {
                reqeustData.Args.Add("accessToken", TestSupport.GetAccessToken());
            }

            KernelContext.Log.Info("request :" + reqeustData.ActionUri);

            string responseText = AjaxRequest.Request(reqeustData);

            KernelContext.Log.Info("response:" + responseText);

            return responseText;
        }

        /// <summary>
        /// �޸��ʺ���Ϣ
        /// </summary>
        /// <param name="reqeustData"></param>
        /// <returns></returns>
        public static string ChangeAccount(string loginName)
        {
            return string.Empty;
        }
    }
}
