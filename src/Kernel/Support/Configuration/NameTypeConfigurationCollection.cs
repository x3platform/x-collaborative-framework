using System;
using System.Configuration;

namespace X3Platform.Configuration
{

    [ConfigurationCollectionAttribute(typeof(NameTypeConfigurationElement),
                       AddItemName = "add",
                       RemoveItemName = "remove",
                       ClearItemsName = "clear",
                       CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public sealed class NameTypeConfigurationCollection : ConfigurationElementCollection
    {
        static ConfigurationPropertyCollection properties;

        static NameTypeConfigurationCollection()
        {
            properties = new ConfigurationPropertyCollection();
        }

        /// <summary></summary>
        public NameTypeConfigurationCollection()
        {
        }

        public string[] AllKeys
        {
            get
            {
                return (string[])BaseGetAllKeys();
            }
        }

        public new NameTypeConfigurationElement this[string name]
        {
            get
            {
                return (NameTypeConfigurationElement)BaseGet(name);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected  override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }

        public void Add(NameTypeConfigurationElement NameType)
        {
            BaseAdd(NameType, false);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new NameTypeConfigurationElement("", "");
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            NameTypeConfigurationElement e = (NameTypeConfigurationElement)element;
            return e.Name;
        }

        public void Remove(NameTypeConfigurationElement NameType)
        {
            throw new NotImplementedException();
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}
