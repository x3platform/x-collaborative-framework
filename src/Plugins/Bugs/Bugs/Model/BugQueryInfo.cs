#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Apps;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    #endregion

    /// <summary>问题查询信息</summary>
    [Serializable]
    public class BugQueryInfo
    {
        #region 构造函数:NewsCategoryQueryInfo()
        /// <summary>默认构造函数</summary>
        public BugQueryInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary>编号</summary>
        public string Code
        {
            get { return this.m_Code; }
            set { this.m_Code = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary>帐号标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        private string m_AccountName;

        /// <summary>创建人姓名</summary>
        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:CategoryIndex
        private string m_CategoryIndex;

        /// <summary></summary>
        public string CategoryIndex
        {
            get { return m_CategoryIndex.Replace(@"\", "-"); }
            set { m_CategoryIndex = value; }
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

        #region 属性:Tags
        private string m_Tags = string.Empty;

        /// <summary>标签</summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region 属性:AssignToAccountId
        private string m_AssignToAccountId;

        /// <summary>指派帐号标识</summary>
        public string AssignToAccountId
        {
            get { return m_AssignToAccountId; }
            set { m_AssignToAccountId = value; }
        }
        #endregion

        #region 属性:AssignToAccountName

        private string m_AssignToAccountName;
        /// <summary>指派帐号名称</summary>
        public string AssignToAccountName
        {
            get { return m_AssignToAccountName; }
            set { m_AssignToAccountName = value; }
        }
        #endregion

        #region 属性:Priority
        private int m_Priority;

        /// <summary>权重值</summary>
        public int Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary>状态: 0.新问题 | 1.确认中 | 2.处理中 | 3.已解决 | 4.已关闭</summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary>修改日期</summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>创建日期</summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion
    }
}
