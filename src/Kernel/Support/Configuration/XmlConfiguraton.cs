namespace X3Platform.Configuration
{
    using System;
    using System.Configuration;
    using System.Xml;
    using System.IO;
    using System.Globalization;
    using System.Reflection;

    using Common.Logging;

    /// <summary>Xml������Ϣ</summary>
    public abstract class XmlConfiguraton
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>��ȡ������������</summary>
        public abstract string GetSectionName();

        #region ����:Initialized
        private bool m_Initialized = false;

        /// <summary>�Ƿ�ɹ���ʼ��</summary>
        /// <remarks>�������õ������ļ������ⲿ�������ļ��ɹ���ʼ��</remarks>
        public bool Initialized
        {
            set { this.m_Initialized = value; }
            get { return this.m_Initialized; }
        }
        #endregion

        #region ����:Keys
        private NameValueConfigurationCollection m_Keys = new NameValueConfigurationCollection();

        /// <summary>��ֵ������Ϣ</summary>
        public NameValueConfigurationCollection Keys
        {
            get { return this.m_Keys; }
        }
        #endregion

        #region ����:Configure(XmlElement element)
        /// <summary>����XmlԪ�����ö�����Ϣ</summary>
        /// <param name="element">���ýڵ��XmlԪ��</param>
        public virtual void Configure(XmlElement element)
        {
            try
            {
                // ���ؼ�ֵ������Ϣ
                XmlConfiguratonOperator.SetKeyValues(this.m_Keys, element.SelectNodes(@"keys/add"));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion

        #region ����:Configure(string configFilePath, string configNodeXPath)
        /// <summary>����XmlԪ�����ö�����Ϣ</summary>
        /// <param name="configFilePath">�����ļ�</param>
        /// <param name="configNodeXPath">���ýڵ�� XPath ���ʽ</param>
        public void Configure(string configFilePath, string configNodeXPath)
        {
            if (File.Exists(configFilePath))
            {
                // XmlDocument �� Load �����������ļ���Ϣ������ʹ�� XmlReader �������ͷ���Դ��
                using (XmlReader reader = XmlReader.Create(configFilePath))
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(reader);

                    XmlNode node = doc.SelectSingleNode(configNodeXPath);

                    if (node != null && node.NodeType == XmlNodeType.Element)
                    {
                        this.Configure((XmlElement)node);
                    }
                }
            }
        }
        #endregion
    }
}
