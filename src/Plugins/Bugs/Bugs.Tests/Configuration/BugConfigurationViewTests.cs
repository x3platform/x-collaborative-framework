using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.IO;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Plugins.Bugs.Configuration;
using X3Platform.IBatis.DataMapper;
using System.Configuration;

namespace X3Platform.Plugins.Bugs.Tests.Configuration
{
  [TestClass]
  public class BugConfigurationViewTests
  {
    /// <summary>在运行每个测试之后使用 TestCleanup 运行代码</summary>
    [TestCleanup()]
    public void Cleanup()
    {
    }

    #region 其他测试属性
    //
    // 您可以在编写测试时使用下列其他属性:
    //
    // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // 在运行每个测试之前使用 TestInitialize 运行代码 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // 在运行每个测试之后使用 TestCleanup 运行代码
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    //-------------------------------------------------------
    // 测试内容
    //-------------------------------------------------------

    [TestMethod]
    public void TestInit()
    {
      BugConfiguration configuration = BugConfigurationView.Instance.Configuration;

      Assert.IsNotNull(configuration);
    }

    [TestMethod]
    public void TestCreateMapper()
    {
      BugConfiguration configuration = BugConfigurationView.Instance.Configuration;

      ISqlMapper ibatisMapper = null;

      string ibatisMapping = configuration.Keys["IBatisMapping"].Value;

      ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);

      Assert.IsNotNull(ibatisMapper);
    }
  }
}
