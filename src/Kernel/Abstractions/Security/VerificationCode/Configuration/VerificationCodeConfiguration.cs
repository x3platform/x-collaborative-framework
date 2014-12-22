namespace X3Platform.Security.Authority.Configuration
{
    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;

    /// <summary>权限配置信息</summary>
    public class AuthorityConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Security.Authority";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "security.authority";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 构造函数:AuthorityConfiguration()
        /// <summary></summary>
        public AuthorityConfiguration()
        {
            // 根据内置 YAML 资源配置文件初始化对象信息
            
            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.Security.Authority.defaults.config.yaml");

            // 加载 Keys 键值配置信息
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
