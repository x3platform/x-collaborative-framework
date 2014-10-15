#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityOperationLogInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Entities.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Membership;
    using X3Platform;
    #endregion

    /// <summary>操作日志</summary>
    public class EntityOperationLogInfo
    {
        #region 构造函数:EntityOperationLogInfo()
        /// <summary>默认构造函数</summary>
        public EntityOperationLogInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:EntityId
        private string m_EntityId;

        /// <summary></summary>
        public string EntityId
        {
            get { return m_EntityId; }
            set { m_EntityId = value; }
        }
        #endregion

        #region 属性:EntityClassName
        private string m_EntityClassName;

        /// <summary></summary>
        public string EntityClassName
        {
            get { return m_EntityClassName; }
            set { m_EntityClassName = value; }
        }
        #endregion

        #region 属性:Account
        private IAccountInfo m_Account = null;

        /// <summary>帐户</summary>
        public IAccountInfo Account
        {
            get
            {
                if (m_Account == null && !string.IsNullOrEmpty(this.AccountId))
                {
                    m_Account = MembershipManagement.Instance.AccountService[this.AccountId];
                }

                return m_Account;
            }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary></summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        /// <summary></summary>
        public string AccountName
        {
            get { return this.Account == null ? string.Empty : this.Account.Name; }
        }
        #endregion

        #region 属性:OperationType
        private int m_OperationType;

        /// <summary>操作类型 1:催办 2:推荐</summary>
        public int OperationType
        {
            get { return m_OperationType; }
            set { m_OperationType = value; }
        }
        #endregion

        #region 属性:ToAuthorizationObject
        private IAuthorizationObject m_ToAuthorizationObject = null;

        /// <summary>帐户</summary>
        public IAuthorizationObject ToAuthorizationObject
        {
            get
            {
                if (m_ToAuthorizationObject == null
                    && !string.IsNullOrEmpty(this.ToAuthorizationObjectType)
                    && !string.IsNullOrEmpty(this.ToAuthorizationObjectId))
                {
                    m_ToAuthorizationObject = MembershipManagement.Instance.AuthorizationObjectService[this.ToAuthorizationObjectType, this.ToAuthorizationObjectId];
                }

                return m_ToAuthorizationObject;
            }
        }
        #endregion

        #region 属性:ToAuthorizationObjectType
        private string m_ToAuthorizationObjectType;

        /// <summary></summary>
        public string ToAuthorizationObjectType
        {
            get { return m_ToAuthorizationObjectType; }
            set { m_ToAuthorizationObjectType = value; }
        }
        #endregion

        #region 属性:ToAuthorizationObjectId
        private string m_ToAuthorizationObjectId;

        /// <summary></summary>
        public string ToAuthorizationObjectId
        {
            get { return m_ToAuthorizationObjectId; }
            set { m_ToAuthorizationObjectId = value; }
        }
        #endregion

        #region 属性:ToAuthorizationObjectName
        /// <summary></summary>
        public string ToAuthorizationObjectName
        {
            get { return this.ToAuthorizationObject == null ? string.Empty : this.ToAuthorizationObject.Name; }
        }
        #endregion

        #region 属性:Reason
        private string m_Reason;

        /// <summary></summary>
        public string Reason
        {
            get { return m_Reason; }
            set { m_Reason = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
