// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AttachmentParentObject.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.AttachmentStorage
{
    using X3Platform.Util;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.Util;

    /// <summary>���⸽���ļ���Ϣ</summary>
    public static class DistributedFileStorage
    {
        /// <summary>���渽����Ϣ</summary>
        public static void Upload(IAttachmentFileInfo file)
        {
            //
            // ���� ���ݿ�
            // ���ݿ� ֧�����ݿ⼯Ⱥ
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
            // ���� ����������
            //

            string path = UploadPathHelper.CombinePhysicalPath(file.Parent.AttachmentFolder, string.Format("{0}{1}", file.Id, file.FileType));

            UploadPathHelper.TryCreateDirectory(path);

            ByteHelper.ToFile(file.FileData, path);
        }

        /// <summary>��ȡ������Ϣ</summary>
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

        /// <summary>��ȡ������Ϣ</summary>
        public static int Delete(string ids)
        {
            AttachmentStorageContext.Instance.AttachmentDistributedFileService.Delete(ids);

            return 0;
        }
    }
}
