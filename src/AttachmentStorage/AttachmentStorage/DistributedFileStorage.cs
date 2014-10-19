namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using X3Platform.Util;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.Util;
    #endregion

    /// <summary>虚拟附件文件信息</summary>
    public static class DistributedFileStorage
    {
        /// <summary>保存附件信息</summary>
        public static void Upload(IAttachmentFileInfo file)
        {
            //
            // 保存 数据库
            // 数据库 支持数据库集群
            // 

            if (AttachmentStorageConfigurationView.Instance.DistributedFileStorageMode == "ON")
            {
                DistributedFileInfo param = new DistributedFileInfo();

                param.Id = file.Id;
                param.VirtualPath = file.VirtualPath;
                param.FileData = file.FileData;

                AttachmentStorageContext.Instance.AttachmentDistributedFileService.Save(param);
            }

            //
            // 保存 二进制数据
            //

            string path = UploadPathHelper.CombinePhysicalPath(file.Parent.AttachmentFolder, string.Format("{0}{1}", file.Id, file.FileType));

            UploadPathHelper.TryCreateDirectory(path);

            ByteHelper.ToFile(file.FileData, path);
        }

        /// <summary>获取附件信息</summary>
        public static byte[] Download(IAttachmentFileInfo file)
        {
            DistributedFileInfo param = AttachmentStorageContext.Instance.AttachmentDistributedFileService.FindOne(file.Id);

            if (param == null)
            {
                return null;
            }
            else
            {
                string path = file.VirtualPath.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.PhysicalUploadFolder);

                UploadPathHelper.TryCreateDirectory(path);

                ByteHelper.ToFile(param.FileData, path);

                return param.FileData;
            }
        }

        /// <summary>获取附件信息</summary>
        public static int Delete(string id)
        {
            AttachmentStorageContext.Instance.AttachmentDistributedFileService.Delete(id);

            return 0;
        }
    }
}
