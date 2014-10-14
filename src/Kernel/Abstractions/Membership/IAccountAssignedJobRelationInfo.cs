#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>帐户和岗位的关联信息</summary>
    public interface IAccountAssignedJobRelationInfo
    {
        #region 属性:AccountId
        /// <summary>帐号标识</summary>
        string AccountId { get; set; }
        #endregion

        #region 属性:AccountGlobalName
        /// <summary>帐号全局名称</summary>
        string AccountGlobalName { get; set; }
        #endregion

        #region 属性:AssignedJobId
        /// <summary>岗位标识</summary>
        string AssignedJobId { get; set; }
        #endregion

        #region 属性:AssignedJobGlobalName
        /// <summary>岗位全局名称</summary>
        string AssignedJobGlobalName { get; set; }
        #endregion

        #region 属性:IsDefault
        /// <summary>是否默认岗位</summary>
        int IsDefault { get; set; }
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

        #region 函数:GetAssignedJob()
        IAssignedJobInfo GetAssignedJob();
        #endregion
    }
}
