using System;
using System.Xml;
using System.Resources;
using System.Reflection;

using Common.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Util;
using X3Platform.Web.APIs.Configuration;

namespace X3Platform.Web.Tests.APIs.Configuration
{
    /// <summary>�ַ���Դ���ع�����</summary>
    [TestClass]
    public class LoggingTestSuite
    {
        /// <summary>�����ļ�</summary>
        [TestMethod]
        public void TestLoad()
        {
            APIsConfiguration configuration = APIsConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
        }

        [TestMethod]
        public void TestAdd()
        {
            //PluginsConfiguration configuration = PluginsConfigurationView.Instance.Configuration;

            //PluginDesigner designer = new PluginDesigner();

            //designer.Name = "test";

            //configuration.PluginDesigners.Add(designer);

            //PluginsConfigurationView.Instance.Save();
        }

        [TestMethod]
        public void TestRemove()
        {
            //PluginsConfiguration configuration = PluginsConfigurationView.Instance.Configuration;

            //PluginDesigner designer = new PluginDesigner();

            //designer.Name = "test";

            //configuration.PluginDesigners.Remove(designer);

            //PluginsConfigurationView.Instance.Save();
        }

        /// <summary>���Ա��������ļ�</summary>
        [TestMethod]
        public void TestSave()
        {
            // APIsConfiguration configuration = APIsConfigurationView.Instance.Configuration;

            // RewriterRule rule = new RewriterRule();

            // rule.Lookfor = "123";

            // rule.Sendto = "456";

            // rule.Remark = "������Ϣ";

            // configuration.Rules.Add(rule);

            // APIsConfigurationView.Instance.Save();
        }
    }
}