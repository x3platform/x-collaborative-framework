namespace X3Platform.Apps.Configuration
{
    #region Using Libraries
    using System.Configuration;
    using System.IO;

    using X3Platform.Configuration;    
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>任务配置信息</summary>
    public class AppsConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "ApplicationManagement";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "apps";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 构造函数:AppsConfiguration()
        public AppsConfiguration()
        {
            using (var stream = typeof(AppsConfiguration).Assembly.GetManifestResourceStream("X3Platform.Apps.defaults.config.yaml"))
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

                    this.Initialized = true;
                }
            }
        }
        #endregion
    }
}
