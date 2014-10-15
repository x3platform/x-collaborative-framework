using System.IO;

using X3Platform.Services.IServiceContracts;
using System;
using System.Xml;

namespace X3Platform.Services.ServiceContracts
{
    /// <summary>调试事件参数</summary>
    public class DebugEventArgs : EventArgs
    {
        /// <summary>事件类型</summary>
        public DebugEventType Type = DebugEventType.Response;

        /// <summary>时间</summary>
        public DateTime Time = DateTime.Now;

        /// <summary>应用系统名称</summary>
        public string ApplicationName;

        /// <summary>消息</summary>
        public string Message;
    }

    public enum DebugEventType
    {
        Response,
        KeepAlive
    }
}
