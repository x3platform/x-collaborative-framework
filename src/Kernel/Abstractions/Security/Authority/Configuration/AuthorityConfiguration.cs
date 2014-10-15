namespace X3Platform.Security.Authority.Configuration
{
    /// <summary>Ȩ��������Ϣ</summary>
    public class AuthorityConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Security.Authority";

        /// <summary>������������</summary>
        public const string SectionName = "security.authority";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
