namespace X3Platform.ActiveDirectory.Configuration
{
    #region Using Libraries
    using System.IO;
    
    using Common.Logging;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>ActiveDirectory 配置信息</summary>
    public class ActiveDirectoryConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>配置区的名称</summary>
        public const string SectionName = "activeDirectory";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        public ActiveDirectoryConfiguration()
        {
            using (var stream = typeof(ActiveDirectoryConfiguration).Assembly.GetManifestResourceStream("X3Platform.ActiveDirectory.defaults.config.yaml"))
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
                }
            }

            this.Initialized = true;
        }
    }
}
