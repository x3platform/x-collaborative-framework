#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IDocumentInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Data
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    /// <summary>数据结果映射</summary>
    public class DataResultMapper
    {
        string dataTableName = string.Empty;

        IDictionary<string, DataResultMap> dictionary = new Dictionary<string, DataResultMap>();

        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public DataResultMap this[string propertyName]
        {
            get
            {
                return dictionary.ContainsKey(propertyName) ? dictionary[propertyName] : null;
            }
        }
        #endregion

        #region 属性:Count
        /// <summary>数量</summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        #endregion

        #region 函数:AddDataResultMap(DataResultMap resultMap)
        /// <summary>索引</summary>
        private void AddDataResultMap(DataResultMap resultMap)
        {
            if (dictionary.ContainsKey(resultMap.PropertyName))
            {
                dictionary.Add(resultMap.PropertyName, resultMap);
            }
            else
            {
                dictionary[resultMap.PropertyName] = resultMap;
            }
        }
        #endregion

        public string GetDataColumnSql()
        {
            StringBuilder outString = new StringBuilder();

            foreach (KeyValuePair<string, DataResultMap> item in this.dictionary)
            {
                if (item.Value.DataColumnName == item.Value.PropertyName)
                {
                    outString.AppendFormat("{0}, ", item.Value.DataColumnName);
                }
                else
                {
                    if (string.IsNullOrEmpty(item.Value.DataColumnCastType))
                    {
                        outString.AppendFormat("{0} AS {1}, ", item.Value.DataColumnName, item.Value.PropertyName);
                    }
                    else
                    {
                        outString.AppendFormat("CAST({0} AS {1}) AS {2}, ", item.Value.DataColumnName, item.Value.DataColumnCastType, item.Value.PropertyName);
                    }
                }
            }

            return outString.ToString().Trim().TrimEnd(',');
        }

        public string GetSelectSql()
        {
            return string.Format(" SELECT {0} FROM {1} ", this.GetDataColumnSql(), this.dataTableName);
        }

        public static DataResultMapper CreateMapper(XmlElement element)
        {
            DataResultMapper mapper = new DataResultMapper();

            // mapper.dataTableName = element.SelectSingleNode("resultMap/dataTableName");

            XmlNodeList nodes = element.SelectNodes("resultMap/result");

            foreach (XmlNode node in nodes)
            {
                mapper.AddDataResultMap(new DataResultMap(node.Attributes["property"].Value, node.Attributes["column"].Value));
            }

            return mapper;
        }
    }
}
