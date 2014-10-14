using System.Configuration;
using System;

namespace X3Platform.Configuration
{
    /// <summary></summary>
    public class NameValueConfigurationElement : NamedConfigurationElement
    {
        private static ConfigurationPropertyCollection _properties;
        private static readonly ConfigurationProperty _propName;
        private static readonly ConfigurationProperty _propValue;

        static NameValueConfigurationElement()
        {
            _properties = new ConfigurationPropertyCollection();

            _propName = new ConfigurationProperty("name", typeof(string), string.Empty, ConfigurationPropertyOptions.IsKey);
            _propValue = new ConfigurationProperty("value", typeof(string), string.Empty);

            _properties.Add(_propName);
            _properties.Add(_propValue);
        }

        public NameValueConfigurationElement()
        {
        }

        public NameValueConfigurationElement(string name, string typeName)
        {
            this[_propName] = name;
            this[_propValue] = typeName;
        }

        public NameValueConfigurationElement(string name, Type type)
        {
            this[_propName] = name;

            if (type == null)
            {
                this[_propValue] = string.Empty;
            }
            else
            {
                string assemblyQualifiedName = type.AssemblyQualifiedName;

                int length = assemblyQualifiedName.IndexOf(',', assemblyQualifiedName.IndexOf(',') + 1);

                this[_propValue] = assemblyQualifiedName.Substring(0, length);
            }
        }

        /// <summary></summary>
        [ConfigurationProperty("value", DefaultValue = "", Options = ConfigurationPropertyOptions.None)]
        public string Value
        {
            get { return (string)this[_propValue]; }
            set { this[_propValue] = value; }
        }

        /// <summary></summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}