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

namespace X3Platform.CacheBuffer.Configuration
{
    using System.Configuration;
    using System.Collections.Generic;

    using X3Platform.Configuration;

    /// <summary>������������</summary>
    public class CacheBufferConfiguration : SerializableConfigurationSection
    {
        /// <summary>Name of the WorkflowPlus configuration section.</summary>
        public const string SectionName = "cacheBufferConfiguration";

        private const string keysProperty = "keys";

        /// <summary>Gets the collection of defined <see cref="NameValueConfigurationCollection"/> parameters.</summary>
        /// <value>The collection of defined <see cref="NameValueConfigurationCollection"/> parameters.</value>
        [ConfigurationProperty(keysProperty, IsRequired = false)]
        public NameValueConfigurationCollection Keys
        {
            get { return (NameValueConfigurationCollection)base[keysProperty]; }
            set { base[keysProperty] = value; }
        }
    }
}
