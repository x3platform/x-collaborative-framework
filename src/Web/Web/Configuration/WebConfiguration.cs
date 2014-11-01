namespace X3Platform.Web.Configuration
{
    #region Using Libraries
    using System;
    using System.Xml;

    using Common.Logging;

    using X3Platform.Configuration;
    using System.IO;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>Web配置信息</summary>
    public class WebConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Web";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "web";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 属性:Customize
        private NameValueConfigurationCollection m_Customize = new NameValueConfigurationCollection();

        /// <summary></summary>
        public NameValueConfigurationCollection Customize
        {
            get { return this.m_Customize; }
        }
        #endregion

        #region 属性:Menu
        private NameValueConfigurationCollection m_Menu = new NameValueConfigurationCollection();

        /// <summary></summary>
        public NameValueConfigurationCollection Menu
        {
            get { return this.m_Menu; }
        }
        #endregion

        #region 属性:Navigation
        private NameValueConfigurationCollection m_Navigation = new NameValueConfigurationCollection();

        /// <summary></summary>
        public NameValueConfigurationCollection Navigation
        {
            get { return this.m_Navigation; }
        }
        #endregion

        #region 函数:Configure(XmlElement element)
        /// <summary>根据Xml元素配置对象信息</summary>
        /// <param name="element">配置节点的Xml元素</param>
        public override void Configure(XmlElement element)
        {
            base.Configure(element);

            // 加载 Customize 配置信息
            XmlConfiguratonOperator.SetKeyValues(this.Customize, element.SelectNodes(@"customize/add"));

            // 加载 Menu 配置信息
            XmlConfiguratonOperator.SetKeyValues(this.Menu, element.SelectNodes(@"menu/add"));

            // 加载 Navigation 配置信息
            XmlConfiguratonOperator.SetKeyValues(this.Navigation, element.SelectNodes(@"navigation/add"));
        }
        #endregion

        #region 构造函数:WebConfiguration()
        public WebConfiguration()
        {
            using (var stream = typeof(WebConfiguration).Assembly.GetManifestResourceStream("X3Platform.Web.defaults.config.yaml"))
            {
                using (var reader = new StreamReader(stream))
                {
                    // 加载内置配置信息
                    var yaml = new YamlStream();

                    yaml.Load(reader);

                    // 设置配置信息根节点
                    var root = (YamlMappingNode)yaml.Documents[0].RootNode;

                    // 加载 Keys 键值配置信息
                    YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

                    // 加载 Navigation 键值配置信息
                    YamlConfiguratonOperator.SetKeyValues(this.Navigation, (YamlMappingNode)root.Children[new YamlScalarNode("navigation")]);

                    // 加载 Menu 键值配置信息
                    YamlConfiguratonOperator.SetKeyValues(this.Menu, (YamlMappingNode)root.Children[new YamlScalarNode("menu")]);

                    // 加载 Customize 键值配置信息
                    YamlConfiguratonOperator.SetKeyValues(this.Customize, (YamlMappingNode)root.Children[new YamlScalarNode("customize")]);

                    this.Initialized = true;
                }
            }

            this.Initialized = true;
        }
        #endregion
    }
}
