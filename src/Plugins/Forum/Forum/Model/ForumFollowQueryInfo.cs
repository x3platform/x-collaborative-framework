#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ForumFollowQueryInfo.cs
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
    #endregion

    /// <summary></summary>
    public class ForumFollowQueryInfo : ForumInfo
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

        #region 属性:MemberDisplayName
        private string m_MemberDisplayName = string.Empty;

        public string MemberDisplayName
        {
            get { return m_MemberDisplayName; }
            set { m_MemberDisplayName = value; }
        }
        #endregion

        #region 属性:MemberIconPath
        private string m_MemberIconPath = string.Empty;

        public string MemberIconPath
        {
            get { return m_MemberIconPath; }
            set { m_MemberIconPath = value; }
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

        #region 属性:Title
        private string m_Title = string.Empty;

        /// <summary></summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
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

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return this.m_ModifiedDate; }
            set { this.m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:Sign
        private int m_Sign;

        /// <summary></summary>
        public int Sign
        {
            get { return this.m_Sign; }
            set { this.m_Sign = value; }
        }
        #endregion
    }
}
