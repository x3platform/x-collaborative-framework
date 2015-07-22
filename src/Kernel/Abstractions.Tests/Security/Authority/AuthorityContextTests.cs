namespace X3Platform.Tests.Security.Authority
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Security.Authority;

    /// <summary>测试权限功能</summary>
    [TestClass]
    public class AuthorityContextTestSuite
    {
        /// <summary>验证配置文档加载</summary>
        [TestMethod]
        public void TestLoad()
        {
            Assert.IsNotNull(AuthorityContext.Instance.AuthorityService);
        }
    }
}
