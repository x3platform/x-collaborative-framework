namespace X3Platform.Web.Tests.Component
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Web.Component.Combobox;

    /// <summary></summary>
    [TestClass]
    public class ComboboxBuilderTests
    {
        /// <summary>测试解析数据源</summary>
        [TestMethod]
        public void TestParse()
        {
            string dataSource = "0:一般;1:重要;2:紧急;";

            IList<ComboboxItem> list = ComboboxBuilder.Parse(dataSource);

            Assert.AreEqual(list.Count, 3);

            Assert.AreEqual("重要", ComboboxBuilder.ParseText("1", dataSource));

            Assert.AreEqual("1", ComboboxBuilder.ParseValue("重要", dataSource));
        }
    }
}
