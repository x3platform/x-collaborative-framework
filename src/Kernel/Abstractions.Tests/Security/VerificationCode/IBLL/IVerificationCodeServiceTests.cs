namespace X3Platform.Tests.Security.VerificationCode.IBLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.IBatis.Common.Utilities;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.IBatis.DataMapper.Configuration;
    using X3Platform.IBatis.DataMapper.Scope;

    using X3Platform.Configuration;

    using X3Platform.DigitalNumber;
    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.Model;
    using X3Platform.DigitalNumber.IBLL;
    using X3Platform.DigitalNumber.IDAL;
    using X3Platform.Util;
    using X3Platform.Security.VerificationCode;
    using X3Platform.Data;

    /// <summary></summary>
    [TestClass]
    public class IVerificationCodeServiceTests
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [TestMethod]
        public void TestNewCode()
        {
            // IList<VerificationCodeInfo> list = VerificationCodeContext.Instance.VerificationCodeService.FindAll();

            // Assert.IsNotNull(list);
        }

        [TestMethod]
        public void FindAll()
        {
            // IList<VerificationCodeInfo> list = VerificationCodeContext.Instance.VerificationCodeService.FindAll();

            // Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestGetPaging()
        {
            int rowCount = -1;

            DataQuery query = new DataQuery();

            IList<VerificationCodeInfo> list = VerificationCodeContext.Instance.VerificationCodeService.GetPaging(0, 10, query, out rowCount);

            Assert.IsNotNull(list);

            Assert.AreNotEqual(rowCount, -1);
        }
    }
}