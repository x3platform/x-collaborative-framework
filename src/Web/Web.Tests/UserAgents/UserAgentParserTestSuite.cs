using System;
using System.Xml;
using System.Resources;
using System.Reflection;

using Common.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Util;
using X3Platform.Web.APIs.Configuration;
using X3Platform.Web.UserAgents;

namespace X3Platform.Web.Tests.UserAgents
{
    /// <summary>字符资源加载工具类</summary>
    [TestClass]
    public class ParserTestSuite
    {
        /// <summary>测试文件</summary>
        [TestMethod]
        public void TestLoad()
        {
            var parser = X3Platform.Web.UserAgents.Parser.GetDefault();

            ClientInfo client = null;

            client = parser.Parse("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0");

            Assert.AreEqual(client.OS.Family, "Windows 7");
            Assert.AreEqual(client.UserAgent.Family, "Firefox");

            // iPhone 4
            client = parser.Parse("Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_2_1 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8C148 Safari/6533.18.5");

            Assert.AreEqual(client.OS.Family, "iOS");
            Assert.AreEqual(client.UserAgent.Family, "Mobile Safari");

            // iPad 2
            client = parser.Parse("Mozilla/5.0 (iPad; CPU OS 4_3_5 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8L1 Safari/6533.18.5");

            Assert.AreEqual(client.OS.Family, "iOS");
            Assert.AreEqual(client.UserAgent.Family, "Mobile Safari");

            // Google Nexus 7
            client = parser.Parse("Mozilla/5.0 (Linux; Android 4.3; Nexus 7 Build/JSS15Q) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.72 Safari/537.36");

            Assert.AreEqual(client.OS.Family, "Android");
            Assert.AreEqual(client.UserAgent.Family, "Chrome");

            // Samsung Galaxy S4
            client = parser.Parse("Mozilla/5.0 (Linux; Android 4.2.2; GT-I9505 Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.59 Mobile Safari/537.36");

            Assert.AreEqual(client.OS.Family, "Android");
            Assert.AreEqual(client.UserAgent.Family, "Chrome Mobile");

            // Amazon Kindle Fire HDX 8.9"
            client = parser.Parse("Mozilla/5.0 (Linux; U; en-us; KFAPWI Build/JDQ39) AppleWebKit/535.19 (KHTML, like Gecko) Silk/3.13 Safari/535.19 Silk-Accelerated=true");

            Assert.AreEqual(client.Device.Family, "Android");
            Assert.AreEqual(client.OS.Family, "Android");
            Assert.AreEqual(client.UserAgent.Family, "Amazon Silk");
        }
    }
}