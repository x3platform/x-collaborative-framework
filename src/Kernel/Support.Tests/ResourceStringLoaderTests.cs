using System.Xml;
using System.Resources;
using System.Reflection;
using Xunit;
using System;

namespace X3Platform.Tests
{
    /// <summary>字符资源加载工具类</summary>
    public class ResourceStringLoaderTests
    {
        [Fact]
        public void TestLoadString()
        {
            // ResourceManager rm = new ResourceManager("X3Platform.Tests.ResourceString", Assembly.GetExecutingAssembly());

            string result = ResourceStringLoader.LoadString("I18N", "aa");

            Assert.NotNull(result);

            result = ResourceStringLoader.LoadString("X3Platform.Tests.I18N","aa");

            Assert.NotNull(result);
        }
    }
}