namespace X3Platform.Tests
{
    using System.Xml;
    using System.Resources;
    using System.Reflection;
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>字符资源加载工具类</summary>
    [TestClass]
    public class ResourceStringLoaderTests
    {
        [TestMethod]
        public void TestLoadString()
        {
            // ResourceManager rm = new ResourceManager("X3Platform.Tests.ResourceString", Assembly.GetExecutingAssembly());
     
            string result = ResourceStringLoader.LoadString("X3Platform.Tests.defaults.test_resources");

            // Assert.IsNotNull(result);

            // result = ResourceStringLoader.LoadString("X3Platform.Tests.I18N", "aa");

            // Assert.IsNotNull(result);
        }
    }
}