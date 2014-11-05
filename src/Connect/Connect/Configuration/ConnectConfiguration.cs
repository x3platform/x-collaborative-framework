namespace X3Platform.Connect.Configuration
{
    #region Using Libraries
    using System;
    using System.Configuration;

    using X3Platform.Configuration;
    using X3Platform.Yaml.RepresentationModel;
    #endregion

    /// <summary>Ӧ�����ӹ����������Ϣ</summary>
    public class ConnectConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Connect";

        /// <summary>�������������</summary>
        public const string SectionName = "connect";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }

        #region ���캯��:ConnectConfiguration()
        /// <summary></summary>
        public ConnectConfiguration()
        {
            // �������� YAML ��Դ�����ļ���ʼ��������Ϣ
            
            var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
                this.GetType().Assembly,
                "X3Platform.Connect.defaults.config.yaml");

            // ���� Keys ��ֵ������Ϣ
            YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

            this.Initialized = true;
        }
        #endregion
    }
}
