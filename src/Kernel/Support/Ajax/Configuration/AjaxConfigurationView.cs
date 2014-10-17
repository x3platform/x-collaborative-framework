namespace X3Platform.Ajax.Configuration
{
    #region Using Libraries
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>Ajax 配置视图</summary>
    public class AjaxConfigurationView : XmlConfigurationView<AjaxConfiguration>
    {
        /// <summary>配置文件的默认路径</summary>
        private const string configFile = "config\\X3Platform.Ajax.config";

        /// <summary>配置信息的全局前缀</summary>
        private const string configGlobalPrefix = AjaxConfiguration.ApplicationName;

        #region 静态属性:Instance
        private static volatile AjaxConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static AjaxConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AjaxConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 构造函数:AjaxConfigurationView()
        /// <summary>构造函数</summary>
        private AjaxConfigurationView()
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

        #region 属性:CamelStyle
        private string m_CamelStyle = string.Empty;

        /// <summary>CamelStyle Camel样式, On : 启用 | Off : 禁用</summary>
        public string CamelStyle
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_CamelStyle))
                {
                    // 读取配置信息
                    this.m_CamelStyle = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "CamelStyle", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    if (string.IsNullOrEmpty(this.m_CamelStyle))
                    {
                        this.m_CamelStyle = "Off";
                    }

                    this.m_CamelStyle = this.m_CamelStyle.ToUpper();
                }

                return this.m_CamelStyle;
            }
        }
        #endregion
    }
}
