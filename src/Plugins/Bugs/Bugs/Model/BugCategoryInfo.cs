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

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    #endregion

    /// <summary>问题类别信息</summary>
    [Serializable]
    public class BugCategoryInfo : EntityClass
    {
        #region 构造函数:BugCategoryInfo()
        /// <summary>默认构造函数</summary>
        public BugCategoryInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary>
        /// 标识
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        private string m_AccountName = string.Empty;

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string AccountName
        {
            get
            {
                if (string.IsNullOrEmpty(m_AccountName) && !string.IsNullOrEmpty(this.AccountId))
                {
                    IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(this.AccountId);

                    if (account != null)
                    {
                        m_AccountName = account.Name;
                    }
                }

                return m_AccountName;
            }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:CategoryIndex
        private string m_CategoryIndex = string.Empty;

        /// <summary>类别索引</summary>
        public string CategoryIndex
        {
            get { return m_CategoryIndex; }
            set { m_CategoryIndex = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>
        /// 办理说明
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = "0";

        /// <summary>
        /// 排序号（默认0）
        /// </summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status = 1;

        /// <summary>
        /// 状态（1有效，0无效）
        /// </summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 可起草人员信息
        // -------------------------------------------------------

        /// <summary>权限：应用_通用_添加权限</summary>
        private AuthorityInfo authorizationAdd = AuthorityContext.Instance.AuthorityService["应用_通用_添加权限"];

        #region 函数:BindAuthorizationAddScope(string scopeText)
        /// <summary>绑定添加权限</summary>
        /// <param name="scopeText"></param>
        public void BindAuthorizationAddScope(string scopeText)
        {
            // 清空缓存数据
            this.m_AuthorizationAddScopeObjectText = null;
            this.m_AuthorizationAddScopeObjectView = null;

            if (this.m_AuthorizationAddScopeObjects == null)
            {
                this.m_AuthorizationAddScopeObjects = new List<MembershipAuthorizationScopeObject>();
            }

            MembershipAuthorizationScopeManagement.BindAuthorizationScopeObjects(this.m_AuthorizationAddScopeObjects, scopeText);
        }
        #endregion

        #region 属性:AuthorizationAddScopeObjects
        private IList<MembershipAuthorizationScopeObject> m_AuthorizationAddScopeObjects = null;

        /// <summary>权限：应用_通用_查看权限范围</summary>
        public IList<MembershipAuthorizationScopeObject> AuthorizationAddScopeObjects
        {
            get
            {
                if (m_AuthorizationAddScopeObjects == null)
                {
                    m_AuthorizationAddScopeObjects = BugContext.Instance.BugCategoryService.GetAuthorizationScopeObjects(
                       this.EntityId,
                       this.authorizationAdd.Name);

                    // 设置默认权限是空
                    if (m_AuthorizationAddScopeObjects.Count == 0)
                    {
                        IAuthorizationObject authorizationObject = MembershipManagement.Instance.RoleService.GetEveryoneObject();

                        m_AuthorizationAddScopeObjects.Add(new MembershipAuthorizationScopeObject(authorizationObject.Type, authorizationObject.Id, authorizationObject.Name));
                    }
                }

                return m_AuthorizationAddScopeObjects;
            }
        }
        #endregion

        #region 属性:AuthorizationAddScopeObjectText
        private string m_AuthorizationAddScopeObjectText = null;

        /// <summary>权限：应用_通用_查看权限范围文本</summary>
        public string AuthorizationAddScopeObjectText
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_AuthorizationAddScopeObjectText))
                {
                    this.m_AuthorizationAddScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationAddScopeObjects);
                }

                return this.m_AuthorizationAddScopeObjectText;
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

        #region 属性:AuthorizationReadScopeObjectView
        private string m_AuthorizationAddScopeObjectView = null;

        /// <summary>权限：应用_通用_查看权限范围视图</summary>
        public string AuthorizationAddScopeObjectView
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationAddScopeObjectView))
                {
                    m_AuthorizationAddScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationAddScopeObjects);
                }

                return m_AuthorizationAddScopeObjectView;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 默认可阅读人员信息
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
                if (m_AuthorizationReadScopeObjects == null)
                {
                    m_AuthorizationReadScopeObjects = BugContext.Instance.BugCategoryService.GetAuthorizationScopeObjects(
                       this.EntityId,
                       this.authorizationRead.Name);

                    // 设置默认权限是所有人
                    if (m_AuthorizationReadScopeObjects.Count == 0)
                    {
                        IAuthorizationObject authorizationObject = MembershipManagement.Instance.RoleService.GetEveryoneObject();

                        m_AuthorizationReadScopeObjects.Add(new MembershipAuthorizationScopeObject(authorizationObject.Type, authorizationObject.Id, authorizationObject.Name));
                    }
                }

                return m_AuthorizationReadScopeObjects;
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
                if (string.IsNullOrEmpty(m_AuthorizationReadScopeObjectText))
                {
                    m_AuthorizationReadScopeObjectText = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectText(this.AuthorizationReadScopeObjects);
                }

                return m_AuthorizationReadScopeObjectText;
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

        #region 属性:AuthorizationReadScopeObjectView
        private string m_AuthorizationReadScopeObjectView = null;

        /// <summary>权限：应用_通用_查看权限范围视图</summary>
        public string AuthorizationReadScopeObjectView
        {
            get
            {
                if (string.IsNullOrEmpty(m_AuthorizationReadScopeObjectView))
                {
                    m_AuthorizationReadScopeObjectView = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjectView(this.AuthorizationReadScopeObjects);
                }

                return m_AuthorizationReadScopeObjectView;
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

        /// <summary>权限：应用_通用_查看权限范围</summary>
        public IList<MembershipAuthorizationScopeObject> AuthorizationEditScopeObjects
        {
            get
            {
                if (this.m_AuthorizationEditScopeObjects == null)
                {
                    this.m_AuthorizationEditScopeObjects = BugContext.Instance.BugCategoryService.GetAuthorizationScopeObjects(
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

        /// <summary>权限：应用_通用_查看权限范围文本</summary>
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

        /// <summary>权限：应用_通用_查看权限范围视图</summary>
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
