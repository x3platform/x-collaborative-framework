namespace X3Platform.Connect.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;
    using X3Platform.Configuration;
    using X3Platform.MessageQueue.Configuration;
    using X3Platform.Util;
    #endregion

    /// <summary>应用连接管理的配置视图</summary>
    public class ConnectConfigurationView : XmlConfigurationView<ConnectConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Connect.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = ConnectConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile ConnectConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static ConnectConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ConnectConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:ConnectConfigurationView()
        /// <summary>构造函数</summary>
        private ConnectConfigurationView()
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

        #region 属性:ApiHostName
        private string m_ApiHostName = string.Empty;

        /// <summary>API主机名</summary>
        public string ApiHostName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ApiHostName))
                {
                    // 读取配置信息
                    this.m_ApiHostName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "ApiHostName",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_ApiHostName = StringHelper.NullOrEmptyTo(this.m_ApiHostName, KernelConfigurationView.Instance.HostName);

                    this.m_ApiHostName = this.m_ApiHostName.ToLower();
                }

                return this.m_ApiHostName;
            }
        }
        #endregion

        #region 属性:SessionTimerInterval
        private int m_SessionTimerInterval = -1;

        /// <summary>会话定时器执行时间间隔(单位:分钟)</summary>
        public int SessionTimerInterval
        {
            get
            {
                if (this.m_SessionTimerInterval == -1)
                {
                    // 读取配置信息
                    this.m_SessionTimerInterval = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionTimerInterval",
                        this.Configuration.Keys));

                    if (this.m_SessionTimerInterval == -1)
                    {
                        // 如果配置文件里没有设置，设置一个默认值。
                        this.m_SessionTimerInterval = 15;
                    }
                }

                return this.m_SessionTimerInterval;
            }
        }
        #endregion

        #region 属性:SessionTimeLimit
        private double m_SessionTimeLimit = 0;

        /// <summary>会话时间限制 (单位:秒)</summary>
        public double SessionTimeLimit
        {
            get
            {
                if (this.m_SessionTimeLimit == 0)
                {
                    // 读取配置信息
                    this.m_SessionTimeLimit = Convert.ToDouble(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionTimeLimit",
                        this.Configuration.Keys));

                    // 如果配置文件里未设置则设置一个默认值
                    if (this.m_SessionTimeLimit == 0)
                    {
                        this.m_SessionTimeLimit = 86400;
                    }
                }

                return this.m_SessionTimeLimit;
            }
        }
        #endregion

        #region 属性:TrackingCall
        private string m_TrackingCall = string.Empty;

        /// <summary>跟踪连接调用</summary>
        public string TrackingCall
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_TrackingCall))
                {
                    // 读取配置信息
                    this.m_TrackingCall = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "TrackingCall",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_TrackingCall = StringHelper.NullOrEmptyTo(this.m_TrackingCall, "NO");

                    this.m_TrackingCall = this.m_TrackingCall.ToUpper();
                }

                return this.m_TrackingCall;
            }
        }
        #endregion

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

        #region 属性:MessageQueueHostName
        private string m_MessageQueueHostName = string.Empty;

        /// <summary>消息队列机器名称</summary>
        public string MessageQueueHostName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueHostName))
                {
                    // 读取配置信息
                    this.m_MessageQueueHostName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueHostName",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_MessageQueueHostName = StringHelper.NullOrEmptyTo(this.m_MessageQueueHostName, MessageQueueConfigurationView.Instance.HostName);
                }

                return this.m_MessageQueueHostName;
            }
        }
        #endregion

        #region 属性:MessageQueuePort
        private int m_MessageQueuePort = 0;

        /// <summary>消息队列端口</summary>
        public int MessageQueuePort
        {
            get
            {
                if (this.m_MessageQueuePort == 0)
                {
                    // 读取配置信息
                    this.m_MessageQueuePort = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueuePort",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_MessageQueuePort == 0)
                    {
                        this.m_MessageQueuePort = MessageQueueConfigurationView.Instance.Port;
                    }
                }

                return this.m_MessageQueuePort;
            }
        }
        #endregion

        #region 属性:MessageQueueUsername
        private string m_MessageQueueUsername = string.Empty;

        /// <summary>消息队列名称</summary>
        public string MessageQueueUsername
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueueUsername))
                {
                    // 读取配置信息
                    this.m_MessageQueueUsername = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueUsername",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_MessageQueueUsername = StringHelper.NullOrEmptyTo(this.m_MessageQueueUsername, MessageQueueConfigurationView.Instance.Username);
                }

                return this.m_MessageQueueUsername;
            }
        }
        #endregion

        #region 属性:MessageQueuePassword
        private string m_MessageQueuePassword = string.Empty;

        /// <summary>消息队列名称</summary>
        public string MessageQueuePassword
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MessageQueuePassword))
                {
                    // 读取配置信息
                    this.m_MessageQueuePassword = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueuePassword",
                        this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_MessageQueuePassword = StringHelper.NullOrEmptyTo(this.m_MessageQueuePassword, MessageQueueConfigurationView.Instance.Password);
                }

                return this.m_MessageQueuePassword;
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
                    this.m_MessageQueueName = StringHelper.NullOrEmptyTo(this.m_MessageQueueName, "connect-call-queue");
                }

                return this.m_MessageQueueName;
            }
        }
        #endregion

        #region 属性:MessageQueueReceivingInterval
        private int m_MessageQueueReceivingInterval = 0;

        /// <summary>消息队列接收时间间隔(单位:秒)</summary>
        public int MessageQueueReceivingInterval
        {
            get
            {
                if (this.m_MessageQueueReceivingInterval == 0)
                {
                    // 读取配置信息
                    this.m_MessageQueueReceivingInterval = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "MessageQueueReceivingInterval",
                        this.Configuration.Keys));

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (this.m_MessageQueueReceivingInterval == 0)
                    {
                        this.m_MessageQueueReceivingInterval = 120;
                    }
                }

                return this.m_MessageQueueReceivingInterval;
            }
        }
        #endregion
    }
}
