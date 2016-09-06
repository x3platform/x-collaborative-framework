using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;

namespace X3Platform.Services
{
    /// <summary>服务跟踪</summary>
    public sealed class ServiceTrace : MarshalByRefObject, IServiceTrace
    {
        #region 属性:Instance
        private static volatile ServiceTrace instance = null;

        private static object lockObject = new object();

        public static ServiceTrace Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceTrace();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        private ServiceTrace()
        {

        }

        /// <summary>广播事件</summary>
        public event BroadcastingEventHandler BroadcastingEvent;

        public void WriteLine(string text)
        {
            text = string.Format("{0} {1}", "{" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "}", text);

            if (BroadcastingEvent != null)
            {
                BroadcastingEventHandler broadcastingEventHandler = null;

                //记录事件订阅者委托的索引，为方便标识，从1开始。
                int index = 1;

                foreach (Delegate delegateObject in BroadcastingEvent.GetInvocationList())
                {
                    try
                    {
                        broadcastingEventHandler = (BroadcastingEventHandler)delegateObject;
                        broadcastingEventHandler(text);
                    }
                    catch
                    {
                        Console.WriteLine("事件订阅者【{0}】发生错误,系统将取消事件订阅!", index);

                        BroadcastingEvent -= broadcastingEventHandler;
                    }

                    index++;
                }
            }
            else
            {
                // MessageBox.Show("事件未被订阅或订阅发生错误!");
            }

            Console.WriteLine(text);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
