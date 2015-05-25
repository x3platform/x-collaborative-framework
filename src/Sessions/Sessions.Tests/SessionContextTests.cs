namespace X3Platform.Sessions.Tests
{
  using System;
  using System.Configuration;
  using System.Diagnostics;

  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using X3Platform.Sessions.Configuration;
  using X3Platform.Sessions;

  [TestClass]
  public class SessionsContextTests
  {
    /// <summary></summary>
    [TestMethod]
    public void TestLoad()
    {
      Assert.IsNotNull(SessionContext.Instance.AccountCacheService);
    }
  }
}
