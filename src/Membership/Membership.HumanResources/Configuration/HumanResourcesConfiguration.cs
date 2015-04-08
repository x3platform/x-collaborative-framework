namespace X3Platform.Membership.HumanResources.Configuration
{
    using System.Configuration;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;

    /// <summary>������Ϣ</summary>
    public class HumanResourcesConfiguration : XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "HumanResources";

        /// <summary>������������</summary>
        public const string SectionName = "humanResources";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
        
        #region ���캯��:MembershipConfiguration()
        public HumanResourcesConfiguration()
        {
            // �������� YAML ��Դ�����ļ���ʼ��������Ϣ

            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.Membership.HumanResources.defaults.config.yaml");

            // ���� Keys ��ֵ������Ϣ
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
