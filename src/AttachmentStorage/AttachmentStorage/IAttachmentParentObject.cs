namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>附件父级对象</summary>
    public interface IAttachmentParentObject
    {
        /// <summary>实体标识</summary>
        string EntityId { get; set; }

        /// <summary>实体类名称</summary>
        string EntityClassName { get; set; }

        /// <summary>附件的实体类名称</summary>
        string AttachmentEntityClassName { get; set; }

        /// <summary>附件的文件夹名称</summary>
        string AttachmentFolder { get; set; }

        void Find(string id);
    }
}
