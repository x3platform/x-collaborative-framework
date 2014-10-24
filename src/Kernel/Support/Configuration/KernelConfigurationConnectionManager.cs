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
    /// <summary>数据库链接配置管理</summary>
    public class KernelConfigurationConnectionManager
    {
        /// <summary>获取数据库连接字符串</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>获取数据库连接提供器名称</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetProviderName(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ProviderName;
        }
    }
}
