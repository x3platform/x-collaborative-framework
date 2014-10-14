// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :KernelConfigurationView.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Diagnostics;
using System.Windows.Forms;

using X3Platform.Util;

namespace X3Platform.Configuration
{
    /// <summary>���ݿ��������ù���</summary>
    public class KernelConfigurationConnectionManager
    {
        /// <summary>��ȡ���ݿ������ַ���</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>��ȡ���ݿ������ṩ������</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetProviderName(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ProviderName;
        }
    }
}
