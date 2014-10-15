namespace X3Platform.Membership.HumanResources.Configuration
{
    using System.Configuration;

    using X3Platform.Configuration;

    /// <summary>配置信息</summary>
    public class HumanResourcesConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "HumanResources";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "humanResourcesConfiguration";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
