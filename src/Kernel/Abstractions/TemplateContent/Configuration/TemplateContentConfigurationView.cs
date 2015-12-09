namespace X3Platform.TemplateContent.Configuration
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Util;
    #endregion

    /// <summary>流水号配置视图</summary>
    public class TemplateContentConfigurationView : XmlConfigurationView<TemplateContentConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.TemplateContent.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = TemplateContentConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile TemplateContentConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static TemplateContentConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new TemplateContentConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:TemplateContentConfigurationView()
        /// <summary>构造函数</summary>
        private TemplateContentConfigurationView()
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

        #region 属性:IgnoreIncrementSeed
        private string m_IgnoreIncrementSeed = string.Empty;

        /// <summary>忽略自增因子的规则 一般适用于随机数和 GUID</summary>
        public string IgnoreIncrementSeed
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_IgnoreIncrementSeed))
                {
                    // 读取配置信息
                    this.m_IgnoreIncrementSeed = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "IgnoreIncrementSeed", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_IgnoreIncrementSeed = StringHelper.NullOrEmptyTo(this.m_IgnoreIncrementSeed, "Key_Guid");
                }

                return this.m_IgnoreIncrementSeed;
            }
        }
        #endregion
    }
}
