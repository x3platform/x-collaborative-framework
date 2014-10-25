
using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.CategoryIndexes;

namespace X3Platform.Tests.CategoryIndexes
{
    /// <summary></summary>
    [TestClass]
    public class TextCategoryIndexTestSuite
    {
        /// <summary>验证初始化对象</summary>
        [TestMethod]
        public void TestLoad()
        {
            ICategoryIndex index = new TextCategoryIndex("用户中心\\帐号管理\\员工帐号注册");

            Assert.IsNotNull(index);
        }

        /// <summary>测试写入数据</summary>
        [TestMethod]
        public void TestWrite()
        {
            CategoryIndexWriter writer = new CategoryIndexWriter("选择类别");
            
            //writer.Read("用户中心\\帐号管理\\员工帐号注册");
            //writer.Read("用户中心\\帐号管理\\员工帐号注册");
            //writer.Read("用户中心\\帐号管理1\\员工帐号注册1");
            //writer.Read("用户中心\\帐号管理2\\员工帐号注册2");
            //writer.Read("用户中心\\帐号管理1\\员工帐号注册2");
            //writer.Read("用户中心\\帐号管理2\\员工帐号注册");
            //writer.Read("11\\113\\123");
            //writer.Read("11\\11\\11");
            //writer.Read("11\\1223\\122");
            //writer.Read("122\\12234");
            writer.Read("22\\12\\2345");
            
            ICategoryIndex index = writer.Write();

            Assert.IsNotNull(index);
            Assert.IsNotNull(index.Text);
        }
    }
}
