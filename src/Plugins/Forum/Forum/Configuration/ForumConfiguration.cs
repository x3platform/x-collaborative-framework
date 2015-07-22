namespace X3Platform.Plugins.Forum.Configuration
{
  #region Using Libraries
  using System;
  using System.Configuration;

  using X3Platform.Configuration;
  using X3Platform.Yaml.RepresentationModel;
  #endregion

  /// <summary>配置信息</summary>
  public class ForumConfiguration : XmlConfiguraton
  {
    /// <summary>所属应用的名称</summary>
    public const string ApplicationName = "Forum";

    /// <summary>配置区域的名称</summary>
    public const string SectionName = "forum";

    /// <summary>获取配置区的名称</summary>
    public override string GetSectionName()
    {
      return SectionName;
    }

    #region 构造函数:NewsConfiguration()
    /// <summary></summary>
    public ForumConfiguration()
    {
      // 根据内置 YAML 资源配置文件初始化对象信息

      var root = YamlConfiguratonOperator.GetRootNodeByResourceStream<YamlMappingNode>(
          this.GetType().Assembly,
          "X3Platform.Plugins.Forum.defaults.config.yaml");

      // 加载 Keys 键值配置信息
      YamlConfiguratonOperator.SetKeyValues(this.Keys, (YamlMappingNode)root.Children[new YamlScalarNode("keys")]);

      this.Initialized = true;
    }
    #endregion
  }
}
