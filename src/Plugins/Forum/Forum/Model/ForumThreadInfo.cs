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
    
    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    public class ForumThreadInfo : ForumInfo
    {
        #region 构造函数:ForumThreadInfo()
        /// <summary>默认构造函数</summary>
        public ForumThreadInfo()
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

        /// <summary>帐户标识</summary>
        public string AccountName
        {
            get
            {
                return m_AccountName;
            }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:Member
        private ForumMemberInfo m_Member = null;

        /// <summary>个人论坛信息</summary>
        public ForumMemberInfo Member
        {
            get
            {
                if (this.m_Member == null && !string.IsNullOrEmpty(this.AccountId))
                {
                    this.m_Member = ForumContext.Instance.ForumMemberService.FindOneByAccountId(this.AccountId);
                }

                return this.m_Member;
            }
        }
        #endregion

        #region 属性:MemberSignature

        public string MemberSignature
        {
            get { return this.Member == null ? string.Empty : this.Member.Signature; }
        }

        #endregion

        #region 属性:MemberIconPath
        private string m_MemberIconPath = null;

        public string MemberIconPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_MemberIconPath))
                {
                    IAccountInfo account = MembershipManagement.Instance.AccountService[this.AccountId];

                    string avatar_120x120 = string.Empty;

                    if (string.IsNullOrEmpty(account.CertifiedAvatar))
                    {
                        avatar_120x120 = AttachmentStorageConfigurationView.Instance.VirtualUploadFolder + "avatar/default_120x120.png";
                    }
                    else
                    {
                        avatar_120x120 = account.CertifiedAvatar.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.VirtualUploadFolder);
                    }

                    this.m_MemberIconPath = avatar_120x120;

                    // return this.Member == null ? string.Empty : this.Member.IconPath;
                }

                return this.m_MemberIconPath;
            }
        }
        #endregion

        #region 属性:MemberPoint

        public int MemberPoint
        {
            get { return this.Member == null ? 0 : this.Member.Point; }
        }
        #endregion

        #region 属性:MemberThreadCount
        public int MemberThreadCount
        {
            get { return this.Member == null ? 0 : this.Member.PublishThreadCount; }
        }
        #endregion

        #region 属性:MemberFollowCount
        public int MemberFollowCount
        {
            get { return this.Member == null ? 0 : this.Member.FollowCount; }
        }
        #endregion

        #region 属性:MemberOrganizationPath
        public string MemberOrganizationPath
        {
            get { return this.Member == null ? string.Empty : this.Member.OrganizationPath; }
        }
        #endregion

        #region 属性:MemberHeadship
        public string MemberHeadship
        {
            get { return this.Member == null ? string.Empty : this.Member.Headship; }
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

        #region 属性:CategoryAdminName
        private string m_CategoryAdminName = string.Empty;

        public string CategoryAdminName
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.AccountId) || string.IsNullOrEmpty(this.ApplicationTag)))
                {
                    m_CategoryAdminName = ForumContext.Instance.ForumCategoryService.GetCategoryAdminName(this.CategoryId);
                }

                return m_CategoryAdminName;
            }
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

        /// <summary>标题</summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }
        #endregion

        #region 属性:Content
        private string m_Content = string.Empty;

        /// <summary>内容</summary>
        public string Content
        {
            get { return this.m_Content; }
            set { this.m_Content = value; }
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

        /// <summary></summary>
        public string LatestCommentAccountId
        {
            get { return this.m_LatestCommentAccountId; }
            set { this.m_LatestCommentAccountId = value; }
        }
        #endregion

        #region 属性:LatestCommentAccountName
        private string m_LatestCommentAccountName = string.Empty;

        /// <summary></summary>
        public string LatestCommentAccountName
        {
            get { return this.m_LatestCommentAccountName; }
            set { this.m_LatestCommentAccountName = value; }
        }
        #endregion

        #region 属性:CommentCount
        private int m_CommentCount;

        /// <summary>回复统计数</summary>
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

        #region 属性:AttachmentFileCount
        private int m_AttachmentFileCount;

        /// <summary>附件个数统计</summary>
        public int AttachmentFileCount
        {
            get { return this.m_AttachmentFileCount; }
            set { this.m_AttachmentFileCount = value; }
        }
        #endregion

        #region 属性:Locking
        private int m_Locking;

        /// <summary>锁定帖子</summary>
        public int Locking
        {
            get { return this.m_Locking; }
            set { this.m_Locking = value; }
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

        #region 属性:UpdateHistoryLog
        private string m_UpdateHistoryLog = string.Empty;

        /// <summary>更新历史记录</summary>
        public string UpdateHistoryLog
        {
            get { return this.m_UpdateHistoryLog; }
            set { this.m_UpdateHistoryLog = value; }
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
