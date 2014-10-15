#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :AppsConfiguration.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Apps.Configuration
{
    /// <summary>Ӧ��������Ϣ</summary>
    public class AppsConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "ApplicationManagement";

        /// <summary>������������</summary>
        public const string SectionName = "apps";

        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
