using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Util;
using X3Platform.Web.UrlRewriter.Configuration;

namespace X3Platform.Tests.UrlRewriter.Configuration
{
    /// <summary>测试地址重写的配置视图</summary>
    [TestClass]
    public class UrlRewriterConfigurationViewTestSuite
    {
        public UrlRewriterConfigurationViewTestSuite()
        {
        }

        #region 其他测试属性
        //
        // 您可以在编写测试时使用下列其他属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前使用 TestInitialize 运行代码 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在运行每个测试之后使用 TestCleanup 运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        
        /// <summary>测试文件</summary>
        [TestMethod]
        public void TestLoad()
        {
            RewriterConfiguration configuration = RewriterConfigurationView.Instance.Configuration;

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
            RewriterConfiguration configuration = RewriterConfigurationView.Instance.Configuration;

            RewriterRule rule = new RewriterRule();

            rule.Lookfor = "123";
            
            rule.Sendto = "456";

            rule.Remark = "测试信息";
            
            configuration.Rules.Add(rule);

            RewriterConfigurationView.Instance.Save();
        }
    }
}
