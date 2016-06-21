namespace X3Platform.Web.APIs.Methods
{
    using Globalization;
    using Json;
    using Messages;
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Xml;
    #endregion

    /// <summary></summary>
    public class GenericMethod : IMethod
    {
        /// <summary>选项</summary>
        protected Dictionary<string, string> options;
        /// <summary>Xml文档</summary>
        protected XmlDocument doc;
        /// <summary>目标对象</summary>
        protected object target;
        /// <summary>类名称</summary>
        protected string className;
        /// <summary>方法名称</summary>
        protected string methodName;

        /// <summary>构造函数</summary>
        public GenericMethod()
        {
        }

        /// <summary>构造函数</summary>
        /// <param name="options">选项</param>
        /// <param name="doc">Xml文档</param>
        public GenericMethod(Dictionary<string, string> options, XmlDocument doc)
        {
            this.options = options;

            this.doc = doc;

            this.className = this.options["className"];

            this.methodName = this.options["methodName"];
        }

        /// <summary>验证必填参数</summary>
        public virtual void Validate()
        {
            if (this.options.ContainsKey("requiredParams"))
            {
                string requiredParams = this.options["requiredParams"];

                if (!string.IsNullOrEmpty(requiredParams))
                {
                    string[] paramNames = requiredParams.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    XmlNodeList nodes = doc.DocumentElement.ChildNodes;

                    bool exists = false;

                    foreach (string paramName in paramNames)
                    {
                        exists = false;

                        foreach (XmlNode node in nodes)
                        {
                            if (node.Name == paramName && !string.IsNullOrEmpty(node.InnerText))
                            {
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)
                        {
                            throw new GenericException(I18n.Exceptions["code_kernel_param_is_required"],
                              string.Format(I18n.Exceptions["text_kernel_param_is_required"], paramName));
                        }
                    }
                }
            }
        }

        /// <summary>验证必填参数</summary>
        public virtual void Mapping()
        {
            if (this.options.ContainsKey("mappingParams"))
            {
                JsonData data = JsonMapper.ToObject(this.options["mappingParams"]);

                XmlNodeList nodes = doc.DocumentElement.ChildNodes;

                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];

                    if (data.Keys.Contains(nodes[i].Name))
                    {
                        // 创建节点
                        XmlNode mappingNode = doc.CreateElement(data[node.Name].ToString());

                        mappingNode.InnerXml = node.InnerXml;

                        doc.DocumentElement.InsertBefore(mappingNode, node);

                        doc.DocumentElement.RemoveChild(node);
                    }
                }
            }
        }

        /// <summary>执行</summary>
        /// <returns></returns>
        public virtual object Execute()
        {
            this.target = KernelContext.CreateObject(this.className);

            Type type = this.target.GetType();

            try
            {
                // 验证必填参数
                this.Validate();
            }
            catch (GenericException genericException)
            {
                return MessageObject.Stringify(genericException.ReturnCode, genericException.Message);
            }

            // 设置映射参数
            Mapping();

            // 执行方法
            return type.InvokeMember(this.methodName, BindingFlags.InvokeMethod, null, target, new object[] { doc });
        }
    }
}