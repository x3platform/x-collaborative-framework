namespace X3Platform.Web.APIs.Configuration
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;
    
    using X3Platform.Collections;
    using X3Platform.Configuration;
    
    /// <summary>APIs 配置</summary>
    public class APIsConfiguration : XmlConfiguraton
    {
        /// <summary>配置区的名称</summary>
        public const string SectionName = "web.apis";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 属性:API 方法
        private IDictionary<string, APIMethod> m_APIMethods = null;

        /// <summary>API 方法</summary>
        public IDictionary<string, APIMethod> APIMethods
        {
            get { return this.m_APIMethods; }
        }
        #endregion

        #region 函数:Configure(XmlElement element)
        /// <summary>根据Xml元素配置对象信息</summary>
        /// <param name="element">配置节点的Xml元素</param>
        public override void Configure(XmlElement element)
        {
            if (this.m_APIMethods == null)
            {
                this.m_APIMethods = new Dictionary<string, APIMethod>();
            }

            // 加载 Methods 配置信息
            XmlNodeList nodes = element.SelectNodes(@"api");

            foreach (XmlNode node in nodes)
            {
                APIMethod apiMethod = new APIMethod();

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.NodeType == XmlNodeType.Element)
                    {
                        XmlElement childElement = (XmlElement)childNode;

                        XmlConfiguratonOperator.SetParameter(apiMethod, (XmlElement)childElement);
                    }
                }

                if (this.m_APIMethods.ContainsKey(apiMethod.Name))
                {
                    this.m_APIMethods[apiMethod.Name] = apiMethod;
                }
                else
                {
                    this.m_APIMethods.Add(apiMethod.Name, apiMethod);
                }
            }
        }
        #endregion
    }
}
