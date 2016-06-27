namespace X3Platform.Web.APIs.Methods
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    using Util;
    #endregion

    /// <summary></summary>
    public class WebServiceMethod : AbstractMethod
    {
        /// <summary>目标对象</summary>
        protected object target;
        /// <summary>类名称</summary>
        protected string url;
        /// <summary>方法名称</summary>
        protected string methodName;
        /// <summary>方法参数类型</summary>
        protected string paramType = "xml";
        /// <summary>方法参数数量</summary>
        protected int paramCount = -1;
        /// <summary>方法返回类型</summary>
        protected string returnType = "string";

        /// <summary>构造函数</summary>
        public WebServiceMethod()
        {
        }

        /// <summary>构造函数</summary>
        /// <param name="options">选项</param>
        /// <param name="doc">Xml文档</param>
        public WebServiceMethod(Dictionary<string, string> options, XmlDocument doc)
        {
            this.options = options;

            this.doc = doc;

            this.url = this.options["url"];

            this.methodName = this.options["methodName"];

            if (this.options.ContainsKey("paramType"))
            {
                this.paramType = this.options["paramType"];
            }

            if (this.options.ContainsKey("paramCount"))
            {
                this.paramCount = Convert.ToInt32(this.options["paramCount"]);
            }

            if (this.options.ContainsKey("returnType"))
            {
                this.returnType = this.options["returnType"];
            }
        }

        /// <summary>执行</summary>
        /// <returns></returns>
        public override object Execute()
        {
            // 验证必填参数
            Validate();

            // 设置映射参数
            Mapping();

            if (paramType == "xml")
            {
                // 执行方法
                return WebServiceHelper.Invoke(this.url, this.methodName, new object[] { doc });
            }
            else
            {
                // 构造参数
                List<object> list = new List<object>();

                XmlNodeList nodes = doc.DocumentElement.ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    if (node.Name == "query")
                        continue;

                    list.Add(node.InnerText);

                    if (paramCount != -1 && list.Count >= paramCount) { break; }
                }

                if (paramCount == 0)
                {
                    list.Clear();
                }

                if (this.returnType == "bool")
                {
                    list.Add(false);
                    list.Add(false);

                    object[] args = list.ToArray();

                    // 执行方法
                    WebServiceHelper.Invoke(this.url, this.methodName, args);

                    return args[args.Length - 2];
                }
                else
                {
                    // 执行方法
                    return WebServiceHelper.Invoke(this.url, this.methodName, list.ToArray());
                }
            }
        }
    }
}