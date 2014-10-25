using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Configuration;

namespace X3Platform.Tests.Configuration
{
    /// <summary>核心配置文件测试类</summary>
    [TestClass]
    public class PropertiesFileTestSuite
    {
        const string path = "E:\\Workspace\\X3Platform\\trunk\\Kernel\\Kernel.TestSuite\\debug.config";

        /// <summary>测试文件</summary>
        [TestMethod]
        public void TestLoad()
        {
            //
            // 1.测试读取测试配置
            //

            KernelConfiguration configuration = null;

            XmlDocument doc = new XmlDocument();

            XmlNode node;

            doc.Load(path);

            node = doc.GetElementsByTagName(KernelConfiguration.SectionName)[0];

            XmlSerializer ser = new XmlSerializer(typeof(KernelConfiguration));

            configuration = (KernelConfiguration)ser.Deserialize(new XmlNodeReader(node));

            Assert.IsNotNull(configuration);


            //
            // 2.测试读取默认应用配置
            //

            configuration = KernelConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
        }

        /// <summary></summary>
        [TestMethod]
        public void TestSave()
        {
            KernelConfiguration configuration = new KernelConfiguration();

            KernelConfigurationKey key1 = new KernelConfigurationKey("HostName", "http://www.kanf.cn");
            KernelConfigurationKey key2 = new KernelConfigurationKey("Version", "1.0.0.0");
            KernelConfigurationKey key3 = new KernelConfigurationKey("Author", "X3Platform");
            KernelConfigurationKey key4 = new KernelConfigurationKey("ApplicationPathRoot", "E:\\Workspace\\X3Platform\\branches\\1.0.0\\trunk\\Kernel\\Kernel.TestSuite\\");

            configuration.Keys.Add(key1);
            configuration.Keys.Add(key2);
            configuration.Keys.Add(key3);
            configuration.Keys.Add(key4);

            // KernelConfigurationView.Instance.Serialize(path, KernelConfiguration.SectionName, configuration);
        }
    }
}
