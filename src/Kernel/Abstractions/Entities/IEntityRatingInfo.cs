namespace X3Platform.Entities
{
    using System;

    /// <summary>实体对象评级信息接口</summary>
    public interface IEntityRatingInfo
    {
        /// <summary>实体类标识</summary>
        string EntityId { get; }

        /// <summary>实体类名称</summary>
        string EntityClassName { get; }

        /// <summary>帐号标识</summary>
        string AccountId { get; }

        /// <summary>帐号名称</summary>
        string AccountName { get; }

        /// <summary>评级</summary>
        int Rating { get; }

        /// <summary>最后更新时间</summary>
        DateTime ModifiedDate { get; }
    }
}
