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

using System;
using System.Xml.Serialization;

namespace X3Platform.Configuration
{
    /// <summary>关键字配置</summary>
    [Serializable()]
    public class KernelConfigurationKey
    {
        // 私有成员变量.
        private string m_Name = null, m_Value = null;

        public KernelConfigurationKey()
        { 
        }
        
        public KernelConfigurationKey(string name, string value)
        {
            m_Name = name;
            m_Value = value;
        }

        /// <summary>名称</summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>值</summary>
        [XmlAttribute("value")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private bool m_SupportClient = false;

        /// <summary>支持客户端输出</summary>
        [XmlAttribute("supportClient")]
        public bool SupportClient
        {
            get { return m_SupportClient; }
            set { m_SupportClient = value; }
        }
    }
}
