namespace X3Platform.Tests.Logging
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Common.Logging;

    /// <summary>������־</summary>
    [TestClass]
    public class LoggingTests
    {
        [TestMethod]
        public void TestLog()
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

            Assert.IsNotNull(logger);

            // д���������
            logger.Trace("INFO:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // д����Ϣ
            logger.Info("INFO:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // д����Ϣ
            logger.Info(text => text("param {0}", "ab"));
            // д�뾯��
            logger.Warn("Warn:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // д���������Ϣ�ľ���
            logger.Warn("Warn:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), new ArgumentNullException("A"));
            // д�����
            logger.Error("Error:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // д��
            logger.Fatal("Fatal:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }
    }
}