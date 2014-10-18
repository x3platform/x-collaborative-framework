namespace X3Platform.Membership
{
    using System;

    /// <summary>帐户和群组的关联信息</summary>
    public interface IAccountGroupRelationInfo
    {
        #region 属性:AccountId
        /// <summary>帐号标识</summary>
        string AccountId { get; set; }
        #endregion

        #region 属性:AccountGlobalName
        /// <summary>帐号全局名称</summary>
        string AccountGlobalName { get; set; }
        #endregion

        #region 属性:GroupId
        /// <summary>群组标识</summary>
        string GroupId { get; set; }
        #endregion

        #region 属性:GroupGlobalName
        /// <summary>群组全局名称</summary>
        string GroupGlobalName { get; set; }
        #endregion

        #region 属性:BeginDate
        /// <summary>生效时间</summary>
        DateTime BeginDate { get; set; }
        #endregion

        #region 属性:EndDate
        /// <summary>失效时间</summary>
        DateTime EndDate { get; set; }
        #endregion

        #region 函数:GetAccount()
        IAccountInfo GetAccount();
        #endregion

        #region 函数:GetGroup()
        IGroupInfo GetGroup();
        #endregion
    }
}
