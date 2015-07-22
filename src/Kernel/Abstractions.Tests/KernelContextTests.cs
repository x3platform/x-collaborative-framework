namespace X3Platform.Tests
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  /// <summary></summary>
  [TestClass]
  public class KernelContextTests
  {
    /// <summary>测试 有权限的方法</summary>
    [TestMethod]
    public void TestInit()
    {
      Assert.IsNotNull(KernelContext.Current.AuthenticationManagement);
    }
  }
}
