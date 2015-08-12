namespace X3Platform.Plugins.Bugs.Configuration
{
  using System;
  using System.IO;

  using X3Platform.Configuration;

  /// <summary>配置视图</summary>
  public class BugConfigurationView : XmlConfigurationView<BugConfiguration>
  {
    /// <summary>配置文件的默认路径.</summary>
    private const string configFile = "config\\X3Platform.Plugins.Bugs.config";

    /// <summary>配置信息的全局前缀</summary>
    private const string configGlobalPrefix = BugConfiguration.ApplicationName;

    #region 静态属性:Instance
    private static volatile BugConfigurationView instance = null;

    private static object lockObject = new object();

    /// <summary>实例</summary>
    public static BugConfigurationView Instance
    {
      get
      {
        if (instance == null)
        {
          lock (lockObject)
          {
            if (instance == null)
            {
              instance = new BugConfigurationView();
            }
          }
        }

        return instance;
      }
    }
    #endregion

    #region 构造函数:BugConfigurationView()
    /// <summary>构造函数</summary>
    private BugConfigurationView()
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

    #region 属性:DataTablePrefix
    private string m_DataTablePrefix = string.Empty;

    /// <summary>数据表前缀</summary>
    public string DataTablePrefix
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_DataTablePrefix))
        {
          // 属性名称
          string propertyName = "DataTablePrefix";
          // 属性全局名称
          string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

          if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
          {
            this.m_DataTablePrefix = KernelConfigurationView.Instance.ReplaceKeyValue(
                      KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
          }
          else if (this.Configuration.Keys[propertyName] != null)
          {
            this.m_DataTablePrefix = KernelConfigurationView.Instance.ReplaceKeyValue(
               this.Configuration.Keys[propertyName].Value);
          }

          // 如果配置文件里没有设置，设置一个默认值。
          if (string.IsNullOrEmpty(this.m_DataTablePrefix))
          {
            this.m_DataTablePrefix = "Empty";
          }
        }

        return this.m_DataTablePrefix == "Empty" ? string.Empty : this.m_DataTablePrefix;
      }
    }
    #endregion

    #region 属性:DigitalNumberEntityTableName
    private string m_DigitalNumberEntityTableName = string.Empty;

    /// <summary>编号的相关实体数据表</summary>
    public string DigitalNumberEntityTableName
    {
      get
      {
        if (string.IsNullOrEmpty(m_DigitalNumberEntityTableName))
        {
          // 读取配置全局信息
          this.m_DigitalNumberEntityTableName = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DigitalNumberEntityTableName", this.Configuration.Keys);

          // 如果配置文件里没有设置，设置一个默认值。
          if (string.IsNullOrEmpty(m_DigitalNumberEntityTableName))
          {
            m_DigitalNumberEntityTableName = "temp";
          }
        }

        return m_DigitalNumberEntityTableName;
      }
    }
    #endregion

    #region 属性:DigitalNumberPrefixCodeRule
    private string m_DigitalNumberPrefixCodeRule = string.Empty;

    /// <summary>编号的前缀编码规则</summary>
    public string DigitalNumberPrefixCodeRule
    {
      get
      {
        if (string.IsNullOrEmpty(m_DigitalNumberPrefixCodeRule))
        {
          // 读取配置全局信息
          this.m_DigitalNumberPrefixCodeRule = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DigitalNumberPrefixCodeRule", this.Configuration.Keys);

          // 如果配置文件里没有设置，设置一个默认值。
          if (string.IsNullOrEmpty(m_DigitalNumberPrefixCodeRule))
          {
            m_DigitalNumberPrefixCodeRule = "{ApplicationPinYin}{CorporationPinYin}{CategoryPrefixCode}";
          }
        }

        return m_DigitalNumberPrefixCodeRule;
      }
    }
    #endregion

    #region 属性:DigitalNumberIncrementCodeLength
    private int m_DigitalNumberIncrementCodeLength;

    /// <summary>编号的自增流水号长度</summary>
    public int DigitalNumberIncrementCodeLength
    {
      get
      {
        if (this.m_DigitalNumberIncrementCodeLength == 0)
        {
          // 读取配置全局信息
          this.m_DigitalNumberIncrementCodeLength = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DigitalNumberIncrementCodeLength", this.Configuration.Keys));

          // 如果配置文件里没有设置，设置一个默认值。
          if (this.m_DigitalNumberIncrementCodeLength == 0)
          {
            this.m_DigitalNumberIncrementCodeLength = 3;
          }
        }

        return this.m_DigitalNumberIncrementCodeLength;
      }
    }
    #endregion

    #region 属性:SendMailAlert
    private string m_SendMailAlert = string.Empty;

    /// <summary>发送邮件提醒</summary>
    public string SendMailAlert
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_SendMailAlert))
        {
          // 读取配置全局信息
          this.m_SendMailAlert = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SendMailAlert", this.Configuration.Keys);

          // 如果配置文件里没有设置，设置一个默认值。
          if (string.IsNullOrEmpty(this.m_SendMailAlert))
          {
            this.m_SendMailAlert = "Off";
          }

          this.m_SendMailAlert = this.m_SendMailAlert.ToUpper();
        }

        return this.m_SendMailAlert;
      }
    }
    #endregion

    #region 属性:ProjectDataTablePrefix
    private string m_ProjectDataTablePrefix = string.Empty;

    /// <summary>项目信息的数据表前缀</summary>
    public string ProjectDataTablePrefix
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_ProjectDataTablePrefix))
        {
          // 属性名称
          string propertyName = "ProjectDataTablePrefix";
          // 属性全局名称
          string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

          if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
          {
            this.m_ProjectDataTablePrefix = KernelConfigurationView.Instance.ReplaceKeyValue(
                      KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
          }
          else if (this.Configuration.Keys[propertyName] != null)
          {
            this.m_ProjectDataTablePrefix = KernelConfigurationView.Instance.ReplaceKeyValue(
               this.Configuration.Keys[propertyName].Value);
          }

          // 如果配置文件里没有设置，设置一个默认值。
          if (string.IsNullOrEmpty(this.m_ProjectDataTablePrefix))
          {
            this.m_ProjectDataTablePrefix = "Empty";
          }
        }

        return this.m_ProjectDataTablePrefix == "Empty" ? string.Empty : this.m_ProjectDataTablePrefix;
      }
    }
    #endregion
  }
}
