using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Data;

namespace X3Platform.TestSuite.Data
{
    [TestClass]
    public class AjaxSqlExpressionParserTests
    {
        [TestMethod]
        public void TestParse()
        {
            string result = null;

            string outString = string.Empty;

            outString = "<expression >";
            outString += "<expression key=\"key1\" value=\"value1\" type=\"string\" />";
            outString += "<expression key=\"key2\" value=\"value2\" type=\"string\" prefix=\"or\" />";
            outString += "<expression key=\"key3\" value=\"value3\" type=\"int\" prefix=\"and\" />";
            outString += "</expression>";

            result = DataQueryExpressionParser.Parse(outString);

            Assert.AreEqual(result, " ((key1=##value1##) OR (key2=##value2##) AND (key3=value3)) ");

            outString = "<expression >";
            outString += "<expression key=\"w\" value=\"r1\" type=\"string\" >";
            outString += "<expression key=\"w\" value=\"r\" type=\"string\" prefix=\"or\" />";
            outString += "</expression>";
            outString += "<expression key=\"w\" value=\"r\" type=\"string\" prefix=\"or\" />";
            outString += "<expression key=\"w\" value=\"r\" type=\"string\" prefix=\"and\" />";
            outString += "</expression>";

            result = DataQueryExpressionParser.Parse(outString);

            Assert.AreEqual(result, " ((w=##r1## OR (w=##r##)) OR (w=##r##) AND (w=##r##)) ");
        }
    }
}
