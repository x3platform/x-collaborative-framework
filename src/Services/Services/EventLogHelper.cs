using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common.Logging;

using X3Platform.Services.Configuration;

namespace X3Platform.Services
{
    /// <summary></summary>
    public sealed class EventLogHelper
    {
        private ILog logger = LogManager.GetLogger("X3Platform.Services");

        /// <summary>日志记录</summary>
        public static ILog Log
        {
            get
            {
                return GetInstance().logger;
            }
        }

        #region 静态属性:Instance
        private static volatile EventLogHelper instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        static EventLogHelper GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new EventLogHelper();
                    }
                }
            }

            return instance;
        }
        #endregion

        private static EventLog eventLog = new EventLog();

        /// <summary>普通消息</summary>
        public static void Information(string text)
        {
            // 普通消息不写入 Windows 事件日志
            Log.Info(text);

            // Write(text, EventLogEntryType.Information);
        }

        /// <summary>警告信息</summary>
        public static void Warning(string text)
        {
            Log.Warn(text);

            Write(text, EventLogEntryType.Warning);
        }

        /// <summary></summary>
        public static void Error(string text)
        {
            Log.Error(text);

            Write(text, EventLogEntryType.Error);
        }

        /// <summary>写入事件消息</summary>
        public static void Write(string text)
        {
            Log.Info(text);

            Write(text, EventLogEntryType.Information);
        }

        /// <summary>写入事件消息</summary>
        private static void Write(string text, EventLogEntryType type)
        {
            try
            {
                eventLog.Log = "Application";

                eventLog.Source = ServicesConfigurationView.Instance.ServiceName;

                eventLog.WriteEntry(text, type);
            }
            catch (System.Security.SecurityException ex)
            {
                throw ex;
            }
        }
    }
}
