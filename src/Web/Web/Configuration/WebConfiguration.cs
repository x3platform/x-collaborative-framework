#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :PagesConfiguration.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

#region Using Libraries
using System;
using System.Xml;

using Common.Logging;

using X3Platform.Configuration;
#endregion

namespace X3Platform.Web.Configuration
{
    /// <summary>Ȩ��������Ϣ</summary>
    public class WebConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Web";

        /// <summary>������������</summary>
        public const string SectionName = "webConfiguration";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 属性:Customize
        private NameValueConfigurationCollection m_Customize = new NameValueConfigurationCollection();

        /// <summary></summary>
        public NameValueConfigurationCollection Customize
        {
            get { return this.m_Customize; }
        }
        #endregion

        #region 属性:Menu
        private NameValueConfigurationCollection m_Menu = new NameValueConfigurationCollection();

        /// <summary></summary>
        public NameValueConfigurationCollection Menu
        {
            get { return this.m_Menu; }
        }
        #endregion

        #region 属性:Navigation
        private NameValueConfigurationCollection m_Navigation = new NameValueConfigurationCollection();

        /// <summary></summary>
        public NameValueConfigurationCollection Navigation
        {
            get { return this.m_Navigation; }
        }
        #endregion

        #region 属性:Configure(XmlElement element)
        /// <summary>����XmlԪ�����ö�����Ϣ</summary>
        /// <param name="element">���ýڵ���XmlԪ��</param>
        public override void Configure(XmlElement element)
        {
            base.Configure(element);

            // ���ؼ���:Navigation
            XmlConfiguratonOperator.SetKeyValues(this.Customize, element.SelectNodes(@"customize/add"));

            // ���ؼ���:Navigation
            XmlConfiguratonOperator.SetKeyValues(this.Menu, element.SelectNodes(@"menu/add"));

            // ���ؼ���:Navigation
            XmlConfiguratonOperator.SetKeyValues(this.Navigation, element.SelectNodes(@"navigation/add"));
        }
        #endregion
    }
}
