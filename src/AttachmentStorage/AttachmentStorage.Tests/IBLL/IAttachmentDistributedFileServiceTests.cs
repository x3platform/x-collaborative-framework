namespace X3Platform.AttachmentStorage.Tests.IBLL
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using NUnit.Framework;
    
    using X3Platform.Spring.Configuration;
    using X3Platform.Util;
    
    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.Data;
    
    /// <summary></summary>
    [TestFixture]
    public class IAttachmentDistributedFileServiceTests
    {
        // -------------------------------------------------------
        // 测试内容
        // -------------------------------------------------------

        [Test]
        public void TestSave()
        {
            DistributedFileInfo param = new DistributedFileInfo();

            param.Id = StringHelper.ToGuid();

            param = AttachmentStorageContext.Instance.AttachmentDistributedFileService.Save(param);

            Assert.IsNotNull(param);
        }

        [Test]
        public void TestFindOne()
        {
            DistributedFileInfo param = new DistributedFileInfo();

            param.Id = StringHelper.ToGuid();

            AttachmentStorageContext.Instance.AttachmentDistributedFileService.Save(param);

            param = AttachmentStorageContext.Instance.AttachmentDistributedFileService.FindOne(param.Id);

            Assert.IsNotNull(param);
        }

        [Test]
        public void TestFindAll()
        {
            IList<DistributedFileInfo> list = AttachmentStorageContext.Instance.AttachmentDistributedFileService.FindAll();

            Assert.IsNotNull(list);
        }
    }
}
