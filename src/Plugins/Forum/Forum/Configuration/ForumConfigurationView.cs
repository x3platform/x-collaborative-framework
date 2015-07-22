#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FilenName    :ForumConfigurationView.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Forum.Configuration
{
  #region Using Libraries
  using System;
  using System.Configuration;
  using System.IO;

  using X3Platform.Configuration;
  #endregion

  /// <summary>配置视图</summary>
  public class ForumConfigurationView : XmlConfigurationView<ForumConfiguration>
  {
    /// <summary>配置文件的默认路径.</summary>
    private const string configFile = "config\\X3Platform.Plugins.Forum.config";

    /// <summary>配置信息的全局前缀</summary>
    private const string configGlobalPrefix = ForumConfiguration.ApplicationName;

    #region 静态属性::Instance
    private static volatile ForumConfigurationView instance = null;

    private static object lockObject = new object();

    /// <summary>实例</summary>
    public static ForumConfigurationView Instance
    {
      get
      {
        if (instance == null)
        {
          lock (lockObject)
          {
            if (instance == null)
            {
              instance = new ForumConfigurationView();
            }
          }
        }

        return instance;
      }
    }
    #endregion

    #region 构造函数:CostConfigurationView()
    /// <summary>构造函数</summary>
    private ForumConfigurationView()
      : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
    {
      // 将配置信息加载到全局的配置中
      KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
    }
    #endregion

    #region 函数:Reload()
    /// <summary>重新加载配置信息</summary>
    public override void Reload()
    {
      base.Reload();

      // 将配置信息加载到全局的配置中
      KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
    }
    #endregion

    // -------------------------------------------------------
    // 自定义属性
    // -------------------------------------------------------

  }
}
