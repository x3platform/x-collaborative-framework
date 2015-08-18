using System;
using System.Configuration;
using System.IO;

using X3Platform.Configuration;

namespace X3Platform.Email.Client.Configuration
{
  /// <summary>配置视图</summary>
  public class EmailClientConfigurationView : XmlConfigurationView<EmailClientConfiguration>
  {
    /// <summary>配置文件的默认路径.</summary>
    private const string configFile = "config\\X3Platform.Email.Client.config";

    /// <summary>配置信息的全局前缀</summary>
    private const string configGlobalPrefix = EmailClientConfiguration.ApplicationName;

    #region 静态属性:Instance
    private static volatile EmailClientConfigurationView instance = null;

    private static object lockObject = new object();

    /// <summary>实例</summary>
    public static EmailClientConfigurationView Instance
    {
      get
      {
        if (instance == null)
        {
          lock (lockObject)
          {
            if (instance == null)
            {
              instance = new EmailClientConfigurationView();
            }
          }
        }

        return instance;
      }
    }
    #endregion

    #region 构造函数:ConnectConfigurationView()
    /// <summary>构造函数</summary>
    private EmailClientConfigurationView()
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
  }
}
