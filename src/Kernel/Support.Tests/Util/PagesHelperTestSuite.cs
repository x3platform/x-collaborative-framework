//=============================================================================
//
// Copyright (c) 2009 X3Platform
//
// Filename     :PagingHelper.cs
//
// Summary      :pages helper
//
// Author       :X3Platform
//
// Date			:2008-04-26
//
//=============================================================================


using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Util;

namespace X3Platform.Tests.Utility
{
    /// <summary>∑÷“≥π‹¿Ì∆˜</summary>
    [TestClass]
    public class PagingHelperTestSuite
    {
        [TestMethod]
        public void TestCreate()
        {
            string outString = @"<?xml version=""1.0"" encoding=""utf-8"" ?>

<root>
<action><![CDATA[getPages]]></action>
<pages><rowIndex>0</rowIndex><pageSize>8</pageSize><rowCount>0</rowCount><pageCount>0</pageCount><currentPage>0</currentPage><firstPage>0</firstPage><previousPage>0</previousPage><nextPage>0</nextPage><lastPage>0</lastPage><whereClause></whereClause><orderBy></orderBy></pages>
</root>";

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(outString);

            string pagesXml = AjaxStorageConvertor.Fetch("pages", doc,"xml");

            PagingHelper pages = PagingHelper.Create(pagesXml);
        }
    }
}