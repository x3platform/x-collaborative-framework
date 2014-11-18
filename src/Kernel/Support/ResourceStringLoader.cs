namespace X3Platform
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using System.Resources;
    using System.IO;
    #endregion

    /// <summary>Helper class to load resources strings.</summary>
    public static class ResourceStringLoader
    {
        /// <summary>加载资源字符串</summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The string from the resource.</returns>
        public static string LoadString(string resourceName)
        {
            return LoadString(resourceName, Assembly.GetCallingAssembly());
        }

        /// <summary>加载资源字符串</summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="assembly">程序集信息</param>
        public static string LoadString(string resourceName, Assembly assembly)
        {
            if (string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException("resourceName");

            string value = null;

            if (null != assembly) value = LoadAssemblyString(assembly, resourceName);
            if (null == value) value = LoadAssemblyString(Assembly.GetExecutingAssembly(), resourceName);
            if (null == value) return string.Empty;

            return value;
        }

        private static string LoadAssemblyString(Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
