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
    /// <summary>字符资源加载工具类</summary>
    [TestClass]
    public class LoggingTestSuite
    {
        /// <summary>测试文件</summary>
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

        /// <summary>测试保存配置文件</summary>
        [TestMethod]
        public void TestSave()
        {
            // APIsConfiguration configuration = APIsConfigurationView.Instance.Configuration;

            // RewriterRule rule = new RewriterRule();

            // rule.Lookfor = "123";

            // rule.Sendto = "456";

            // rule.Remark = "测试信息";

            // configuration.Rules.Add(rule);

            // APIsConfigurationView.Instance.Save();
        }
    }
}