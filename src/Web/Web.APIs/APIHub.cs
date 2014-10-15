// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Web.APIs
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Xml;
    using System.Threading;

    using X3Platform.Ajax;
    using X3Platform.Apps;
    using X3Platform.Configuration;
    using X3Platform.Connect;
    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.Location.IPQuery;
    using X3Platform.Util;
    using Common.Logging;
    using System.Reflection;
    using System.Collections.Generic;
    using X3Platform.Web.APIs.Configuration;
    using System.Collections.Specialized;

    /// <summary></summary>
    /// <param name="methodName"></param>
    /// <param name="doc"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public delegate object APIInvokeDelegate(string methodName, XmlDocument doc, ILog logger);

    /// <summary></summary>
    sealed class APIHub
    {
        /// <summary>��������</summary>
        public static string ProcessRequest(HttpContextBase context, string methodName, ILog logger, APIInvokeDelegate methodInvoke)
        {
            // ��Ӧ���ı���Ϣ
            string responseText = string.Empty;

            string xml = (context.Request.Form["xhr-xml"] == null) ? string.Empty : context.Request.Form["xhr-xml"];

            if (!string.IsNullOrEmpty(xml) || context.Request.QueryString.Count > 0 || (context.Request.HttpMethod == "POST" && context.Request.Form.Count > 0))
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

                // �� QueryString �У��� xhr-name �������в���תΪͳһ��Xml�ĵ�������
                if (context.Request.QueryString.Count > 0)
                {
                    for (int i = 0; i < context.Request.QueryString.Count; i++)
                    {
                        if (context.Request.QueryString.Keys[i] == null) { continue; }

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

                // �����У��� name �� xml �������в���תΪͳһ��Xml�ĵ�������                 
                if (context.Request.HttpMethod == "POST" && context.Request.Form.Count > 0)
                {
                    for (int i = 0; i < context.Request.Form.Count; i++)
                    {
                        if (context.Request.Form.Keys[i] == null) { continue; }

                        if (context.Request.Form.Keys[i] != "xhr-name" && context.Request.Form.Keys[i] != "xhr-xml")
                        {
                            XmlElement element = doc.CreateElement(context.Request.Form.Keys[i]);

                            element.InnerText = context.Request.Form[i];

                            doc.DocumentElement.AppendChild(element);
                        }
                    }

                    doc = AnalyzeQueryXml(doc, context.Request.QueryString);
                }

                string clientTargetObject = AjaxStorageConvertor.Fetch("clientTargetObject", doc);

                string resultType = (context.Request.Form["xhr-resultType"] == null) ? "json" : context.Request.Form["xhr-resultType"];

                // �����������������ͣ�Ĭ��Ϊ json ��ʽ��
                HttpContentTypeHelper.SetValue(resultType);

                try
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("responseText = Transact(" + methodName + ", doc)");
                    }

                    var responseObject = methodInvoke(methodName, doc, logger); ;

                    responseText = (responseObject == null) ? string.Empty : responseObject.ToString();

                    if (resultType == "json"
                        && responseText.IndexOf("\"message\":") > -1
                        && !string.IsNullOrEmpty(clientTargetObject))
                    {
                        responseText = responseText.Insert(responseText.IndexOf("\"message\":"), "\"clientTargetObject\":\"" + clientTargetObject + "\",");
                    }
                }
                catch (ThreadAbortException threadAbortException)
                {
                    responseText = "{\"success\":0,\"msg\":\"" + StringHelper.ToSafeJson(threadAbortException.Message) + "\",\"message\":{"
                        + "\"returnCode\":\"2\","
                        + "\"category\":\"exception\","
                        + "\"value\":\"" + StringHelper.ToSafeJson(threadAbortException.Message) + "\","
                        + "\"description\":\"" + StringHelper.ToSafeJson(threadAbortException.ToString()) + "\""
                        + "}}";
                }
                catch (Exception generalException)
                {
                    responseText = "{\"success\":0,\"msg\":\"" + StringHelper.ToSafeJson(generalException.Message) + "\",\"message\":{"
                        + "\"returnCode\":\"1\","
                        + "\"category\":\"exception\","
                        + "\"value\":\"" + StringHelper.ToSafeJson(generalException.Message) + "\","
                        + "\"description\":\"" + StringHelper.ToSafeJson(generalException.ToString()) + "\""
                        + "}}";
                }
            }

            return responseText;
        }

        /// <summary>������ѯ�������ص�����</summary>
        /// <param name="doc"></param>
        /// <param name="nameValues"></param>
        /// <returns></returns>
        public static XmlDocument AnalyzePagingXml(XmlDocument doc, NameValueCollection nameValues)
        {
            // ��ҳ����
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

        /// <summary>������ѯ�������ص�����</summary>
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