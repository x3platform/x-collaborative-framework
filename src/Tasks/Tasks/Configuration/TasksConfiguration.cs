namespace X3Platform.Tasks.Configuration
{
    #region Using Libraries
    using System.Configuration;

    using X3Platform.Configuration;
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
    }
}
