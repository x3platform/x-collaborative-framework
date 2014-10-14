using System;
using System.Configuration;

namespace X3Platform.Configuration
{

    [ConfigurationCollectionAttribute(typeof(NameValueConfigurationElement),
                       AddItemName = "add",
                       RemoveItemName = "remove",
                       ClearItemsName = "clear",
                       CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public sealed class NameValueConfigurationCollection : ConfigurationElementCollection
    {
        static ConfigurationPropertyCollection properties;

        static NameValueConfigurationCollection()
        {
            properties = new ConfigurationPropertyCollection();
        }

        /// <summary></summary>
        public NameValueConfigurationCollection()
        {
        }

        public string[] AllKeys
        {
            get
            {
                // Mono 平台不能处理 
                string[] keys = new string[this.Count];

                for (int i = 0; i < this.Count; i++)
                {
                    object key = this.BaseGetKey(i);

                    keys[i] = (key == null) ? string.Empty : key.ToString();
                }

                return keys;

                // return (string[])BaseGetAllKeys();
            }
        }

        public new NameValueConfigurationElement this[string name]
        {
            get
            {
                return (NameValueConfigurationElement)BaseGet(name);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }

        public void Add(NameValueConfigurationElement NameValue)
        {
            BaseAdd(NameValue, false);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new NameValueConfigurationElement("", "");
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            NameValueConfigurationElement e = (NameValueConfigurationElement)element;
            return e.Name;
        }

        public void Remove(NameValueConfigurationElement NameValue)
        {
            throw new NotImplementedException();
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}
