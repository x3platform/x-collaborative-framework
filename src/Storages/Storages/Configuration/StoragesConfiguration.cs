namespace X3Platform.Storages.Configuration
{
    #region Using Libraries
    using System.Configuration;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    using System.IO;
    #endregion

    /// <summary>应用存储配置信息</summary>
    public class StoragesConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Storages";
        
        /// <summary>配置区的名称</summary>
        public const string SectionName = "storages";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 构造函数:StoragesConfiguration()
        /// <summary></summary>
        public StoragesConfiguration()
        {
            using (var stream = typeof(StoragesConfiguration).Assembly.GetManifestResourceStream("X3Platform.Storages.defaults.config.yaml"))
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
