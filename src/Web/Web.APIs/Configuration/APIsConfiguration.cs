// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :RewriterConfiguration.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System.Xml;
using System.Xml.Serialization;

using X3Platform.Configuration;
using System.Collections.Generic;
using X3Platform.Collections;

namespace X3Platform.Web.APIs.Configuration
{
    /// <summary>APIs ������Ϣ</summary>
    public class APIsConfiguration : XmlConfiguraton
    {
        /// <summary>��������������</summary>
        public const string SectionName = "web.apis";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 属性:API ����
        private IDictionary<string, APIMethod> m_APIMethods = null;

        /// <summary>API ��������</summary>
        public IDictionary<string, APIMethod> APIMethods
        {
            get { return this.m_APIMethods; }
        }
        #endregion

        #region 属性:Configure(XmlElement element)
        /// <summary>����XmlԪ�����ö�����Ϣ</summary>
        /// <param name="element">���ýڵ���XmlԪ��</param>
        public override void Configure(XmlElement element)
        {
            if (this.m_APIMethods == null)
            {
                this.m_APIMethods = new Dictionary<string, APIMethod>();
            }

            // ���ؼ���:Methods
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
