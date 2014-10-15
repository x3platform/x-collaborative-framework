namespace X3Platform.Connect.Configuration
{
    using System.Configuration;

    using X3Platform.Configuration;

    /// <summary>Ӧ�����ӹ����������Ϣ</summary>
    public class ConnectConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Connect";

        /// <summary>�������������</summary>
        public const string SectionName = "connectConfiguration";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
