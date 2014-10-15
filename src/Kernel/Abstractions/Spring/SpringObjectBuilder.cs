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
    /// <summary>�����Ķ��󹹽���(Spring.NET)</summary>
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

        /// <summary>Ӧ�������Ķ���</summary>
        private IApplicationContext context = null;

        #region ���캯��:SpringObjectBuilder(string file)
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

        #region ����:GetObject<T>(Type type)
        /// <summary>��ȡ����</summary>
        /// <param name="type">����</param>
        public T GetObject<T>(Type type)
        {
            return this.GetObject<T>(type, new object[] { });
        }
        #endregion

        #region ����:GetObject<T>(Type type, object[] args)
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

            return this.GetObject<T>(name, args);
        }
        #endregion

        #region ����:GetObject<T>(string name)
        /// <summary>��ȡ����</summary>
        /// <param name="name">Sring�����ļ��еĶ�������</param>
        public T GetObject<T>(string name)
        {
            return this.GetObject<T>(name, new object[] { });
        }
        #endregion

        #region ����:GetObject<T>(string name, object[] args)
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
