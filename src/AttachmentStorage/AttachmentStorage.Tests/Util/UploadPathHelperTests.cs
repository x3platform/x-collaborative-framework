namespace X3Platform.AttachmentStorage.Tests.Util
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;

    using NUnit.Framework;

    using X3Platform.Configuration;
    using X3Platform.Spring.Configuration;
    using X3Platform.Util;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.Util;
    using X3Platform.AttachmentStorage.Images;

    /// <summary></summary>
    [TestFixture]
    public class UploadPathHelperTests
    {
        /// <summary>垃圾桶</summary>
        public Dictionary<string, AttachmentFileInfo> trash = new Dictionary<string, AttachmentFileInfo>();

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [Test]
        public void TestTryCreateDirectory()
        {
            string path = KernelConfigurationView.Instance.ApplicationPathRoot + "uploads\\";

            DateTime datetime = DateTime.Now;

            path = path + datetime.ToString("yyyy\\MM\\dd\\");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            Assert.IsTrue(Directory.Exists(path));
        }

        [Test]
        public void TestCombinePhysicalPath()
        {
            string attachmentFolder = "test";
            string fileName = "123.doc";

            DateTime datetime = DateTime.Now;

            string path1 = AttachmentStorageConfigurationView.Instance.PhysicalUploadFolder
                + attachmentFolder + "/"
                + datetime.Year + "/" + (((datetime.Month - 1) / 3) + 1) + "Q/" + datetime.Month + "/"
                + fileName;

            string path2 = UploadPathHelper.CombinePhysicalPath(attachmentFolder, fileName);

            Assert.AreEqual(path1, path2);
        }

        [Test]
        public void TestCombineVirtualPath()
        {
            string attachmentFolder = "test";
            string fileName = "123.doc";

            DateTime datetime = DateTime.Now;

            string path1 = AttachmentStorageConfigurationView.Instance.VirtualUploadFolder
                + attachmentFolder + "/"
                + datetime.Year + "/" + (((datetime.Month - 1) / 3) + 1) + "Q/" + datetime.Month + "/"
                + fileName;

            string path2 = UploadPathHelper.CombineVirtualPath(attachmentFolder, fileName);

            Assert.AreEqual(path1, path2);
        }

        [Test]
        public void TestGetVirtualPathFormat()
        {
            string attachmentFolder = "test";

            AttachmentFileInfo attachment = new AttachmentFileInfo();
            
            attachment.Id = StringHelper.ToGuid();
            attachment.FileType = ".doc";
            attachment.AttachmentName = "123.doc";
            attachment.CreatedDate = new DateTime(2001, 1, 1);

            DateTime datetime = attachment.CreatedDate;

            string path1 = "{uploads}" + attachmentFolder + "/"
                + datetime.Year + "/" + (((datetime.Month - 1) / 3) + 1) + "Q/" + datetime.Month + "/"
                + attachment.Id
                + attachment.FileType;
            
            string path2 = UploadPathHelper.GetVirtualPathFormat(attachmentFolder, attachment);

            Assert.AreEqual(path1, path2);
        }
    }
}
