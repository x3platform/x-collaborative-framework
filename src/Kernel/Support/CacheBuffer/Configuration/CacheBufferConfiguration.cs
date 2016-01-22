namespace X3Platform.CacheBuffer.Configuration
{
    #region Using Libraries
    using System;
    using System.Xml;
    using System.IO;

    using Common.Logging;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>缓存配置信息</summary>
    public class CacheBufferConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "CacheBuffer";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "cacheBuffer";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 构造函数:CacheBufferConfiguration()
        /// <summary></summary>
        public CacheBufferConfiguration()
        {
            using (var stream = typeof(CacheBufferConfiguration).Assembly.GetManifestResourceStream("X3Platform.CacheBuffer.defaults.config.yaml"))
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

            this.Initialized = true;
        }
        #endregion
    }
}
