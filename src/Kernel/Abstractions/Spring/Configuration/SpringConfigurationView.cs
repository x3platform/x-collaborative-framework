// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :SpringConfigurationView.cs
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

using X3Platform.Configuration;

namespace X3Platform.Spring.Configuration
{
    /// <summary>������ͼ</summary>
    public class SpringConfigurationView : XmlConfigurationView<SpringConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Spring.config";

        #region ��̬����:Instance
        private static volatile SpringConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static SpringConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                    instance = new SpringConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:SpringConfigurationView()
        /// <summary>���캯��</summary>
        private SpringConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }
        #endregion

        #region ���캯��:SpringConfigurationView(string configFilePath)
        /// <summary>���캯��</summary>
        private SpringConfigurationView(string configFilePath)
            : base(configFilePath)
        {
        }
        #endregion

        #region ��̬����:LoadInstance(string fullConfigPath)
        /// <summary>ͨ�������ļ�����ʵ��</summary>
        /// <param name="fullConfigPath"></param>
        public static void LoadInstance(string fullConfigPath)
        {
            instance = new SpringConfigurationView(fullConfigPath);
        }
        #endregion
    }
}
