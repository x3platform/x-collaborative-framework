using X3Platform.Configuration;
using X3Platform.Security;

namespace X3Platform.Data.ConnectionPlugins
{
    /// <summary>数据库连接插件信息</summary>
    public interface IConnectionPlugin
    {
        /// <summary>数据库的默认连接是否有效</summary>
        public bool Valid { get; }

        /// <summary>数据库的默认连接</summary>
        public string ConnectionString { get; }

        /// <summary>数据库的服务器地址</summary>
        public string DataSource { get; }

        /// <summary>数据库的默认数据库名称</summary>
        public string Database { get; }

        /// <summary>数据库的登录帐号</summary>
        public string LoginName { get; }

        /// <summary>数据库的登录密码</summary>
        public string Password { get; }

        /// <summary>数据库的提供器名称</summary>
        public string Provider { get; }

        /// <summary>数据库的 IBatis 配置文件所在根目录位置</summary>
        public string IBatisSqlMapFilePathRoot { get; }
    }
}
