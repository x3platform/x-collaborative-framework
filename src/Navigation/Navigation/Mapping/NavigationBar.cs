using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.Navigation
{
    public class NavigationBar : NavigationMapNode
    {
        private List<NavigationMenu> items;

        public NavigationBar()
        {
            items = new List<NavigationMenu>();
        }

        public NavigationMenu this[string nodeName]
        {
            get
            {
                foreach (NavigationMenu item in items)
                {
                    if (item.Name == nodeName)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        [XmlElement("menu")]
        public List<NavigationMenu> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
