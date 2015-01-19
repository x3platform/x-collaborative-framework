namespace X3Platform.Security.VerificationCode.Configuration
{
    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;

    /// <summary>Ȩ��������Ϣ</summary>
    public class VerificationCodeConfiguration : XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Security.VerificationCode";

        /// <summary>������������</summary>
        public const string SectionName = "security.verificationCode";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region ���캯��:VerificationCodeConfiguration()
        /// <summary></summary>
        public VerificationCodeConfiguration()
        {
            // �������� YAML ��Դ�����ļ���ʼ��������Ϣ
            
            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.Security.VerificationCode.defaults.config.yaml");

            // ���� Keys ��ֵ������Ϣ
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
