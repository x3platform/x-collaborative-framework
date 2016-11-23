
namespace X3Platform.Services.ServiceObservers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using X3Platform.Services.Configuration;
    using Common.Logging;

    public sealed class TcpRequestServiceObserver : IServiceObserver
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 属性:Name
        private string m_Name;

        public string Name
        {
            get { return m_Name; }
        }
        #endregion

        private EventLog eventLog = null;

        private bool running = false;

        /// <summary>是否正在运行</summary>
        public bool IsRunning
        {
            get { return running; }
        }

        #region 属性:Sleeping
        public bool Sleeping
        {
            get { return (nextRunTime > DateTime.Now) ? true : false; }
        }
        #endregion

        public TcpRequestServiceObserver()
            : this("TcpRequest", string.Empty)
        {
        }

        public TcpRequestServiceObserver(string name, string args)
        {
            this.m_Name = name;

            this.eventLog = new EventLog();

            this.eventLog.Log = "Application";
            this.eventLog.Source = ServicesConfigurationView.Instance.ServiceName;

            this.eventLog.WriteEntry("Tcp请求监听服务初始化.");

            Reload();
        }

        private ServicesConfiguration configuration = null;

        /// <summary>
        /// 上一次执行的结束时间
        /// </summary>
        private DateTime nextRunTime = new DateTime(2000, 1, 1);

        // 单位(小时)
        // private int runTimeInterval = 1;

        // 跟踪运行时间
        private bool trackRunTime = false;

        private Thread thread;

        public void Reload()
        {
            this.configuration = ServicesConfigurationView.Instance.Configuration;

            this.trackRunTime = ServicesConfigurationView.Instance.TrackRunTime;

            this.nextRunTime = this.configuration.Observers[this.Name].NextRunTime;
        }

        /// <summary>运行</summary>
        public void Run()
        {
            if (running)
                return;

            try
            {
                running = true;

                // 需要在新的线程里监听客户端
                thread = new Thread(new ThreadStart(TcpListening));
                thread.Start();
            }
            catch (Exception ex)
            {
                // 发生异常是, 记录异常信息  并把运行标识为false.
                logger.Error(ex);
                    
                eventLog.WriteEntry(string.Format("Tcp请求监听服务发生异常信息\r\n{1}", ex.ToString()));

                running = false;
            }
        }

        private void TcpListening()
        {
            //创建一个监听对象
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);

            tcpListener.Start();

            // LoggingContext.Instance.Write("启动监听服务");

            // 循环监听
            while (true)
            {
                try
                {
                    // 取得客户端的连接
                    using (TcpClient client = tcpListener.AcceptTcpClient())
                    {
                        // 取得客户端发过来的字节流
                        using (NetworkStream stream = client.GetStream())
                        {
                            // 把字节流读入字节数组
                            byte[] buffer = new byte[10];

                            stream.Read(buffer, 0, buffer.Length);

                            // 不可以在此直接设置this.Text,线程问题.
                            string requestData = System.Text.Encoding.Unicode.GetString(buffer);

                            // LoggingContext.Instance.Write(string.Format("{0} 收到客户端消息:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), requestData));

                            // 向客户端发送数据
                            buffer = System.Text.Encoding.Unicode.GetBytes(string.Format("服务器时间:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).ToCharArray());

                            stream.Write(buffer, 0, buffer.Length);

                            stream.Close();
                        }

                        client.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        public void Start()
        {
        }

        public void Close()
        {
        }
    }
}
