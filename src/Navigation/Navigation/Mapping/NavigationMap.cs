using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.Navigation
{
    [Serializable]
    [XmlRoot("navigationMap")]
    public class NavigationMap
    {
        protected NavigationBarCollection bars;
        protected string sitemap;
        public NavigationMap()
        {
            bars = new NavigationBarCollection();
        }

        [XmlElement("bars")]
        public NavigationBarCollection Bars
        {
            get { return bars; }
            set { bars = value; }
        }
    }
}
