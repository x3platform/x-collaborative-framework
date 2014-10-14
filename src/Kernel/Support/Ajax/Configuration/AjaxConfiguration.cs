// =============================================================================
//
// Copyright (c) x3platfrom.com
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

#region Using Libraries
using System;
using System.Xml;

using Common.Logging;

using X3Platform.Configuration;
using System.IO;
using X3Platform.Yaml.RepresentationModel;
#endregion

namespace X3Platform.Ajax.Configuration
{
    /// <summary>������Ϣ</summary>
    public class AjaxConfiguration : XmlConfiguraton
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Ajax";

        /// <summary>������������</summary>
        public const string SectionName = "ajax";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region ����:SpecialWords
        private NameValueConfigurationCollection m_SpecialWords = new NameValueConfigurationCollection();

        /// <summary>�����ʻ�</summary>
        public NameValueConfigurationCollection SpecialWords
        {
            get { return this.m_SpecialWords; }
            set { this.m_SpecialWords = value; }
        }
        #endregion

        #region ����:Configure(XmlElement element)
        /// <summary>����XmlԪ�����ö�����Ϣ</summary>
        /// <param name="element">���ýڵ���XmlԪ��</param>
        public override void Configure(XmlElement element)
        {
            base.Configure(element);

            // ���ؼ���:SpecialWords
            XmlConfiguratonOperator.SetKeyValues(this.SpecialWords, element.SelectNodes(@"specialWords/add"));
        }
        #endregion

        public AjaxConfiguration()
        {
            using (var stream = typeof(AjaxConfiguration).Assembly.GetManifestResourceStream("X3Platform.defaults.Ajax.yaml"))
            {
                using (var reader = new StreamReader(stream))
                {
                    // ��������
                    var yaml = new YamlStream();

                    yaml.Load(reader);

                    // ���ø��ڵ�
                    var root = (YamlMappingNode)yaml.Documents[0].RootNode;

                    // ���ؼ���:Keys
                    YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

                    // ���ؼ���:SpecialWords
                    YamlConfiguratonOperator.SetKeyValues(this.SpecialWords, (YamlMappingNode)root.Children[new YamlScalarNode("specialWords")]);
                }
            }

            this.Initialized = true;
        }
    }
}
