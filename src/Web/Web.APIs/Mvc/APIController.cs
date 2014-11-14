namespace X3Platform.Web.APIs.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    using System.Web;
    using System.Web.Mvc;

    using Common.Logging;

    using X3Platform.Apps;
    using X3Platform.Configuration;
    using X3Platform.Connect;
    using X3Platform.Connect.Model;

    using X3Platform.Web.APIs.Configuration;
    using X3Platform.Web.APIs.Methods;
    using X3Platform.Security;

    /// <summary></summary>
    public sealed class APIController : Controller
    {
        /// <summary>日志记录器</summary>
        private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        /// <summary></summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Index(string methodName)
        {
            if (string.IsNullOrEmpty(methodName)) { return new EmptyResult(); }

            IDictionary<string, APIMethod> dictionary = APIsConfigurationView.Instance.Configuration.APIMethods;

            HttpContextBase context = this.HttpContext;

            string responseText = string.Empty;

            if (logger.IsDebugEnabled)
            {
                logger.Debug("methodName:" + methodName);
            }

            if (dictionary.ContainsKey(methodName))
            {
                // 优先执行 WebAPI 配置文件中设置的方法.
                responseText = APIHub.ProcessRequest(context, methodName, logger, APIMethodInvoke);
            }
            else
            {
                // 尝试执行 Application Method 中设置的方法.
                if (Authenticate(context))
                {
                    responseText = APIHub.ProcessRequest(context, methodName, logger, MethodInvoker.Invoke);
                }
                else
                {
                    responseText = "401";
                }
            }

            //try
            //{
            //    if (Response.StatusCode == 200)
            //    {
            //        Response.ContentType = HttpContentTypeHelper.GetValue(true);
            //    }
            //}
            //catch
            //{
            //}

            return Content(responseText);
        }

        /// <summary>验证请求合法性</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authenticate(HttpContextBase context)
        {
            // 获取客户端签名 clientId 和 clientSecret 或 clientId, clientSignature, timestamp, nonce

            string clientId = this.TryFetchRequstValue(context, "clientId", "client_id");
            string clientSecret = this.TryFetchRequstValue(context, "clientSecret", "client_secret");

            string clientSignature = this.TryFetchRequstValue(context, "clientSignature", "client_signature");
            string timestamp = context.Request["timestamp"] == null ? string.Empty : context.Request["timestamp"];
            string nonce = context.Request["nonce"] == null ? string.Empty : context.Request["nonce"];

            string accessToken = this.TryFetchRequstValue(context, "accessToken", "access_token");

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

        /// <summary>处理请求</summary>
        private ActionResult ProcessRequest(HttpContextBase context, string methodName, ILog logger)
        {
            // 示例: /api/application.method.hi.aspx

            // 获取客户端签名 clientId 和 clientSecret

            string clientSignature = this.TryFetchRequstValue(context, "clientSignature", "client_signature");

            string clientId = this.TryFetchRequstValue(context, "clientId", "client_id");
            string clientSecret = this.TryFetchRequstValue(context, "clientSecret", "client_secret");
            string accessToken = this.TryFetchRequstValue(context, "accessToken", "access_token");

            // string name = methodName;

            // 验证权限
            bool allowAccess = false;

            if (!string.IsNullOrEmpty(accessToken) && ConnectContext.Instance.ConnectAccessTokenService.IsExist(accessToken))
            {
                // 验证会话
                allowAccess = true;
            }
            else if (!string.IsNullOrEmpty(clientId))
            {
                // 2.第三方应用连接
                ConnectInfo connect = ConnectContext.Instance.ConnectService[clientId];

                if (connect == null)
                {
                    clientId = string.Empty;
                    allowAccess = false;
                }
                else
                {
                    if (context.Request["APISessionId"] == context.Session.SessionID)
                    {
                        allowAccess = true;
                    }
                    else if (!string.IsNullOrEmpty(clientSecret) && connect.AppSecret == clientSecret)
                    {
                        allowAccess = true;
                    }
                    //else if (name == "connect.auth.authorize" || name == "connect.auth.token" || name == "connect.auth.callback" || name == "connect.oauth2.authorize" || name == "connect.oauth2.token" || name == "connect.oauth2.callback" || name == "session.me")
                    //{
                    //    // 3.如果以上场景都不是，确认是否是用户登录验证的方法
                    //    allowAccess = true;
                    //}
                    else
                    {
                        clientId = string.Empty;
                        allowAccess = false;
                    }
                }
            }
            //else if (name == "membership.member.login" || name == "session.me")
            //{
            //    // 3.如果以上场景都不是，确认是否是用户登录验证的方法
            //    allowAccess = true;
            //}

            if (!allowAccess)
            {
                ApplicationError.Write(401);
            }

            var responseText = APIHub.ProcessRequest(context, methodName, logger, X3Platform.Web.APIs.Methods.MethodInvoker.Invoke);

            /*
            string xml = (context.Request.Form["xhr-xml"] == null) ? string.Empty : context.Request.Form["xhr-xml"];

            if (!string.IsNullOrEmpty(xml) || context.Request.QueryString.Count > 1)
            {
                XmlDocument doc = new XmlDocument();

                if (string.IsNullOrEmpty(xml))
                {
                    doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<root></root>");
                }
                else
                {
                    doc.LoadXml(xml);
                }

                // 将 QueryString 中，除 name 外的所有参数转为统一的Xml文档的数据
                if (context.Request.QueryString.Count > 1)
                {
                    for (int i = 0; i < context.Request.QueryString.Count; i++)
                    {
                        if (context.Request.QueryString.Keys[i] == null) { continue; }

                        if (context.Request.QueryString.Keys[i] != "xhr-name")
                        {
                            XmlElement element = CreateXmlElement(doc, context.Request.QueryString.Keys[i]);

                            element.InnerText = context.Request.QueryString[i];

                            doc.DocumentElement.AppendChild(element);
                        }
                    }
                }

                if (context.Request.HttpMethod == "POST")
                {
                    // 将表单中，除 name 和 xml 外的所有参数转为统一的Xml文档的数据
                    if (context.Request.Form.Count > 1)
                    {
                        for (int i = 0; i < context.Request.Form.Count; i++)
                        {
                            if (context.Request.Form.Keys[i] == null) { continue; }

                            if (context.Request.Form.Keys[i] != "xhr-name" && context.Request.Form.Keys[i] != "xhr-xml")
                            {
                                XmlElement element = CreateXmlElement(doc, context.Request.Form.Keys[i]);

                                element.InnerText = context.Request.Form[i];

                                doc.DocumentElement.AppendChild(element);
                            }
                        }
                    }
                }

                string clientTargetObject = XmlHelper.Fetch("clientTargetObject", doc);

                string resultType = (context.Request.Form["xhr-resultType"] == null) ? "json" : context.Request.Form["xhr-resultType"];

                // 设置输出的内容类型，默认为 json 格式。
                HttpContentTypeHelper.SetValue(resultType);

                string responseText = string.Empty;

                try
                {
                    // 记录
                    if (ConnectConfigurationView.Instance.TrackingCall == "ON")
                    {
                        ConnectCallInfo call = new ConnectCallInfo(clientId, context.Request.RawUrl, doc.InnerXml);

                        try
                        {
                            call.Start();

                            responseText = X3Platform.Web.APIs.Methods.MethodInvoker.Invoke(methodName, doc, logger);

                            call.ReturnCode = 0;
                        }
                        catch
                        {
                            call.ReturnCode = 1;

                            throw;
                        }
                        finally
                        {
                            call.Finish();

                            call.IP = IPQueryContext.GetClientIP();

                            ConnectContext.Instance.ConnectCallService.Save(call);
                        }
                    }
                    else
                    {
                        responseText = X3Platform.Web.APIs.Methods.MethodInvoker.Invoke(methodName, doc, logger);
                    }

                    if (resultType == "json"
                        && responseText.IndexOf("\"message\":") > -1
                        && !string.IsNullOrEmpty(clientTargetObject))
                    {
                        responseText = responseText.Insert(responseText.IndexOf("\"message\":"), "\"clientTargetObject\":\"" + clientTargetObject + "\",");
                    }
                }
                catch (ThreadAbortException threadAbortException)
                {
                    responseText = "{\"message\":{"
                        + "\"returnCode\":\"2\","
                        + "\"category\":\"exception\","
                        + "\"value\":\"" + StringHelper.ToSafeJson(threadAbortException.Message) + "\","
                        + "\"description\":\"" + StringHelper.ToSafeJson(threadAbortException.ToString()) + "\""
                        + "}}";
                }
                catch (Exception generalException)
                {
                    responseText = "{\"message\":{"
                        + "\"returnCode\":\"1\","
                        + "\"category\":\"exception\","
                        + "\"value\":\"" + StringHelper.ToSafeJson(generalException.Message) + "\","
                        + "\"description\":\"" + StringHelper.ToSafeJson(generalException.ToString()) + "\""
                        + "}}";
                }

                context.Response.ContentType = HttpContentTypeHelper.GetValue(true);

                return Content(responseText);
            }

            return new EmptyResult();
            */
            context.Response.ContentType = HttpContentTypeHelper.GetValue(true);

            return Content(responseText);
        }

        /// <summary></summary>
        /// <returns></returns>
        private string TryFetchRequstValue(HttpContextBase context, string defaultName, string alias)
        {
            return this.TryFetchRequstValue(context, defaultName, new string[] { alias });
        }

        /// <summary></summary>
        /// <returns></returns>
        private string TryFetchRequstValue(HttpContextBase context, string defaultName, string[] alias)
        {
            // GET 方式
            string value = context.Request.QueryString[defaultName];

            // POST 方式
            if (string.IsNullOrEmpty(value))
            {
                value = context.Request.Form[defaultName];
            }

            if (string.IsNullOrEmpty(value))
            {
                // 其他兼容的名称
                foreach (string item in alias)
                {
                    if (!string.IsNullOrEmpty(context.Request.QueryString[item]))
                    {
                        return context.Request.QueryString[item];
                    }

                    if (!string.IsNullOrEmpty(context.Request.Form[item]))
                    {
                        return context.Request.Form[item];
                    }
                }

                return string.Empty;
            }
            else
            {
                return value;
            }
        }

        /// <summary>统一格式化参数</summary>
        /// <returns></returns>
        private XmlElement CreateXmlElement(XmlDocument doc, string name)
        {
            if (name == "client_id")
            {
                return doc.CreateElement("clientId");
            }
            else if (name == "client_secret")
            {
                return doc.CreateElement("clientSecret");
            }
            else if (name == "access_token")
            {
                return doc.CreateElement("accessToken");
            }
            else
            {
                return doc.CreateElement(name);
            }
        }
    }
}
