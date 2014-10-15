using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Util;
using System.Resources;
using System.Reflection;
using Common.Logging;
using System;

namespace X3Platform.Tests.Logging
{
    /// <summary>�ַ���Դ���ع�����</summary>
    [TestClass]
    public class LoggingTests
    {
        [TestMethod]
        public void TestLog()
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

            Assert.IsNotNull(logger);
            
            // д����Ϣ
            logger.Trace("INFO:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            
            // д����Ϣ
            logger.Info("INFO:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //
            logger.Warn("Warn:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //
            logger.Warn("Warn:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), new ArgumentNullException("A"));
            //
            logger.Error("Error:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //
            logger.Fatal("Fatal:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }
    }
}