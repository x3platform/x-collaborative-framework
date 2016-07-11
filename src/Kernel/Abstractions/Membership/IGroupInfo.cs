namespace X3Platform.Membership
{
    #region Using Libraries
    using System.Collections.Generic;
    #endregion

    /// <summary>群组信息</summary>
    public interface IGroupInfo : IAuthorizationObject
    {
        #region 属性:Code
        /// <summary>编号</summary>
        string Code { get; set; }
        #endregion

        #region 属性:GlobalName
        /// <summary>全局名称</summary>
        string GlobalName { get; }
        #endregion

        #region 属性:PinYin
        /// <summary>拼音</summary>
        string PinYin { get; set; }
        #endregion

        #region 属性:CatalogItemId
        /// <summary>分组类别节点标识</summary>
        string CatalogItemId { get; set; }
        #endregion

        #region 属性:EnableExchangeEmail
        /// <summary>启用企业邮箱</summary>
        int EnableExchangeEmail { get; set; }
        #endregion

        #region 属性:FullPath
        /// <summary>所属组织架构全路径</summary>
        string FullPath { get; set; }
        #endregion

        #region 属性:DistinguishedName
        /// <summary>唯一名称</summary>
        string DistinguishedName { get; set; }
        #endregion

        #region 属性:Members
        /// <summary>成员列表</summary>
        IList<IAccountInfo> Members { get; }
        #endregion

        // -------------------------------------------------------
        // 重置成员关系
        // -------------------------------------------------------

        #region 函数:ResetMemberRelations(string relationText)
        /// <summary>重置成员关系</summary>
        /// <param name="relationText"></param>
        void ResetMemberRelations(string relationText);
        #endregion
    }
}
