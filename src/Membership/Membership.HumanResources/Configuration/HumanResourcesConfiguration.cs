namespace X3Platform.Membership.HumanResources.Configuration
{
    using System.Configuration;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;

    /// <summary>配置信息</summary>
    public class HumanResourcesConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "HumanResources";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "humanResources";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
        
        #region 构造函数:MembershipConfiguration()
        public HumanResourcesConfiguration()
        {
            // 根据内置 YAML 资源配置文件初始化对象信息

            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.Membership.HumanResources.defaults.config.yaml");

            // 加载 Keys 键值配置信息
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
