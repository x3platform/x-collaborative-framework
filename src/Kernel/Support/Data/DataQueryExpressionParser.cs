using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace X3Platform.Data
{
    /// <summary>数据查询表达式解析器</summary>
    public sealed class DataQueryExpressionParser
    {
        #region 静态函数:Parse(string xml)
        /// <summary></summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string Parse(string xml)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            return Parse(doc.DocumentElement);
        }
        #endregion

        #region 静态函数:Parse(XmlElement element)
        /// <summary></summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string Parse(XmlElement element)
        {
            // <expression >
            //  <expression key="" value"" type="" prefix="and" >
            //  <expression key="" value"" type="" >
            // </expression >
            //

            DataQueryExpression expression = new DataQueryExpression();

            expression.LoadXml(element);

            return expression.ToString();
        }
        #endregion
    }
}
