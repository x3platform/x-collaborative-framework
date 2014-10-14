using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Security;
using System.Security.Cryptography;
using X3Platform.Security.Authority;
using X3Platform.Membership;

namespace X3Platform.TestSuite.Security
{
    /// <summary>
    /// 加密解密测试
    /// </summary>
    [TestClass]
    public class SecurityClassTestSuite
    {
        /// <summary>测试 有权限的方法</summary>
        [TestMethod]
        public void TestSecurityMethod()
        {
            AuthorityTicketTestClass target = new AuthorityTicketTestClass();

            string result = string.Empty;

            result = target.Insert();
            result = target.Update();
            result = target.Delete();
        }

        /// <summary>测试 有权限的方法</summary>
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void TestUnSecurityMethod()
        {
            AuthorityTicketTestClass target = new AuthorityTicketTestClass();

            string result = string.Empty;

            result = target.Search();
        }
    }

    //
    // 测试类
    //

    //此属性是必须的 否则调用不会被捕获从而无法进行验证
    [SecurityClass]
    public sealed class AuthorityTicketTestClass : ContextBoundObject
    {

        [AuthorityMethod("添加")]//设置验证属性和所需要的权限，以下同
        public string Insert()
        {
            return "Insert成功";
        }

        [AuthorityMethod("修改")]
        public string Update()
        {
            return "Update成功";
        }

        [AuthorityMethod("删除")]
        public string Delete()
        {
            return "Delete成功";
        }

        [AuthorityMethod("查询")]
        public string Search()
        {
            return "Search成功";
        }
    }
}
