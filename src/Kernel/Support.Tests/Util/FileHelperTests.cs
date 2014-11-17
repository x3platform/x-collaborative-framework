namespace X3Platform.Tests.Util
{
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Util;
    using System.Text;

    /// <summary></summary>
    [TestClass]
    public class FileHelperTests
    {
        [TestMethod]
        public void TestGetEncoding()
        {
            Encoding encoding = FileHelper.GetEncoding(@"D:\GitHub\x-collaborative-framework\src\Apps\Apps\Ajax\ApplicationEventWrapper.cs");

            Assert.AreEqual(Encoding.UTF8, encoding);

            encoding = FileHelper.GetEncoding(@"D:\GitHub\x-collaborative-framework\src\clean.cmd");

            Assert.AreEqual(Encoding.Default, encoding);
        }
    }
}