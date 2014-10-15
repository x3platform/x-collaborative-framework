namespace X3Platform.Membership.HumanResources.Configuration
{
    using System;
    using System.IO;

    using X3Platform.Configuration;

    /// <summary>配置视图</summary>
    public class HumanResourcesConfigurationView : XmlConfigurationView<HumanResourcesConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config/X3Platform.Membership.HumanResources.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = HumanResourcesConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static HumanResourcesConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static HumanResourcesConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new HumanResourcesConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:HumanResourcesConfigurationView()
        /// <summary>构造函数</summary>
        private HumanResourcesConfigurationView()
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

    }
}
