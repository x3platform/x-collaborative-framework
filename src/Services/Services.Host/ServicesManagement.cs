using System.ServiceProcess;

using X3Platform.Services.Configuration;

namespace X3Platform.Services
{
    /// <summary>
    /// 服务管理者
    /// </summary>
    partial class ServicesManagement : ServiceBase
    {
        // private ServicesConfiguration configuration = null;

        public ServicesManagement()
        {
            this.ServiceName = ServicesConfigurationView.Instance.ServiceName;
        }

        public void Start()
        {
            // 启动所有的监听服务
            EventLogHelper.Write(string.Format("{0}正在启动自定义监听器。", this.ServiceName));

            ServiceObserverManagement.Start();

            // 启动所有的WCF服务
            EventLogHelper.Write(string.Format("{0}正在启动自定义服务。", this.ServiceName));

            ServiceHostManagement.Start();
        }

        public new void Stop()
        {
            EventLogHelper.Write(string.Format("{0}服务被停止。", this.ServiceName));

            // 关闭所有的监听服务
            ServiceObserverManagement.Close();

            // 关闭所有的WCF服务
            ServiceHostManagement.Close();

            this.OnStop();
        }

        public void Pause()
        {
            EventLogHelper.Write(string.Format("{0} 服务被暂停。", this.ServiceName));

            this.OnPause();
        }
    }
}