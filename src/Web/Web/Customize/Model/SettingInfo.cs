//=============================================================================
//
// Copyright (c) 2008 X3Platform
//
// FileName     :
//
// Description  :
//
// Author       :X3Platform
//
// Date         :2008-06-22
//
//=============================================================================
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using X3Platform.Membership;

namespace X3Platform.Web.Customize.Model
{
    /// <summary>页面信息</summary>
    [Serializable]
    public class SettingInfo : EntityClass
    {
        public SettingInfo()
        {
        }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Account
        private IAccountInfo m_Account = null;

        /// <summary>帐号</summary>
        public IAccountInfo Account
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_Id))
                {
                    m_Account = MembershipManagement.Instance.AccountService[this.AccountId];
                }

                return m_Account;
            }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary>模板标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        /// <summary>帐号名称</summary>
        public string AccountName
        {
            get { return string.Format("{0}({1})", this.Account.Name, this.Account.LoginName); }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:Title
        private string m_Title;

        /// <summary>标题</summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        #endregion

        #region 属性:Html
        private string m_Html;

        /// <summary>Html代码</summary>
        public string Html
        {
            get { return m_Html; }
            set { m_Html = value; }
        }
        #endregion

        #region 属性:Version
        private int m_Version;

        /// <summary>版本</summary>
        public int Version
        {
            get { return m_Version; }
            set { m_Version = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private string m_UpdateDate;

        /// <summary>修改时间</summary>
        public string UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private string m_CreateDate;

        /// <summary>创建时间</summary>
        public string CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

        //
        // 设置 EntityClass 标识
        // 

        #region 属性:EntityId
        /// <summary>实体对象标识</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion

    }
}
