using System;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Reflection;

using X3Platform.Configuration;
using X3Platform.IBatis.Common.Logging;
using X3Platform.IBatis.DataMapper;
using X3Platform.IBatis.DataMapper.Configuration;
using X3Platform.Logging;
using X3Platform.IBatis.Common.Utilities;
using System.Collections.Generic;
using X3Platform.Security;
using X3Platform.IBatis.DataMapper.SessionStore;

namespace X3Platform.IBatis.DataMapper
{
    /// <summary>配置映射文件辅助函数</summary>
    public sealed class ISqlMapHelper
    {
        private static readonly X3Platform.Logging.IInternalLog logger = X3Platform.Logging.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<string, ISqlMapper> dict = new Dictionary<string, ISqlMapper>();

        /// <summary>创建 SqlMapper</summary>
        /// <param name="ibatisMaping">配置文件路径</param>
        /// <returns></returns>
        public static ISqlMapper CreateSqlMapper(string ibatisMaping)
        {
            return CreateSqlMapper(ibatisMaping, true);
        }

        /// <summary>创建 SqlMapper</summary>
        /// <param name="ibatisMaping">配置文件路径</param>
        /// <param name="sessionStore"></param>
        /// <returns></returns>
        public static ISqlMapper CreateSqlMapper(string ibatisMaping, ISessionStore sessionStore)
        {
            return CreateSqlMapper(ibatisMaping, sessionStore, true);
        }

        /// <summary>创建 SqlMapper</summary>
        /// <param name="ibatisMaping">配置文件路径</param>
        /// <param name="throwException">是否抛出异常信息</param>
        /// <returns></returns>
        public static ISqlMapper CreateSqlMapper(string ibatisMaping, bool throwException)
        {
            return CreateSqlMapper(ibatisMaping, null, throwException);
        }

        /// <summary>创建 SqlMapper</summary>
        /// <param name="ibatisMaping">配置文件路径</param>
        /// <param name="sessionStore"></param>
        /// <param name="throwException">是否抛出异常信息</param>
        /// <returns></returns>
        public static ISqlMapper CreateSqlMapper(string ibatisMaping, ISessionStore sessionStore, bool throwException)
        {
            // 部分操作系统系统上 MD5 加密方法 报如下异常
            // This implementation is not part of the Windows Platform FIPS validated cryptographic algorithms
            // string key = Encrypter.EncryptMD5(ibatisMaping);
            string key = Encrypter.EncryptSHA1(ibatisMaping);

            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            ISqlMapper db = null;

            try
            {
                DomSqlMapBuilder builder = new DomSqlMapBuilder();

                XmlDocument doc = null;

                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    ibatisMaping = ibatisMaping.Replace("\\", "/");
                }

                if (ibatisMaping.IndexOf("embedded:") == 0)
                {
                    doc = Resources.GetEmbeddedResourceAsXmlDocument(ibatisMaping.Substring(9));
                }
                else if (ibatisMaping.IndexOf("url:") == 0)
                {
                    doc = Resources.GetUrlAsXmlDocument(ibatisMaping.Substring(4));
                }
                else if (ibatisMaping.IndexOf("resource:") == 0)
                {
                    doc = Resources.GetResourceAsXmlDocument(ibatisMaping.Substring(9));
                }
                else
                {
                    doc = new XmlDocument();

                    doc.Load(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, KernelConfigurationView.Instance.ReplaceKeyValue(ibatisMaping)));
                }

                db = builder.Configure(doc);

                if (sessionStore != null)
                {
                    db.SessionStore = sessionStore;
                }

                dict.Add(key, db);
            }
            catch (Exception ex)
            {
                db = null;

                if (throwException)
                    throw ex;

                logger.Error("Error loading file : " + ibatisMaping + ".\r\n" + ex.ToString());
            }

            return db;
        }

        public static ISqlMapper ChangeConnectionString(ISqlMapper db, string connectionString)
        {
            db.DataSource.ConnectionString = connectionString;

            return db;
        }

        public static ISqlMapper ChangeConnectionStringByDataSourceName(ISqlMapper db, string dataSourceName)
        {
            return ChangeConnectionString(db, ConfigurationManager.ConnectionStrings[dataSourceName].ConnectionString);
        }
    }
}