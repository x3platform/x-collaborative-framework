namespace X3Platform.Tests.Security.VerificationCode
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Security.VerificationCode;

    /// <summary>测试权限功能</summary>
    [TestClass]
    public class VerificationCodeContextTests
    {
        /// <summary>验证配置文档加载</summary>
        [TestMethod]
        public void TestLoad()
        {
            Assert.IsNotNull(VerificationCodeContext.Instance.VerificationCodeService);
        }
    }
}
