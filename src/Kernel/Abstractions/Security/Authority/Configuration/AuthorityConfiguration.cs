namespace X3Platform.Security.Authority.Configuration
{
    /// <summary>权限配置信息</summary>
    public class AuthorityConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Security.Authority";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "security.authority";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
