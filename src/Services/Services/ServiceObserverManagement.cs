namespace X3Platform.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Timers;

    using X3Platform.Services.Configuration;
    
    public static class ServiceObserverManagement
    {
        /// <summary>服务列表</summary>
        private static IList<IServiceObserver> serviceObservers = new List<IServiceObserver>();

        private static Timer timer = new Timer();

        /// <summary>启动</summary>
        public static void Start()
        {
            try
            {
                ServicesConfiguration configuration = ServicesConfigurationView.Instance.Configuration;

                foreach (ServiceObserverConfiguration observer in configuration.Observers)
                {
                    IServiceObserver serviceObserver = (IServiceObserver)Assembly.Load(observer.TypeName.Substring(observer.TypeName.IndexOf(",") + 1).Trim()).CreateInstance(observer.TypeName.Substring(0, observer.TypeName.IndexOf(",")).Trim(), false, BindingFlags.Default, null, new object[] { observer.Name, observer.Args }, null, null);

                    ServiceTrace.Instance.WriteLine("正在创建监听器【" + observer.Name + "】。");

                    if (serviceObserver == null)
                    {
                        EventLogHelper.Write(string.Format("监听器【{0}】初始化类型【{1}】失败，请确认配置是否正确。", observer.Name, observer.TypeName));
                    }
                    else
                    {
                        serviceObserver.Start();

                        serviceObservers.Add(serviceObserver);
                    }
                }

            }
            catch (Exception ex)
            {
                EventLogHelper.Error(ex.ToString());

                throw ex;
            }

            // -------------------------------------------------------
            // 设置定时器
            // -------------------------------------------------------

            timer.Enabled = true;

            timer.Interval = ServicesConfigurationView.Instance.TimerInterval * 1000;

            timer.Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                ServiceObserverManagement.Run();
            };

            timer.Start();
        }

        /// <summary>关闭</summary>
        public static void Close()
        {
            foreach (IServiceObserver serviceObserver in serviceObservers)
            {
                serviceObserver.Close();
            }
        }

        /// <summary></summary>
        public static void Run()
        {
            ServiceTrace.Instance.WriteLine("监听服务正在运行(周期:" + (timer.Interval / 1000) + "秒)。");

            foreach (IServiceObserver serviceObserver in serviceObservers)
            {
                if (!(serviceObserver.IsRunning || serviceObserver.Sleeping))
                {
                    serviceObserver.Run();
                }
            }
        }
    }
}