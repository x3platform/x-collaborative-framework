namespace X3Platform.CacheBuffer.Configuration
{
    #region Using Libraries
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    using X3Platform.Util;
    #endregion

    /// <summary>缓存配置视图</summary>
    public class CacheBufferConfigurationView : XmlConfigurationView<CacheBufferConfiguration>
    {
        private const string configFile = "config\\X3Platform.CacheBuffer.config";
        
        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = CacheBufferConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile CacheBufferConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static CacheBufferConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new CacheBufferConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:CacheBufferConfigurationView()
        /// <summary>构造函数</summary>
        private CacheBufferConfigurationView()
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

        #region 属性:CacheBufferProvider
        private string m_CacheBufferProvider = string.Empty;

        /// <summary>缓存提供器, MemoryCache | MemCache</summary>
        public string CacheBufferProvider
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_CacheBufferProvider))
                {
                    // 读取配置信息
                    this.m_CacheBufferProvider = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "CacheBufferProvider", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_CacheBufferProvider = StringHelper.NullOrEmptyTo(this.m_CacheBufferProvider, "Off");

                    this.m_CacheBufferProvider = this.m_CacheBufferProvider.ToUpper();
                }

                return this.m_CacheBufferProvider;
            }
        }
        #endregion
    }
}
