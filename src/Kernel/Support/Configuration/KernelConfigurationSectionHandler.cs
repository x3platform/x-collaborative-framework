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

using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace X3Platform.Configuration
{
    public class KernelConfigurationSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>����һ�� <see cref="KernelConfiguration"/> ����ʵ��.</summary>
        /// <remarks>Uses XML Serialization to deserialize the XML in the web.config file into an
        /// <see cref="KernelConfiguration"/> instance.</remarks>
        /// <returns>һ�� <see cref="KernelConfiguration"/> ����ʵ��.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            // Create an instance of XmlSerializer based on the RewriterConfiguration type...
            XmlSerializer ser = new XmlSerializer(typeof(KernelConfiguration));

            // Return the Deserialized object from the web.config Xml
            return ser.Deserialize(new XmlNodeReader(section));
        }
    }
}
