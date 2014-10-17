namespace X3Platform.AttachmentStorage.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Drawing;
    using System.Threading;
    using System.Web;

    using X3Platform.Util;
    using X3Platform.Spring;

    using X3Platform.AttachmentStorage.Configuration;

    /// <summary>�ϴ��ļ����ߺ���</summary>
    public sealed class UploadFileHelper
    {
        /// <summary>���ɸ���</summary>
        /// <returns></returns>
        public static string GenerateIdentity()
        {
            switch (AttachmentStorageConfigurationView.Instance.IdentityFormat.ToLower())
            {
                case "yyyymmddhhmmssfff":
                    // ��ͣ1���룬���������ϵͳ��������ظ�����
                    // Thread.Sleep(1);
                    return DateTime.Now.ToString("yyyyMMddHHmmssfff") + StringHelper.ToRandom("0123456789", 6);

                case "guid":
                default:
                    return StringHelper.ToGuid();
            }
        }

        /// <summary>��������</summary>
        /// <param name="applicationName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IAttachmentFileInfo CreateAttachmentFile(IAttachmentParentObject parent, HttpPostedFile file)
        {
            return CreateAttachmentFile(parent,
                file.FileName,
                Path.GetExtension(file.FileName),
                file.ContentLength,
                StreamHelper.ToBytes(file.InputStream));
        }

        /// <summary>��������</summary>
        /// <param name="applicationName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IAttachmentFileInfo CreateAttachmentFile(IAttachmentParentObject parent, string attachmentName, string fileType, int fileSize, byte[] fileData)
        {
            return CreateAttachmentFile(parent,
                GenerateIdentity(),
                attachmentName,
                fileType,
                fileSize,
                fileData);
        }

        /// <summary>��������</summary>
        /// <param name="applicationName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IAttachmentFileInfo CreateAttachmentFile(IAttachmentParentObject parent, string attachmentId, string attachmentName, string fileType, int fileSize, byte[] fileData)
        {
            // �������󹹽���(Spring.NET)
            string springObjectFile = AttachmentStorageConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // �������ݷ������
            IAttachmentFileInfo param = objectBuilder.GetObject<IAttachmentFileInfo>(typeof(IAttachmentFileInfo), new object[] { parent });

            param.Id = string.IsNullOrEmpty(attachmentId) ? GenerateIdentity() : attachmentId;
            param.AttachmentName = attachmentName;
            param.FileType = fileType;
            param.FileSize = fileSize;
            param.FileData = fileData;
            param.CreateDate = DateTime.Now;

            //����·����Ҫ����ʱ����ļ����Ͳ���
            param.VirtualPath = UploadPathHelper.GetVirtualPathFormat(parent.AttachmentFolder, param);

            return param;
        }
    }
}