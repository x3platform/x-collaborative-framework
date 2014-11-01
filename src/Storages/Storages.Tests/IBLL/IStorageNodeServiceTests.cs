namespace X3Platform.Storages.Tests.IBLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IStorageNodeServiceTests
    {
        [TestMethod]
        public void TestFindAllBySchemaId()
        {
            var result = StorageContext.Instance.StorageNodeService.FindAllBySchemaId("01-14-Sessions");

            Assert.IsNotNull(result);

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void TestFindOne()
        {
            var result = StorageContext.Instance.StorageNodeService.FindOne("01-14-Sessions-Node-01");

            Assert.IsNotNull(result);
        }
    }
}
