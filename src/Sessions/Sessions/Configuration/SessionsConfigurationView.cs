namespace X3Platform.Sessions.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Util;
    #endregion

    /// <summary>会话管理配置视图</summary>
    public class SessionsConfigurationView : XmlConfigurationView<SessionsConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Sessions.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = SessionsConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile SessionsConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static SessionsConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new SessionsConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:SessionsConfigurationView()
        /// <summary>构造函数</summary>
        private SessionsConfigurationView()
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

        #region 属性:SessionPersistentMode
        private string m_SessionPersistentMode = string.Empty;

        /// <summary>单点登录</summary>
        public string SessionPersistentMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SessionPersistentMode))
                {
                    // 读取配置信息
                    this.m_SessionPersistentMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionPersistentMode",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_SessionPersistentMode = StringHelper.NullOrEmptyTo(this.m_SessionPersistentMode, "Off");

                    this.m_SessionPersistentMode = this.m_SessionPersistentMode.ToUpper();
                }

                return this.m_SessionPersistentMode;
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
        private int m_SessionTimeLimit = -1;

        /// <summary>会话时间限制(单位:分钟)</summary>
        public int SessionTimeLimit
        {
            get
            {
                if (this.m_SessionTimeLimit == -1)
                {
                    // 读取配置信息
                    this.m_SessionTimeLimit = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionTimeLimit",
                        this.Configuration.Keys));

                    if (this.m_SessionTimeLimit == -1)
                    {
                        // 如果配置文件里没有设置，设置一个默认值。
                        this.m_SessionTimeLimit = 3600;
                    }
                }

                return this.m_SessionTimeLimit;
            }
        }
        #endregion
    }
}
