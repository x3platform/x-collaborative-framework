#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :FavoriteInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Favorite.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary></summary>
    public class FavoriteInfo : EntityClass
    {
        #region 构造函数:FavoriteInfo()
        /// <summary>默认构造函数</summary>
        public FavoriteInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary></summary>
        public string AccountId
        {
            get { return this.m_AccountId; }
            set { this.m_AccountId = value; }
        }
        #endregion

        #region 属性:CategoryIndex
        private string m_CategoryIndex;

        /// <summary></summary>
        public string CategoryIndex
        {
            get { return this.m_CategoryIndex; }
            set { this.m_CategoryIndex = value; }
        }
        #endregion

        #region 属性:Title
        private string m_Title;

        /// <summary></summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description;

        /// <summary></summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }
        #endregion

        #region 属性:Url
        private string m_Url;

        /// <summary></summary>
        public string Url
        {
            get { return this.m_Url; }
            set { this.m_Url = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags;

        /// <summary></summary>
        public string Tags
        {
            get { return this.m_Tags; }
            set { this.m_Tags = value; }
        }
        #endregion

        #region 属性:IconPath
        private string m_IconPath;

        /// <summary></summary>
        public string IconPath
        {
            get { return this.m_IconPath; }
            set { this.m_IconPath = value; }
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

        #region 属性:Click
        private int m_Click;

        /// <summary></summary>
        public int Click
        {
            get { return this.m_Click; }
            set { this.m_Click = value; }
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

        // -------------------------------------------------------
        // 设置 EntityClass 标识
        // -------------------------------------------------------

        #region 属性:EntityId
        /// <summary>实体对象标识</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion
    }
}
