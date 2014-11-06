using X3Platform.Configuration;

namespace X3Platform.Plugins.Contacts.Configuration
{
    /// <summary>收藏夹插件的配置信息</summary>
    public class ContactConfiguration : XmlConfiguraton
    {
        /// <summary>应用名称</summary>
        public const string ApplicationName = "Contacts";

        /// <summary>配置区域的名称</summary>
        public const string SectionName = "plugins.contacts";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
