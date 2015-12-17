namespace X3Platform.AttachmentStorage.Tests
{
    using NUnit.Framework;

    /// <summary></summary>
    [TestFixture]
    public class AttachmentStorageContextTests
    {
        // -------------------------------------------------------
        // 测试内容
        // -------------------------------------------------------

        [Test]
        public void TestRestart()
        {
            AttachmentStorageContext.Instance.Restart();

            Assert.IsNotNull(AttachmentStorageContext.Instance.AttachmentFileService);
            Assert.IsNotNull(AttachmentStorageContext.Instance.AttachmentDistributedFileService);
            Assert.IsNotNull(AttachmentStorageContext.Instance.AttachmentWarnService);
        }
    }
}
