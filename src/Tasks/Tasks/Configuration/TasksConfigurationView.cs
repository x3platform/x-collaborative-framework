namespace X3Platform.Tasks.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>任务配置视图</summary>
    public class TasksConfigurationView : XmlConfigurationView<TasksConfiguration>
    {
        private const string configFile = "config\\X3Platform.Tasks.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = TasksConfiguration.ApplicationName;

        #region 静态属性::Instance
        private static volatile TasksConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static TasksConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new TasksConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:TasksConfigurationView()
        /// <summary>构造函数</summary>
        private TasksConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // 将配置信息加载到全局的配置中
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载配置信息</summary>
        public override void Reload()
        {
            base.Reload();

            // 将配置信息加载到全局的配置中
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义属性
        // -------------------------------------------------------

        #region 属性:MessageQueueMode
        private string m_MessageQueueMode = string.Empty;

        /// <summary>消息队列模式</summary>
        public string MessageQueueMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueMode))
                {
                    // 读取配置信息
                    this.m_MessageQueueMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueMode",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_MessageQueueMode = StringHelper.NullOrEmptyTo(this.m_MessageQueueMode, "OFF");

                    this.m_MessageQueueMode = this.m_MessageQueueMode.ToUpper();
                }

                return this.m_MessageQueueMode;
            }
        }
        #endregion

        #region 属性:MessageQueueMachineName
        private string m_MessageQueueMachineName = string.Empty;

        /// <summary>消息队列机器名称</summary>
        public string MessageQueueMachineName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueMachineName))
                {
                    // 读取配置信息
                    this.m_MessageQueueMachineName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueMachineName",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_MessageQueueMachineName = StringHelper.NullOrEmptyTo(this.m_MessageQueueMachineName, @".\private$");
                }

                return this.m_MessageQueueMachineName;
            }
        }
        #endregion

        #region 属性:MessageQueueName
        private string m_MessageQueueName = string.Empty;

        /// <summary>消息队列名称</summary>
        public string MessageQueueName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueName))
                {
                    // 读取配置信息
                    this.m_MessageQueueName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueName",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_MessageQueueName = StringHelper.NullOrEmptyTo(this.m_MessageQueueName, "tasks$default$queue");
                }

                return this.m_MessageQueueName;
            }
        }
        #endregion

        #region 属性:PrefixTargetUrl
        private string m_PrefixTargetUrl = string.Empty;

        /// <summary>任务信息的地址前缀</summary>
        public string PrefixTargetUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_PrefixTargetUrl))
                {
                    // 读取配置信息
                    this.m_PrefixTargetUrl = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "PrefixTargetUrl",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_PrefixTargetUrl = StringHelper.NullOrEmptyTo(this.m_PrefixTargetUrl, KernelConfigurationView.Instance.HostName);
                }

                return this.m_PrefixTargetUrl;
            }
        }
        #endregion

        #region 属性:ClientRefreshInterval
        private int m_ClientRefreshInterval = 0;

        /// <summary>客户端刷新间隔</summary>
        public int ClientRefreshInterval
        {
            get
            {
                if (this.m_ClientRefreshInterval == 0)
                {
                    // 读取配置信息
                    this.m_ClientRefreshInterval = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "ClientRefreshInterval",
                        this.Configuration.Keys));

                    // 如果配置文件里未设置则设置一个默认值
                    if (this.m_ClientRefreshInterval == 0)
                    {
                        this.m_ClientRefreshInterval = 10;
                    }
                }

                return this.m_ClientRefreshInterval;
            }
        }
        #endregion
    }
}
