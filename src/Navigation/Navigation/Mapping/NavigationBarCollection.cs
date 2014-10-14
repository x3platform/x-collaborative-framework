using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.Navigation
{
    public class NavigationBarCollection : NavigationMapNode
    {
        private List<NavigationBar> items;

        public NavigationBarCollection()
        {
            items = new List<NavigationBar>();
        }

        public NavigationBar this[string nodeName]
        {
            get
            {
                foreach (NavigationBar item in items)
                {
                    if (item.Name == nodeName)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        [XmlElement("bar")]
        public List<NavigationBar> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
