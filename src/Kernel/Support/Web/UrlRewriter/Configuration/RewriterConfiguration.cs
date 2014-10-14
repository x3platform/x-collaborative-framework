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


namespace X3Platform.Web.UrlRewriter.Configuration
{
    /// <summary>��ַ��д������Ϣ</summary>
    [XmlRoot("rewriter")]
    public class RewriterConfiguration
    {
        /// <summary>��������������</summary>
        public const string SectionName = "rewriter";

        private const string ruleProperty = "rule";

        private RewriterRuleCollection rules;

        #region ����:Rules
        /// <summary>��д��ַ�Ĺ���</summary>
        [XmlElement(ruleProperty)]
        public RewriterRuleCollection Rules
        {
            get { return rules; }
            set { rules = value; }
        }
        #endregion
    }
}
