
namespace X3Platform.Plugins.Favorite.Configuration
{
    using System.Configuration;

    using X3Platform.Configuration;

    /// <summary>收藏夹插件的配置信息</summary>
    public class FavoriteConfiguration : XmlConfiguraton
    {
        /// <summary>应用名称</summary>
        public const string ApplicationName = "Favorite";

        /// <summary>配置区域的名称</summary>
        public const string SectionName = "favoriteConfiguration";

        /// <summary>获取配置区的名称</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
