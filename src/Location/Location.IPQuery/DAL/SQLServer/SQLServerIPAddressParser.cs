using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Practices.EnterpriseLibrary.Data;

using X3platform.Location.IPQuery.Configuration;

namespace X3platform.Location.IPQuery
{
    public sealed class SQLServerIPAddressParser : IPAddressParser
    {
        private string connectionString = null;

        private string tableName = "Data_IP";

        #region 属性:DataSourceName
        private string dataSourceName = null;

        /// <summary>
        /// 设置公共数据源
        /// </summary>
        public override string DataSourceName
        {
            get
            {
                return dataSourceName;
            }
            set
            {
                if (!string.Equals(dataSourceName, value))
                {
                    dataSourceName = value;

                    connectionString = ConfigurationManager.ConnectionStrings[dataSourceName].ConnectionString;
                }
            }
        }
        #endregion

        private IPQueryConfiguration configuration = null;

        public SQLServerIPAddressParser()
        {
            configuration = IPQueryConfigurationView.Instance.Configuration;

            dataSourceName = configuration.Keys["DataSourceName"].Value;

            connectionString = ConfigurationManager.ConnectionStrings[dataSourceName].ConnectionString;
        }

        /// <summary>
        /// 返回 IP地址段
        /// </summary>
        /// <param name="IPUnit">IP地址的元素</param>
        /// <param name="depth">深度 [1-4]</param>
        /// <returns>IP地址段</returns>
        public override List<IPAddressQueryInfo> Parse(byte[] IPUnit, out int depth)
        {
            List<IPAddressQueryInfo> list = new List<IPAddressQueryInfo>();

            IPAddressQueryInfo address = null;

            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();

            SqlDataAdapter da = new SqlDataAdapter();

            DataTable table = new DataTable();
            
            string SQL = string.Empty;

            cmd.Connection = conn;

            for (depth = 4; depth > 0; depth--)
            {

                SQL = string.Format(@"

SELECT 
    [Id],[StartIP],[EndIP],[StartId],[EndId],[Country],[Province],[City],[Language],[Summary] 

FROM {0} 

WHERE 
    StartIP LIKE '{1}%'

", tableName, ParseWhereClause(depth, IPUnit));

                cmd.CommandText = SQL;

                da.SelectCommand = cmd;

                da.Fill(table);

                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        address = new IPAddressQueryInfo();

                        address.Id = (table.Rows[i]["Id"] == DBNull.Value) ? 0 : (int)table.Rows[i]["Id"];

                        address.Country = (table.Rows[i]["Country"] == DBNull.Value) ? string.Empty : (string)table.Rows[i]["Country"];

                        address.Province = (table.Rows[i]["Province"] == DBNull.Value) ? string.Empty : (string)table.Rows[i]["Province"];

                        address.City = (table.Rows[i]["City"] == DBNull.Value) ? string.Empty : (string)table.Rows[i]["City"];

                        address.Language = (table.Rows[i]["Language"] == DBNull.Value) ? string.Empty : (string)table.Rows[i]["Language"];

                        address.Summary = (table.Rows[i]["Summary"] == DBNull.Value) ? string.Empty : (string)table.Rows[i]["Summary"];

                        address.StartIP = (table.Rows[i]["StartIP"] == DBNull.Value) ? string.Empty : (string)table.Rows[i]["StartIP"];

                        address.EndIP = (table.Rows[i]["EndIP"] == DBNull.Value) ? string.Empty : (string)table.Rows[i]["EndIP"];

                        Parse(address);

                        list.Add(address);
                    }

                    break;
                }
            }

            return list;
        }

        /// <summary>
        /// 分析
        /// </summary>
        /// <param name="address"></param>
        private void Parse(IPAddressQueryInfo address)
        {
            //创建表达式
            string pattern;

            bool isUpdate = false;

            Match match;
            pattern = @"(?<province>.*?)省[\s\S]*?(?<city>.*?)市\s";

            if (Regex.IsMatch(address.Country, pattern))
            {
                match = Regex.Match(address.Country, pattern);

                address.Summary = address.Country;

                address.Language = "zh-CN";

                address.Country = "中国";

                address.Province = match.Groups["province"].Value;
                address.City = match.Groups["city"].Value;

                isUpdate = true;
            }
            else
            {
                pattern = @"(?<city>.*?)市\s";

                if (Regex.IsMatch(address.Country, pattern))
                {
                    match = Regex.Match(address.Country, pattern);

                    address.Summary = address.Country;

                    address.Language = "zh-CN";

                    address.Country = "中国";

                    address.Province = match.Groups["city"].Value;
                    address.City = match.Groups["city"].Value;

                    isUpdate = true;
                }
            }

            if (isUpdate)
            {
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();

                SqlParameter param;

                string SQL = string.Format(@"

UPDATE {0} SET
 
    [Country] = @Country,
    [Province] = @Province,
    [City]=@City,
    [Language]=@Language,
    [Summary]=@Summary 

WHERE 
    [Id] = @Id 

", tableName);

                cmd.Connection = conn;

                cmd.CommandText = SQL;

                param = new SqlParameter("@Id", SqlDbType.Int);
                param.Value = address.Id;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@Country", SqlDbType.NVarChar);
                param.Value = address.Country;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@Province", SqlDbType.NVarChar);
                param.Value = address.Province;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@City", SqlDbType.NVarChar);
                param.Value = address.City;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@Language", SqlDbType.NVarChar);
                param.Value = address.Language;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@Summary", SqlDbType.NVarChar);
                param.Value = address.Summary;
                cmd.Parameters.Add(param);

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        /// <summary>
        /// 分析查询语句
        /// </summary>
        /// <param name="value">深度</param>
        /// <param name="keys">地址 数组</param>
        /// <returns></returns>
        private string ParseWhereClause(int value, byte[] keys)
        {
            string outString = string.Empty;

            int length = value;

            for (int i = 0; i < length; i++)
            {
                outString += keys[i] + ".";
            }

            return outString;
        }
    }
}
