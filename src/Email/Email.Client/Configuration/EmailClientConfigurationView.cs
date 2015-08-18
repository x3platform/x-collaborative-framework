using System;
using System.Configuration;
using System.IO;

using X3Platform.Configuration;

namespace X3Platform.Email.Client.Configuration
{
  /// <summary>������ͼ</summary>
  public class EmailClientConfigurationView : XmlConfigurationView<EmailClientConfiguration>
  {
    /// <summary>�����ļ���Ĭ��·��.</summary>
    private const string configFile = "config\\X3Platform.Email.Client.config";

    /// <summary>������Ϣ��ȫ��ǰ׺</summary>
    private const string configGlobalPrefix = EmailClientConfiguration.ApplicationName;

    #region ��̬����:Instance
    private static volatile EmailClientConfigurationView instance = null;

    private static object lockObject = new object();

    /// <summary>ʵ��</summary>
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

    #region ���캯��:ConnectConfigurationView()
    /// <summary>���캯��</summary>
    private EmailClientConfigurationView()
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
  }
}
