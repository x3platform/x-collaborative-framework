namespace X3Platform.Sessions.Configuration
{
    #region Using Libraries
    using System;
    using System.Xml;

    using Common.Logging;

    using X3Platform.Configuration;
    #endregion

    /// <summary>会话管理配置信息</summary>
    public class SessionsConfiguration : XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Sessions";

        /// <summary>配置区的名称</summary>
        public const string SectionName = "sessions";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
