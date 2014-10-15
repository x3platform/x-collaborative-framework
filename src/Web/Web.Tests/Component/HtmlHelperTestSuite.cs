using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace X3Platform.Web.Component.TestSuite
{
    /// <summary></summary>
    [TestClass]
    public class HtmlHelperTestSuite
    {
        /// <summary></summary>
        [TestMethod]
        public void TestTextBox()
        {
            // TextBox
            Assert.AreEqual(
                HtmlHelper.TextBox("account", "admin"),
                "");
        }
    }
}
