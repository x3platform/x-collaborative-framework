namespace X3Platform.Entities
{
    using System;

<<<<<<< HEAD
    /// <summary>实体对象点击信息接口</summary>
=======
    /// <summary>实体类点击信息接口</summary>
>>>>>>> 58ed63ff8ace2b23714ed4928a94b727f28c707e
    public interface IEntityClickInfo
    {
        /// <summary>实体类标识</summary>
        string EntityId { get; }

        /// <summary>实体类名称</summary>
        string EntityClassName { get; }

        /// <summary>帐号标识</summary>
        string AccountId { get; }

        /// <summary>帐号名称</summary>
        string AccountName { get; }

        /// <summary>点击数</summary>
        int Click { get; }

        /// <summary>最后更新时间</summary>
        DateTime UpdateDate { get; }
    }
}
