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
    /// <summary>���ĵ�������Ϣ</summary>
    [XmlRoot("kernel")]
    public class KernelConfiguration
    {
        /// <summary>������������</summary>
        public const string SectionName = "kernel";

        private const string keyProperty = "key";

        #region ����:Keys
        private KernelConfigurationKeyCollection keys = new KernelConfigurationKeyCollection();

        /// <summary>������Ϣ�ļ���</summary>
        [XmlElement(keyProperty)]
        public KernelConfigurationKeyCollection Keys
        {
            get { return keys; }
            set { keys = value; }
        }
        #endregion
    }
}
