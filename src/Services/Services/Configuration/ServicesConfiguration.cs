
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
    /// <summary>配置信息</summary>
    public class ServicesConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Services";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "servicesConfiguration";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        private const string observersProperty = "observers";

        private NameValueConfigurationCollection m_Keys = new NameValueConfigurationCollection();

        /// <summary>关键词</summary>
        public NameValueConfigurationCollection Keys
        {
            get { return this.m_Keys; }
            set { this.m_Keys = value; }
        }

        private NamedConfigurationElementCollection<NameTypeConfigurationElement> m_Services = new NamedConfigurationElementCollection<NameTypeConfigurationElement>();

        /// <summary>服务信息</summary>
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
            // 序列化信息
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
            // 赋值
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
