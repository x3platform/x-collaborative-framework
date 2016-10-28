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

    /// <summary>上传文件工具函数</summary>
    public sealed class UploadFileHelper
    {
        /// <summary>生成新的附件标识</summary>
        /// <returns></returns>
        public static string NewIdentity()
        {
            // 注:StringHelper.ToRandom("0123456789", 6)
            // 随机数生成函数在产生随机数时, 系统内部机制线程会暂停1毫秒钟以等待定时器的推进, 避免在时间极短的情况下生成相同的随机数
            
            switch (AttachmentStorageConfigurationView.Instance.IdentityFormat.ToLower())
            {
                case "yyyymmddhhmmssfff":
                    // datetime(17) + randomtext(6) = 唯一标识长度(23) 
                    return string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssfff"), StringHelper.ToRandom("0123456789", 6));
                case "timestamp":
                    // timestamp(13) + randomtext(6) = 唯一标识长度(19)
                    return string.Concat(DateHelper.GetTimestamp(), StringHelper.ToRandom("0123456789", 6));
                case "guid":
                default:
                    // 唯一标识长度 = 36
                    return StringHelper.ToGuid();
            }
        }

        #region 函数:CreateAttachmentFile(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder, string fileName, byte[] fileData)
        /// <summary>创建附件</summary>
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

        #region 函数:CreateAttachmentFile(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder, string fileName, byte[] fileData)
        /// <summary>创建附件</summary>
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

        /// <summary>创建附件</summary>
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

        /// <summary>创建附件</summary>
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

        /// <summary>创建附件</summary>
        /// <param name="applicationName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IAttachmentFileInfo CreateAttachmentFile(IAttachmentParentObject parent, string attachmentId, string attachmentName, string fileType, int fileSize, byte[] fileData)
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = AttachmentStorageConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            IAttachmentFileInfo param = objectBuilder.GetObject<IAttachmentFileInfo>(typeof(IAttachmentFileInfo), new object[] { parent });

            param.Id = string.IsNullOrEmpty(attachmentId) ? NewIdentity() : attachmentId;
            param.AttachmentName = attachmentName;
            param.FileType = fileType;
            param.FileSize = fileSize;
            param.FileData = fileData;
            param.CreatedDate = DateTime.Now;

            // 虚拟路径需要创建时间和文件类型参数
            param.VirtualPath = UploadPathHelper.GetVirtualPathFormat(parent.AttachmentFolder, param);

            param.FolderRule = AttachmentStorageConfigurationView.Instance.PhysicalUploadFolderRule;

            return param;
        }
    }
}