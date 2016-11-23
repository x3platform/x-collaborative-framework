using System;
using System.Configuration;
using System.Text;
using System.Xml;

using X3Platform.Configuration;
using X3Platform.Util;

namespace X3Platform.Services.Configuration
{
    public class ServiceObserverConfigurationElement : NameTypeConfigurationElement
    {
        private const string argsProperty = "args";

        private const string nextRunTimeProperty = "nextRunTime";

        /// <summary>
        /// Initialize a new instance of the <see cref="ServiceObserverConfigurationElement"/> class.
        /// </summary>
        public ServiceObserverConfigurationElement() { }

        /// <summary>
        /// Intializes a new instance of the <see cref="ServiceObserverConfigurationElement"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        public ServiceObserverConfigurationElement(string name)
            : base(name, typeof(ServiceObserverConfigurationElement))
        {
        }

        /// <summary>
        /// Intializes a new instance of the <see cref="ServiceObserverConfigurationElement"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        public ServiceObserverConfigurationElement(string name, string typeName)
            : base(name, typeof(ServiceObserverConfigurationElement))
        {
            this.TypeName = typeName;
        }

        /// <summary>
        /// Intializes a new instance of the <see cref="ServiceObserverConfigurationElement"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        public ServiceObserverConfigurationElement(string name, string typeName, string args, string nextRunTime)
            :base(name,typeof(ServiceObserverConfigurationElement))
        {
            this.TypeName = typeName;
            this.Args = args;
            this.NextRunTime = Convert.ToDateTime(nextRunTime);
        }
        
        private string m_Args = null;

        /// <summary></summary>
        [ConfigurationProperty(argsProperty)]
        public string Args
        {
            get { return (this.m_Args == null) ? string.Empty : this.m_Args; }
            set { this.m_Args = value; }
        }

        private DateTime m_NextRunTime = DateHelper.DefaultTime;

        /// <summary></summary>
        [ConfigurationProperty(nextRunTimeProperty)]
        public DateTime NextRunTime
        {
            get { return this.m_NextRunTime; }
            set { this.m_NextRunTime = value; }
        }
    }
}