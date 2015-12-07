#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ForumCategoryQueryInfo.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Forum.Model
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary></summary>
    public class ForumCategoryQueryInfo : ForumInfo
    {
        #region 构造函数:ForumCategoryInfo()
        /// <summary>默认构造函数</summary>
        public ForumCategoryQueryInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary></summary>
        public string AccountId
        {
            get { return this.m_AccountId; }
            set { this.m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        private string m_AccountName = string.Empty;

        /// <summary></summary>
        public string AccountName
        {
            get { return this.m_AccountName; }
            set { this.m_AccountName = value; }
        }
        #endregion

        #region 属性:CategoryIndex
        private string m_CategoryIndex = string.Empty;

        /// <summary></summary>
        public string CategoryIndex
        {
            get { return this.m_CategoryIndex; }
            set { this.m_CategoryIndex = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary></summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return this.m_Status; }
            set { this.m_Status = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return this.m_ModifiedDate; }
            set { this.m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return this.m_CreatedDate; }
            set { this.m_CreatedDate = value; }
        }
        #endregion
    }
}
