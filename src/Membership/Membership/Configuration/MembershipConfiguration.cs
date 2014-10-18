namespace X3Platform.Membership.Configuration
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.Xml;

    using Common.Logging;

    using X3Platform.Configuration;
    #endregion

    /// <summary>人员及权限管理的配置信息</summary>
    public class MembershipConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Membership";

        /// <summary>配置区域的名称</summary>
        public const string SectionName = "membership";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
