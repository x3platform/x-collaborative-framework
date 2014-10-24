// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

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

namespace X3Platform.IBatis.DataMapper
{
    /// <summary>配置映射文件辅助函数</summary>
    public sealed class ISqlMapHelper
    {
        private static readonly X3Platform.Logging.IInternalLog logger = X3Platform.Logging.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>创建 SqlMapper</summary>
        /// <param name="ibatisMaping">配置文件路径</param>
        /// <returns></returns>
        public static ISqlMapper CreateSqlMapper(string ibatisMaping)
        {
            return CreateSqlMapper(ibatisMaping, false);
        }

        /// <summary>创建 SqlMapper</summary>
        /// <param name="ibatisMaping">配置文件路径</param>
        /// <param name="throwException">是否抛出异常信息</param>
        /// <returns></returns>
        public static ISqlMapper CreateSqlMapper(string ibatisMaping, bool throwException)
        {
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