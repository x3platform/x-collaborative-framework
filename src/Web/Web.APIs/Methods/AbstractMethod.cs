namespace X3Platform.Web.APIs.Methods
{
    using Globalization;
    using Json;
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary>抽象方法</summary>
    public abstract class AbstractMethod : IMethod
    {
        /// <summary>选项</summary>
        protected Dictionary<string, string> options;
        /// <summary>Xml文档</summary>
        protected XmlDocument doc;

        /// <summary>构造函数</summary>
        public AbstractMethod()
        {
        }

        /// <summary>构造函数</summary>
        /// <param name="options">选项</param>
        public AbstractMethod(Dictionary<string, string> options, XmlDocument doc)
        {
            this.options = options;
            this.doc = doc;
        }

        /// <summary>映射参数</summary>
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
                        XmlElement mappingNode = doc.CreateElement(data[node.Name].ToString());

                        mappingNode.SetAttribute("originalName", node.Name);

                        mappingNode.InnerXml = node.InnerXml;

                        doc.DocumentElement.InsertBefore(mappingNode, node);

                        doc.DocumentElement.RemoveChild(node);
                    }
                }
            }
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

                        foreach (XmlElement node in nodes)
                        {
                            if ((node.Name == paramName || node.GetAttribute("originalName") == paramName)
                                && !string.IsNullOrEmpty(node.InnerText))
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

        /// <summary>执行</summary>
        /// <returns></returns>
        public abstract object Execute();
    }
}