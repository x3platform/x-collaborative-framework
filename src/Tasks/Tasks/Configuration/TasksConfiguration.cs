namespace X3Platform.Tasks.Configuration
{
    #region Using Libraries
    using System.Configuration;

    using X3Platform.Configuration;
    using System.IO;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>任务配置信息</summary>
    public class TasksConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Tasks";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "tasks";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region 构造函数:TasksConfiguration()
        public TasksConfiguration()
        {
            // 根据内置 YAML 资源配置文件初始化对象信息

            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.Tasks.defaults.config.yaml");

            // 加载 Keys 键值配置信息
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
