using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X3Platform.Services
{
    /// <summary>广播时的事件</summary>
    /// <param name="text"></param>
    public delegate void BroadcastingEventHandler(string text);

    /// <summary>服务跟踪</summary>
    public interface IServiceTrace
    {
        /// <summary>广播事件</summary>
        event BroadcastingEventHandler BroadcastingEvent;

        /// <summary></summary>
        /// <param name="text"></param>
        void WriteLine(string text);
    }
}
