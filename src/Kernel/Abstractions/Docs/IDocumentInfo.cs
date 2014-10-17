namespace X3Platform.Docs
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>文档接口</summary>
    public interface IDocumentInfo
    {
        /// <summary>文档标识</summary>
        string Id { get; }

        string DocVersion { get; }

        /// <summary>文档状态</summary>
        DocStatus DocStatus { get; set; }
    }
}
