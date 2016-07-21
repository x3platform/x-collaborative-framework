using X3Platform.Configuration;
using X3Platform.Security;
using System.Collections.Generic;

namespace X3Platform.Data.ConnectionPlugins
{
    /// <summary>内置的数据库连接插件信息</summary>
    public class InnerConnectionPlugin : GenericConnectionPlugin
    {
        /// <summary></summary>
        /// <param name="configuration"></param>
        public InnerConnectionPlugin(KernelConfiguration configuration)
        {
            this.dictionary = new Dictionary<string, string>();

            foreach (KernelConfigurationKey key in configuration.Keys)
            {
                if (!this.dictionary.ContainsKey(key.Name))
                {
                    this.dictionary.Add(key.Name, key.Value);
                }
                else
                {
                    this.dictionary[key.Name] = key.Value;
                }
            }
        }
    }
}
