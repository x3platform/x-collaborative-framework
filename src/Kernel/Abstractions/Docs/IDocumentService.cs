using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Docs
{
    /// <summary>文档服务接口</summary>
    public interface IDocumentService
    {
        void Save(IDocumentInfo document);
    }
}
