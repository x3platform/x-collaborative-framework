namespace X3Platform.Plugins.Bugs.Configuration
{
  using System;
  using System.IO;

  using X3Platform.Configuration;

  /// <summary>������ͼ</summary>
  public class BugConfigurationView : XmlConfigurationView<BugConfiguration>
  {
    /// <summary>�����ļ���Ĭ��·��.</summary>
    private const string configFile = "config\\X3Platform.Plugins.Bugs.config";

    /// <summary>������Ϣ��ȫ��ǰ׺</summary>
    private const string configGlobalPrefix = BugConfiguration.ApplicationName;

    #region ��̬����:Instance
    private static volatile BugConfigurationView instance = null;

    private static object lockObject = new object();

    /// <summary>ʵ��</summary>
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

    #region ���캯��:BugConfigurationView()
    /// <summary>���캯��</summary>
    private BugConfigurationView()
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

    #region ����:DataTablePrefix
    private string m_DataTablePrefix = string.Empty;

    /// <summary>���ݱ�ǰ׺</summary>
    public string DataTablePrefix
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_DataTablePrefix))
        {
          // ��������
          string propertyName = "DataTablePrefix";
          // ����ȫ������
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

          // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
          if (string.IsNullOrEmpty(this.m_DataTablePrefix))
          {
            this.m_DataTablePrefix = "Empty";
          }
        }

        return this.m_DataTablePrefix == "Empty" ? string.Empty : this.m_DataTablePrefix;
      }
    }
    #endregion

    #region ����:DigitalNumberEntityTableName
    private string m_DigitalNumberEntityTableName = string.Empty;

    /// <summary>��ŵ����ʵ�����ݱ�</summary>
    public string DigitalNumberEntityTableName
    {
      get
      {
        if (string.IsNullOrEmpty(m_DigitalNumberEntityTableName))
        {
          // ��ȡ����ȫ����Ϣ
          this.m_DigitalNumberEntityTableName = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DigitalNumberEntityTableName", this.Configuration.Keys);

          // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
          if (string.IsNullOrEmpty(m_DigitalNumberEntityTableName))
          {
            m_DigitalNumberEntityTableName = "temp";
          }
        }

        return m_DigitalNumberEntityTableName;
      }
    }
    #endregion

    #region ����:DigitalNumberPrefixCodeRule
    private string m_DigitalNumberPrefixCodeRule = string.Empty;

    /// <summary>��ŵ�ǰ׺�������</summary>
    public string DigitalNumberPrefixCodeRule
    {
      get
      {
        if (string.IsNullOrEmpty(m_DigitalNumberPrefixCodeRule))
        {
          // ��ȡ����ȫ����Ϣ
          this.m_DigitalNumberPrefixCodeRule = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DigitalNumberPrefixCodeRule", this.Configuration.Keys);

          // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
          if (string.IsNullOrEmpty(m_DigitalNumberPrefixCodeRule))
          {
            m_DigitalNumberPrefixCodeRule = "{ApplicationPinYin}{CorporationPinYin}{CategoryPrefixCode}";
          }
        }

        return m_DigitalNumberPrefixCodeRule;
      }
    }
    #endregion

    #region ����:DigitalNumberIncrementCodeLength
    private int m_DigitalNumberIncrementCodeLength;

    /// <summary>��ŵ�������ˮ�ų���</summary>
    public int DigitalNumberIncrementCodeLength
    {
      get
      {
        if (this.m_DigitalNumberIncrementCodeLength == 0)
        {
          // ��ȡ����ȫ����Ϣ
          this.m_DigitalNumberIncrementCodeLength = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "DigitalNumberIncrementCodeLength", this.Configuration.Keys));

          // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
          if (this.m_DigitalNumberIncrementCodeLength == 0)
          {
            this.m_DigitalNumberIncrementCodeLength = 3;
          }
        }

        return this.m_DigitalNumberIncrementCodeLength;
      }
    }
    #endregion

    #region ����:SendMailAlert
    private string m_SendMailAlert = string.Empty;

    /// <summary>�����ʼ�����</summary>
    public string SendMailAlert
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_SendMailAlert))
        {
          // ��ȡ����ȫ����Ϣ
          this.m_SendMailAlert = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "SendMailAlert", this.Configuration.Keys);

          // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
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

    #region ����:ProjectDataTablePrefix
    private string m_ProjectDataTablePrefix = string.Empty;

    /// <summary>��Ŀ��Ϣ�����ݱ�ǰ׺</summary>
    public string ProjectDataTablePrefix
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_ProjectDataTablePrefix))
        {
          // ��������
          string propertyName = "ProjectDataTablePrefix";
          // ����ȫ������
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

          // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
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
