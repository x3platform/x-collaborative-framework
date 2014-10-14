#region Using Libraries
using System;
using System.Collections.Generic;
using System.IO;

using Spring.Context;
using Spring.Context.Support;

using X3Platform.Configuration;
using X3Platform.Spring.Configuration;
#endregion

namespace X3Platform.Spring
{
    /// <summary>独立的对象构建器(Spring.NET)</summary>
    public class SpringObjectBuilder
    {
        private static Dictionary<string, SpringObjectBuilder> dictionary = new Dictionary<string, SpringObjectBuilder>();

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static SpringObjectBuilder Create(string name, string file)
        {
            SpringObjectBuilder objectBuilder = null;

            if (dictionary.ContainsKey(name))
            {
                objectBuilder = dictionary[name];

                if (objectBuilder == null) { dictionary.Remove(name); }
            }
            else
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    file = file.Replace("\\", "/");
                }

                objectBuilder = new SpringObjectBuilder(file);

                dictionary.Add(name, objectBuilder);
            }

            return objectBuilder;
        }

        /// <summary>应用上下文对象</summary>
        private IApplicationContext context = null;

        #region 构造函数:SpringObjectBuilder(string file)
        /// <summary></summary>
        /// <param name="file"></param>
        public SpringObjectBuilder(string file)
        {
            string[] fileValues = new string[] { file };

            for (int i = 0; i < fileValues.Length; i++)
            {
                fileValues[i] = Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, KernelConfigurationView.Instance.ReplaceKeyValue(fileValues[i]));

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

            return this.GetObject<T>(name, args);
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
