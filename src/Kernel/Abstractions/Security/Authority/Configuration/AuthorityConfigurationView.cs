// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :LoggingConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Configuration;
using System.IO;

using X3Platform.Configuration;

using X3Platform.Security.Authority.Configuration;

namespace X3Platform.Security.Authority
{
    /// <summary>Ȩ��������ͼ</summary>
    public class AuthorityConfigurationView
    {
        #region 静态属性:Instance
        private static volatile AuthorityConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static AuthorityConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AuthorityConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Security.Authority.config";

        #region 属性:ConfigFilePath
        private string configFilePath = null;

        /// <summary>�����ļ�������·��.</summary>
        public string ConfigFilePath
        {
            get { return configFilePath; }
        }
        #endregion

        /// <summary>�����ļ������޸ĵ�ʱ��</summary>
        private DateTime lastModifiedTime;

        #region 属性:Configuration
        private AuthorityConfiguration configuration = null;

        /// <summary>������Ϣ</summary>
        public AuthorityConfiguration Configuration
        {
            get { return this.configuration; }
        }
        #endregion

        private AuthorityConfigurationView()
            : this(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }

        public AuthorityConfigurationView(string path)
        {
            this.configFilePath = path;

            Load(path);
        }

        /// <summary>���¼���������Ϣ</summary>
        public void Reload()
        {
            Load(this.configFilePath);
        }

        /// <summary>�Զ�������������Ϣ</summary>
        /// <param name="path">�����ļ�·��</param>
        private void Load(string path)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                path = path.Replace("\\", "/");
            }

            if (File.Exists(path))
            {
                AuthorityConfiguration configuration = new AuthorityConfiguration();

                configuration.Configure(path, string.Format(@"configuration/{0}", AuthorityConfiguration.SectionName));

                this.configuration = configuration;
            }
        }

        /// <summary>ͨ�������ļ�����ʵ��</summary>
        /// <param name="fullConfigPath"></param>
        public static void LoadInstance(string fullConfigPath)
        {
            instance = new AuthorityConfigurationView(fullConfigPath);
        }
    }
}
