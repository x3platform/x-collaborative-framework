namespace X3Platform.Security.Configuration
{
    #region Using Libraries
    using System;
    using System.Xml;
    using System.IO;

    using Common.Logging;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>安全配置信息</summary>
    public class SecurityConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Security";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "security";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 构造函数:SecurityConfiguration()
        /// <summary></summary>
        public SecurityConfiguration()
        {
            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly, 
                "X3Platform.Security.defaults.config.yaml");

            // 加载 Keys 键值配置信息
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
