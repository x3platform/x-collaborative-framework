namespace X3Platform.Connect.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>配置视图</summary>
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

        #region 属性:SessionTimeLimit
        private int m_SessionTimeLimit = 0;

        /// <summary>会话时间限制 (单位:秒)</summary>
        public int SessionTimeLimit
        {
            get
            {
                if (this.m_SessionTimeLimit == 0)
                {
                    // 读取配置信息
                    this.m_SessionTimeLimit = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
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
    }
}
