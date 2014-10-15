using Xunit;

using X3Platform.Ajax.Configuration;

namespace X3Platform.Tests.Ajax.Configuration
{
    /// <summary>Ajax ������Ϣ������Գ���</summary>
    public class AjaxConfigurationViewTests
    {
        /// <summary>���Գ�ʼ��������Ϣ�Ƿ���ȷ</summary>
        [Fact]
        public void TestInit()
        {
            AjaxConfiguration configuration = AjaxConfigurationView.Instance.Configuration;

            Assert.NotNull(configuration);
            Assert.True(configuration.Keys.Count > 0);

            // �ж�������ȷ��
            Assert.True(configuration.SpecialWords["QQ"].Value == "qq");
    
            Assert.True(configuration.SpecialWords["QQ"].Value == "qq");
        }
    }
}