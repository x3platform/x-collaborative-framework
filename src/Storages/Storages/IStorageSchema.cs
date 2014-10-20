namespace X3Platform.Storages
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>应用存储架构</summary>
    public interface IStorageSchema
    {
        #region 属性:Id
        /// <summary>标识</summary>
        string Id { get; set; }
        #endregion

        #region 属性:ApplicationId
        /// <summary>所属应用标识</summary>
        string ApplicationId { get; set; }
        #endregion

        #region 属性:AdapterClassName
        /// <summary>适配器类名称</summary>
        string AdapterClassName { get; set; }
        #endregion

        #region 属性:StrategyClassName
        /// <summary>策略类名称</summary>
        string StrategyClassName { get; set; }
        #endregion    
    }
}
