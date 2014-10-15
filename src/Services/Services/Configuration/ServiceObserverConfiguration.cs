using System;
using System.Configuration;
using System.Text;
using System.Xml;

using X3Platform.Configuration;

namespace X3Platform.Services.Configuration
{
    public class ServiceObserverConfiguration : NameTypeConfigurationElement
    {
        private const string argsProperty = "args";

        private const string nextRunTimeProperty = "nextRunTime";

        /// <summary>
        /// Initialize a new instance of the <see cref="EmailClientProviderData"/> class.
        /// </summary>
        public ServiceObserverConfiguration() { }

        /// <summary>
        /// Intializes a new instance of the <see cref="EmailClientProviderData"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        public ServiceObserverConfiguration(string name)
            : base(name, typeof(ServiceObserverConfiguration))
        { }

        /// <summary>
        /// Intializes a new instance of the <see cref="EmailClientProviderData"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        public ServiceObserverConfiguration(string name, Type type)
            : base(name, type)
        { }

        /// <summary>
        /// Gets or sets the default EmailClient of the provider. This EmailClient will be used when
        /// no message can be found for the user specified by the caller.
        /// </summary>
        [ConfigurationProperty(argsProperty, IsRequired = true)]
        public string Args
        {
            get { return (base[argsProperty] == null) ? string.Empty : base[argsProperty].ToString(); }
            set { base[argsProperty] = value; }
        }

        /// <summary></summary>
        [ConfigurationProperty(nextRunTimeProperty)]
        public DateTime NextRunTime
        {
            get { return (base[nextRunTimeProperty] == null) ? new DateTime(2000, 1, 1) : DateTime.Parse(base[nextRunTimeProperty].ToString()); }
            set { base[nextRunTimeProperty] = value.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
    }
}