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
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CategoryIndexes;
    using X3Platform.DigitalNumber;
    using X3Platform.Entities;
    using X3Platform.Entities.Model;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Web.Component.Combobox;

    using X3Platform.Plugins.Bugs.Configuration;
    using X3Platform.Plugins.Bugs.Model;
    using X3Platform.Plugins.Bugs.IBLL;
    using X3Platform.Plugins.Bugs.IDAL;
  using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class BugCategoryService : IBugCategoryService
    {
        private BugConfiguration configuration = null;

        private IBugCategoryProvider provider = null;

        /// <summary></summary>
        public BugCategoryService()
        {
            this.configuration = BugConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IBugCategoryProvider>(typeof(IBugCategoryProvider));
        }

        /// <summary></summary>
        public BugCategoryInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(BugCategoryInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="BugCategoryInfo"/>详细信息</param>
        /// <returns>实例<see cref="BugCategoryInfo"/>详细信息</returns>
        public BugCategoryInfo Save(BugCategoryInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                throw new Exception("实例标识不能为空。");
            }

            bool isNewObject = !this.IsExist(param.Id);

            string methodName = isNewObject ? "新增" : "编辑";

            IAccountInfo account = KernelContext.Current.User;

            if (methodName == "新增")
            {
                param.AccountId = account.Id;
                param.AccountName = account.Name;
            }

            this.provider.Save(param);

            // 保存实体数据操作记录
            EntityLifeHistoryInfo history = new EntityLifeHistoryInfo();

            history.Id = DigitalNumberContext.Generate("Key_Guid");
            history.AccountId = account.Id;
            history.MethodName = methodName;
            history.EntityId = param.Id;
            history.EntityClassName = KernelContext.ParseObjectType(typeof(BugInfo));
            history.ContextDiffLog = string.Empty;

            EntitiesManagement.Instance.EntityLifeHistoryService.Save(history);

            return param;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        public int Delete(string id)
        {
            return this.provider.Delete(id);
        }
        #endregion

        #region 函数:CanDelete(string id)
        /// <summary>判断类别是否能够被物理删除</summary>
        /// <param name="id">实例的标识</param>
        /// <returns></returns>
        public bool CanDelete(string id)
        {
            return this.provider.CanDelete(id);
        }
        #endregion

        #region 函数:Remove(string id)
        /// <summary>物理删除记录</summary>
        /// <param name="id">实例的类别标识</param>
        public int Remove(string id)
        {
            return this.provider.Remove(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="BugCategoryInfo"/>的详细信息</returns>
        public BugCategoryInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>查询某条记录</summary>
        /// <param name="categoryIndex">类别索引</param>
        /// <returns>返回实例<see cref="BugCategoryInfo"/>的详细信息</returns>
        public BugCategoryInfo FindOneByCategoryIndex(string categoryIndex)
        {
            return this.provider.FindOneByCategoryIndex(categoryIndex);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="BugCategoryInfo"/>的详细信息</returns>
        public IList<BugCategoryInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="BugCategoryInfo"/>的详细信息</returns>
        public IList<BugCategoryInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="BugCategoryInfo"/>的详细信息</returns>
        public IList<BugCategoryInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllQueryObject(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="BugCategoryQueryInfo"/>的详细信息</returns>
        public IList<BugCategoryQueryInfo> FindAllQueryObject(string whereClause, int length)
        {
            return this.provider.FindAllQueryObject(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
        public IList<BugCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy,out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="BugCategoryQueryInfo"/></returns>
        public IList<BugCategoryQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">会员标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:SetStatus(int status)
        /// <summary>设置类别状态(停用/启用)</summary>
        /// <param name="id">新闻类别标识</param>
        /// <param name="status">1 将停用的类别启用，0 将启用的类别停用</param>
        /// <returns></returns>
        public bool SetStatus(string id, int status)
        {
            return this.provider.SetStatus(id, status);
        }
        #endregion

        #region 函数:GetComboboxByWhereClause(string whereClause, string selectedValue)
        /// <summary>获取拉框数据源</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="selectedValue">默认选择的值</param>
        /// <returns></returns>
        public IList<ComboboxItem> GetComboboxByWhereClause(string whereClause, string selectedValue)
        {
            return this.provider.GetComboboxByWhereClause(whereClause, selectedValue);
        }
        #endregion

        #region 函数:GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick, bool elevatedPrivileges)
        /// <summary>获取异步生成的树</summary>
        /// <param name="treeName">树</param>
        /// <param name="parentId">父级节点标识</param>
        /// <param name="url">链接地址</param>
        /// <param name="enabledLeafClick">只允许点击叶子节点</param>
        /// <param name="elevatedPrivileges">提升权限</param>
        /// <returns>布尔值</returns>
        public DynamicTreeView GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick, bool elevatedPrivileges)
        {
            string searchPath = (parentId == "0") ? string.Empty : parentId.Replace(@"$", @"\");

            IList<DynamicTreeNode> list = this.provider.GetDynamicTreeNodes(searchPath, string.Empty);

            DynamicTreeView treeView = new DynamicTreeView(treeName, parentId);

            Dictionary<string, DynamicTreeNode> dictionary = new Dictionary<string, DynamicTreeNode>();

            foreach (DynamicTreeNode node in list)
            {
                node.ParentId = parentId;
                node.Url = url;

                if (!dictionary.ContainsKey(node.Id))
                {
                    if (node.HasChildren && enabledLeafClick)
                    {
                        node.Disabled = 1;
                    }

                    treeView.Add(node);

                    dictionary.Add(node.Id, node);
                }
            }

            return treeView;
        }
        #endregion

        #region 函数:GetDynamicTreeNodes(string searchPath, string whereClause)
        /// <summary>获取树的节点列表</summary>
        /// <param name="searchPath">查询路径</param>
        /// <param name="whereClause">SQL条件</param>
        /// <returns>树的节点</returns>
        public IList<DynamicTreeNode> GetDynamicTreeNodes(string searchPath, string whereClause)
        {
            return this.provider.GetDynamicTreeNodes(searchPath, whereClause);
        }
        #endregion

        // -------------------------------------------------------
        // 授权范围管理
        // -------------------------------------------------------

        #region 函数:HasAuthority(string entityId, string authorityName, IAccountInfo account)
        /// <summary>判断用户是否拥应用菜单有权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string entityId, string authorityName, IAccountInfo account)
        {
            return this.provider.HasAuthority(entityId, authorityName, account);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>配置应用菜单的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        {
            this.provider.BindAuthorizationScopeObjects(entityId, authorityName, scopeText);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>查询应用菜单的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            return this.provider.GetAuthorizationScopeObjects(entityId, authorityName);
        }
        #endregion

        // -------------------------------------------------------
        // 权限
        // -------------------------------------------------------

        #region 私有函数:BindAuthorizationScopeSQL(string whereClause)
        /// <summary>绑定SQL查询条件</summary>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <returns></returns>
        private string BindAuthorizationScopeSQL(string whereClause)
        {
            string scope = string.Format(@" (
(   Id IN ( 
        SELECT DISTINCT EntityId FROM view_AuthorizationObject_Account View1, tb_Bug_Category_Scope Scope
        WHERE 
            View1.AccountId = ##{0}##
            AND View1.AuthorizationObjectId = Scope.AuthorizationObjectId
            AND View1.AuthorizationObjectType = Scope.AuthorizationObjectType)) 
) ", KernelContext.Current.User.Id);

            if (whereClause.IndexOf(scope) == -1)
            {
                whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
            }

            return whereClause;
        }
        #endregion
    }
}
