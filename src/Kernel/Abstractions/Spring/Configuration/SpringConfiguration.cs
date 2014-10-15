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
#endregion

namespace X3Platform.Spring.Configuration
{
    /// <summary>Spring������Ϣ</summary>
    public class SpringConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>Name of the WorkflowPlus configuration section.</summary>
        public const string SectionName = "spring";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        private NameValueConfigurationCollection m_Files = new NameValueConfigurationCollection();

        /// <summary>�����ļ�</summary>
        public NameValueConfigurationCollection Files
        {
            get { return this.m_Files; }
        }

        #region 属性:Configure(XmlElement element)
        /// <summary>����XmlԪ�����ö�����Ϣ</summary>
        /// <param name="element">���ýڵ���XmlԪ��</param>
        public override void Configure(XmlElement element)
        {
            try
            {
                base.Configure(element);

                // ���ؼ���:Files
                XmlConfiguratonOperator.SetKeyValues(this.Files, element.SelectNodes(@"files/add"));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion
    }
}
