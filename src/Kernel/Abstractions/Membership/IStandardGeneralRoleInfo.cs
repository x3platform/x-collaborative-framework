namespace X3Platform.Membership
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>标准通用角色信息</summary>
    public interface IStandardGeneralRoleInfo : IAuthorizationObject
    {
        #region 属性:Code
        /// <summary>编号</summary>
        string Code { get; set; }
        #endregion

        #region 属性:CatalogItemId
        /// <summary>分组类别节点标识</summary>
        string CatalogItemId { get; set; }
        #endregion

        #region 属性:OrderId
        /// <summary>排序标识</summary>
        string OrderId { get; set; }
        #endregion
    }
}
