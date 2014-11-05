namespace X3Platform.Security.Authority.Configuration
{
    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;

    /// <summary>Ȩ��������Ϣ</summary>
    public class AuthorityConfiguration : XmlConfiguraton
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

        #region ���캯��:AuthorityConfiguration()
        /// <summary></summary>
        public AuthorityConfiguration()
        {
            // �������� YAML ��Դ�����ļ���ʼ��������Ϣ
            
            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.Security.Authority.defaults.config.yaml");

            // ���� Keys ��ֵ������Ϣ
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
