
namespace X3Platform.Data
{
    #region Using Libraries
    using System;

    using X3Platform.Util;
    using System.Text;
    using System.Data;
    using System.Collections.Generic;
    #endregion

    /// <summary>数据查询构建器</summary>
    public class DataQueryBuilder
    {
        /// <summary></summary>
        /// <param name="where"></param>
        /// <param name="paramName"></param>
        /// <param name="whereClause"></param>
        public static void Equal(Dictionary<string, object> where, string paramName, StringBuilder whereClause)
        {
            Operate(where, paramName, "=", whereClause);
        }

        /// <summary></summary>
        /// <param name="where"></param>
        /// <param name="paramName"></param>
        /// <param name="whereClause"></param>
        public static void Operate(Dictionary<string, object> where, string paramName, string op, StringBuilder whereClause)
        {
            string temp = null;

            // 忽略空值
            if (string.IsNullOrEmpty(paramName) || !where.ContainsKey(paramName)) return;

            temp = paramName + " " + op + " " + FormatValue(where[paramName]) + " ";

            Concat(whereClause, temp);
        }

        /// <summary></summary>
        /// <param name="where"></param>
        /// <param name="paramName"></param>
        /// <param name="beginParamName"></param>
        /// <param name="endParamName"></param>
        /// <param name="whereClause"></param>
        public static void Between(Dictionary<string, object> where, string paramName, string beginParamName, string endParamName, StringBuilder whereClause)
        {
            string temp = null;

            // 忽略空值
            if (string.IsNullOrEmpty(paramName)) return;

            if (where.ContainsKey(beginParamName) && where.ContainsKey(endParamName))
            {
                temp = paramName + " BETWEEN " + FormatValue(where[beginParamName]) + " AND " + FormatValue(where[endParamName]) + " ";
            }
            else if (where.ContainsKey(beginParamName))
            {
                temp = paramName + " >= " + FormatValue(where[beginParamName]) + " ";
            }
            else if (where.ContainsKey(endParamName))
            {
                temp = paramName + " <= " + FormatValue(where[endParamName]) + " ";
            }

            Concat(whereClause, temp);
        }

        /// <summary>格式化输出</summary>
        /// <param name="whereClause"></param>
        /// <param name="temp"></param>
        private static void Concat(StringBuilder whereClause, string temp)
        {
            if (!string.IsNullOrEmpty(temp))
            {
                if (whereClause.Length == 0)
                {
                    whereClause.Append(temp);
                }
                else
                {
                    whereClause.Append("AND " + temp);
                }
            }
        }

        /// <summary>格式化输出的值</summary>
        /// <param name="value">值</param>
        public static string FormatValue(object value)
        {
            string typeName = value.GetType().FullName;

            switch (typeName)
            {
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Double":
                case "System.Decimal":
                    return value.ToString();
                case "System.Boolean":
                    return Convert.ToBoolean(value) ? "1" : "0";
                case "System.Guid":
                    return string.Concat("'", value, "'");
                case "System.DateTime":
                    return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss.ff");
                case "System.String":
                    return string.Concat("'", value, "'");
                case "System.Array":
                    return string.Empty;
                default:
                    return string.Concat("'", value, "'");
            }
        }
    }
}