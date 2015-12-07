using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X3Platform.Membership;
using X3Platform.Membership.Model;

namespace X3Platform.Plugins.Forum.Model
{
    public class ForumMemberInfo : ForumInfo
    {
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

        #region 属性:OrganizationPath
        private string m_OrganizationPath = string.Empty;

        /// <summary>帐户标识</summary>
        public string OrganizationPath
        {
            get { return m_OrganizationPath; }
            set { m_OrganizationPath = value; }
        }
        #endregion

        #region 属性:Headship
        private string m_Headship = string.Empty;

        /// <summary>论坛头衔</summary>
        public string Headship
        {
            get { return m_Headship; }
            set { m_Headship = value; }
        }
        #endregion

        #region 属性:Signature
        private string m_Signature = string.Empty;

        /// <summary>帐户标识</summary>
        public string Signature
        {
            get { return m_Signature; }
            set { m_Signature = value; }
        }
        #endregion

        #region 属性:IconPath
        private string m_IconPath = string.Empty;

        /// <summary>帐户标识</summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }
        #endregion

        #region 属性:Point
        private int m_Point = 0;

        /// <summary>论坛积分</summary>
        public int Point
        {
            get { return m_Point; }
            set { m_Point = value; }
        }
        #endregion

        #region 属性:PublishThreadCount
        private int m_PublishThreadCount;

        /// <summary>发布的帖子统计</summary>
        public int PublishThreadCount
        {
            get { return this.m_PublishThreadCount; }
            set { this.m_PublishThreadCount = value; }
        }
        #endregion

        #region 属性:PublishCommentCount
        private int m_PublishCommentCount;

        /// <summary>发布的回帖统计</summary>
        public int PublishCommentCount
        {
            get { return this.m_PublishCommentCount; }
            set { this.m_PublishCommentCount = value; }
        }
        #endregion

        #region 属性:FollowCount
        private int m_FollwCount;

        /// <summary>关注的用户统计</summary>
        public int FollowCount
        {
            get { return this.m_FollwCount; }
            set { this.m_FollwCount = value; }
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
