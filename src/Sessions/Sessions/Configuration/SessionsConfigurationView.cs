#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :SessionsConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

#region Using Libraries
using System;
using System.IO;

using X3Platform.Configuration;
using X3Platform.Util;
#endregion

namespace X3Platform.Sessions.Configuration
{
    /// <summary>Ӧ��������ͼ</summary>
    public class SessionsConfigurationView : XmlConfigurationView<SessionsConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Sessions.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = SessionsConfiguration.ApplicationName;

        #region 静态属性::Instance
        private static volatile SessionsConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static SessionsConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new SessionsConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:SessionsConfigurationView()
        /// <summary>���캯��</summary>
        private SessionsConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        #region 属性:Reload()
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

        #region 属性:SessionPersistentMode
        private string m_SessionPersistentMode = string.Empty;

        /// <summary>������¼</summary>
        public string SessionPersistentMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SessionPersistentMode))
                {
                    // ��ȡ������Ϣ
                    this.m_SessionPersistentMode = KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionPersistentMode",
                        this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    this.m_SessionPersistentMode = StringHelper.NullOrEmptyTo(this.m_SessionPersistentMode, "Off");

                    this.m_SessionPersistentMode = this.m_SessionPersistentMode.ToUpper();
                }

                return this.m_SessionPersistentMode;
            }
        }
        #endregion

        #region 属性:SessionTimerInterval
        private int m_SessionTimerInterval = -1;

        /// <summary>�Ự��ʱ��ִ��ʱ������(��λ:����)</summary>
        public int SessionTimerInterval
        {
            get
            {
                if (this.m_SessionTimerInterval == -1)
                {
                    // ��ȡ������Ϣ
                    this.m_SessionTimerInterval = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionTimerInterval",
                        this.Configuration.Keys));

                    if (this.m_SessionTimerInterval == -1)
                    {
                        // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                        this.m_SessionTimerInterval = 15;
                    }
                }

                return this.m_SessionTimerInterval;
            }
        }
        #endregion

        #region 属性:SessionTimeLimit
        private int m_SessionTimeLimit = -1;

        /// <summary>�Ựʱ������(��λ:����)</summary>
        public int SessionTimeLimit
        {
            get
            {
                if (this.m_SessionTimeLimit == -1)
                {
                    // ��ȡ������Ϣ
                    this.m_SessionTimeLimit = Convert.ToInt32(KernelConfigurationView.Instance.GetKeyValue(
                        configGlobalPrefix,
                        "SessionTimeLimit",
                        this.Configuration.Keys));

                    if (this.m_SessionTimeLimit == -1)
                    {
                        // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                        this.m_SessionTimeLimit = 3600;
                    }
                }

                return this.m_SessionTimeLimit;
            }
        }
        #endregion
    }
}
