namespace X3Platform.Docs
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>文档服务接口</summary>
    public interface IDocumentService
    {
        void Save(IDocumentInfo document);
    }
}
