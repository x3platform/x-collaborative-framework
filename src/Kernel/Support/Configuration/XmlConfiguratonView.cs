using System;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Globalization;

using Common.Logging;

namespace X3Platform.Configuration
{
  /// <summary>Xml������ͼ</summary>
  public abstract class XmlConfigurationView<T>
      where T : XmlConfiguraton, new()
  {
    /// <summary>��־��¼��</summary>
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    #region ����:ConfigFilePath
    /// <summary>�����ļ���·��</summary>
    protected string m_ConfigFilePath = null;

    /// <summary>�����ļ�������·��</summary>
    public string ConfigFilePath
    {
      get { return this.m_ConfigFilePath; }
    }
    #endregion

    #region ����:Configuration
    private T m_Configuration = new T();

    /// <summary>������Ϣ</summary>
    public T Configuration
    {
      get { return this.m_Configuration; }
    }
    #endregion

    #region ���캯��:XmlConfigurationView(string path)
    /// <summary>���캯��</summary>
    public XmlConfigurationView(string path)
    {
      this.m_ConfigFilePath = path;

      this.Load(this.ConfigFilePath);
    }
    #endregion

    #region ����:Reload()
    /// <summary>���¼���������Ϣ</summary>
    public virtual void Reload()
    {
      this.Load(this.ConfigFilePath);
    }
    #endregion

    #region �ڲ�����:Load(string path)
    /// <summary>����������Ϣ</summary>
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
