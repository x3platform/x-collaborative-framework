namespace X3Platform.Storages.Tests
{
    using System;

    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;

    using X3Platform.Storages.Configuration;

    [TestClass]
    public class StoragesContextTests
    {
        /// <summary></summary>
        [TestMethod]
        // [DeploymentItem("MySql.Data.dll")]
        public void TestLoad()
        {
            Assert.IsNotNull(StorageContext.Instance.StorageNodeService);
            Assert.IsNotNull(StorageContext.Instance.StorageSchemaService);
        }
        
        [TestMethod]
        public void TestCreate()
        {
            Trace.WriteLine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

            /*
            // 1. create adapter
            IStorageAdapter adapter1 = StorageAdapterFactory.Create<SingletonStorageAdapter>("News");

            Assert.IsNotNull(adapter1);

            IStorageAdapter adapter2 = StorageAdapterFactory.Create<SingletonStorageAdapter>("AttachmentStorage");

            Assert.IsNotNull(adapter2);
            
            // adapter2.GetStorageConnection();

            // 2. create schema

            // 3. create sh

            string commandText = "";
            // id
            // adapter.Execute(fileName, commandText);

            // StorageManager
            */
        }
    }
}
