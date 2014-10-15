// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Web.APIs.Configuration
{
    using System.Configuration;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary></summary>
    public class APIsConfigurationSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates an instance of the <see cref="APIsConfiguration"/> class.
        /// </summary>
        /// <remarks>Uses XML Serialization to deserialize the XML in the Web.config file into an
        /// <see cref="APIsConfiguration"/> instance.</remarks>
        /// <returns>An instance of the <see cref="APIsConfiguration"/> class.</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return section;
        }
    }
}
