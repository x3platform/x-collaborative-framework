using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.Navigation
{
    public class NavigationMapNode
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("description")]
        public string Description;

        [XmlAttribute("url")]
        public string Url;

        [XmlAttribute("roles")]
        public string Roles;

        [XmlAttribute("onclick")]
        public string OnClick;

        /// <summary>
        /// х╗оч
        /// </summary>
        [XmlAttribute("purview")]
        public string Purview;

        private string m_ForbidState;

        /// <summary>
        /// Show | Hidden;
        /// </summary>
        [XmlAttribute("forbidstate")]
        public string ForbidState
        {
            get { return m_ForbidState; }
            set { m_ForbidState = (value == string.Empty) ? "show" : value; }
        }
    }
}
