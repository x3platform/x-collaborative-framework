
namespace X3Platform.Web.Component.TestSuite
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using X3Platform.Web.Component.Combobox;

    /// <summary></summary>
    [TestClass]
    public class ComboboxBuilderTestSuite
    {
        /// <summary>测试解析数据源</summary>
        [TestMethod]
        public void TestParse()
        {
            string dataSource = "0:一般;1:重要;2:紧急;";

            IList<ComboboxItem> list = ComboboxBuilder.Parse(dataSource);

            Assert.AreEqual(list.Count, 3);

            Assert.AreEqual(ComboboxBuilder.ParseText("1", dataSource), "重要");

            Assert.AreEqual(ComboboxBuilder.ParseValue("重要", dataSource), "1");
        }
    }
}
