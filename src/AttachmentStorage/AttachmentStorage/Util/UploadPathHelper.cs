using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using X3Platform.AttachmentStorage.Configuration;
using X3Platform.Util;

namespace X3Platform.AttachmentStorage.Util
{
    /// <summary>上传路径管理</summary>
    public sealed class UploadPathHelper
    {
        public static string PhysicalUploadFolder
        {
            get { return AttachmentStorageConfigurationView.Instance.PhysicalUploadFolder; }
        }

        public static string VirtualUploadFolder
        {
            get { return AttachmentStorageConfigurationView.Instance.VirtualUploadFolder; }
        }

        /// <summary>组合物理路径</summary>
        /// <param name="applicationName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CombinePhysicalPath(string attachmentFolder, string fileName)
        {
            return PhysicalUploadFolder
                + attachmentFolder.ToLower() + "/"
                + DirectoryHelper.FormatTimePath(DateTime.Now) + "/"
                + fileName;
        }

        public static string CombineVirtualPath(string attachmentFolder, string fileName)
        {
            return VirtualUploadFolder
                + attachmentFolder.ToLower() + "/"
                + DirectoryHelper.FormatTimePath(DateTime.Now) + "/"
                + fileName;
        }

        public static string GetVirtualPathFormat(string attachmentFolder, IAttachmentFileInfo attachment)
        {
            return "{uploads}" + attachmentFolder.ToLower() + "/"
                + DirectoryHelper.FormatTimePath(attachment.CreateDate)
                + attachment.Id
                + attachment.FileType;
        }

        public static string GetAttachmentFolder(string virtualPath)
        {
            string result = virtualPath.Replace("{uploads}", "");
            return result.Substring(0, result.IndexOf("/"));
        }

        public static void TryCreateDirectory(string path)
        {
            path = Path.GetDirectoryName(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

    }
}