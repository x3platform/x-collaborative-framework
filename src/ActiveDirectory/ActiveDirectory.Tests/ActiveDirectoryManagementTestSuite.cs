using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Elane.X.ActiveDirectory.Configuration;

namespace Elane.X.ActiveDirectory.TestSuite
{
    /// <summary></summary>
    [TestClass]
    public class ActiveDirectoryManagementTestSuite
    {
        public string fullConfigPath = ConfigurationManager.AppSettings["fullConfigPath"];

        /// <summary>在运行每个测试之前使用 TestInitialize 运行代码</summary>
        [TestInitialize()]
        public void Initialize()
        {
            ActiveDirectoryConfigurationView.LoadInstance(fullConfigPath);
        }

        /// <summary>在运行每个测试之后使用 TestCleanup 运行代码</summary>
        [TestCleanup()]
        public void Cleanup()
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

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        /// <summary>测试 Reload()</summary>
        [TestMethod]
        public void TestRestart()
        {
            ActiveDirectoryManagement.Instance.Restart();

            Assert.IsNotNull(ActiveDirectoryManagement.Instance.Organization);
            Assert.IsNotNull(ActiveDirectoryManagement.Instance.Group);
            Assert.IsNotNull(ActiveDirectoryManagement.Instance.User);
        }
    }
}
