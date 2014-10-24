namespace X3Platform
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using System.Resources;
    #endregion

    /// <summary>Helper class to load resources strings.</summary>
    public static class ResourceStringLoader
    {
        /// <summary>加载资源字符串</summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The string from the resource.</returns>
        public static string LoadString(string baseName, string resourceName)
        {
            return LoadString(baseName, resourceName, Assembly.GetCallingAssembly());
        }

        /// <summary>加载资源字符串</summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="asm">The assembly to load the resource from.</param>
        /// <returns>The string from the resource.</returns>
        public static string LoadString(string baseName, string resourceName, Assembly assembly)
        {
            if (string.IsNullOrEmpty(baseName)) throw new ArgumentNullException("baseName");
            if (string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException("resourceName");

            string value = null;

            if (null != assembly) value = LoadAssemblyString(assembly, baseName, resourceName);
            if (null == value) value = LoadAssemblyString(Assembly.GetExecutingAssembly(), baseName, resourceName);
            if (null == value) return string.Empty;
            
            return value;
        }

        private static string LoadAssemblyString(Assembly assembly, string baseName, string resourceName)
        {
            try
            {
                ResourceManager resourceManager = new ResourceManager(baseName, assembly);

                return resourceManager.GetString(resourceName);
            }
            catch (MissingManifestResourceException)
            {
            }

            return null;
        }
    }
}
