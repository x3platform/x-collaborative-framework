namespace X3Platform.Entities.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;

    /// <summary>配置视图</summary>
    public class EntitiesConfigurationView : XmlConfigurationView<EntitiesConfiguration>
    {
        /// <summary>配置文件的默认路径.</summary>
        private const string configFile = "config\\X3Platform.Entities.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = EntitiesConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile EntitiesConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static EntitiesConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new EntitiesConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:EntitiesConfigurationView()
        /// <summary>构造函数</summary>
        private EntitiesConfigurationView()
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
    }
}
