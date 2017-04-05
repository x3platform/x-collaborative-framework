namespace X3Platform.Configuration
{
#if !NETSTANDARD
    using System.Configuration;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary></summary>
    public class KernelConfigurationSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>创建一个 <see cref="KernelConfiguration"/> 类的实例.</summary>
        /// <remarks>Uses XML Serialization to deserialize the XML in the web.config file into an
        /// <see cref="KernelConfiguration"/> instance.</remarks>
        /// <returns>一个 <see cref="KernelConfiguration"/> 类的实例.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            // Create an instance of XmlSerializer based on the RewriterConfiguration type...
            XmlSerializer ser = new XmlSerializer(typeof(KernelConfiguration));

            // Return the Deserialized object from the web.config Xml
            return ser.Deserialize(new XmlNodeReader(section));
        }
    }
#endif
}
