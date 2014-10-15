using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X3Platform.Services
{
    public sealed class ServiceTraceProxy : MarshalByRefObject
    {
        public event BroadcastingEventHandler LocalBroadcastingEvent;

        public void Broadcasting(string text)
        {
            if (LocalBroadcastingEvent != null)
            {
                LocalBroadcastingEvent(text);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
