using X3Platform.Configuration;
using X3Platform.Security;

namespace X3Platform.Data.ConnectionPlugins
{
    /// <summary>数据库连接插件信息</summary>
    public interface IConnectionPlugin
    {
        /// <summary>数据库的默认连接是否有效</summary>
        bool Valid { get; }

        /// <summary>数据库的默认连接</summary>
        string ConnectionString { get; }

        /// <summary>数据库的服务器地址</summary>
        string DataSource { get; }

        /// <summary>数据库的服务器端口</summary>
        string Port { get; }

        /// <summary>数据库的默认数据库名称</summary>
        string Database { get; }

        /// <summary>数据库的登录帐号</summary>
        string LoginName { get; }

        /// <summary>数据库的登录密码</summary>
        string Password { get; }
        
        /// <summary>连接池化</summary>
        string Pooling { get; }

        /// <summary>连接重置</summary>
        string ConnectionReset { get; }

        /// <summary>连接回收时间 单位:秒</summary>
        string ConnectionLifetime { get; }

        /// <summary>连接池最小数量</summary>
        string MinPoolSize { get; }

        /// <summary>连接池最大数量</summary>
        string MaxPoolSize { get; }

        /// <summary>连接超时时间 单位:秒</summary>
        string ConnectionTimeout { get; }

        /// <summary>数据库的提供器名称</summary>
        string Provider { get; }

        /// <summary>数据库的 IBatis 配置文件所在根目录位置</summary>
        string IBatisSqlMapFilePathRoot { get; }
    }
}
