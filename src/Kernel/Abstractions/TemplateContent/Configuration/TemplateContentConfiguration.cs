namespace X3Platform.TemplateContent.Configuration
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Xml;

    using Common.Logging;

    using X3Platform.Configuration;
    using System.IO;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>流水号配置信息</summary>
    public class TemplateContentConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "TemplateContent";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "templateContent";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
        
        #region 构造函数:TemplateContentConfiguration()
        /// <summary></summary>
        public TemplateContentConfiguration()
        {
            // 根据内置 YAML 资源配置文件初始化对象信息
            
            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.TemplateContent.defaults.config.yaml");

            // 加载 Keys 键值配置信息
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
