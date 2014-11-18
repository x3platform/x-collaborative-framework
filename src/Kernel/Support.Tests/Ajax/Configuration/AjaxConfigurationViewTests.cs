#region Using Testing Libraries
#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestContext = System.Object;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

using NMock;
#endregion

namespace X3Platform.Tests.Ajax.Configuration
{
    using X3Platform.Ajax.Configuration;

    /// <summary>Ajax ������Ϣ������Գ���</summary>
    [TestClass]
    public class AjaxConfigurationViewTests
    {
        /// <summary>���Գ�ʼ��������Ϣ�Ƿ���ȷ</summary>
        [TestMethod]
        public void TestInit()
        {
            AjaxConfiguration configuration = AjaxConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
            Assert.IsTrue(configuration.Keys.Count > 0);

            // �ж�������ȷ��
            Assert.IsTrue(configuration.SpecialWords["QQ"].Value == "qq");

            Assert.IsTrue(configuration.SpecialWords["RMB"].Value == "rmb");
        }
    }
}