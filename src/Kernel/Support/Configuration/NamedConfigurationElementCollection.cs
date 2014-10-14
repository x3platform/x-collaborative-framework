using System;
using System.Configuration;

namespace X3Platform.Configuration
{
    /// <summary></summary>
    /// <typeparam name="T"></typeparam>
    [ConfigurationCollectionAttribute(typeof(NamedConfigurationElement),
                       AddItemName = "add",
                       RemoveItemName = "remove",
                       ClearItemsName = "clear",
                       CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public sealed class NamedConfigurationElementCollection<T> : ConfigurationElementCollection
        where T : NamedConfigurationElement
    {
        static ConfigurationPropertyCollection properties;

        static NamedConfigurationElementCollection()
        {
            properties = new ConfigurationPropertyCollection();
        }

        /// <summary></summary>
        public NamedConfigurationElementCollection()
        {
        }

        public string[] AllKeys
        {
            get
            {
                return (string[])BaseGetAllKeys();
            }
        }

        public new T this[string name]
        {
            get
            {
                return (T)BaseGet(name);
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

        public void Add(T NameType)
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
            T e = (T)element;
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
