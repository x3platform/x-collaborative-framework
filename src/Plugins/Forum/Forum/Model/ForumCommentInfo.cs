#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ForumCommentInfo.cs
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
    public class ForumCommentInfo : ForumInfo
    {
        #region 构造函数:ForumCommentInfo()
        /// <summary>默认构造函数</summary>
        public ForumCommentInfo()
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
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:ThreadId
        private string m_ThreadId = string.Empty;

        /// <summary></summary>
        public string ThreadId
        {
            get { return this.m_ThreadId; }
            set { this.m_ThreadId = value; }
        }
        #endregion

        #region 属性:Member
        private ForumMemberInfo m_Member;

        /// <summary>个人论坛信息</summary>
        public ForumMemberInfo Member
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.AccountId) || string.IsNullOrEmpty(this.ApplicationTag)))
                {
                    m_Member = ForumContext.Instance.ForumMemberService.FindOneByAccountId(this.AccountId);
                }
                else
                {
                    m_Member = null;
                }

                return m_Member;
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

        #region 属性:MemberScore

        public int MemberScore
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

        #region 属性:ReplyComment
        private ForumCommentInfo m_ReplyComment;
        /// <summary>
        /// 回复信息
        /// </summary>
        public ForumCommentInfo ReplyComment
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.ReplyCommentId) || string.IsNullOrEmpty(this.ThreadId) || string.IsNullOrEmpty(this.ApplicationTag)))
                {
                    m_ReplyComment = ForumContext.Instance.ForumCommentService.FindOne(this.ReplyCommentId, this.ThreadId);
                }
                return m_ReplyComment;
            }
        }
        #endregion

        #region 属性:ReplyCommentId
        private string m_ReplyCommentId = string.Empty;

        /// <summary></summary>
        public string ReplyCommentId
        {
            get { return this.m_ReplyCommentId; }
            set { this.m_ReplyCommentId = value; }
        }
        #endregion

        #region 属性:ReplyCommentRowIndex

        public int ReplyCommentRowIndex
        {
            get { return this.ReplyComment == null ? 0 : this.ReplyComment.RowIndex; }
        }
        #endregion

        #region 属性:ReplyCommentName
        public string ReplyCommentName
        {
            get
            {
                string name = "";
                if (this.ReplyComment != null)
                {
                    name = this.ReplyComment.Anonymous == 1 ? "匿名" : this.ReplyComment.AccountName;
                }
                return name;
            }
        }
        #endregion

        #region 属性:ReplyCommentContent
        public string ReplyCommentContent
        {
            get { return this.ReplyComment == null ? string.Empty : this.ReplyComment.Content; }
        }
        #endregion

        #region 属性:ReplyCommentCreateDate
        public DateTime ReplyCommentCreateDate
        {
            get { return this.ReplyComment == null ? new DateTime() : this.ReplyComment.CreateDate; }
        }
        #endregion

        #region 属性:ReplyCommentAnonymous
        public int ReplyCommentAnonymous
        {
            get { return this.ReplyComment == null ? 0 : this.ReplyComment.Anonymous; }
        }
        #endregion

        #region 属性:Content
        private string m_Content = string.Empty;

        /// <summary></summary>
        public string Content
        {
            get { return this.m_Content; }
            set { this.m_Content = value; }
        }
        #endregion

        #region 属性:Heart
        private string m_Heart = string.Empty;

        public string Heart
        {
            get { return this.m_Heart; }
            set { this.m_Heart = value; }
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

        /// <summary></summary>
        public int AttachmentFileCount
        {
            get { return this.m_AttachmentFileCount; }
            set { this.m_AttachmentFileCount = value; }
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

        /// <summary>创建时间</summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion

        #region 属性:RowIndex
        private int m_RowIndex;

        public int RowIndex
        {
            get { return this.m_RowIndex; }
            set { this.m_RowIndex = value; }
        }
        #endregion
    }
}
