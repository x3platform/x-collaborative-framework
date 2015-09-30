namespace X3Platform.MessageQueue.Configuration
{
    #region Using Libraries
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    using X3Platform.Util;
    using System;
    #endregion

    /// <summary>MessageQueue 配置视图</summary>
    public class MessageQueueConfigurationView : XmlConfigurationView<MessageQueueConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.MessageQueue.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = MessageQueueConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile MessageQueueConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static MessageQueueConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new MessageQueueConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:MessageQueueConfigurationView()
        /// <summary>构造函数</summary>
        private MessageQueueConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // 基类初始化后会默认执行 Reload() 函数
        }
        #endregion

        #region 属性:Reload()
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

        #region 属性:HostName
        private string m_HostName = string.Empty;

        /// <summary>主机名</summary>
        public string HostName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_HostName))
                {
                    // 读取配置信息
                    this.m_HostName = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "HostName", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_HostName = StringHelper.NullOrEmptyTo(this.m_HostName, "localhost");
                }

                return this.m_HostName;
            }
        }
        #endregion

        #region 属性:Port
        private int m_Port = 0;

        /// <summary>端口</summary>
        public int Port
        {
            get
            {
                if (this.m_Port == 0)
                {
                    // 读取配置信息
                    this.m_Port = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "Port", this.Configuration.Keys));

                    // 如果配置文件里未设置则设置一个默认值
                    if (this.m_Port == 0)
                    {
                        this.m_Port = 5672;
                    }
                }

                return this.m_Port;
            }
        }
        #endregion

        #region 属性:Username
        private string m_Username = string.Empty;

        /// <summary>用户名</summary>
        public string Username
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Username))
                {
                    // 读取配置信息
                    this.m_Username = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "Username", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_Username = StringHelper.NullOrEmptyTo(this.m_Username, "guest");
                }

                return this.m_Username;
            }
        }
        #endregion

        #region 属性:Password
        private string m_Password = string.Empty;

        /// <summary>密码</summary>
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Password))
                {
                    // 读取配置信息
                    this.m_Password = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "Password", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_Password = StringHelper.NullOrEmptyTo(this.m_Username, "guest");
                }

                return this.m_Password;
            }
        }
        #endregion
    }
}
