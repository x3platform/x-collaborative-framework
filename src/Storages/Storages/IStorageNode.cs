namespace X3Platform.Storages
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>应用存储节点接口</summary>
    public interface IStorageNode
    {
        #region 属性:Id
        /// <summary>节点标识</summary>
        string Id { get; set; }
        #endregion

        #region 属性:Name
        /// <summary>节点名称</summary>
        string Name { get; set; }
        #endregion

        #region 属性:Type
        /// <summary></summary>
        string Type { get; set; }
        #endregion

        #region 属性:ProviderName
        /// <summary></summary>
        string ProviderName { get; set; }
        #endregion

        #region 属性:ConnectionString
        /// <summary></summary>
        string ConnectionString { get; set; }
        #endregion

        #region 属性:ConnectionState
        /// <summary>链接状态</summary>
        int ConnectionState { get; set; }
        #endregion

        #region 属性:OrderId
        /// <summary></summary>
        string OrderId { get; set; }
        #endregion

        #region 属性:Status
        /// <summary></summary>
        int Status { get; set; }
        #endregion

        #region 属性:Remark
        /// <summary></summary>
        string Remark { get; set; }
        #endregion

        #region 属性:UpdateDate
        /// <summary></summary>
        DateTime UpdateDate { get; set; }
        #endregion

        #region 属性:CreateDate
        /// <summary></summary>
        DateTime CreateDate { get; set; }
        #endregion
    }
}
