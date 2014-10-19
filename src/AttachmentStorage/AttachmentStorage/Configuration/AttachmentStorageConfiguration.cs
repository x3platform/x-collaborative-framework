namespace X3Platform.AttachmentStorage.Configuration
{
    /// <summary>附件存储管理配置信息</summary>
    public class AttachmentStorageConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "AttachmentStorage";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "attachmentStorage";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
