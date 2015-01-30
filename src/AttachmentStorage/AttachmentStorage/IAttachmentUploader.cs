namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using System;
    using System.Web;
    #endregion

    /// <summary>附件父级对象</summary>
    public interface IAttachmentUploader
    {
        int Upload(HttpContext context);
    }
}
