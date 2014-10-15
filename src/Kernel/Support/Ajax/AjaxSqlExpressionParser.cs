// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxSqlExpressionParser.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace X3Platform.Ajax
{
    /// <summary>Ajax SQL ����ʽ������</summary>
    public sealed class AjaxSqlExpressionParser
    {
        #region ��̬属性:Parse(string xml)
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

        #region ��̬属性:Parse(XmlElement element)
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

            AjaxSqlExpression expression = new AjaxSqlExpression();

            expression.LoadXml(element);

            return expression.ToString();
        }
        #endregion
    }
}
