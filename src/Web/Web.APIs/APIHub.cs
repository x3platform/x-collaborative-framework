namespace X3Platform.Web.APIs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;
    using System.Xml;
    using System.Threading;

    using Common.Logging;

    using X3Platform.Util;
    using X3Platform.Connect.Configuration;
    using X3Platform.Location.IPQuery;
    using X3Platform.Connect;
    using X3Platform.Connect.Model;

    /// <summary></summary>
    /// <param name="methodName"></param>
    /// <param name="doc"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public delegate object APIInvokeDelegate(string methodName, XmlDocument doc, ILog logger);

    /// <summary></summary>
    sealed class APIHub
    {
        /// <summary>处理请求</summary>
        public static string ProcessRequest(HttpContextBase context, string methodName, ILog logger, APIInvokeDelegate methodInvoke)
        {
            // 请求响应的内容
            string responseText = string.Empty;

            string clientId = (context.Request["clientId"] == null) ? string.Empty : context.Request["clientId"];

            string accessToken = (context.Request["accessToken"] == null) ? string.Empty : context.Request["accessToken"];

            string xml = (context.Request.Form["xhr-xml"] == null) ? string.Empty : context.Request.Form["xhr-xml"];

            if (!string.IsNullOrEmpty(xml) || context.Request.QueryString.Count > 0 || (context.Request.HttpMethod == "POST" && context.Request.Form.Count > 0))
            {
                XmlDocument doc = new XmlDocument();

                if (string.IsNullOrEmpty(xml))
                {
                    doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<data></data>");
                }
                else
                {
                    doc.LoadXml(xml);
                }

                // 将 QueryString 中，除 xhr-name 外的所有参数转为统一的Xml文档的数据
                if (context.Request.QueryString.Count > 0)
                {
                    for (int i = 0; i < context.Request.QueryString.Count; i++)
                    {
                        if (string.IsNullOrEmpty(context.Request.QueryString.Keys[i])) { continue; }

                        if (context.Request.QueryString.Keys[i] != "xhr-name")
                        {
                            XmlElement element = doc.CreateElement(context.Request.QueryString.Keys[i]);

                            element.InnerText = context.Request.QueryString[i];

                            doc.DocumentElement.AppendChild(element);
                        }
                    }

                    doc = AnalyzePagingXml(doc, context.Request.QueryString);

                    doc = AnalyzeQueryXml(doc, context.Request.QueryString);
                }

                // 将表单中，除 xhr-name 和 xhr-xml 外的所有参数转为统一的Xml文档的数据         
                if (context.Request.HttpMethod == "POST" && context.Request.Form.Count > 0)
                {
                    for (int i = 0; i < context.Request.Form.Count; i++)
                    {
                        if (string.IsNullOrEmpty(context.Request.Form.Keys[i])) { continue; }

                        if (context.Request.Form.Keys[i] != "xhr-name" && context.Request.Form.Keys[i] != "xhr-xml")
                        {
                            XmlElement element = doc.CreateElement(context.Request.Form.Keys[i]);

                            element.InnerText = context.Request.Form[i];

                            doc.DocumentElement.AppendChild(element);
                        }
                    }

                    doc = AnalyzeQueryXml(doc, context.Request.QueryString);
                }

                string clientTargetObject = XmlHelper.Fetch("clientTargetObject", doc);

                string resultType = (context.Request.Form["xhr-resultType"] == null) ? "json" : context.Request.Form["xhr-resultType"];

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

                            var responseObject = methodInvoke(methodName, doc, logger);

                            responseText = (responseObject == null) ? string.Empty : responseObject.ToString();

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
                        var responseObject = methodInvoke(methodName, doc, logger);

                        responseText = (responseObject == null) ? string.Empty : responseObject.ToString();
                    }

                    if (resultType == "json"
                        && responseText.IndexOf("\"message\":") > -1
                        && !string.IsNullOrEmpty(clientTargetObject))
                    {
                        responseText = responseText.Insert(responseText.IndexOf("\"message\":"), "\"clientTargetObject\":\"" + clientTargetObject + "\",");
                    }
                }
                catch (GenericException genericException)
                {
                    responseText = genericException.ToString();
                }
                catch (ThreadAbortException threadAbortException)
                {
                    GenericException exception = new GenericException(0, 9999, threadAbortException);

                    responseText = exception.ToString();

                    //responseText = "{\"success\":0,\"msg\":\"" + StringHelper.ToSafeJson(threadAbortException.Message) + "\",\"message\":{"
                    //    + "\"returnCode\":\"9999\","
                    //    + "\"category\":\"exception\","
                    //    + "\"value\":\"" + StringHelper.ToSafeJson(threadAbortException.Message) + "\","
                    //    + "\"description\":\"" + StringHelper.ToSafeJson(threadAbortException.ToString()) + "\""
                    //    + "}}";
                }
                catch (Exception ex)
                {
                    GenericException exception = new GenericException(0, -1, ex);

                    responseText = exception.ToString();

                    //responseText = "{\"success\":0,\"msg\":\"" + StringHelper.ToSafeJson(ex.Message) + "\",\"message\":{"
                    //    + "\"returnCode\":\"-1\","
                    //    + "\"category\":\"exception\","
                    //    + "\"value\":\"" + StringHelper.ToSafeJson(ex.Message) + "\","
                    //    + "\"description\":\"" + StringHelper.ToSafeJson(ex.ToString()) + "\""
                    //    + "}}";
                }
            }

            // JSONP
            string callback = context.Request["callback"];

            return string.IsNullOrEmpty(callback)
                ? responseText
                : callback + "(" + responseText + ")";
        }

        /// <summary>解析分页参数</summary>
        /// <param name="doc"></param>
        /// <param name="nameValues"></param>
        /// <returns></returns>
        public static XmlDocument AnalyzePagingXml(XmlDocument doc, NameValueCollection nameValues)
        {
            // 必填项检测
            if (nameValues["page"] != null && nameValues["start"] != null && nameValues["limit"] != null)
            {
                XmlElement element = doc.CreateElement("paging");

                element.InnerXml = string.Format("<currentPage>{0}</currentPage><rowIndex>{1}</rowIndex><pageSize>{2}</pageSize>",
                    nameValues["page"],
                    nameValues["start"],
                    nameValues["limit"]);

                doc.DocumentElement.AppendChild(element);
            }

            return doc;
        }

        /// <summary>解析查询参数</summary>
        /// <param name="doc"></param>
        /// <param name="nameValues"></param>
        /// <returns></returns>
        public static XmlDocument AnalyzeQueryXml(XmlDocument doc, NameValueCollection nameValues)
        {
            XmlNode query = doc.DocumentElement.SelectSingleNode("query");

            if (query == null)
            {
                query = doc.CreateElement("query");

                doc.DocumentElement.AppendChild(query);
            }

            XmlNode where = query.SelectSingleNode("where");

            if (where == null)
            {
                where = doc.CreateElement("where");

                query.AppendChild(where);
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int i = 0; i < nameValues.Count; i++)
            {
                if (nameValues.Keys[i] == null) { continue; }

                if (nameValues.Keys[i].IndexOf("query-") == 0 && !string.IsNullOrEmpty(nameValues[i]))
                {
                    dictionary.Add(nameValues.Keys[i].Substring(6), nameValues[i]);
                }
            }

            foreach (KeyValuePair<string, string> item in dictionary)
            {
                XmlElement element = null;

                if (item.Key == "table")
                {
                    element = doc.CreateElement("table");

                    element.InnerText = item.Value;

                    query.AppendChild(element);
                }
                else if (item.Key == "fields")
                {
                    element = doc.CreateElement("fields");

                    element.InnerText = item.Value;

                    query.AppendChild(element);
                }
                else if (item.Key == "orders")
                {
                    element = doc.CreateElement("orders");

                    element.InnerText = item.Value;

                    query.AppendChild(element);
                }
                else if (item.Key.IndexOf("where-") == 0)
                {
                    string key = item.Key.Substring(6);

                    string name, type;

                    if (key.IndexOf("-") == -1)
                    {
                        name = key;
                        type = "string";
                    }
                    else
                    {
                        name = key.Split('-')[0];
                        type = key.Split('-')[1];
                    }

                    switch (type)
                    {
                        case "s":
                            type = "string";
                            break;
                        case "i":
                            type = "int";
                            break;
                        case "t":
                            type = "datetime";
                            break;
                        default:
                            break;
                    }

                    element = doc.CreateElement("key");

                    element.SetAttribute("name", name);
                    element.SetAttribute("type", type);
                    element.InnerText = item.Value;

                    where.AppendChild(element);
                }
            }

            return doc;
        }
    }
}