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

    /// <summary>Ӧ��������Ϣ</summary>
    public class StoragesConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Storages";
        
        /// <summary>������������</summary>
        public const string SectionName = "storagesConfiguration";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
