#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IPQueryConfiguration.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

#region Using Libraries
using Common.Logging;

using X3Platform.Configuration;
#endregion

namespace X3Platform.Location.IPQuery.Configuration
{
    /// <summary>IP��ѯ��������Ϣ</summary>
    public class IPQueryConfiguration : XmlConfiguraton
    {
        /// <summary>��������������</summary>
        public const string SectionName = "ipQueryConfiguration";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
