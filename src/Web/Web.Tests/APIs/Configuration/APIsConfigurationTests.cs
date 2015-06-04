namespace X3Platform.Web.Tests.APIs.Configuration
{
  using System;
  using System.Xml;

  using Common.Logging;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using X3Platform.Ajax;
  using X3Platform.Util;

  using X3Platform.Web.APIs.Configuration;

  /// <summary>≤‚ ‘ Web APIs ≈‰÷√</summary>
  [TestClass]
  public class APIsConfigurationTests
  {
    /// <summary>≤‚ ‘Œƒº˛</summary>
    [TestMethod]
    public void TestLoad()
    {
      APIsConfiguration configuration = APIsConfigurationView.Instance.Configuration;

      Assert.IsNotNull(configuration);
    }
  }
}