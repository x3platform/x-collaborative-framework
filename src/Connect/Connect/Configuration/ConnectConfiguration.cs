namespace X3Platform.Connect.Configuration
{
    using System.Configuration;

    using X3Platform.Configuration;

    /// <summary>应用连接管理的配置信息</summary>
    public class ConnectConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>所属应用的名称</summary>
        public const string ApplicationName = "Connect";

        /// <summary>配置区域的名称</summary>
        public const string SectionName = "connectConfiguration";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
