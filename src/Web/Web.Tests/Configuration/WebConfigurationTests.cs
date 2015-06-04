namespace X3Platform.Web.Tests.Configuration
{
  using System;
  using System.Xml;
  using System.Resources;
  using System.Reflection;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Common.Logging;

  using X3Platform.Ajax;
  using X3Platform.Util;

  using X3Platform.Web.Configuration;
  using X3Platform.IBatis.DataMapper;

  /// <summary>字符资源加载工具类</summary>
  [TestClass]
  public class WebConfigurationTests
  {
    /// <summary>测试文件</summary>
    [TestMethod]
    public void TestInit()
    {
      var configuration = WebConfigurationView.Instance.Configuration;

      Assert.IsNotNull(configuration);

      Assert.IsNotNull(configuration.Keys["SpringObjectFile"]);
      Assert.IsNotNull(configuration.Keys["IBatisMapping"]);
    }

    [TestMethod]
    public void TestCreateMapper()
    {
      ISqlMapper ibatisMapper = null;

      string ibatisMapping = WebConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

      ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

      Assert.IsNotNull(ibatisMapper);
    }
  }
}