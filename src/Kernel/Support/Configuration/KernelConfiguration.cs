// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :KernelConfiguration.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System.Xml;
using System.Xml.Serialization;

namespace X3Platform.Configuration
{
    /// <summary>核心的配置信息</summary>
    [XmlRoot("kernel")]
    public class KernelConfiguration
    {
        /// <summary>配置区的名称</summary>
        public const string SectionName = "kernel";

        private const string keyProperty = "key";

        #region 属性:Keys
        private KernelConfigurationKeyCollection keys = new KernelConfigurationKeyCollection();

        /// <summary>配置信息的集合</summary>
        [XmlElement(keyProperty)]
        public KernelConfigurationKeyCollection Keys
        {
            get { return keys; }
            set { keys = value; }
        }
        #endregion
    }
}
