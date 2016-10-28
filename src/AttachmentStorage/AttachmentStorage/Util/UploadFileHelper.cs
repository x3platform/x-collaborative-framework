namespace X3Platform.AttachmentStorage.Util
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Drawing;
    using System.Threading;
    using System.Web;

    using X3Platform.Util;
    using X3Platform.Spring;

    using X3Platform.AttachmentStorage.Configuration;
    #endregion

    /// <summary>�ϴ��ļ����ߺ���</summary>
    public sealed class UploadFileHelper
    {
        /// <summary>�����µĸ�����ʶ</summary>
        /// <returns></returns>
        public static string NewIdentity()
        {
            // ע:StringHelper.ToRandom("0123456789", 6)
            // ��������ɺ����ڲ��������ʱ, ϵͳ�ڲ������̻߳���ͣ1�������Եȴ���ʱ�����ƽ�, ������ʱ�伫�̵������������ͬ�������
            
            switch (AttachmentStorageConfigurationView.Instance.IdentityFormat.ToLower())
            {
                case "yyyymmddhhmmssfff":
                    // datetime(17) + randomtext(6) = Ψһ��ʶ����(23) 
                    return string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssfff"), StringHelper.ToRandom("0123456789", 6));
                case "timestamp":
                    // timestamp(13) + randomtext(6) = Ψһ��ʶ����(19)
                    return string.Concat(DateHelper.GetTimestamp(), StringHelper.ToRandom("0123456789", 6));
                case "guid":
                default:
                    // Ψһ��ʶ���� = 36
                    return StringHelper.ToGuid();
            }
        }

        #region ����:CreateAttachmentFile(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder, string fileName, byte[] fileData)
        /// <summary>��������</summary>
        /// <param name="entityId"></param>
        /// <param name="entityClassName"></param>
        /// <param name="attachmentEntityClassName"></param>
        /// <param name="attachmentFolder"></param>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public static IAttachmentFileInfo CreateAttachmentFile(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder, HttpPostedFile file)
        {
            return CreateAttachmentFile(
                entityId,
                entityClassName,
                attachmentEntityClassName,
                attachmentFolder,
                file.FileName,
                StreamHelper.ToBytes(file.InputStream));
        }
        #endregion

        #region ����:CreateAttachmentFile(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder, string fileName, byte[] fileData)
        /// <summary>��������</summary>
        /// <param name="entityId"></param>
        /// <param name="entityClassName"></param>
        /// <param name="attachmentEntityClassName"></param>
        /// <param name="attachmentFolder"></param>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public static IAttachmentFileInfo CreateAttachmentFile(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder, string fileName, byte[] fileData)
        {
            IAttachmentParentObject parent = new AttachmentParentObject(entityId, entityClassName, attachmentEntityClassName, attachmentFolder);

            return UploadFileHelper.CreateAttachmentFile(parent, fileName, Path.GetExtension(fileName), fileData.Length, fileData);
        }
        #endregion

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
                NewIdentity(),
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

            param.Id = string.IsNullOrEmpty(attachmentId) ? NewIdentity() : attachmentId;
            param.AttachmentName = attachmentName;
            param.FileType = fileType;
            param.FileSize = fileSize;
            param.FileData = fileData;
            param.CreatedDate = DateTime.Now;

            // ����·����Ҫ����ʱ����ļ����Ͳ���
            param.VirtualPath = UploadPathHelper.GetVirtualPathFormat(parent.AttachmentFolder, param);

            param.FolderRule = AttachmentStorageConfigurationView.Instance.PhysicalUploadFolderRule;

            return param;
        }
    }
}