
using System;
using System.Configuration;
using System.IO;

using X3Platform.Configuration;
using X3Platform.Util;

namespace X3Platform.Velocity.Configuration
{
    /// <summary>Velocity ��������ͼ</summary>
    public class VelocityConfigurationView : XmlConfigurationView<VelocityConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��.</summary>
        private const string configFile = "config\\X3Platform.Velocity.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = VelocityConfiguration.ApplicationName;

        #region ��̬����:Instance
        private static volatile VelocityConfigurationView instance = null;

        private static object lockObject = new object();

        public static VelocityConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new VelocityConfigurationView();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region ���캯��:VelocityConfigurationView()
        /// <summary>���캯��</summary>
        private VelocityConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
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

        #region ����:TemplatePath
        private string m_TemplatePath = string.Empty;

        /// <summary>ģ��·��</summary>
        public string TemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_TemplatePath))
                {
                    // ��ȡ������Ϣ
                    this.m_TemplatePath = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "TemplatePath",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_TemplatePath = StringHelper.NullOrEmptyTo(this.m_TemplatePath,
                        KernelConfigurationView.Instance.ApplicationPathRoot + "views\\templates");

                    if (Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        this.m_TemplatePath = this.m_TemplatePath.Replace("\\", "/");
                    }
                }

                return this.m_TemplatePath;
            }
        }
        #endregion

        #region ����:TemplateCacheMode
        private string m_TemplateCacheMode = string.Empty;

        /// <summary>ģ�建��״̬</summary>
        public string TemplateCacheMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_TemplateCacheMode))
                {
                    // ��ȡ������Ϣ
                    this.m_TemplateCacheMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "TemplateCacheMode",
                        this.Configuration.Keys);

                    // ��������ļ���δ����������һ��Ĭ��ֵ
                    this.m_TemplateCacheMode = StringHelper.NullOrEmptyTo(this.m_TemplateCacheMode, "Off");

                    this.m_TemplateCacheMode = this.m_TemplateCacheMode.ToUpper();
                }

                return this.m_TemplateCacheMode;
            }
        }
        #endregion
    }
}