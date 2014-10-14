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
    /// <summary>�ؼ�������</summary>
    [Serializable()]
    public class KernelConfigurationKey
    {
        // ˽�г�Ա����.
        private string m_Name = null, m_Value = null;

        public KernelConfigurationKey()
        { 
        }
        
        public KernelConfigurationKey(string name, string value)
        {
            m_Name = name;
            m_Value = value;
        }

        /// <summary>����</summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>ֵ</summary>
        [XmlAttribute("value")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private bool m_SupportClient = false;

        /// <summary>֧�ֿͻ�������</summary>
        [XmlAttribute("supportClient")]
        public bool SupportClient
        {
            get { return m_SupportClient; }
            set { m_SupportClient = value; }
        }
    }
}
