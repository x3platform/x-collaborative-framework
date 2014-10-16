// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :RewriterConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

using X3Platform.Configuration;
using X3Platform.Util;

namespace X3Platform.Web.UrlRewriter.Configuration
{
    /// <summary>��ַ��д������ͼ</summary>
    public class RewriterConfigurationView
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Web.UrlRewriter.config";

        private string configFilePath = null;

        /// <summary>�����ļ�������·��.</summary>
        public string ConfigFilePath
        {
            get { return configFile; }
        }

        /// <summary>�����ļ������޸ĵ�ʱ��</summary>
        private DateTime lastModifiedTime;

        #region 静态属性::Instance
        private static volatile RewriterConfigurationView instance = null;

        private static object lockObject = new object();

        public static RewriterConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new RewriterConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        private RewriterConfiguration configurationSource;

        #region 属性:Configuration
        /// <summary>������Ϣ</summary>
        public RewriterConfiguration Configuration
        {
            get { return configurationSource; }
        }
        #endregion

        private RewriterConfigurationView()
            : this(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }

        public RewriterConfigurationView(string path)
        {
            this.configFilePath = path;

            Load(path);
        }

        /// <summary>
        /// �����ļ�����ʱ�������¼�. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">���ݸ��¼����������Ĳ���</param>
        /// <remarks>���������ļ���������</remarks>
        protected void OnChanged(object sender, FileSystemEventArgs e)
        {
            // ��������������Ϣ.
            RewriterConfigurationView.Instance.Reload();

            // ��¼�����ļ�����ʱ��
            this.lastModifiedTime = File.GetLastWriteTime(configFilePath);
        }

        /// <summary>
        /// ���¼���������Ϣ
        /// </summary>
        public void Reload()
        {
            Load(this.configFilePath);
        }

        /// <summary>����������Ϣ</summary>
        /// <param name="path">�����ļ���·��</param>
        private void Load(string path)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                path = path.Replace("\\", "/");
            }

            if (File.Exists(path))
            {
                // XmlDocument �� Load �����������ļ���Ϣ������ʹ�� XmlTextReader �������ͷ���Դ��
                using (XmlTextReader reader = new XmlTextReader(path))
                {
                    XmlDocument doc = new XmlDocument();

                    XmlNode node;

                    doc.Load(reader);

                    node = doc.GetElementsByTagName(RewriterConfiguration.SectionName)[0];

                    XmlSerializer ser = new XmlSerializer(typeof(RewriterConfiguration));

                    this.configurationSource = (RewriterConfiguration)ser.Deserialize(new XmlNodeReader(node));

                    reader.Close();
                }
            }
        }

        /// <summary>����������Ϣ</summary>
        public void Save()
        {
            Monitor.Enter(lockObject);

            // XmlDocument �� Load �����������ļ���Ϣ������ʹ�� XmlTextReader �������ͷ���Դ��
            using (XmlTextReader reader = new XmlTextReader(this.configFilePath))
            {
                XmlDocument doc = new XmlDocument();

                XmlNode node;

                doc.Load(reader);

                node = doc.GetElementsByTagName(RewriterConfiguration.SectionName)[0];

                node.InnerXml = XmlHelper.ToXml(this.configurationSource);

                doc.Save(this.configFilePath);

                reader.Close();
            }

            Monitor.Exit(lockObject);
        }

        /// <summary>ͨ�������ļ�����ʵ��</summary>
        /// <param name="fullConfigPath"></param>
        public static void LoadInstance(string fullConfigPath)
        {
            instance = new RewriterConfigurationView(fullConfigPath);
        }
    }
}
