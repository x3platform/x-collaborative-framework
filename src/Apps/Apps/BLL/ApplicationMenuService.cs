namespace X3Platform.Apps.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class ApplicationMenuService : IApplicationMenuService
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IApplicationMenuProvider provider = null;

        #region 构造函数:ApplicationMenuService()
        /// <summary>构造函数</summary>
        public ApplicationMenuService()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationMenuProvider>(typeof(IApplicationMenuProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationMenuInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationMenuInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationMenuInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationMenuInfo"/>详细信息</returns>
        public ApplicationMenuInfo Save(ApplicationMenuInfo param)
        {
            param.FullPath = CombineFullPath(param);

            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        public void Delete(string id)
        {
            provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationMenuInfo"/>的详细信息</returns>
        public ApplicationMenuInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationMenuInfo"/>的详细信息</returns>
        public IList<ApplicationMenuInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationMenuInfo"/>的详细信息</returns>
        public IList<ApplicationMenuInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationMenuInfo"/>的详细信息</returns>
        public IList<ApplicationMenuInfo> FindAll(string whereClause, int length)
        {
            // 验证管理员身份
            if (AppsSecurity.IsAdministrator(KernelContext.Current.User, AppsConfiguration.ApplicationName))
            {
                return this.provider.FindAll(whereClause, length);
            }
            else
            {
                return this.provider.FindAll(this.BindAuthorizationScopeSQL(whereClause), length);
            }
        }
        #endregion

        #region 函数:FindAllQueryObject(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationMenuQueryInfo"/>的详细信息</returns>
        public IList<ApplicationMenuQueryInfo> FindAllQueryObject(string whereClause, int length)
        {
            // 验证管理员身份
            if (AppsSecurity.IsAdministrator(KernelContext.Current.User, AppsConfiguration.ApplicationName))
            {
                return this.provider.FindAllQueryObject(whereClause, length);
            }
            else
            {
                return this.provider.FindAllQueryObject(this.BindAuthorizationScopeSQL(whereClause), length);
            }
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationMethodInfo"/></returns>
        public IList<ApplicationMenuInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
            // 验证管理员身份
            // if (AppsSecurity.IsAdministrator(KernelContext.Current.User, AppsConfiguration.ApplicationName))
            // {
            //    return this.provider.GetPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
            // }
            // else
            // {
            //    return this.provider.GetPaging(startIndex, pageSize, this.BindAuthorizationScopeSQL(whereClause), orderBy, out rowCount);
            // }
        }
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationMenuQueryInfo"/></returns>
        public IList<ApplicationMenuQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            // 验证管理员身份
            if (AppsSecurity.IsAdministrator(KernelContext.Current.User, AppsConfiguration.ApplicationName))
            {
                return this.provider.GetQueryObjectPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
            }
            else
            {
                return this.provider.GetQueryObjectPaging(startIndex, pageSize, this.BindAuthorizationScopeSQL(whereClause), orderBy, out rowCount);
            }
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:CombineFullPath(ApplicationMenuInfo param)
        ///<summary>组合菜单全路径</summary>
        ///<param name="param">菜单信息</param>
        ///<returns></returns>
        public string CombineFullPath(ApplicationMenuInfo param)
        {
            string fullPath = string.Empty;

            fullPath = param.Name;

            ApplicationMenuInfo parent = param.Parent;

            int depthCount = 0;

            while (parent != null)
            {
                fullPath = string.Format("{0}\\{1}", parent.Name, fullPath);

                parent = parent.Parent;

                // 如果深度超过50层，则跳出循环。(可能陷入死循环)
                if (depthCount++ > 50)
                    break;
            }

            if (param.MenuType == "ApplicationMenu")
            {
                fullPath = string.Format("{0}\\{1}", param.ApplicationDisplayName, fullPath);
            }
            else
            {
                fullPath = string.Format("{0}\\{1}", param.MenuTypeView, fullPath);
            }

            return fullPath;
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
            return provider.HasAuthority(entityId, authorityName, account);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>配置应用菜单的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        {
            provider.BindAuthorizationScopeObjects(entityId, authorityName, scopeText);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>查询应用菜单的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            return provider.GetAuthorizationScopeObjects(entityId, authorityName);
        }
        #endregion

        // -------------------------------------------------------
        // 权限
        // -------------------------------------------------------

        #region 私有函数:GetAuthorizationReadObject(ApplicationMenuInfo param)
        ///<summary>验证对象的权限</summary>
        ///<param name="param">需验证的对象</param>
        ///<returns>对象</returns>
        private ApplicationMenuInfo GetAuthorizationReadObject(ApplicationMenuInfo param)
        {
            IAccountInfo account = KernelContext.Current.User;

            if (AppsSecurity.IsAdministrator(account, AppsConfiguration.ApplicationName))
            {
                return param;
            }
            else
            {
                if (MembershipAuthorizationScopeManagement.Authenticate(param.AuthorizationReadScopeObjects, account))
                {
                    return param;
                }

                return null;
            }
        }
        #endregion

        #region 私有函数:BindAuthorizationScopeSQL(string whereClause)
        ///<summary>绑定SQL查询条件</summary>
        /// <param name="whereClause">WHERE 查询条件</param>
        ///<returns></returns>
        private string BindAuthorizationScopeSQL(string whereClause)
        {
            string accountId = KernelContext.Current.User == null ? Guid.Empty.ToString() : KernelContext.Current.User.Id;

            string scope = string.Format(@" (
(   T.Id IN ( 
        SELECT DISTINCT EntityId FROM view_AuthObject_Account View1, tb_Application_Menu_Scope Scope
        WHERE 
            View1.AccountId = ##{0}##
            AND View1.AuthorizationObjectId = Scope.AuthorizationObjectId
            AND View1.AuthorizationObjectType = Scope.AuthorizationObjectType)) 
) ", accountId);

            if (whereClause.IndexOf(scope) == -1)
            {
                whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
            }

            return whereClause;
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        ///<summary>获取需要同步的数据</summary>
        ///<param name="param">应用菜单信息</param>
        public IList<ApplicationMenuInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        {
            string whereClause = string.Format(" ModifiedDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:SyncFromPackPage(ApplicationMenuInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用菜单信息</param>
        public void SyncFromPackPage(ApplicationMenuInfo param)
        {
            param.FullPath = CombineFullPath(param);

            this.provider.SyncFromPackPage(param);

            this.BindAuthorizationScopeObjects(param.Id, "应用_通用_查看权限", param.AuthorizationReadScopeObjectText);
        }
        #endregion
    }
}