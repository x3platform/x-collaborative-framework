
namespace X3Platform.Navigation.Configuration
{
    using System;
    using System.IO;
    using X3Platform.Configuration;

    /// <summary>配置视图</summary>
    public class NavigationConfigurationView : XmlConfigurationView<NavigationConfiguration>
    {
        private const string configFile = "config\\X3Platform.Navigation.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = NavigationConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile NavigationConfigurationView instance = null;

        private static object lockObject = new object();

        public static NavigationConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new NavigationConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:NavigationConfigurationView()
        /// <summary>构造函数</summary>
        private NavigationConfigurationView()
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
