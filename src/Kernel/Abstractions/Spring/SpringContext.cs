#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
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
#endregion

#region Using Libraries
using System;
using System.IO;

using Spring.Context;
using Spring.Context.Support;

using X3Platform.Configuration;
using X3Platform.Plugins;
using X3Platform.Spring.Configuration;
#endregion

namespace X3Platform.Spring
{
    [CLSCompliantAttribute(false)]
    public sealed class SpringContext : CustomPlugin
    {
        #region 静态属性::Instance
        private static volatile SpringContext instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static SpringContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new SpringContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Name
        /// <summary>
        /// ����
        /// </summary>
        public override string Name
        {
            get { return "Spring"; }
        }
        #endregion

        #region 属性:Application
        private IApplicationContext context = null;

        /// <summary>Ӧ�������Ľӿ�</summary>
        public IApplicationContext Application
        {
            get { return context; }
        }
        #endregion

        #region 属性:Configuration
        private SpringConfiguration configuration = null;

        /// <summary>
        /// ����
        /// </summary>
        public SpringConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region ���캯��:SpringContext()
        private SpringContext()
        {
            this.Restart();
        }
        #endregion

        #region 属性:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }
        #endregion

        #region 属性:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            // ���� KernelConfigurationView.Instance.ApplicationSpringConfigFilePath ���������ļ�·��
            if (SpringConfigurationView.Instance.ConfigFilePath != KernelConfigurationView.Instance.ApplicationSpringConfigFilePath)
            {
                SpringConfigurationView.LoadInstance(KernelConfigurationView.Instance.ApplicationSpringConfigFilePath);
            }

            configuration = SpringConfigurationView.Instance.Configuration;

            string[] fileKeys = configuration.Files.AllKeys;

            string[] fileValues = new string[fileKeys.Length];

            for (int i = 0; i < fileKeys.Length; i++)
            {
                fileValues[i] = KernelConfigurationView.Instance.ReplaceKeyValue(configuration.Files[fileKeys[i]].Value);

                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    fileValues[i] = fileValues[i].Replace("\\", "/");
                }
            }

            this.context = new XmlApplicationContext(fileValues);
        }
        #endregion

        #region 属性:GetObject<T>(Type type)
        /// <summary>��ȡ����</summary>
        /// <param name="type">����</param>
        public T GetObject<T>(Type type)
        {
            return this.GetObject<T>(type, new object[] { });
        }
        #endregion

        #region 属性:GetObject<T>(Type type, object[] args)
        /// <summary>��ȡ����</summary>
        /// <param name="type">����</param>
        public T GetObject<T>(Type type, object[] args)
        {
            string name = null;

            Attribute[] attributes = Attribute.GetCustomAttributes(type);

            foreach (Attribute attribute in attributes)
            {
                if (attribute is SpringObjectAttribute)
                {
                    name = ((SpringObjectAttribute)attribute).Name;
                    break;
                }
            }

            return this.GetObject<T>(name, new object[] { });
        }
        #endregion

        #region 属性:GetObject<T>(string name)
        /// <summary>��ȡ����</summary>
        /// <param name="name">Sring�����ļ��еĶ�������</param>
        public T GetObject<T>(string name)
        {
            return this.GetObject<T>(name, new object[] { });
        }
        #endregion

        #region 属性:GetObject<T>(string name)
        /// <summary>��ȡ����</summary>
        /// <param name="name">Sring�����ļ��еĶ�������</param>
        public T GetObject<T>(string name, object[] args)
        {
            if (string.IsNullOrEmpty(name))
            {
                return default(T);
            }

            return (T)this.context.GetObject(name, args);
        }
        #endregion
    }
}
