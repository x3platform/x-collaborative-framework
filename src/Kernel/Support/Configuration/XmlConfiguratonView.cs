using System;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Globalization;

using Common.Logging;

namespace X3Platform.Configuration
{
  /// <summary>Xml配置视图</summary>
  public abstract class XmlConfigurationView<T>
      where T : XmlConfiguraton, new()
  {
    /// <summary>日志记录器</summary>
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    #region 属性:ConfigFilePath
    /// <summary>配置文件的路径</summary>
    protected string m_ConfigFilePath = null;

    /// <summary>配置文件的完整路径</summary>
    public string ConfigFilePath
    {
      get { return this.m_ConfigFilePath; }
    }
    #endregion

    #region 属性:Configuration
    private T m_Configuration = new T();

    /// <summary>配置信息</summary>
    public T Configuration
    {
      get { return this.m_Configuration; }
    }
    #endregion

    #region 构造函数:XmlConfigurationView(string path)
    /// <summary>构造函数</summary>
    public XmlConfigurationView(string path)
    {
      this.m_ConfigFilePath = path;

      this.Load(this.ConfigFilePath);
    }
    #endregion

    #region 函数:Reload()
    /// <summary>重新加载配置信息</summary>
    public virtual void Reload()
    {
      this.Load(this.ConfigFilePath);
    }
    #endregion

    #region 内部函数:Load(string path)
    /// <summary>加载配置信息</summary>
    protected virtual void Load(string path)
    {
      if (Environment.OSVersion.Platform == PlatformID.Unix)
      {
        path = path.Replace("\\", "/");
      }

      if (File.Exists(path))
      {
        if (logger.IsDebugEnabled)
        {
          logger.Debug("configuring file " + path);
        }

        string configNodeXPath = string.Format(@"configuration/{0}", this.m_Configuration.GetSectionName());

        this.m_Configuration.Configure(path, configNodeXPath);

        this.m_Configuration.Initialized = true;
      }
      else
      {
        if (!this.m_Configuration.Initialized)
        {
          logger.Error("file not found - " + path);
        }
      }
    }
    #endregion
  }
}
