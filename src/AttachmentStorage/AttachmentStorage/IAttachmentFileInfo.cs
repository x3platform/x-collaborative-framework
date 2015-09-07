namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using System;
    using X3Platform.Spring;
    #endregion

    /// <summary>附件信息接口</summary>
    [SpringObject("X3Platform.AttachmentStorage.IAttachmentFileInfo")]
    public interface IAttachmentFileInfo
    {
        /// <summary>文件标识</summary>
        string Id { get; set; }

        /// <summary>父级对象</summary>
        IAttachmentParentObject Parent { get; }

        /// <summary>父级对象实体标识</summary>
        string EntityId { get; }

        /// <summary>父级对象实体类名称</summary>
        string EntityClassName { get; }

        /// <summary>附件名称</summary>
        string AttachmentName { get; set; }

        /// <summary>虚拟路径</summary>
        string VirtualPath { get; set; }

        /// <summary>目录规则</summary>
        string FolderRule { get; set; }
        
        /// <summary>文件类型</summary>
        string FileType { get; set; }

        /// <summary>文件大小</summary>
        int FileSize { get; set; }

        /// <summary>文件状态</summary>
        int FileStatus { get; set; }

        /// <summary>数据</summary>
        byte[] FileData { get; set; }

        /// <summary>创建时间</summary>
        DateTime CreatedDate { get; set; }

        /// <summary>还原</summary>
        void Restore(string id);
        
        /// <summary>保存</summary>
        void Save();
    }
}
