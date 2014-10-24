namespace X3Platform.DigitalNumber.Configuration
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
    public class DigitalNumberConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "DigitalNumber";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "digitalNumber";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
        
        #region 构造函数:DigitalNumberConfiguration()
        /// <summary></summary>
        public DigitalNumberConfiguration()
        {
            using (var stream = typeof(DigitalNumberConfiguration).Assembly.GetManifestResourceStream("X3Platform.DigitalNumber.defaults.config.yaml"))
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
