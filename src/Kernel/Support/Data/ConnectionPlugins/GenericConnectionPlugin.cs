using X3Platform.Configuration;
using X3Platform.Security;
using System.Collections.Generic;

namespace X3Platform.Data.ConnectionPlugins
{
    /// <summary>通常的数据库连接信息</summary>
    public class GenericConnectionPlugin : IConnectionPlugin
    {
        protected const string PREFIX_KEY = "DatabaseSettings.";

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
                    return string.Concat("server=", DataSource,
                        ";database=", Database,
                        ";uid=", LoginName,
                        ";pwd=", Password,
                        ";pooling=true;connection reset=false;connection lifetime=10;min pool size=1;max pool size=100;connection timeout=30;");
                }
                else if (Provider == "SqlServer")
                {
                    return string.Concat("server=", DataSource,
                        ";database=", Database,
                        ";user id=", LoginName,
                        ";password=", Password,
                        ";connection reset=false;connection lifetime=10;min pool size=1;max pool size=100;connection timeout=30;");
                }
                else if (Provider == "Oracle")
                {
                    return string.Concat("data source=", DataSource,
                        ";user id=", LoginName,
                        ";password=", Password,
                        ";connection lifetime=10;min pool size=1;max pool size=100");
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
