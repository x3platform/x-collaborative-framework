#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2011 Elane, ruany@chinasic.com
//
// FileName     :ForumCategoryInfo.cs
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
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    #endregion

    /// <summary></summary>
    public class ForumCategoryInfo : EntityClass
    {
        #region 构造函数:ForumCategoryInfo()
        /// <summary>默认构造函数</summary>
        public ForumCategoryInfo()
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

        #region 属性:Anonymous
        private int m_Anonymous;

        /// <summary></summary>
        public int Anonymous
        {
            get { return this.m_Anonymous; }
            set { this.m_Anonymous = value; }
        }
        #endregion

        #region 属性:PublishThreadPoint
        private int m_PublishThreadPoint;

        /// <summary>发布帖子的积分</summary>
        public int PublishThreadPoint
        {
            get { return this.m_PublishThreadPoint; }
            set { this.m_PublishThreadPoint = value; }
        }
        #endregion

        #region 属性:PublishCommentPoint
        private int m_PublishCommentPoint;
        /// <summary>发布回复的积分</summary>
        public int PublishCommentPoint
        {
            get { return this.m_PublishCommentPoint; }
            set { this.m_PublishCommentPoint = value; }
        }
        #endregion

        #region 属性:TodayCount
        private int m_TodayCount;

        /// <summary></summary>
        public int TodayCount
        {
            get { return this.m_TodayCount; }
            set { this.m_TodayCount = value; }
        }
        #endregion

        #region 属性:WeekCount
        private int m_WeekCount;

        /// <summary></summary>
        public int WeekCount
        {
            get { return this.m_WeekCount; }
            set { this.m_WeekCount = value; }
        }
        #endregion

        #region 属性:MonthCount
        private int m_MonthCount;

        /// <summary></summary>
        public int MonthCount
        {
            get { return this.m_MonthCount; }
            set { this.m_MonthCount = value; }
        }
        #endregion

        #region 属性:TotalCount
        private int m_TotalCount;

        /// <summary></summary>
        public int TotalCount
        {
            get { return this.m_TotalCount; }
            set { this.m_TotalCount = value; }
        }
        #endregion

        #region 属性:Hidden
        private int m_Hidden;

        /// <summary></summary>
        public int Hidden
        {
            get { return this.m_Hidden; }
            set { this.m_Hidden = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = string.Empty;

        /// <summary></summary>
        public string OrderId
        {
            get { return this.m_OrderId; }
            set { this.m_OrderId = value; }
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

        #region 属性:Remark
        private string m_Remark = string.Empty;

        /// <summary></summary>
        public string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
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
        // 可阅读人员信息
        // -------------------------------------------------------

        /// <summary>权限：应用_通用_查看权限</summary>
        private AuthorityInfo authorizationRead = AuthorityContext.Instance.AuthorityService["应用_通用_查看权限"];

        #region 函数:BindAuthorizationReadScope(string scopeText)
        /// <summary>绑定查看权限</summary>
        /// <param name="scopeText"></param>
        public void BindAuthorizationReadScope(string scopeText)
        {
            // 清空缓存数据
            this.m_AuthorizationReadScopeObjectText = null;
            this.m_AuthorizationReadScopeObjectView = null;

            if (this.m_AuthorizationReadScopeObjects == null)
            {
                this.m_AuthorizationReadScopeObjects = new List<MembershipAuthorizationScopeObject>();
            }

            MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.m_AuthorizationReadScopeObjects, scopeText);
        }
        #endregion

        #region 属性:AuthorizationReadScopeObjects
        private IList<MembershipAuthorizationScopeObject> m_AuthorizationReadScopeObjects = null;

        /// <summary>权限：应用_通用_查看权限范围</summary>
        public IList<MembershipAuthorizationScopeObject> AuthorizationReadScopeObjects
        {
            get
            {
                if (this.m_AuthorizationReadScopeObjects == null)
                {
                    this.m_AuthorizationReadScopeObjects = ForumContext.Instance.ForumCategoryService.GetAuthorizationScopeObjects(
                       this.EntityId,
                       this.authorizationRead.Name);

                    // 设置默认权限是所有人
                    if (this.m_AuthorizationReadScopeObjects.Count == 0)
                    {
                        IAuthorizationObject authorizationObject = MembershipManagement.Instance.RoleService.GetEveryoneObject();

                        this.m_AuthorizationReadScopeObjects.Add(new MembershipAuthorizationScopeObject(authorizationObject.Type, authorizationObject.Id, authorizationObject.Name));
                    }
                }

                return this.m_AuthorizationReadScopeObjects;
            }
        }
        #endregion

        #region 属性:AuthorizationReadScopeObjectText
        private string m_AuthorizationReadScopeObjectText = null;

        /// <summary>权限：应用_通用_查看权限范围文本</summary>
        public string AuthorizationReadScopeObjectText
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AuthorizationReadScopeObjectText))
                {
                    this.m_AuthorizationReadScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationReadScopeObjects);
                }

                return this.m_AuthorizationReadScopeObjectText;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.BindAuthorizationReadScope(value);
                }
            }
        }
        #endregion

        #region 属性:AuthorizationReadScopeObjectView
        private string m_AuthorizationReadScopeObjectView = null;

        /// <summary>权限：应用_通用_查看权限范围视图</summary>
        public string AuthorizationReadScopeObjectView
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AuthorizationReadScopeObjectView))
                {
                    this.m_AuthorizationReadScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationReadScopeObjects);
                }

                return this.m_AuthorizationReadScopeObjectView;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 可维护人员信息
        // -------------------------------------------------------

        /// <summary>权限：应用_通用_修改权限</summary>
        private AuthorityInfo authorizationEdit = AuthorityContext.Instance.AuthorityService["应用_通用_修改权限"];

        #region 函数:BindAuthorizationEditScope(string scopeText)
        /// <summary>绑定修改权限</summary>
        /// <param name="scopeText"></param>
        public void BindAuthorizationEditScope(string scopeText)
        {
            // 清空缓存数据
            this.m_AuthorizationEditScopeObjectText = null;
            this.m_AuthorizationEditScopeObjectView = null;

            if (this.m_AuthorizationEditScopeObjects == null)
            {
                this.m_AuthorizationEditScopeObjects = new List<MembershipAuthorizationScopeObject>();
            }

            MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.m_AuthorizationEditScopeObjects, scopeText);
        }
        #endregion

        #region 属性:AuthorizationEditScopeObjects
        private IList<MembershipAuthorizationScopeObject> m_AuthorizationEditScopeObjects = null;

        /// <summary>权限：应用_通用_修改权限范围</summary>
        public IList<MembershipAuthorizationScopeObject> AuthorizationEditScopeObjects
        {
            get
            {
                if (this.m_AuthorizationEditScopeObjects == null)
                {
                    this.m_AuthorizationEditScopeObjects = ForumContext.Instance.ForumCategoryService.GetAuthorizationScopeObjects(
                       this.EntityId,
                       this.authorizationEdit.Name);

                    // 设置默认权限是空
                    if (this.m_AuthorizationEditScopeObjects.Count == 0)
                    {

                    }
                }

                return this.m_AuthorizationEditScopeObjects;
            }
        }
        #endregion

        #region 属性:AuthorizationEditScopeObjectText
        private string m_AuthorizationEditScopeObjectText = null;

        /// <summary>权限：应用_通用_修改权限范围文本</summary>
        public string AuthorizationEditScopeObjectText
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationEditScopeObjectText))
                {
                    m_AuthorizationEditScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationEditScopeObjects);
                }

                return m_AuthorizationEditScopeObjectText;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BindAuthorizationReadScope(value);
                }
            }
        }
        #endregion

        #region 属性:AuthorizationEditScopeObjectView
        private string m_AuthorizationEditScopeObjectView = null;

        /// <summary>权限：应用_通用_修改权限范围视图</summary>
        public string AuthorizationEditScopeObjectView
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationEditScopeObjectView))
                {
                    m_AuthorizationEditScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationEditScopeObjects);
                }

                return m_AuthorizationEditScopeObjectView;
            }
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
