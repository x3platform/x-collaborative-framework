namespace X3Platform.Tests.DigitalNumber
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;

    using NUnit.Framework;

    using X3Platform.DigitalNumber;
    using X3Platform.DigitalNumber.Configuration;

    /// <summary></summary>
    [TestFixture]
    public class DigitalNumberScriptTests
    {
        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        /// <summary>测试执行脚本是否运行正确</summary>
        [Test]
        public void TestRunScript()
        {
            string result = null;

            int seed = 0;

            seed = 99;

            result = DigitalNumberScript.RunScript("{dailyIncrement:seed:6}", new DateTime(2000, 1, 1), ref seed);

            Assert.AreEqual("000001", result);

            seed = 99;

            result = DigitalNumberScript.RunScript("{dailyIncrement:seed:6}", DateTime.Now, ref seed);

            Assert.AreEqual("000100", result);
        }
    }
}
