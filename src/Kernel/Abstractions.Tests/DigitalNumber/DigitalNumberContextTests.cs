namespace X3Platform.Tests.DigitalNumber
{
    using System;

    using NUnit.Framework;

    using X3Platform.DigitalNumber;

    using X3Platform.DigitalNumber.Configuration;

    /// <summary></summary>
    [TestFixture]
    public class DigitalNumberContextTests
    {
        //-------------------------------------------------------
        // ��������
        //-------------------------------------------------------

        /// <summary>���� ����</summary>
        [Test]
        public void TestLoad()
        {
            Assert.IsNotNull(DigitalNumberContext.Instance.DigitalNumberService);
        }
    }
}
