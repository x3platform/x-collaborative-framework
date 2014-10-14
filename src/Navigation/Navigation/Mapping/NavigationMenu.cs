using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.Navigation
{
    public class NavigationMenu : NavigationMapNode
    {
        private List<NavigationMapNode> items;

        public NavigationMenu()
        {
            items = new List<NavigationMapNode>();
        }

        public NavigationMapNode this[string nodeName]
        {
            get
            {
                foreach (NavigationMapNode item in items)
                {
                    if (item.Name == nodeName)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        [XmlElement("item")]
        public List<NavigationMapNode> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
