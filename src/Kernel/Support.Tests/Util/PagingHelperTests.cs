namespace X3Platform.Tests.Util
{
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Util;

    /// <summary>∑÷“≥π‹¿Ì∆˜</summary>
    [TestClass]
    public class PagingHelperTests
    {
        [TestMethod]
        public void TestCreate()
        {
            string outString = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<root>
    <action><![CDATA[getPaging]]></action>
    <paging><rowIndex>100</rowIndex><pageSize>15</pageSize><rowCount>1000</rowCount><pageCount>100</pageCount><currentPage>10</currentPage><firstPage>0</firstPage><previousPage>0</previousPage><nextPage>0</nextPage><lastPage>0</lastPage><whereClause></whereClause><orderBy></orderBy></paging>
</root>";

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(outString);

            string pagingXml = XmlHelper.Fetch("paging", doc, "xml");

            PagingHelper paging = PagingHelper.Create(pagingXml);

            Assert.AreEqual(15, paging.PageSize);
            Assert.AreEqual(10, paging.CurrentPage);
        }
    }
}