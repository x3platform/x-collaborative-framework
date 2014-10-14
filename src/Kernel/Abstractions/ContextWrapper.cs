// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :ContextWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.SessionState;
    using System.Xml;
    using System.Xml.Xsl;

    using X3Platform.Ajax;
    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>���л���������������</summary>
    public abstract class ContextWrapper : IHttpHandler, IRequiresSessionState, IContextWrapper
    {
        /// <summary>��������</summary>
        private string resultType = "xml";

        #region ����:IsReusable
        /// <summary>�����õ�</summary>
        public bool IsReusable
        {
            get { return true; }
        }
        #endregion

        #region ����:ProcessRequest(HttpContext context)
        /// <summary>��������</summary>
        /// <param name="context">�����Ļ���</param>
        public virtual void ProcessRequest(HttpContext context)
        {
            string outString = null;

            Hashtable args = new Hashtable();

            try
            {
                bool sleep = (context.Request.Form["sleep"] == null) ? false : true;

                if (sleep) { Thread.Sleep(3000); }

                string xml = (context.Request.Form["xml"] == null) ? string.Empty : context.Request.Form["xml"];

                if (!string.IsNullOrEmpty(xml))
                {
                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(xml);

                    string action = AjaxStorageConvertor.Fetch("action", doc);

                    string clientTargetObject = AjaxStorageConvertor.Fetch("clientTargetObject", doc);

                    string xslt = AjaxStorageConvertor.Fetch("xslt", doc);

                    args.Add("action", action);
                    args.Add("xslt", xslt);

                    if (!string.IsNullOrEmpty(action))
                    {
                        this.resultType = (context.Request.Form["resultType"] == null) ? "json" : context.Request.Form["resultType"];

                        outString = AjaxMethodParser.Parse(this, action, doc);

                        if (this.resultType == "json"
                            && outString.IndexOf("\"message\":") > -1
                            && !string.IsNullOrEmpty(clientTargetObject))
                        {
                            outString = outString.Insert(outString.IndexOf("\"message\":"), "\"clientTargetObject\":\"" + clientTargetObject + "\",");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.resultType = "json";

                ContextUnknownException exception = new ContextUnknownException("message", 1, ex);

                outString = exception.ToString();
            }

            // ������Ϣ
            this.Export(context, this.resultType, outString, args);
        }
        #endregion

        #region ����:Export(HttpContext context, string resultType, string responseText, Hashtable agrs)
        /// <summary>����</summary>
        /// <param name="context">�����Ļ���</param>
        /// <param name="resultType">��������</param>
        /// <param name="responseText">��Ӧ���ı���Ϣ</param>
        /// <param name="agrs">��չ����</param>
        public virtual void Export(HttpContext context, string resultType, string responseText, Hashtable agrs)
        {
            if (string.IsNullOrEmpty(responseText)) { return; }

            switch (this.resultType)
            {
                case "json":
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(responseText);
                    break;

                case "xml":
                    context.Response.ContentType = "text/xml";
                    context.Response.Write(this.ParseXml(responseText));
                    break;

                case "html":

                    XslCompiledTransform xslt = new XslCompiledTransform();

                    xslt.Load(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, agrs["xslt"].ToString()));

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        XmlDocument doc = new XmlDocument();

                        doc.LoadXml(this.ParseXml(responseText));

                        using (XmlWriter writer = new XmlTextWriter(memoryStream, Encoding.UTF8))
                        {
                            xslt.Transform(doc, writer);

                            writer.Close();
                        }

                        context.Response.ContentType = "text/html";
                        context.Response.Write(Encoding.UTF8.GetString(memoryStream.ToArray()));
                    }

                    break;
                default:
                    break;
            }
        }
        #endregion

        #region ����:ParseXml(string json)
        private string ParseXml(string json)
        {
            string message = string.Empty;

            string pages = string.Empty;

            string ajaxStorage = string.Empty;

            int index = 0;

            if (json.IndexOf("\"ajaxStorage\":") > 0)
            {
                index = json.IndexOf("\"message\":{");

                if (index > -1)
                {
                    message = "{" + json.Substring(index, json.Length - index - 1) + "}";

                    message = XmlHelper.ToXml(message);

                    json = json.Remove(index - 1);
                }

                index = json.IndexOf("\"pages\":{");

                if (index > -1)
                {
                    pages = "{" + json.Substring(index, json.Length - index) + "}";

                    pages = XmlHelper.ToXml(pages);

                    json = json.Remove(index - 1);
                }

                index = json.IndexOf("\"ajaxStorage\":");

                if (index > -1)
                {
                    ajaxStorage = "{" + json.Substring(index, json.Length - index) + "}";

                    ajaxStorage = XmlHelper.ToXml(ajaxStorage);
                }
            }

            return string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<response>{0}{1}{2}</response>", ajaxStorage, pages, message);
        }
        #endregion
    }
}