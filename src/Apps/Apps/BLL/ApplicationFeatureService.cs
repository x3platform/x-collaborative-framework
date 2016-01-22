namespace X3Platform.Apps.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;

    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Membership.Scope;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class ApplicationFeatureService : IApplicationFeatureService
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IApplicationFeatureProvider provider = null;

        #region 构造函数:ApplicationFeatureService()
        /// <summary>构造函数</summary>
        public ApplicationFeatureService()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationFeatureProvider>(typeof(IApplicationFeatureProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationFeatureInfo this[string id]
        {
            get
            {
                // 预加载缓存信息
                PreloadCache();

                return this.FindOne(id);
            }
        }
        #endregion

        // -------------------------------------------------------
        // 缓存
        // -------------------------------------------------------

        /// <summary>缓存存储</summary>
        private IList<ApplicationFeatureInfo> Dictionary = null;

        #region 函数:PreloadCache()
        /// <summary>预加载缓存信息</summary>
        public int PreloadCache()
        {
            if (Dictionary == null)
            {
                Dictionary = this.provider.FindAll(string.Empty, 0);
            }

            return 0;
        }
        #endregion

        #region 函数:RefreshCache()
        /// <summary>刷新缓存信息</summary>
        public int RefreshCache()
        {
            AsyncMethod.AsyncRefreshCacheDelegate asyncDelegate = delegate()
            {
                foreach (ApplicationFeatureInfo cacheItem in Dictionary)
                {
                    if (string.IsNullOrEmpty(cacheItem.AuthorizationReadScopeObjectText))
                    {
                        // 此段代码目的是为了初始化授权对象
                    }
                }

                return 0;
            };

            IAsyncResult result = asyncDelegate.BeginInvoke(new AsyncCallback(RefreshCacheCallBack), null);

            return 0;
        }
        #endregion

        #region 函数:RefreshCacheCallBack()
        /// <summary>回调函数得到异步线程的返回结果</summary>  
        /// <param name="iAsyncResult"></param>  
        private void RefreshCacheCallBack(IAsyncResult iAsyncResult)
        {
            AsyncResult async = (AsyncResult)iAsyncResult;

            AsyncMethod.AsyncRefreshCacheDelegate asyncDelegate = (AsyncMethod.AsyncRefreshCacheDelegate)async.AsyncDelegate;

            int iResult = (int)asyncDelegate.EndInvoke(iAsyncResult);

            if (iResult == 0)
            {
                // 执行成功
                Dictionary = this.provider.FindAll(string.Empty, 0);
            }
            else
            {
                // 执行失败
            }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationFeatureInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationFeatureInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationFeatureInfo"/>详细信息</returns>
        public ApplicationFeatureInfo Save(ApplicationFeatureInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public ApplicationFeatureInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="parentId">父级对象的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAllByParentId(string parentId)
        {
            return this.provider.FindAllByParentId(parentId);
        }
        #endregion

        #region 函数:FindAllByApplicationId(string applicationId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAllByApplicationId(string applicationId)
        {
            return this.provider.FindAllByApplicationId(applicationId);
        }
        #endregion

        #region 函数:FindAllBetweenDateTime(DateTime beginDate, DateTime endDate)
        /// <summary>查询所有相关记录</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">开始时间</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAllBetweenDateTime(DateTime beginDate, DateTime endDate)
        {
            return Dictionary.Where(cacheItem => cacheItem.ModifiedDate > beginDate && cacheItem.ModifiedDate <= endDate).ToList();
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
        public IList<ApplicationFeatureInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
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

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">应用名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 函数:Authorize(string name, string accountId)
        /// <summary>检测用户是否拥有某一应用功能权限</summary>
        /// <param name="name">应用功能名称</param>
        /// <param name="accountId">用户帐号标识</param>
        /// <returns>布尔值</returns>
        public bool Authorize(string name, string accountId)
        {
            return this.provider.Authorize(name, accountId);
        }
        #endregion

        // -------------------------------------------------------
        // 授权范围管理
        // -------------------------------------------------------

        #region 函数:HasAuthority(string entityId, string authorityName, string authorizationObjectId, string entityId)
        /// <summary>判断授权对象是否拥有实体权限的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string entityId, string authorityName, string authorizationObjectType, string authorizationObjectId)
        {
            return this.provider.HasAuthority(entityId, authorityName, authorizationObjectType, authorizationObjectId);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>配置实体对象的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        {
            this.provider.BindAuthorizationScopeObjects(entityId, authorityName, scopeText);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>查询实体对象的权限信息</summary> 
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            return this.provider.GetAuthorizationScopeObjects(entityId, authorityName);
        }
        #endregion

        #region 函数:GetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId)
        /// <summary>设置某一个应用系统功能的授权范围的记录.</summary>
        /// <param name="applicationId">所属应用标识</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        public DataTable GetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId)
        {
            return this.provider.GetApplicationFeatureScope(applicationId, authorizationObjectType, authorizationObjectId);
        }
        #endregion

        #region 函数:SetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId, string applicationFeatureIds)
        /// <summary>设置某一个应用系统功能的授权范围的记录.</summary>
        /// <param name="applicationId">所属应用标识</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="applicationFeatureIds">应用功能标识, 多个以逗号隔开.</param>
        public void SetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId, string applicationFeatureIds)
        {
            if (string.IsNullOrEmpty(authorizationObjectType))
            {
                throw new NullReferenceException("未设置相关参数[AuthorizationObjectType]。");
            }

            authorizationObjectType = StringHelper.ToFirstUpper(authorizationObjectType);

            this.provider.SetApplicationFeatureScope(applicationId, authorizationObjectType, authorizationObjectId, applicationFeatureIds);
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        ///<summary>获取需要同步的数据</summary>
        ///<param name="param">应用功能信息</param>
        public IList<ApplicationFeatureInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        {
            string whereClause = string.Format(" ModifiedDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:SyncFromPackPage(ApplicationFeatureInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用功能信息</param>
        public void SyncFromPackPage(ApplicationFeatureInfo param)
        {
            this.provider.SyncFromPackPage(param);

            this.BindAuthorizationScopeObjects(param.Id, "应用_通用_查看权限", param.AuthorizationReadScopeObjectText);
        }
        #endregion
    }
}
