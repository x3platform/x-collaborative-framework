#region Using Libraries
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using X3Platform.AttachmentStorage.Configuration;
using X3Platform.Util;
#endregion

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
                + ParseRule(attachmentFolder, DateTime.Now).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
                + fileName;
        }

        public static string CombineVirtualPath(string attachmentFolder, string fileName)
        {
            return VirtualUploadFolder
                + ParseRule(attachmentFolder, DateTime.Now).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + fileName;
        }

        public static string GetVirtualPathFormat(string attachmentFolder, IAttachmentFileInfo attachment)
        {
            return "{uploads}"
                + ParseRule(attachmentFolder, attachment.CreateDate).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + attachment.Id
                + attachment.FileType;
        }

        public static string GetAttachmentFolder(string virtualPath, string folderRule)
        {
            string[] keys = folderRule.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            string[] results = virtualPath.Replace("{uploads}", string.Empty).Split(new char[] { '\\', '/' });

            if (keys.Length == results.Length - 1)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] == "folder")
                    {
                        return results[i];
                    }
                }
            }

            return string.Empty;
        }

        public static void TryCreateDirectory(string path)
        {
            path = Path.GetDirectoryName(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static string ParseRule(string folder, DateTime datetime)
        {
            // 路径规则示例
            // folder\year\quarter\month\
            // folder\year\quarter\folder

            string text = AttachmentStorageConfigurationView.Instance.PhysicalUploadFolderRule;

            return text.Replace("folder", folder.ToLower())
                       .Replace("year", datetime.Year.ToString())
                       .Replace("quarter", ((((datetime.Month - 1) / 3) + 1) + "Q"))
                       .Replace("month", datetime.Month.ToString());
        }
    }
}