#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :StoragesConfiguration.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Storages.Configuration
{
    #region Using Libraries
    using System.Configuration;

    using X3Platform.Configuration;
    #endregion

    /// <summary>应用存储配置信息</summary>
    public class StoragesConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Storages";
        
        /// <summary>配置区的名称</summary>
        public const string SectionName = "storagesConfiguration";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
