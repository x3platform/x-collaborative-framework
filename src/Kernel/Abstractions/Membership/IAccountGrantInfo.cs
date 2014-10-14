#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :AccountGrantInfo.cs
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
    
    /// <summary>帐号委托信息接口</summary>
    public interface IAccountGrantInfo : ISerializedObject
    {
        #region 属性:Id
        /// <summary>委托标识</summary>
        string Id { get; set; }
        #endregion

        #region 属性:Code
        /// <summary>编码</summary>
        string Code { get; set; }
        #endregion

        #region 属性:Grantor
        /// <summary>委托人</summary>
        IAccountInfo Grantor { get; }
        #endregion

        #region 属性:GrantorId
        /// <summary>委托人</summary>
        string GrantorId { get; set; }
        #endregion

        #region 属性:GrantorName
        /// <summary>委托人姓名</summary>
        string GrantorName { get; }
        #endregion

        #region 属性:Grantee
        /// <summary>被委托人</summary>
        IAccountInfo Grantee { get; }
        #endregion

        #region 属性:GranteeId
        /// <summary>被委托人</summary>
        string GranteeId { get; set; }
        #endregion

        #region 属性:GranteeName
        /// <summary>被委托人姓名</summary>
        string GranteeName { get; }
        #endregion

        #region 属性:GrantedTimeFrom
        /// <summary></summary>
        DateTime GrantedTimeFrom { get; set; }
        #endregion

        #region 属性:GrantedTimeTo
        /// <summary></summary>
        DateTime GrantedTimeTo { get; set; }
        #endregion

        #region 属性:WorkflowGrantMode
        /// <summary>流程审批委托类别：1 未激活的流程审批只发待办给被委托的人 | 2 未激活的流程审批同时发送待办给委托人和被委托人 | 4 已激活的流程审批移交给被委托人 | 8 已激活的流程审批不移交给被委托人</summary>
        int WorkflowGrantMode { get; set; }
        #endregion

        #region 属性:DataQueryGrantMode
        /// <summary>数据查询的模式：0 不委托 | 1 委托</summary>
        int DataQueryGrantMode { get; set; }
        #endregion

        #region 属性:IsAborted
        /// <summary>是否中止</summary>
        bool IsAborted { get; set; }
        #endregion

        #region 属性:Status
        /// <summary>状态</summary>
        int Status { get; set; }
        #endregion

        #region 属性:Remark
        /// <summary>备注信息</summary>
        string Remark { get; set; }
        #endregion

        #region 属性:UpdateDate
        /// <summary>修改时间</summary>
        DateTime UpdateDate { get; set; }
        #endregion

        #region 属性:CreateDate
        /// <summary>创建时间</summary>
        DateTime CreateDate { get; set; }
        #endregion
    }
}
