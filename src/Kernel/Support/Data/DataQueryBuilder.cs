
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
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="whereClause"></param>
        public static void Equal(Dictionary<string, object> args, string paramName, StringBuilder whereClause)
        {
            Operate(args, paramName, "=", whereClause);
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="valueName"></param>
        /// <param name="whereClause"></param>
        public static void Equal(Dictionary<string, object> args, string paramName, string valueName, StringBuilder whereClause)
        {
            Operate(args, paramName, "=", valueName, whereClause);
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="op"></param>
        /// <param name="whereClause"></param>
        public static void Operate(Dictionary<string, object> args, string paramName, string op, StringBuilder whereClause)
        {
            Operate(args, paramName, op, paramName, whereClause);
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="op"></param>
        /// <param name="valueName"></param>
        /// <param name="whereClause"></param>
        public static void Operate(Dictionary<string, object> args, string paramName, string op, string valueName, StringBuilder whereClause)
        {
            string temp = null;

            // 忽略空值
            if (string.IsNullOrEmpty(paramName) || !args.ContainsKey(valueName) || args[valueName] == null) return;

            temp = paramName + " " + op + " " + FormatValue(args[valueName]) + " ";

            Concat(whereClause, temp);
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="whereClause"></param>
        public static void In(Dictionary<string, object> args, string paramName, StringBuilder whereClause)
        {
            In(args, paramName, paramName, whereClause);
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="valueName"></param>
        /// <param name="whereClause"></param>
        public static void In(Dictionary<string, object> args, string paramName, string valueName, StringBuilder whereClause)
        {
            string temp = null;

            // 忽略空值
            if (string.IsNullOrEmpty(paramName) || !args.ContainsKey(valueName) || string.IsNullOrEmpty(args[valueName].ToString())) return;

            temp = paramName + " IN (" + args[valueName] + ")";

            Concat(whereClause, temp);
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="whereClause"></param>
        public static void Like(Dictionary<string, object> args, string paramName, StringBuilder whereClause)
        {
            Like(args, paramName, paramName, whereClause);
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="valueName"></param>
        /// <param name="whereClause"></param>
        public static void Like(Dictionary<string, object> args, string paramName, string valueName, StringBuilder whereClause)
        {
            string temp = null;

            // 忽略空值
            if (string.IsNullOrEmpty(paramName) || !args.ContainsKey(valueName) || string.IsNullOrEmpty(args[valueName].ToString())) return;

            temp = paramName + " LIKE '%" + args[valueName] + "%'";

            Concat(whereClause, temp);
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="paramName"></param>
        /// <param name="beginValueName"></param>
        /// <param name="endValueName"></param>
        /// <param name="whereClause"></param>
        public static void Between(Dictionary<string, object> args, string paramName, string beginValueName, string endValueName, StringBuilder whereClause)
        {
            string temp = null;

            // 忽略空值
            if (string.IsNullOrEmpty(paramName)) return;

            if (args.ContainsKey(beginValueName) && args.ContainsKey(endValueName))
            {
                temp = paramName + " BETWEEN " + FormatValue(args[beginValueName]) + " AND " + FormatValue(args[endValueName]) + " ";
            }
            else if (args.ContainsKey(beginValueName))
            {
                temp = paramName + " >= " + FormatValue(args[beginValueName]) + " ";
            }
            else if (args.ContainsKey(endValueName))
            {
                temp = paramName + " <= " + FormatValue(args[endValueName]) + " ";
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