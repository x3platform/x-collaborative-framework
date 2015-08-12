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
    /// <summary>������ÿ������֮��ʹ�� TestCleanup ���д���</summary>
    [TestCleanup()]
    public void Cleanup()
    {
    }

    #region ������������
    //
    // �������ڱ�д����ʱʹ��������������:
    //
    // ���������еĵ�һ������֮ǰʹ�� ClassInitialize ���д���
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // �����е����в��Զ�������֮��ʹ�� ClassCleanup ���д���
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // ������ÿ������֮ǰʹ�� TestInitialize ���д��� 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // ������ÿ������֮��ʹ�� TestCleanup ���д���
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    //-------------------------------------------------------
    // ��������
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
