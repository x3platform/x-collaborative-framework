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

  /// <summary>������ͼ</summary>
  public class ForumConfigurationView : XmlConfigurationView<ForumConfiguration>
  {
    /// <summary>�����ļ���Ĭ��·��.</summary>
    private const string configFile = "config\\X3Platform.Plugins.Forum.config";

    /// <summary>������Ϣ��ȫ��ǰ׺</summary>
    private const string configGlobalPrefix = ForumConfiguration.ApplicationName;

    #region ��̬����::Instance
    private static volatile ForumConfigurationView instance = null;

    private static object lockObject = new object();

    /// <summary>ʵ��</summary>
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

    #region ���캯��:CostConfigurationView()
    /// <summary>���캯��</summary>
    private ForumConfigurationView()
      : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
    {
      // ��������Ϣ���ص�ȫ�ֵ�������
      KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
    }
    #endregion

    #region ����:Reload()
    /// <summary>���¼���������Ϣ</summary>
    public override void Reload()
    {
      base.Reload();

      // ��������Ϣ���ص�ȫ�ֵ�������
      KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
    }
    #endregion

    // -------------------------------------------------------
    // �Զ�������
    // -------------------------------------------------------

  }
}
