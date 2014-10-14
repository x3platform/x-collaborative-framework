using System;
using System.Configuration;

namespace X3Platform.Configuration
{
    /// <summary></summary>
    public class NamedConfigurationElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection properties;
        private static readonly ConfigurationProperty propName;

        /// <summary></summary>
        static NamedConfigurationElement()
        {
            properties = new ConfigurationPropertyCollection();

            propName = new ConfigurationProperty("name", typeof(string), string.Empty, ConfigurationPropertyOptions.IsKey);

            properties.Add(propName);
        }

        /// <summary>≈‰÷√‘™Àÿ</summary>
        public NamedConfigurationElement()
        {
        }

        /// <summary></summary>
        public NamedConfigurationElement(string name)
        {
            this[propName] = name;
        }

        /// <summary></summary>
        [ConfigurationProperty("name", DefaultValue = "", Options = ConfigurationPropertyOptions.IsKey)]
        public string Name
        {
            get { return (string)this[propName]; }
        }

        /// <summary></summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }
    }
}