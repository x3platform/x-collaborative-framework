using Xunit;

using X3Platform.Ajax.Configuration;

namespace X3Platform.Tests.Ajax.Configuration
{
    /// <summary>Ajax 配置信息检验测试程序</summary>
    public class AjaxConfigurationViewTests
    {
        /// <summary>测试初始化配置信息是否正确</summary>
        [Fact]
        public void TestInit()
        {
            AjaxConfiguration configuration = AjaxConfigurationView.Instance.Configuration;

            Assert.NotNull(configuration);
            Assert.True(configuration.Keys.Count > 0);

            // 判断数据正确性
            Assert.True(configuration.SpecialWords["QQ"].Value == "qq");
    
            Assert.True(configuration.SpecialWords["QQ"].Value == "qq");
        }
    }
}