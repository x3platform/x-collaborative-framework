namespace X3Platform.SMS.Client.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Util;
    #endregion

    /// <summary>会话管理配置视图</summary>
    public class SMSConfigurationView : XmlConfigurationView<SMSConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.SMS.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = SMSConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile SMSConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static SMSConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new SMSConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:SMSConfigurationView()
        /// <summary>构造函数</summary>
        private SMSConfigurationView()
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
        #region 属性:ClientProvider
        private string m_ClientProvider = string.Empty;

        /// <summary>短信发送客户端</summary>
        public string ClientProvider
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_ClientProvider))
                {
                    // 读取配置信息
                    this.m_ClientProvider = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "ClientProvider",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_ClientProvider = StringHelper.NullOrEmptyTo(this.m_ClientProvider, "X3Platform.SMS.Client.Providers.NullShortMessageClientProvider,X3Platform.SMS.Client");
                }

                return this.m_ClientProvider;
            }
        }
        #endregion

        #region 属性:SendUrl
        private string m_SendUrl = string.Empty;

        /// <summary>发送地址</summary>
        public string SendUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SendUrl))
                {
                    // 读取配置信息
                    this.m_SendUrl = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SendUrl",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_SendUrl = StringHelper.NullOrEmptyTo(this.m_SendUrl, "http://localhost/api/sms.send");
                }

                return this.m_SendUrl;
            }
        }
        #endregion

        #region 属性:EnterpriseCode
        private string m_EnterpriseCode = string.Empty;

        /// <summary>企业编号</summary>
        public string EnterpriseCode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_EnterpriseCode))
                {
                    // 读取配置信息
                    this.m_EnterpriseCode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "EnterpriseCode",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_EnterpriseCode = StringHelper.NullOrEmptyTo(this.m_EnterpriseCode, "0");
                }

                return this.m_EnterpriseCode;
            }
        }
        #endregion

        #region 属性:Username
        private string m_Username = string.Empty;

        /// <summary>帐号</summary>
        public string Username
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Username))
                {
                    // 读取配置信息
                    this.m_Username = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "Username",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_Username = StringHelper.NullOrEmptyTo(this.m_Username, "{username}");
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
                    this.m_Password = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "Password",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_Password = StringHelper.NullOrEmptyTo(this.m_Password, "{password}");
                }

                return this.m_Password;
            }
        }
        #endregion

        #region 属性:SignName
        private string m_SignName = string.Empty;

        /// <summary>帐号</summary>
        public string SignName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SignName))
                {
                    // 读取配置信息
                    this.m_SignName = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SignName",
                        this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    this.m_SignName = StringHelper.NullOrEmptyTo(this.m_SignName, KernelConfigurationView.Instance.SystemName);
                }

                return this.m_SignName;
            }
        }
        #endregion
    }
}
