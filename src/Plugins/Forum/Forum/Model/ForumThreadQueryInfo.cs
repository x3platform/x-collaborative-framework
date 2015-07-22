#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ForumThreadInfo.cs
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
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Membership;
    #endregion

    /// <summary>帖子查询对象</summary>
    public class ForumThreadQueryInfo : ForumInfo
    {
        #region 构造函数:ForumThreadQueryInfo()
        /// <summary>默认构造函数</summary>
        public ForumThreadQueryInfo()
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

        /// <summary>帐户标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        private string m_AccountName = string.Empty;

        /// <summary>帐户姓名</summary>
        public string AccountName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AccountName))
                {
                    this.m_AccountName = "匿名";
                }
                return m_AccountName;
            }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:CategoryId
        private string m_CategoryId = string.Empty;

        /// <summary></summary>
        public string CategoryId
        {
            get { return this.m_CategoryId; }
            set { this.m_CategoryId = value; }
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

        #region 属性:CategoryName
        public string CategoryName
        {
            get { return this.CategoryIndex.Replace("\\", "-"); }
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

        #region 属性:Title
        private string m_Title = string.Empty;

        /// <summary></summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }
        #endregion

        #region 属性:Click
        private int m_Click;

        /// <summary></summary>
        public int Click
        {
            get { return this.m_Click; }
            set { this.m_Click = value; }
        }
        #endregion

        #region 属性:LatestCommentAccountId
        private string m_LatestCommentAccountId = string.Empty;

        public string LatestCommentAccountId
        {
            get { return this.m_LatestCommentAccountId; }
            set { this.m_LatestCommentAccountId = value; }
        }
        #endregion

        #region 属性:LatestCommentAccountName
        private string m_LatestCommentAccountName = string.Empty;

        public string LatestCommentAccountName
        {
            get { return this.m_LatestCommentAccountName; }
            set { this.m_LatestCommentAccountName = value; }
        }
        #endregion

        #region 属性:CommentCount
        private int m_CommentCount;

        public int CommentCount
        {
            get { return this.m_CommentCount; }
            set { this.m_CommentCount = value; }
        }
        #endregion

        #region 属性:IsTop
        private int m_IsTop;

        /// <summary></summary>
        public int IsTop
        {
            get { return this.m_IsTop; }
            set { this.m_IsTop = value; }
        }
        #endregion

        #region 属性:TopExpiryDate
        private DateTime m_TopExpiryDate;

        /// <summary></summary>
        public DateTime TopExpiryDate
        {
            get { return this.m_TopExpiryDate; }
            set { this.m_TopExpiryDate = value; }
        }
        #endregion

        #region 属性:IsHot
        private int m_IsHot;

        /// <summary></summary>
        public int IsHot
        {
            get { return this.m_IsHot; }
            set { this.m_IsHot = value; }
        }
        #endregion

        #region 属性:HotExpiryDate
        private DateTime m_HotExpiryDate;

        /// <summary></summary>
        public DateTime HotExpiryDate
        {
            get { return this.m_HotExpiryDate; }
            set { this.m_HotExpiryDate = value; }
        }
        #endregion

        #region 属性:IsEssential
        private int m_IsEssential;

        public int IsEssential
        {
            get { return this.m_IsEssential; }
            set { this.m_IsEssential = value; }
        }
        #endregion

        #region 属性:Anonymous
        private int m_Anonymous;

        /// <summary></summary>
        public int Anonymous
        {
            get { return this.m_Anonymous; }
            set { this.m_Anonymous = value; }
        }
        #endregion

        #region 属性:IP
        private string m_IP = string.Empty;

        /// <summary></summary>
        public string IP
        {
            get { return this.m_IP; }
            set { this.m_IP = value; }
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

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion
    }
}
