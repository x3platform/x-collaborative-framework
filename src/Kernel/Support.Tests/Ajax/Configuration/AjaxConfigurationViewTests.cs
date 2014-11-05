namespace X3Platform.Tests.Ajax.Configuration
{
    using X3Platform.Ajax.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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