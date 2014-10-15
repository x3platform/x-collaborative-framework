#region Using Libraries
using System;
using System.Configuration;
using System.IO;

using X3Platform.Configuration;
using X3Platform.Util;
#endregion

namespace X3Platform.Apps.Configuration
{
    /// <summary>Ӧ��������ͼ</summary>
    public class AppsConfigurationView : XmlConfigurationView<AppsConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Apps.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = "Apps";

        #region ��̬����:Instance
        private static volatile AppsConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static AppsConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AppsConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:AppsConfigurationView()
        /// <summary>���캯��</summary>
        private AppsConfigurationView()
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

        #region ����:Administrators
        private string m_Administrators = string.Empty;

        /// <summary>ϵͳ��������Ա��Ϣ</summary>
        public string Administrators
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Administrators))
                {
                    // ��������
                    string propertyName = "Administrators";

                    // ����ȫ������
                    string propertyGlobalName = string.Format("{0}.{1}", configGlobalPrefix, propertyName);

                    if (KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName] != null)
                    {
                        this.m_Administrators = KernelConfigurationView.Instance.ReplaceKeyValue(
                             KernelConfigurationView.Instance.Configuration.Keys[propertyGlobalName].Value);
                    }
                    else if (this.Configuration.Keys[propertyName] != null)
                    {
                        this.m_Administrators = KernelConfigurationView.Instance.ReplaceKeyValue(
                             this.Configuration.Keys[propertyName].Value);
                    }

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_Administrators))
                    {
                        this.m_Administrators = "#";
                    }
                }

                return this.m_Administrators;
            }
        }
        #endregion

        #region ����:HiddenStartMenu
        private string m_HiddenStartMenu = string.Empty;

        /// <summary>���ؿ�ʼ�˵�</summary>
        public string HiddenStartMenu
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_HiddenStartMenu))
                {
                    // ��ȡ������Ϣ
                    this.m_HiddenStartMenu = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "HiddenStartMenu",
                        this.Configuration.Keys);

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_HiddenStartMenu = StringHelper.NullOrEmptyTo(this.m_HiddenStartMenu, "Off");

                    this.m_HiddenStartMenu = this.m_HiddenStartMenu.ToUpper();
                }

                return this.m_HiddenStartMenu;
            }
        }
        #endregion

        #region ����:HiddenTopMenu
        private string m_HiddenTopMenu = string.Empty;

        /// <summary>���ض����˵�</summary>
        public string HiddenTopMenu
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_HiddenTopMenu))
                {
                    // ��ȡ������Ϣ
                    this.m_HiddenTopMenu = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "HiddenTopMenu",
                        this.Configuration.Keys);

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_HiddenTopMenu = StringHelper.NullOrEmptyTo(this.m_HiddenTopMenu, "Off");

                    this.m_HiddenTopMenu = this.m_HiddenTopMenu.ToUpper();
                }

                return this.m_HiddenTopMenu;
            }
        }
        #endregion

        #region ����:HiddenShortcutMenu
        private string m_HiddenShortcutMenu = string.Empty;

        /// <summary>���ؿ�ݷ�ʽ�˵�</summary>
        public string HiddenShortcutMenu
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_HiddenShortcutMenu))
                {
                    // ��ȡ������Ϣ
                    this.m_HiddenShortcutMenu = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "HiddenShortcutMenu",
                        this.Configuration.Keys);

                    // ��������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_HiddenShortcutMenu = StringHelper.NullOrEmptyTo(this.m_HiddenShortcutMenu, "Off");

                    this.m_HiddenShortcutMenu = this.m_HiddenShortcutMenu.ToUpper();
                }

                return this.m_HiddenShortcutMenu;
            }
        }
        #endregion
    }
}
