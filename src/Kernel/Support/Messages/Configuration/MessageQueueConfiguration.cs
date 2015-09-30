namespace X3Platform.MessageQueue.Configuration
{
    #region Using Libraries
    using System;
    using System.Xml;
    using System.IO;

    using Common.Logging;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>MessageQueue 配置信息</summary>
    public class MessageQueueConfiguration : XmlConfiguraton
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "MessageQueue";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "messageQueue";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 属性:SpecialWords
        private NameValueConfigurationCollection m_SpecialWords = new NameValueConfigurationCollection();

        /// <summary>特殊关键字集合</summary>
        public NameValueConfigurationCollection SpecialWords
        {
            get { return this.m_SpecialWords; }
            set { this.m_SpecialWords = value; }
        }
        #endregion

        #region 函数:Configure(XmlElement element)
        /// <summary>根据Xml元素配置对象信息</summary>
        /// <param name="element">配置节点的Xml元素</param>
        public override void Configure(XmlElement element)
        {
            base.Configure(element);

            // 加载 SpecialWords 键值配置信息
            XmlConfiguratonOperator.SetKeyValues(this.SpecialWords, element.SelectNodes(@"specialWords/add"));
        }
        #endregion

        #region 构造函数:MessageQueueConfiguration()
        public MessageQueueConfiguration()
        {
            using (var stream = typeof(MessageQueueConfiguration).Assembly.GetManifestResourceStream("X3Platform.Messages.defaults.config.yaml"))
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
