namespace X3Platform.Web.APIs.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;

    using Common.Logging;

    using X3Platform.Apps;
    using X3Platform.Configuration;
    using X3Platform.Connect;
    using X3Platform.Connect.Model;

    using X3Platform.Web.APIs.Configuration;
    using X3Platform.Web.APIs.Methods;
    using X3Platform.Security;
    using X3Platform.Apps.Model;
    using X3Platform.Json;
    using System.Text;
    using Globalization;
    using X3Platform.Util;    
    
    /// <summary></summary>
    public sealed class APIController : Controller
    {
        /// <summary>日志记录器</summary>
        private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        /// <summary></summary>
        /// <param name="methodName"></param>
        /// <param name="rawInput"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Index(string methodName, string rawInput)
        {
            if (string.IsNullOrEmpty(methodName)) { return new EmptyResult(); }

            if (Request.Browser.Crawler)
            {
                KernelContext.Log.Info("crawler:" + Request.UserAgent);

                return Content(GenericException.Stringify(I18n.Exceptions["code_web_api_ban_search_engine_spider"],
                   I18n.Exceptions["text_web_api_ban_search_engine_spider"]));
            }

            // 限制 IP 访问频次 两个小时 500 次
            if (HttpRequestLimit.LimitIP())
            {
                return Content(GenericException.Stringify(I18n.Exceptions["code_web_api_request_exceed_limit"],
                   I18n.Exceptions["text_web_api_request_exceed_limit"]));
            }

            DateTime timestamp = DateTime.Now;

            HttpContextBase context = this.HttpContext;

            IDictionary<string, APIMethod> dictionary = APIsConfigurationView.Instance.Configuration.APIMethods;

            string responseText = string.Empty;

            // 支持两种格式 connect/auth/authorize 和 connect.auth.authorize, 内部统一使用 connect.auth.authorize 格式
            if (methodName.IndexOf("/") > -1)
            {
                methodName = methodName.Replace("/", ".");
            }

            // 调试情况下记录输入参数
            if (context.Request.QueryString["xhr-debug"] == "1")
            {
                logger.Info("api:" + methodName + " start.");

                logger.Info(RequestHelper.Dump(context.Request, rawInput));
            }

            if (dictionary.ContainsKey(methodName))
            {
                // 优先执行 WebAPI 配置文件中设置的方法.
                responseText = APIHub.ProcessRequest(context, methodName, rawInput, logger, APIMethodInvoke);
            }
            else
            {
                // 匿名方法允许用户跳过验证直接访问
                // 应用方法信息
                ApplicationMethodInfo method = AppsContext.Instance.ApplicationMethodService.FindOneByName(methodName);

                if (method == null)
                {
                    logger.Warn(string.Format(I18n.Exceptions["text_web_api_method_not_exists"], methodName));

                    responseText = GenericException.Stringify(I18n.Exceptions["code_web_api_method_not_exists"],
                       string.Format(I18n.Exceptions["text_web_api_method_not_exists"], methodName));
                }
                else if (method.EffectScope == 1 || Authenticate(context, methodName))
                {
                    // 直接执行匿名方法 或者 验证需要身份验证方法

                    // 尝试执行 Application Method 中设置的方法.
                    responseText = APIHub.ProcessRequest(context, methodName, rawInput, logger, MethodInvoker.Invoke);
                }
                else
                {
                    responseText = "401";
                }
            }

            // 调试情况下记录输出参数
            if (context.Request.QueryString["xhr-debug"] == "1")
            {
                KernelContext.Log.Info("api " + methodName + " finished, timespan:" + DateHelper.GetTimeSpan(timestamp).TotalSeconds + "s.");

                KernelContext.Log.Info(responseText);
            }

            return Content(responseText);
        }

        /// <summary>验证请求合法性</summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool Authenticate(HttpContextBase context, string method)
        {
            // 获取客户端签名 clientId 和 clientSecret 或 clientId, clientSignature, timestamp, nonce

            string clientId = RequestHelper.Fetch(context.Request, "clientId", "client_id");
            string clientSecret = RequestHelper.Fetch(context.Request, "clientSecret", "client_secret");

            string clientSignature = RequestHelper.Fetch(context.Request, "clientSignature", "client_signature");
            string timestamp = context.Request["timestamp"] == null ? string.Empty : context.Request["timestamp"];
            string nonce = context.Request["nonce"] == null ? string.Empty : context.Request["nonce"];
            
            string accessToken = RequestHelper.Fetch(context.Request, "accessToken", "access_token");

            // 验证权限
            bool allowAccess = false;

            if (!string.IsNullOrEmpty(accessToken) && ConnectContext.Instance.ConnectAccessTokenService.IsExist(accessToken))
            {
                // 验证会话
                allowAccess = true;
            }
            else if (!string.IsNullOrEmpty(clientId))
            {
                // 1.内部应用
                if (clientId == KernelConfigurationView.Instance.ApplicationClientId)
                {
                    if (!string.IsNullOrEmpty(clientSignature) && !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(nonce))
                    {
                        var signature = Encrypter.EncryptSHA1(Encrypter.SortAndConcat(
                            KernelConfigurationView.Instance.ApplicationClientSecret, timestamp, nonce));

                        if (clientSignature == signature)
                        {
                            allowAccess = true;
                        }
                    }
                }
                else
                {
                    // 2.第三方应用连接
                    ConnectInfo connect = ConnectContext.Instance.ConnectService[clientId];

                    if (connect == null)
                    {
                        allowAccess = false;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(clientSignature) && !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(nonce))
                        {
                            // 加密方式签名

                            var signature = Encrypter.EncryptSHA1(Encrypter.SortAndConcat(connect.AppSecret, timestamp, nonce));

                            if (clientSignature == signature)
                            {
                                allowAccess = true;
                            }
                        }
                        else if (!string.IsNullOrEmpty(clientSecret) && connect.AppSecret == clientSecret)
                        {
                            // 明文客户端密钥

                            allowAccess = true;
                        }
                        else
                        {
                            allowAccess = false;
                        }
                    }
                }
            }

            return allowAccess;
        }

        private object APIMethodInvoke(string methodName, XmlDocument doc, ILog logger)
        {
            IDictionary<string, APIMethod> dictionary = APIsConfigurationView.Instance.Configuration.APIMethods;

            if (dictionary.ContainsKey(methodName))
            {
                if (string.IsNullOrEmpty(dictionary[methodName].ClassName))
                {
                    logger.Info("method:" + methodName + " className is null.");

                    return null;
                }

                Type type = Type.GetType(dictionary[methodName].ClassName);

                if (type == null)
                {
                    logger.Info("method:" + methodName + " can't find " + dictionary[methodName].ClassName + ".");
                    return null;
                }

                var target = Activator.CreateInstance(type);

                return type.InvokeMember(dictionary[methodName].MethodName, BindingFlags.InvokeMethod, null, target, new object[] { doc });
            }
            else
            {
                return null;
            }
        }
        
        ///// <summary>统一格式化参数</summary>
        ///// <returns></returns>
        //private XmlElement CreateXmlElement(XmlDocument doc, string name)
        //{
        //    if (name == "client_id")
        //    {
        //        return doc.CreateElement("clientId");
        //    }
        //    else if (name == "client_secret")
        //    {
        //        return doc.CreateElement("clientSecret");
        //    }
        //    else if (name == "access_token")
        //    {
        //        return doc.CreateElement("accessToken");
        //    }
        //    else
        //    {
        //        return doc.CreateElement(name);
        //    }
        //}
    }
}
