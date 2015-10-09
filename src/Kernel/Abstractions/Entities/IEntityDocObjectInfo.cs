namespace X3Platform.Entities
{
    using System;

    /// <summary>实体对象文档信息接口</summary>
    public interface IEntityDocObjectInfo
    {
        /// <summary>文档唯一标识</summary>
        string Id { get; }

        /// <summary>文档标题</summary>
        string DocTitle { get; }

        /// <summary>文档全局标识(不同的版本之间，共用一个 DocToken。)</summary>
        string DocToken { get; }

        /// <summary>文档版本</summary>
        string DocVersion { get; }

        /// <summary>文档状态</summary>
        string DocStatus { get; }

        /// <summary>更新日期</summary>
        DateTime ModifiedDate { get; }

        /// <summary>创建日期</summary>
        DateTime CreatedDate { get; }
    }
}
