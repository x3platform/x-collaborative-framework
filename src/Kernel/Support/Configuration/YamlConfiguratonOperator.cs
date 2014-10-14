using System;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Globalization;

using Common.Logging;
using X3Platform.Yaml.RepresentationModel;

namespace X3Platform.Configuration
{
    /// <summary>Yaml配置辅助操作类</summary>
    public static class YamlConfiguratonOperator
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary></summary>
        /// <param name="list"></param>
        /// <param name="node"></param>
        public static void SetKeyValues(NameValueConfigurationCollection list, YamlMappingNode node)
        {
            if (list == null) { throw new ArgumentNullException("list"); }

            foreach (var entry in node.Children)
            {
                bool isExist = false;

                NameValueConfigurationElement nameValue = new NameValueConfigurationElement(entry.Key.ToString(), entry.Value.ToString());

                string[] keys = list.AllKeys;

                foreach (string key in keys)
                {
                    if (nameValue.Name == key)
                    {
                        list[nameValue.Name].Value = nameValue.Value;
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    list.Add(nameValue);
                }
            }
        }
    }
}
