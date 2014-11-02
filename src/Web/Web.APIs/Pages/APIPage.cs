namespace X3Platform.Web.APIs.Pages
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Xml;
    using System.Threading;
    using System.Reflection;

    using Common.Logging;

    using X3Platform.Ajax;
    using X3Platform.Apps;
    using X3Platform.Configuration;
    using X3Platform.Connect;
    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.Location.IPQuery;
    using X3Platform.Util;
    using X3Platform.Security;

    /// <summary></summary>
    public sealed class APIPage : Page
    {
        /// <summary>日志记录器</summary>
        private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        /// <summary></summary>
        public override void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;

            // 示例: /api/application.method.hi.aspx

            // 获取客户端签名 clientId 和 clientSecret 或 clientId, clientSignature, timestamp, nonce

            string clientId = this.TryFetchRequstValue(context, "clientId", "client_id");
            string clientSecret = this.TryFetchRequstValue(context, "clientSecret", "client_secret");

            string clientSignature = this.TryFetchRequstValue(context, "clientSignature", "client_signature");
            string timestamp = context.Request["timestamp"] == null ? string.Empty : context.Request["timestamp"];
            string nonce = context.Request["nonce"] == null ? string.Empty : context.Request["nonce"];

            string accessToken = this.TryFetchRequstValue(context, "accessToken", "access_token");

            string name = context.Request.QueryString["name"];

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
                    else if (name == "connect.auth.authorize" || name == "connect.auth.token" || name == "connect.auth.callback" || name == "connect.oauth2.authorize" || name == "connect.oauth2.token" || name == "connect.oauth2.callback" || name == "session.me")
                    {
                        // 3.如果以上场景都不是，确认是否是用户登录验证的方法
                        allowAccess = true;
                    }
                    else
                    {
                        allowAccess = false;
                    }
                }
            }
            else if (name == "membership.member.login" || name == "session.me")
            {
                // 3.如果以上场景都不是，确认是否是用户登录验证的方法
                allowAccess = true;
            }

            if (!allowAccess)
            {
                ApplicationError.Write(401);
            }

            string xml = (context.Request.Form["xhr-xml"] == null) ? string.Empty : context.Request.Form["xhr-xml"];

            if (!string.IsNullOrEmpty(name) && (!string.IsNullOrEmpty(xml) || context.Request.QueryString.Count > 1))
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

                // 将 QueryString 中，除 xhr-name 外的所有参数转为统一的Xml文档的数据
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

                // 将表单中，除 xhr-name 和 xhr-xml 外的所有参数转为统一的Xml文档的数据
                if (context.Request.HttpMethod == "POST" && context.Request.Form.Count > 1)
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

                string clientTargetObject = XmlHelper.Fetch("clientTargetObject", doc);

                string resultType = (context.Request.Form["resultType"] == null) ? "json" : context.Request.Form["resultType"];

                // 设置输出的内容类型，默认为 json 格式。
                HttpContentTypeHelper.SetValue(resultType);

                try
                {
                    // 记录
                    if (ConnectConfigurationView.Instance.TrackingCall == "ON")
                    {
                        ConnectCallInfo call = new ConnectCallInfo(clientId, context.Request.RawUrl, doc.InnerXml);

                        try
                        {
                            call.Start();

                            responseText = X3Platform.Web.APIs.Methods.MethodInvoker.Invoke(name, doc, logger);

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
                        responseText = X3Platform.Web.APIs.Methods.MethodInvoker.Invoke(name, doc, logger);
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

                HttpContext.Current.Response.ContentType = HttpContentTypeHelper.GetValue(true);

                HttpContext.Current.Response.Write(responseText);
                HttpContext.Current.Response.End();
            }
        }

        /// <summary></summary>
        /// <returns></returns>
        private string TryFetchRequstValue(HttpContext context, string defaultName, string alias)
        {
            return this.TryFetchRequstValue(context, defaultName, new string[] { alias });
        }

        /// <summary></summary>
        /// <returns></returns>
        private string TryFetchRequstValue(HttpContext context, string defaultName, string[] alias)
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

        /// <summary>ͳһ��ʽ������</summary>
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