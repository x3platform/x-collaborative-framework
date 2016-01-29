namespace X3Platform.Location.Regions.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;

    /// <summary>配置视图</summary>
    public class RegionsConfigurationView : XmlConfigurationView<RegionsConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Location.Regions.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = RegionsConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile RegionsConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static RegionsConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new RegionsConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:RegionsConfigurationView()
        /// <summary>构造函数</summary>
        private RegionsConfigurationView()
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

        #region 属性:DB_CONFIG
        private string m_DB_CONFIG = string.Empty;

        /// <summary>友好的地址重写</summary>
        public string DB_CONFIG
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DB_CONFIG))
                {
                    // 读取配置全局信息
                    this.m_DB_CONFIG = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DB_CONFIG", this.Configuration.Keys);

                    // 如果配置文件里没有设置，设置一个默认值。
                    if (string.IsNullOrEmpty(this.m_DB_CONFIG))
                    {
                        this.m_DB_CONFIG = "#";
                    }

                    this.m_DB_CONFIG = this.m_DB_CONFIG.ToUpper();
                }

                return this.m_DB_CONFIG;
            }
        }
        #endregion
    }
}
