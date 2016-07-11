namespace X3Platform.Tests.Logging
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Common.Logging;

    /// <summary>测试日志</summary>
    [TestClass]
    public class LoggingTests
    {
        [TestMethod]
        public void TestLog()
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

            Assert.IsNotNull(logger);

            // 写入跟踪数据
            logger.Trace("INFO:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // 写入信息
            logger.Info("INFO:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // 写入信息
            logger.Info(text => text("param {0}", "ab"));
            // 写入警告
            logger.Warn("Warn:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // 写入带错误信息的警告
            logger.Warn("Warn:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), new ArgumentNullException("A"));
            // 写入错误
            logger.Error("Error:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // 写入
            logger.Fatal("Fatal:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }
    }
}