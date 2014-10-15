#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :MembershipConfiguration.cs
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
using System;
using System.Configuration;
using System.Xml;

using Common.Logging;

using X3Platform.Configuration;
#endregion

namespace X3Platform.Membership.Configuration
{
    /// <summary>������Ϣ</summary>
    public class MembershipConfiguration : XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Membership";

        /// <summary>������������</summary>
        public const string SectionName = "membershipConfiguration";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
