// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

//
// ������¼
//
// == 2010-01-01 == 
//
// �½�
//

using System;
using System.Configuration;

using X3Platform.Configuration;

using X3Platform.Configuration;
using System.IO;


namespace X3Platform.CacheBuffer.Configuration
{
    /// <summary>
    /// ��������������ͼ
    /// </summary>
    public class CacheBufferConfigurationView
    {
        private const string configFile = "config\\X3Platform.CacheBuffer.config";

        private string configFilePath = null;

        /// <summary>�����ļ�������·��.</summary>
        public string ConfigFilePath
        {
            get { return configFile; }
        }

        /// <summary>�����ļ������޸ĵ�ʱ��</summary>
        private DateTime lastModifiedTime;

        /// <summary>�����ļ��ļ�����.</summary>
        private ConfigurationFileWatcher watcher = null;

        private static CacheBufferConfigurationView instance = new CacheBufferConfigurationView();

        public static CacheBufferConfigurationView Instance
        {
            get
            {
                if (instance == null)
                    instance = new CacheBufferConfigurationView();

                return instance;
            }
        }

        private IConfigurationSource configurationSource;

        #region ����:Configuration
        /// <summary>������Ϣ</summary>
        public CacheBufferConfiguration Configuration
        {
            get { return (CacheBufferConfiguration)configurationSource.GetSection(CacheBufferConfiguration.SectionName); }
        }
        #endregion

        private CacheBufferConfigurationView()
              : this(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }

        public CacheBufferConfigurationView(string path)
        {
            this.configFilePath = path;

            Load(path);

            FileSystemEventHandler handler = new FileSystemEventHandler(OnChanged);

            watcher = new ConfigurationFileWatcher(path, handler);
        }

        /// <summary>Initialize a new instance of the <see cref="CacheBufferConfigurationView" /> with a <see cref="Common.Configuration.IConfigurationSource"/>.</summary>
        /// <param name="configurationSource">An <see cref="IConfigurationSource"/> object.</param>
        public CacheBufferConfigurationView(IConfigurationSource configurationSource)
        {
            this.configurationSource = configurationSource;
        }

        /// <summary>
        /// �����ļ�����ʱ�������¼�. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">A System.IO.FileSystemEventArgs that contains the event data.</param>
        /// <remarks>���������ļ���������</remarks>
        protected void OnChanged(object sender, FileSystemEventArgs e)
        {
            // ��������������Ϣ.
            CacheBufferConfigurationView.Instance.Reload();

            // ��¼�����ļ�����ʱ��
            this.lastModifiedTime = File.GetLastWriteTime(configFilePath);
        }

        /// <summary>���¼���������Ϣ</summary>
        public void Reload()
        {
            Load(this.configFilePath);
        }

        /// <summary>����������Ϣ</summary>
        private void Load(string path)
        {
            if (File.Exists(path))
            {
                FileConfigurationSource configurationSource = new FileConfigurationSource(path);

                this.configurationSource = configurationSource;
            }
        }

        /// <summary>ͨ�������ļ�����ʵ��</summary>
        /// <param name="fullConfigPath"></param>
        public static void LoadInstance(string fullConfigPath)
        {
            instance = new CacheBufferConfigurationView(fullConfigPath);
        } 
    }
}
