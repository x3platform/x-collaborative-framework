namespace X3Platform.Tests.Ajax.Configuration
{
    using X3Platform.Ajax.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>Ajax 配置信息检验测试程序</summary>
    [TestClass]
    public class AjaxConfigurationViewTests
    {
        /// <summary>测试初始化配置信息是否正确</summary>
        [TestMethod]
        public void TestInit()
        {
            AjaxConfiguration configuration = AjaxConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
            Assert.IsTrue(configuration.Keys.Count > 0);

            // 判断数据正确性
            Assert.IsTrue(configuration.SpecialWords["QQ"].Value == "qq");

            Assert.IsTrue(configuration.SpecialWords["RMB"].Value == "rmb");
        }
    }
}