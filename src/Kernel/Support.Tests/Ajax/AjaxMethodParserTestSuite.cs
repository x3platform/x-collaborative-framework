using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using X3Platform.Ajax;
using System.Xml;

namespace X3Platform.Tests.Ajax
{
    /// <summary>
    /// UnitTest1 的摘要说明
    /// </summary>
    [TestClass]
    public class AjaxMethodParserTestSuite
    {
        #region 属性:TestContext
        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        #region 附加测试属性
        //
        // 编写测试时，还可使用以下附加属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestParse()
        {
            MockAjaxWrapper wrapper = new MockAjaxWrapper();

            wrapper.Reqeuest();
        }
    }

    //
    // 测试对象
    //

    public sealed class MockAjaxWrapper
    {
        public void Reqeuest()
        {
            string action = "ajaxMethod";

            string result = AjaxMethodParser.Parse(this, action, new XmlDocument());
        }

        [AjaxMethod("ajaxMethod")]
        public string GetAjaxMethod(XmlDocument doc)
        {
            return "hi brother.";
        }
    }
}
