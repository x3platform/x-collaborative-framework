using System;
using System.ServiceModel;
using System.Collections.Generic;

using X3Platform.Services.IServiceContracts;

namespace X3Platform.Services.ServiceContracts
{
    /// <summary></summary>
    public class DebugService : IDebugService
    {
        private string sessionId = null;

        private string applicationName = null;

        private IDebugServiceCallback callback = null;

        private static Object syncObject = new Object();

        private DebugEventHandler debugEventHandler = null;

        private static Dictionary<string, DebugEventHandler> apps = new Dictionary<string, DebugEventHandler>();

        public string[] Connect(string applicationName)
        {
            bool isNewApplication = false;

            //
            OperationContext.Current.Channel.Faulted += new EventHandler(OnFaulted);
            //
            OperationContext.Current.Channel.Closing += new EventHandler(OnClosing);

            if (string.IsNullOrEmpty(applicationName))
            {
                throw new NullReferenceException("未填写参数信息: applicationName.");
            }

            debugEventHandler = new DebugEventHandler(MyEventHandler);

            lock (syncObject)
            {
                this.sessionId = OperationContext.Current.SessionId;

                this.applicationName = applicationName;

                if (!apps.ContainsKey(this.applicationName))
                {
                    apps.Add(this.sessionId, MyEventHandler);

                    isNewApplication = true;
                }
            }

            if (isNewApplication)
            {
                callback = OperationContext.Current.GetCallbackChannel<IDebugServiceCallback>();

                DebugEventArgs e = new DebugEventArgs();

                e.ApplicationName = applicationName;
                e.Message = "连接到控制台.\r\n";

                BroadcastMessage(e);

                DebugEvent += debugEventHandler;

                string[] list = new string[apps.Count];

                lock (syncObject)
                {
                    apps.Keys.CopyTo(list, 0);
                }

                return list;
            }
            else
            {
                return null;
            }
        }

        private void OnFaulted(object sender, EventArgs e)
        {
            WriteLine("发生异常.");
            Disconnect();
        }

        private void OnClosing(object sender, EventArgs e)
        {
            WriteLine("正在关闭连接...");
            Disconnect();
        }

        public void Disconnect()
        {
            lock (syncObject)
            {
                apps.Remove(this.sessionId);
            }

            if (this.applicationName == null)
                return;

            DebugEvent -= debugEventHandler;

            DebugEventArgs e = new DebugEventArgs();

            e.ApplicationName = this.applicationName;
            e.Message = "已断开连接.\r\n";

            BroadcastMessage(e);
        }

        public static void WriteLine(string applicationName, string message)
        {
            DebugEventArgs e = new DebugEventArgs();

            e.ApplicationName = applicationName;
            e.Message = message + Environment.NewLine;

            DebugEventHandler temp = DebugEvent;

            if (temp != null)
            {
                foreach (DebugEventHandler handler in temp.GetInvocationList())
                {
                    handler.BeginInvoke(null, e, new AsyncCallback(BroadcastMessageCallback), null);
                }
            }
        }

        public void Write(string message)
        {
            this.WriteWithApplicationName(this.applicationName, message);
        }

        public void WriteWithApplicationName(string applicationName, string message)
        {
            DebugEventArgs e = new DebugEventArgs();

            e.ApplicationName = applicationName;
            e.Message = message;

            BroadcastMessage(e);
        }

        public void WriteLine(string message)
        {
            this.WriteLineWithApplicationName(this.applicationName, message);
        }

        public void WriteLineWithApplicationName(string applicationName, string message)
        {
            this.WriteWithApplicationName(applicationName, string.Format("{0}{1}", message, System.Environment.NewLine));
        }

        private void MyEventHandler(object sender, DebugEventArgs e)
        {
            try
            {
                ICommunicationObject communicationObject = callback as ICommunicationObject;

                if (communicationObject.State == CommunicationState.Opened)
                {
                    switch (e.Type)
                    {
                        case DebugEventType.KeepAlive:
                            // callback.KeepAlive();
                            break;
                        default:
                            callback.Response("[" + e.Time.ToString("HH:mm:ss.fff") + "]: {" + e.ApplicationName + "} " + e.Message);
                            break;
                    }
                }

            }
            catch
            {
                Disconnect();
            }
        }

        private void BroadcastMessage(DebugEventArgs e)
        {
            DebugEventHandler temp = DebugEvent;

            if (temp != null)
            {
                foreach (DebugEventHandler handler in temp.GetInvocationList())
                {
                    handler.BeginInvoke(this, e, new AsyncCallback(BroadcastMessageCallback), null);
                }
            }
        }

        public delegate void DebugEventHandler(object sender, DebugEventArgs e);

        public static event DebugEventHandler DebugEvent;

        private static void BroadcastMessageCallback(IAsyncResult result)
        {
            DebugEventHandler asyncDelegate = null;

            try
            {
                System.Runtime.Remoting.Messaging.AsyncResult asyncResult = (System.Runtime.Remoting.Messaging.AsyncResult)result;

                asyncDelegate = ((DebugEventHandler)asyncResult.AsyncDelegate);

                asyncDelegate.EndInvoke(result);
            }
            catch
            {
                DebugEvent -= asyncDelegate;
            }
        }

        //public void CheckCallbackChannels()
        //{
        //    RegisteredClient[] clientList = new RegisteredClient[0];

        //    callback = OperationContext.Current.GetCallbackChannel<IDebugServiceCallback>();
        
        //    lock (m_callbackList)
        //    {

        //        clientList = new RegisteredClient[apps.Count];

        //        apps.CopyTo(clientList);

        //        foreach (IDebugServiceCallback callback in clientList)
        //        {
        //            ICommunicationObject callbackChannel = callback.CallBack as ICommunicationObject;

        //            if (callbackChannel.State == CommunicationState.Closed || callbackChannel.State == CommunicationState.Faulted)
        //            {
        //                this.RemoveClientMachine(registeredClient.CallBack);
        //            }
        //        }
        //    }
        //}

        /// <summary>默认测试方法</summary>
        /// <returns></returns>
        public string Hi()
        {
            return string.Format("当前时间:{0} 服务器({1})这是一个测试方法, 说明你已经成功连接到此服务.", DateTime.Now, System.Environment.MachineName);
        }
    }
}
