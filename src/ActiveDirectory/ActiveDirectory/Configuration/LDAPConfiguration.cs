namespace X3Platform.LDAP.Configuration
{
    #region Using Libraries
    using System.IO;
    
    using Common.Logging;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>LDAP 配置信息</summary>
    public class LDAPConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>配置区的名称</summary>
        public const string SectionName = "ldap";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        public LDAPConfiguration()
        {
            using (var stream = typeof(LDAPConfiguration).Assembly.GetManifestResourceStream("X3Platform.LDAP.defaults.config.yaml"))
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
