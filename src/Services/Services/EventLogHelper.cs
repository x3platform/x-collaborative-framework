using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using X3Platform.Services.Configuration;

namespace X3Platform.Services
{
    /// <summary></summary>
    public sealed class EventLogHelper
    {
        public static EventLog eventLog = new EventLog();

        /// <summary>普通消息</summary>
        public static void Information(string text)
        {
            Write(text, EventLogEntryType.Information);
        }

        /// <summary>警告信息</summary>
        public static void Warning(string text)
        {
            Write(text, EventLogEntryType.Warning);
        }

        /// <summary></summary>
        public static void Error(string text)
        {
            Write(text, EventLogEntryType.Error);
        }

        /// <summary>写入事件消息</summary>
        public static void Write(string text)
        {
            Write(text, EventLogEntryType.Information);
        }

        /// <summary>写入事件消息</summary>
        public static void Write(string text, EventLogEntryType type)
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
