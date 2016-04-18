namespace X3Platform.Ajax.Configuration
{
    #region Using Libraries
    using System.IO;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    using X3Platform.Util;
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

        #region 属性:NamingRule
        private string m_NamingRule = string.Empty;

        /// <summary>命名规则 camel 驼峰命名规则 首字母小写其余单词的首字母大写 | underline 下划线命名规则 所有字母小写加下划线分隔</summary>
        public string NamingRule
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_NamingRule))
                {
                    // 读取配置信息
                    this.m_NamingRule = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "NamingRule", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_NamingRule = StringHelper.NullOrEmptyTo(this.m_NamingRule, "camel");
                }

                return this.m_NamingRule;
            }
        }
        #endregion

        #region 属性:DateTimeSerializer
        private string m_DateTimeSerializer = string.Empty;

        /// <summary>命名规则 camel 驼峰命名规则 首字母小写其余单词的首字母大写 | underline 下划线命名规则 所有字母小写加下划线分隔</summary>
        public string DateTimeSerializer
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_DateTimeSerializer))
                {
                    // 读取配置信息
                    this.m_DateTimeSerializer = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DateTimeSerializer", this.Configuration.Keys);

                    // 如果配置文件里未设置则设置一个默认值
                    this.m_DateTimeSerializer = StringHelper.NullOrEmptyTo(this.m_DateTimeSerializer, "X3Platform.Ajax.AjaxDateTimeSerializer,X3Platform.Support");
                }

                return this.m_DateTimeSerializer;
            }
        }
        #endregion
    }
}
