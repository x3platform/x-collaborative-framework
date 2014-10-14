using System.Configuration;
using System;

namespace X3Platform.Configuration
{
    public class NameTypeConfigurationElement : NamedConfigurationElement
    {
        private static ConfigurationPropertyCollection _properties;
        private static readonly ConfigurationProperty _propName;
        private static readonly ConfigurationProperty _propType;

        static NameTypeConfigurationElement()
        {
            _properties = new ConfigurationPropertyCollection();

            _propName = new ConfigurationProperty("name", typeof(string), string.Empty, ConfigurationPropertyOptions.IsKey);
            _propType = new ConfigurationProperty("type", typeof(string), string.Empty);

            _properties.Add(_propName);
            _properties.Add(_propType);
        }

        public NameTypeConfigurationElement()
        {
        }

        public NameTypeConfigurationElement(string name, string typeName)
        {
            this[_propName] = name;
            this[_propType] = typeName;
        }

        public NameTypeConfigurationElement(string name, Type type)
        {
            this[_propName] = name;

            if (type == null)
            {
                this[_propType] = string.Empty;
            }
            else
            {
                string assemblyQualifiedName = type.AssemblyQualifiedName;

                int length = assemblyQualifiedName.IndexOf(',', assemblyQualifiedName.IndexOf(',') + 1);

                this[_propType] = assemblyQualifiedName.Substring(0, length);
            }
        }

        /// <summary></summary>
        [ConfigurationProperty("type", DefaultValue = "", Options = ConfigurationPropertyOptions.None)]
        public string TypeName
        {
            get { return (string)this[_propType]; }
            set { this[_propType] = value; }
        }

        /// <summary></summary>
        public Type Type
        {
            get { return Type.GetType(this.TypeName); }
        }

        /// <summary></summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}