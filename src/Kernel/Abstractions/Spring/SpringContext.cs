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
        #region 静态属性:Instance
        private static volatile SpringContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
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
        /// 名称
        /// </summary>
        public override string Name
        {
            get { return "Spring"; }
        }
        #endregion

        #region 属性:Application
        private IApplicationContext context = null;

        /// <summary>应用上下文接口</summary>
        public IApplicationContext Application
        {
            get { return context; }
        }
        #endregion

        #region 属性:Configuration
        private SpringConfiguration configuration = null;

        /// <summary>
        /// 配置
        /// </summary>
        public SpringConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 构造函数:SpringContext()
        private SpringContext()
        {
            this.Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
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

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            // 根据 KernelConfigurationView.Instance.ApplicationSpringConfigFilePath 加载配置文件路径
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

        #region 函数:GetObject<T>(Type type)
        /// <summary>获取对象</summary>
        /// <param name="type">类型</param>
        public T GetObject<T>(Type type)
        {
            return this.GetObject<T>(type, new object[] { });
        }
        #endregion

        #region 函数:GetObject<T>(Type type, object[] args)
        /// <summary>获取对象</summary>
        /// <param name="type">类型</param>
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

        #region 函数:GetObject<T>(string name)
        /// <summary>获取对象</summary>
        /// <param name="name">Sring配置文件中的对象名称</param>
        public T GetObject<T>(string name)
        {
            return this.GetObject<T>(name, new object[] { });
        }
        #endregion

        #region 函数:GetObject<T>(string name, object[] args)
        /// <summary>获取对象</summary>
        /// <param name="name">Sring配置文件中的对象名称</param>
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
