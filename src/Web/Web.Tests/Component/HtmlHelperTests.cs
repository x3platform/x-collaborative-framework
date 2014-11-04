namespace X3Platform.Web.Tests.Component
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Web.Component;

    /// <summary></summary>
    [TestClass]
    public class HtmlHelperTests
    {
        /// <summary></summary>
        [TestMethod]
        public void TestTextBox()
        {
            // TextBox
            Assert.AreEqual("<input id=\"account\" name=\"account\" value=\"admin\" type=\"text\" />", 
                HtmlHelper.TextBox("account", "admin"));
        }
    }
}
