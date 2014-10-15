
#region Using Libraries
using System;
using System.Collections.Generic;
using System.Xml;

using Common.Logging;

using X3Platform.Configuration;
#endregion

namespace X3Platform.Velocity.Configuration
{
    /// <summary>ģ�������������Ϣ</summary>
    public class VelocityConfiguration : XmlConfiguraton
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Velocity";

        /// <summary>�������������</summary>
        public const string SectionName = "velocity";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        private NameValueConfigurationCollection m_Keys = new NameValueConfigurationCollection();

        /// <summary>��ֵ������Ϣ</summary>
        public NameValueConfigurationCollection Keys
        {
            get { return this.m_Keys; }
            set { this.m_Keys = value; }
        }

        #region ����:Configure(XmlElement element)
        /// <summary>����XmlԪ�����ö�����Ϣ</summary>
        /// <param name="element">���ýڵ��XmlԪ��</param>
        public override void Configure(XmlElement element)
        {
            try
            {
                // ���ؼ���:Keys
                XmlNodeList nodes = element.SelectNodes(@"keys/add");

                foreach (XmlNode node in nodes)
                {
                    this.Keys.Add(new NameValueConfigurationElement(node.Attributes["name"].Value, node.Attributes["value"].Value));
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion
    }
}
