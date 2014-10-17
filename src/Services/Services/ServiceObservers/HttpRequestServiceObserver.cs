namespace X3Platform.Services.ServiceObservers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.IO;
    using System.Text;

    using X3Platform.Services.Configuration;
    using X3Platform.Configuration;
    using Common.Logging;

    public sealed class HttpRequestServiceObserver : IServiceObserver
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

        private EventLog eventLog = new EventLog();

        private bool running = false;

        /// <summary>是否正在运行</summary>
        public bool IsRunning
        {
            get { return running; }
        }

        #region 属性:Sleeping
        /// <summary>是否正在运行</summary>
        public bool Sleeping
        {
            get { return (nextRunTime > DateTime.Now) ? true : false; }
        }
        #endregion

        public HttpRequestServiceObserver()
            : this("HttpRequest", string.Empty)
        {
        }

        public HttpRequestServiceObserver(string name, string args)
        {
            this.m_Name = name;

            this.eventLog = new EventLog();

            this.eventLog.Log = "Application";
            this.eventLog.Source = ServicesConfigurationView.Instance.ServiceName;

            this.eventLog.WriteEntry("Http请求监听服务初始化.");

            Reload();
        }

        private ServicesConfiguration configuration = null;

        /// <summary>
        /// 上一次执行的结束时间
        /// </summary>
        private DateTime nextRunTime = new DateTime(2000, 1, 1);

        // 单位(小时)
        private int runTimeInterval = 1;

        // 跟踪运行时间
        private bool trackRunTime = false;

        public void Reload()
        {
            this.configuration = ServicesConfigurationView.Instance.Configuration;

            this.trackRunTime = ServicesConfigurationView.Instance.TrackRunTime;

            this.nextRunTime = this.configuration.Observers[this.Name].NextRunTime;
        }

        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            if (running)
                return;

            try
            {
                // 检测是否符合运行时间的范围. 
                //
                // 检测上次运行时间的间隔是否大于最小时间间隔.

                if (trackRunTime)
                {
                    // [调试] 跟踪运行的时间.
                    // LoggingContext.Instance.Write(string.Format(@"时间:{0},Http请求监听服务正在运行.下次执行时间:[{1}]. ",
                    //    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    //    nextRunTime));
                }

                if (nextRunTime < DateTime.Now)
                {
                    nextRunTime = DateTime.Now.AddHours(runTimeInterval);

                    this.configuration.Observers[this.Name].NextRunTime = nextRunTime;

                    ServicesConfigurationView.Instance.Save();

                    running = true;

                    string html = string.Empty;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(KernelConfigurationView.Instance.HostName);

                    try
                    {
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    html = reader.ReadToEnd();

                                    reader.Close();
                                }

                                stream.Close();
                            }

                            response.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }

                    running = false;
                }
            }
            catch (Exception ex)
            {
                // 发生异常是, 记录异常信息  并把运行标识为false.

                // LoggingContext.Instance.Write(ex);

                eventLog.WriteEntry(string.Format("Http请求监听服务发生异常信息\r\n{1}", ex.ToString()));

                running = false;
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
