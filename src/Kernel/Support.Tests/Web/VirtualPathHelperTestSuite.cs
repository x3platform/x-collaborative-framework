using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Util;
using X3Platform.Web;

namespace X3Platform.Tests.Web
{
    /// <summary>虚拟路径工具类</summary>
    [TestClass]
    public class VirtualPathHelperTestSuite
    {
        [TestMethod]
        public void TestCombine()
        {
            string url = string.Empty;

            url = VirtualPathHelper.Combine("http://www.kanf.cn","issue/");

            Assert.AreEqual(url,"http://www.kanf.cn/issue/");

            url = VirtualPathHelper.Combine("http://www.kanf.cn/", "issue/");

            Assert.AreEqual(url, "http://www.kanf.cn/issue/");

            url = VirtualPathHelper.Combine("http://www.kanf.cn/news", "issue");

            Assert.AreEqual(url, "http://www.kanf.cn/news/issue");
        }
    }
}
