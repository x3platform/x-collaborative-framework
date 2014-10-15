
#region Using Libraries
using System;
using System.IO;
using System.Text;
using System.Xml;

using Common.Logging;

using X3Platform.Configuration;
#endregion

namespace X3Platform.Services.Configuration
{
    /// <summary>������Ϣ</summary>
    public class ServicesConfiguration : XmlConfiguraton
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Services";

        /// <summary>������������</summary>
        public const string SectionName = "servicesConfiguration";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        private const string observersProperty = "observers";

        private NameValueConfigurationCollection m_Keys = new NameValueConfigurationCollection();

        /// <summary>�ؼ���</summary>
        public NameValueConfigurationCollection Keys
        {
            get { return this.m_Keys; }
            set { this.m_Keys = value; }
        }

        private NamedConfigurationElementCollection<NameTypeConfigurationElement> m_Services = new NamedConfigurationElementCollection<NameTypeConfigurationElement>();

        /// <summary>������Ϣ</summary>
        public NamedConfigurationElementCollection<NameTypeConfigurationElement> Services
        {
            get { return this.m_Services; }
            set { this.m_Services = value; }
        }

        private NamedConfigurationElementCollection<ServiceObserverConfiguration> m_Observers = new NamedConfigurationElementCollection<ServiceObserverConfiguration>();

        public NamedConfigurationElementCollection<ServiceObserverConfiguration> Observers
        {
            get { return this.m_Observers; }
        }

        public ServicesConfiguration Clone()
        {
            ServicesConfiguration configuration = new ServicesConfiguration();

            //
            // ���л���Ϣ
            //

            StringBuilder outString = new StringBuilder();

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();

            xmlWriterSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outString, xmlWriterSettings))
            {
                // WriteXml(writer);
                writer.Flush();

                writer.Close();
            }

            //
            // ��ֵ
            //

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();

            xmlReaderSettings.CloseInput = true;

            using (StringReader sr = new StringReader(outString.ToString()))
            {
                using (XmlReader reader = XmlReader.Create(sr, xmlReaderSettings))
                {
                    // configuration.ReadXml(reader);
                    reader.Close();
                }

                sr.Close();
            }

            return configuration;
        }
    }
}
