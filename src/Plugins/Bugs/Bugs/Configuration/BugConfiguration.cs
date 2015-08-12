namespace X3Platform.Plugins.Bugs.Configuration
{
  #region Using Libraries
  using System;
  using System.Configuration;
  using System.Xml;

  using Common.Logging;

  using X3Platform.Configuration;
  using X3Platform.Yaml.RepresentationModel;
  #endregion

  /// <summary>������ٵ�������Ϣ</summary>
  public class BugConfiguration : XmlConfiguraton
  {
    /// <summary>����Ӧ�õ�����</summary>
    public const string ApplicationName = "Bugs";

    /// <summary>�������������</summary>
    public const string SectionName = "bugs";

    /// <summary>��ȡ������������</summary>
    public override string GetSectionName()
    {
      return SectionName;
    }

    #region ���캯��:CostConfiguration()
    /// <summary></summary>
    public BugConfiguration()
    {
      // �������� YAML ��Դ�����ļ���ʼ��������Ϣ

      var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
          this.GetType().Assembly,
          "X3Platform.Plugins.Bugs.defaults.config.yaml");

      // ���� Keys ��ֵ������Ϣ
      YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

      this.Initialized = true;
    }
    #endregion
  }
}
