namespace X3Platform.AttachmentStorage.TestSuite.IBLL
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;

    using X3Platform.Util;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.Util;
    using X3Platform.Data;
    using NUnit.Framework;
    using NMock;

    /// <summary></summary>
    [TestFixture]
    public class IAttachmentFileServiceTests
    {
        //初始化mockery
        private MockFactory factory = new MockFactory();

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [Test]
        public void TestSave()
        {
            Mock<IAttachmentParentObject> mock = this.factory.CreateMock<IAttachmentParentObject>(); //产生一个mock对象

            mock.Expects.Between(0,5).GetProperty(m => m.EntityId, "test_" + DateHelper.GetTimestamp());
            mock.Expects.Between(0, 5).GetProperty(m => m.EntityClassName, KernelContext.ParseObjectType(typeof(AttachmentParentObject)));
            mock.Expects.Between(0, 5).GetProperty(m => m.AttachmentEntityClassName, KernelContext.ParseObjectType(typeof(AttachmentFileInfo)));
            mock.Expects.Between(0, 5).GetProperty(m => m.AttachmentFolder, "test");

            IAttachmentParentObject parent = mock.MockObject;

            IAttachmentFileInfo param = new AttachmentFileInfo(parent);

            param.Id = UploadFileHelper.NewIdentity();
            param.AttachmentName = "test_" + DateHelper.GetTimestamp();
            param.FileType = ".txt";

            param = AttachmentStorageContext.Instance.AttachmentFileService.Save(param);

            Assert.IsNotNull(param);
        }

        [Test]
        public void TestFindOne()
        {
            IAttachmentParentObject parent = new AttachmentParentObject(StringHelper.ToGuid(),
                KernelContext.ParseObjectType(typeof(AttachmentFileInfo)),
                KernelContext.ParseObjectType(typeof(AttachmentFileInfo)),
                "test");

            IAttachmentFileInfo param = new AttachmentFileInfo(parent);

            param.Id = StringHelper.ToGuid();

            AttachmentStorageContext.Instance.AttachmentFileService.Save(param);

            param = AttachmentStorageContext.Instance.AttachmentFileService.FindOne(param.Id);

            Assert.IsNotNull(param);
        }

        [Test]
        public void TestFindAll()
        {
            IList<IAttachmentFileInfo> list = AttachmentStorageContext.Instance.AttachmentFileService.FindAll();

            Assert.IsNotNull(list);
        }

        [Test]
        public void TestGetPaging()
        {
            DataQuery query = new DataQuery();

            int rowCount = -1;

            IList<IAttachmentFileInfo> list = AttachmentStorageContext.Instance.AttachmentFileService.GetPaging(0, 10, query, out rowCount);

            Assert.IsNotNull(list);
        }
    }
}
