using X3Platform.Configuration;
using X3Platform.Security;
using System.Collections.Generic;

namespace X3Platform.Data.ConnectionPlugins
{
    /// <summary>通常的数据库连接信息</summary>
    public class GenericConnectionPlugin : IConnectionPlugin
    {
        // 参数说明
        // http://www.connectionstrings.com/oracle/

        /// <summary></summary>
        protected const string PREFIX_KEY = "DatabaseSettings.";

        /// <summary></summary>
        protected IDictionary<string, string> dictionary = null;

        /// <summary>数据库的默认连接是否有效</summary>
        public bool Valid
        {
            get
            {
                if (Provider == "MySql" && !string.IsNullOrEmpty(DataSource) && !string.IsNullOrEmpty(Database)
                    && !string.IsNullOrEmpty(LoginName) && !string.IsNullOrEmpty(Password))
                {
                    return true;
                }
                else if (Provider == "SqlServer" && !string.IsNullOrEmpty(DataSource) && !string.IsNullOrEmpty(Database)
                    && !string.IsNullOrEmpty(LoginName) && !string.IsNullOrEmpty(Password))
                {
                    return true;
                }
                else if (Provider == "Oracle" && !string.IsNullOrEmpty(DataSource)
                    && !string.IsNullOrEmpty(LoginName) && !string.IsNullOrEmpty(Password))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>数据库的默认连接</summary>
        public string ConnectionString
        {
            get
            {
                // 参考资料: http://www.connectionstrings.com/
                if (Provider == "MySql")
                {
                    return string.Concat("server=", DataSource, ((Port == "3306") ? "" : ";port=" + Port),
                        ";database=", Database,
                        ";uid=", LoginName,
                        ";pwd=", Password,
                        ";pooling=", Pooling,
                        ";connection reset=", ConnectionReset,
                        ";connection lifetime=", ConnectionLifetime,
                        ";min pool size=", MinPoolSize,
                        ";max pool size=", MaxPoolSize,
                        ";connection timeout=", ConnectionTimeout, ";");
                }
                else if (Provider == "SqlServer")
                {
                    return string.Concat("server=", DataSource, ((Port == "1433") ? "" : "," + Port),
                        ";database=", Database,
                        ";user id=", LoginName,
                        ";password=", Password,
                        ";connection reset=", ConnectionReset,
                        ";connection lifetime=", ConnectionLifetime,
                        ";min pool size=", MinPoolSize,
                        ";max pool size=", MaxPoolSize,
                        ";connection timeout=", ConnectionTimeout, ";");
                }
                else if (Provider == "Oracle")
                {
                    return string.Concat("data source=", DataSource,
                        ";user id=", LoginName,
                        ";password=", Password,
                        ";connection lifetime=", ConnectionLifetime,
                        ";min pool size=", MinPoolSize,
                        ";max pool size=", MaxPoolSize, ";");
                }

                return string.Empty;
            }
        }

        /// <summary>数据库的服务器地址</summary>
        public string DataSource
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "DataSource", string.Empty);
            }
        }

        /// <summary>数据库的服务器端口</summary>
        public string Port
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "Port", string.Empty);
            }
        }

        /// <summary>数据库的默认数据库名称</summary>
        public string Database
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "Database", string.Empty);
            }
        }

        /// <summary>数据库的登录帐号</summary>
        public string LoginName
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "LoginName", string.Empty);
            }
        }

        /// <summary>数据库的登录密码</summary>
        public virtual string Password
        {
            get
            {
                string password = GetKeyValue(PREFIX_KEY + "Password", string.Empty);

                return password;
                // return Encrypter.EncryptAES(password);
            }
        }

        /// <summary>连接池化</summary>
        public virtual string Pooling
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "Pooling", "true");
            }
        }

        /// <summary>连接重置</summary>
        public virtual string ConnectionReset
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "ConnectionReset", "false");
            }
        }

        /// <summary>连接回收时间 单位:秒</summary>
        public virtual string ConnectionLifetime
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "ConnectionLifetime", "10");
            }
        }

        /// <summary>连接池最小数量</summary>
        public virtual string MinPoolSize
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "MinPoolSize", "1");
            }
        }

        /// <summary>连接池最大数量</summary>
        public virtual string MaxPoolSize
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "MaxPoolSize", "100");
            }
        }

        /// <summary>连接超时时间 单位:秒</summary>
        public virtual string ConnectionTimeout
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "ConnectionTimeout", "120");
            }
        }

        /// <summary>数据库的提供器名称</summary>
        public string Provider
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "Provider", "MySql");
            }
        }

        /// <summary>数据库的 IBatis 配置文件所在根目录位置</summary>
        public string IBatisSqlMapFilePathRoot
        {
            get
            {
                return GetKeyValue(PREFIX_KEY + "IBatisSqlMapFilePathRoot", string.Empty);
            }
        }

        /// <summary>获取配置的值</summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetKeyValue(string key, string defaultValue)
        {
            return dictionary.ContainsKey(key)
                ? dictionary[key]
                : defaultValue;
        }
    }
}
