namespace X3Platform.Web.Tests.Customize
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Web.Customize;

    /// <summary>测试日志功能</summary>
    [TestClass]
    public class CustomizeContextTestSuite
    {
        public CustomizeContextTestSuite()
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

        [TestMethod]
        public void TestRestart()
        {
            CustomizeContext.Instance.Restart();

            Assert.IsNotNull(CustomizeContext.Instance.WidgetInstanceService);
            Assert.IsNotNull(CustomizeContext.Instance.WidgetService);
            Assert.IsNotNull(CustomizeContext.Instance.PageService);
        }
    }
}
