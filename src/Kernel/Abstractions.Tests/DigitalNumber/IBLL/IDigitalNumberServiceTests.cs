namespace X3Platform.Tests.DigitalNumber.IBLL
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

    /// <summary></summary>
    [TestClass]
    public class IDigitalNumberServiceTests
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        /// <summary>测试 加载</summary>
        [TestMethod]
        public void TestFindAll()
        {
            IList<DigitalNumberInfo> list = DigitalNumberContext.Instance.DigitalNumberService.FindAll();

            Assert.IsNotNull(list);
        }

        /// <summary>测试 保存</summary>
        [TestMethod]
        public void TestSave()
        {
            DigitalNumberInfo param = new DigitalNumberInfo();

            param.Name = "test_" + DateHelper.GetTimestamp();
            param.Expression = "{guid}";
            param.Seed = 0;

            DigitalNumberContext.Instance.DigitalNumberService.Save(param);
        }

        /// <summary>测试 保存</summary>
        [TestMethod]
        public void TestGenerate()
        {
            string result = null;

            result = DigitalNumberContext.Instance.DigitalNumberService.Generate("Key_32DigitGuid");

            Assert.IsFalse(string.IsNullOrEmpty(result));

            result = DigitalNumberContext.Instance.DigitalNumberService.Generate("Key_Guid");

            Assert.IsFalse(string.IsNullOrEmpty(result));

            result = DigitalNumberContext.Instance.DigitalNumberService.Generate("Key_Nonce");

            Assert.IsFalse(string.IsNullOrEmpty(result));

            result = DigitalNumberContext.Instance.DigitalNumberService.Generate("Key_RunningNumber");

            Assert.IsFalse(string.IsNullOrEmpty(result));

            result = DigitalNumberContext.Instance.DigitalNumberService.Generate("Key_Session");

            Assert.IsFalse(string.IsNullOrEmpty(result));

            result = DigitalNumberContext.Instance.DigitalNumberService.Generate("Key_Timestamp");

            Assert.IsFalse(string.IsNullOrEmpty(result));

        }

        /// <summary>测试 保存</summary>
        [TestMethod]
        public void TestGenerateCodeByPrefixCode()
        {
            string result = null;

            // {date:yyyyMMdd}{tag:-}{int:seed}
            result = DigitalNumberContext.Instance.DigitalNumberService.GenerateCodeByPrefixCode("tb_News", "NEWS", "{prefixCode}{code:4}");

            Assert.IsFalse(string.IsNullOrEmpty(result));

            // {date:yyyyMMdd}{tag:-}{int:seed}
            result = DigitalNumberContext.Instance.DigitalNumberService.GenerateCodeByPrefixCode("tb_News", "NEWS", "{prefixCode}{date}{code:4}");

            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        /// <summary>测试 保存</summary>
        [TestMethod]
        public void TestGenerateCodeByCategoryId()
        {
            string result = null;

            result = DigitalNumberContext.Instance.DigitalNumberService.GenerateCodeByCategoryId("tb_News", "tb_News_Category", "00000000-0000-0000-0000-000000000001", "{prefix}{date}{code}");

            Assert.IsFalse(string.IsNullOrEmpty(result));

            // result = DigitalNumberContext.Instance.DigitalNumberService.GenerateCodeByCategoryId("tb_News", "tb_News_Category", "eb14db8f-46c5-40ee-b985-49c5aa9385ed", "{prefix}{date}{code}");

            // Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}