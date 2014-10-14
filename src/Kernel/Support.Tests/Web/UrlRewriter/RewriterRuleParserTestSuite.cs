using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using X3Platform.Util;
using X3Platform.Web.UrlRewriter.Configuration;
using X3Platform.Web.UrlRewriter;

namespace X3Platform.Tests.UrlRewriter.Configuration
{
    /// <summary>测试地址重写的配置视图</summary>
    [TestClass]
    public class RewriterRuleParserTestSuite
    {
        [TestInitialize()]
        public void Initialize()
        {
            string fullConfigPath = @"E:\Workspace\X3Platform\trunk\Kernel\Kernel\config\X3Platform.Web.UrlRewriter.config";

            RewriterConfigurationView.LoadInstance(fullConfigPath);
        }

        /// <summary>测试文件</summary>
        [TestMethod]
        public void TestParse()
        {
            // string url = RewriterRuleParser.Parse("/sale/default.aspx", "/");
        }
    }
}
