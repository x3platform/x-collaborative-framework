namespace X3Platform.Plugins.Forum.Configuration
{
  #region Using Libraries
  using System;
  using System.Configuration;

  using X3Platform.Configuration;
  using X3Platform.Yaml.RepresentationModel;
  #endregion

  /// <summary>������Ϣ</summary>
  public class ForumConfiguration : XmlConfiguraton
  {
    /// <summary>����Ӧ�õ�����</summary>
    public const string ApplicationName = "Forum";

    /// <summary>�������������</summary>
    public const string SectionName = "forum";

    /// <summary>��ȡ������������</summary>
    public override string GetSectionName()
    {
      return SectionName;
    }

    #region ���캯��:NewsConfiguration()
    /// <summary></summary>
    public ForumConfiguration()
    {
      // �������� YAML ��Դ�����ļ���ʼ��������Ϣ

      var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
          this.GetType().Assembly,
          "X3Platform.Plugins.Forum.defaults.config.yaml");

      // ���� Keys ��ֵ������Ϣ
      YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

      this.Initialized = true;
    }
    #endregion
  }
}
