namespace X3Platform.Membership.HumanResources.Configuration
{
    using System.Configuration;

    using X3Platform.Configuration;

    /// <summary>������Ϣ</summary>
    public class HumanResourcesConfiguration : XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "HumanResources";

        /// <summary>������������</summary>
        public const string SectionName = "humanResourcesConfiguration";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
