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
        event BroadcastingEventHandler BroadcastingEvent;

        void WriteLine(string text);
    }
}
