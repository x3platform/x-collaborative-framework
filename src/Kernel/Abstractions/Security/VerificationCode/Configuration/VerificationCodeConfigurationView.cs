namespace X3Platform.Security.VerificationCode.Configuration
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.IO;

    using X3Platform.Configuration;

    using X3Platform.Security.VerificationCode;
    #endregion

    /// <summary>权限配置视图</summary>
    public class VerificationCodeConfigurationView : XmlConfigurationView<VerificationCodeConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Security.VerificationCode.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = VerificationCodeConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile VerificationCodeConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static VerificationCodeConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new VerificationCodeConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:VerificationCodeConfigurationView()
        /// <summary>构造函数</summary>
        private VerificationCodeConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
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

        #region 属性:SendInterval
        private int m_SendInterval = -1;

        /// <summary>验证码发送时间间隔(单位:秒)</summary>
        public int SendInterval
        {
            get
            {
                if (this.m_SendInterval == -1)
                {
                    // 读取配置信息
                    this.m_SendInterval = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SendInterval",
                        this.Configuration.Keys));

                    if (this.m_SendInterval == -1)
                    {
                        // 如果配置文件里没有设置，设置一个默认值。
                        this.m_SendInterval = 120;
                    }
                }

                return this.m_SendInterval;
            }
        }
        #endregion

    }
}
