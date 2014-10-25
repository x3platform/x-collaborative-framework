namespace X3Platform
{
    #region Using Libraries
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
    #endregion

    /// <summary>运行环境处理器抽象类</summary>
    public abstract class ContextWrapper : IHttpHandler, IRequiresSessionState, IContextWrapper
    {
        /// <summary>返回类型</summary>
        private string resultType = "xml";

        #region 属性:IsReusable
        /// <summary>可再用的</summary>
        public bool IsReusable
        {
            get { return true; }
        }
        #endregion

        #region 函数:ProcessRequest(HttpContext context)
        /// <summary>处理请求</summary>
        /// <param name="context">上下文环境</param>
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

                    string action = XmlHelper.Fetch("action", doc);

                    string clientTargetObject = XmlHelper.Fetch("clientTargetObject", doc);

                    string xslt = XmlHelper.Fetch("xslt", doc);

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

            // 输出信息
            this.Export(context, this.resultType, outString, args);
        }
        #endregion

        #region 函数:Export(HttpContext context, string resultType, string responseText, Hashtable agrs)
        /// <summary>输出</summary>
        /// <param name="context">上下文环境</param>
        /// <param name="resultType">输出结果</param>
        /// <param name="responseText">响应的文本信息</param>
        /// <param name="agrs">扩展参数</param>
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

        #region 函数:ParseXml(string json)
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