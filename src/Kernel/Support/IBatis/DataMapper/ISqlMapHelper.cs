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

namespace X3Platform.IBatis.DataMapper
{
    /// <summary>����ӳ���ļ���������</summary>
    public sealed class ISqlMapHelper
    {
        private static readonly X3Platform.Logging.IInternalLog logger = X3Platform.Logging.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����SQLMapper</summary>
        /// <param name="dataSourceName"></param>
        /// <returns></returns>
        public static ISqlMapper CreateSqlMapper(string ibatisMaping)
        {
            return CreateSqlMapper(ibatisMaping, false);
        }

        /// <summary>����SQLMapper</summary>
        /// <param name="dataSourceName"></param>
        /// <returns></returns>
        public static ISqlMapper CreateSqlMapper(string ibatisMaping, bool throwException)
        {
            ISqlMapper db = null;

            try
            {
                XmlDocument doc = new XmlDocument();

                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    ibatisMaping = ibatisMaping.Replace("\\", "/");
                }

                doc.Load(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, KernelConfigurationView.Instance.ReplaceKeyValue(ibatisMaping)));

                DomSqlMapBuilder builder = new DomSqlMapBuilder();

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