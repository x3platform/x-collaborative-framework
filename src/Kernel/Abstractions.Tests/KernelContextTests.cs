namespace X3Platform.Tests
{
  using System;

  using NUnit.Framework;

  /// <summary></summary>
  [TestFixture]
  public class KernelContextTests
  {
    /// <summary>测试 有权限的方法</summary>
    [Test]
    public void TestInit()
    {
      Assert.IsNotNull(KernelContext.Current.AuthenticationManagement);
    }
  }
}
