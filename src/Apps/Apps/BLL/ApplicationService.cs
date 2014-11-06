namespace X3Platform.Apps.BLL
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using X3Platform.CacheBuffer;
    using X3Platform.Collections;
    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    #endregion
    
    /// <summary>应用配置服务</summary>
    public class ApplicationService : IApplicationService
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IApplicationProvider provider = null;

        /// <summary>缓存存储</summary>
        private IDictionary<string, ApplicationInfo> Dictionary = new SyncDictionary<string, ApplicationInfo>();

        #region 构造函数:ApplicationService()
        /// <summary>构造函数</summary>
        public ApplicationService()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationProvider>(typeof(IApplicationProvider));
        }
        #endregion

        #region 索引:this[string applicationName]
        /// <summary>索引</summary>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public ApplicationInfo this[string applicationName]
        {
            get { return this.FindOneByApplicationName(applicationName); }
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(ApplicationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="ApplicationInfo"/>详细信息</param>
        /// <returns>ApplicationInfo 实例详细信息</returns>
        public ApplicationInfo Save(ApplicationInfo param)
        {
            // 将应用信息设置到应用交互连接器

            return provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识信息, 多个以逗号分开.</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="Id">ApplicationInfo Id号</param>
        /// <returns>返回一个 ApplicationInfo 实例的详细信息</returns>
        public ApplicationInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByApplicationName(string applicationName)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationName">applicationName</param>
        /// <returns>返回一个 ApplicationInfo 实例的详细信息</returns>
        public ApplicationInfo FindOneByApplicationName(string applicationName)
        {
            ApplicationInfo param = null;

            // 初始化缓存
            if (this.Dictionary.Count == 0)
            {
                IList<ApplicationInfo> list = this.FindAll();

                foreach (ApplicationInfo item in list)
                {
                    this.Dictionary.Add(item.ApplicationName, item);
                }
            }

            // 查找缓存数据
            if (this.Dictionary.ContainsKey(applicationName))
            {
                param = this.Dictionary[applicationName];
            }

            // 如果缓存中未找到相关数据，则查找数据库内容
            return param == null ? provider.FindOneByApplicationName(applicationName) : param;
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 ApplicationInfo 实例的详细信息</returns>
        public IList<ApplicationInfo> FindAll()
        {
            return provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 ApplicationInfo 实例的详细信息</returns>
        public IList<ApplicationInfo> FindAll(string whereClause)
        {
            return provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 ApplicationInfo 实例的详细信息</returns>
        public IList<ApplicationInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>根据帐号所属的标准角色信息对应的应用系统的功能点, 查询此帐号有权限启用的应用系统信息.</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="ApplicationInfo"/>实例的详细信息</returns>
        public IList<ApplicationInfo> FindAllByAccountId(string accountId)
        {
            return provider.FindAllByAccountId(accountId);
        }
        #endregion

        #region 函数:FindAllByRoleIds(string roleIds)
        /// <summary>根据角色所属的标准角色信息对应的应用系统的功能点, 查询此帐号有权限启用的应用系统信息.</summary>
        /// <param name="roleIds">角色标识</param>
        /// <returns>返回所有<see cref="ApplicationInfo"/>实例的详细信息</returns>
        public IList<ApplicationInfo> FindAllByRoleIds(string roleIds)
        {
            return provider.FindAllByRoleIds(roleIds);
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
        /// <returns>返回一个列表实例</returns> 
        public IList<ApplicationInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="thirdPartyAccountId">第三方系统的标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">应用名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 函数:HasAuthority(IAccountInfo account, string applicationId, string authorityName)
        /// <summary>判断用户是否拥应用有权限信息</summary>
        /// <param name="account">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(IAccountInfo account, string applicationId, string authorityName)
        {
            return HasAuthority(account.Id, applicationId, authorityName);
        }
        #endregion

        #region 函数:HasAuthority(IAccountInfo account, string applicationId, string authorityName)
        /// <summary>判断用户是否拥应用有权限信息</summary>
        /// <param name="accountId">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string accountId, string applicationId, string authorityName)
        {
            return provider.HasAuthority(accountId, applicationId, authorityName);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string applicationId, string authorityName, string scopeText)
        /// <summary>配置应用的权限信息</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string applicationId, string authorityName, string scopeText)
        {
            provider.BindAuthorizationScopeObjects(applicationId, authorityName, scopeText);

            //
            // [重点]
            // 设置完管理员后, 重置缓存数据.
            //
            ApplicationInfo configration = FindOne(applicationId);

            if (configration != null)
            {
                AppsSecurity.ResetApplicationAdministrators(configration.ApplicationName);
            }
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string applicationId, string authorityName)
        /// <summary>查询应用的权限信息</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string applicationId, string authorityName)
        {
            return provider.GetAuthorizationScopeObjects(applicationId, authorityName);
        }
        #endregion

        #region 函数:IsAdministrator(IAccountInfo account, string applicationId)
        /// <summary>判断用户是否是应用的默认管理员</summary>
        /// <param name="account">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <returns>布尔值</returns>
        public bool IsAdministrator(IAccountInfo account, string applicationId)
        {
            string loginName = account.LoginName;

            string administrators = AppsConfigurationView.Instance.Administrators;

            administrators = "[" + administrators.Replace(",", "],[") + "]";

            // 如果为内置超级管理员则返回 True。
            if (loginName == "admin" || administrators.IndexOf(loginName) > -1)
            {
                return true;
            }

            return HasAuthority(account, applicationId, "应用_默认_管理员");
        }
        #endregion

        #region 函数:IsReviewer(IAccountInfo account, string applicationId)
        /// <summary>判断用户是否是应用的默认审查员</summary>
        /// <param name="account">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <returns>布尔值</returns>
        public bool IsReviewer(IAccountInfo account, string applicationId)
        {
            if (IsAdministrator(account, applicationId))
                return true;

            return HasAuthority(account, applicationId, "应用_默认_审查员");
        }
        #endregion

        #region 函数:IsMember(IAccountInfo account, string applicationId)
        /// <summary>判断用户是否是应用的默认可访问成员</summary>
        /// <param name="account">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <returns>布尔值</returns>
        public bool IsMember(IAccountInfo account, string applicationId)
        {
            if (IsAdministrator(account, applicationId))
                return true;

            if (IsReviewer(account, applicationId))
                return true;

            return HasAuthority(account, applicationId, "应用_默认_可访问成员");
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        ///<summary>获取需要同步的数据</summary>
        ///<param name="param">应用参数信息</param>
        public IList<ApplicationInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        {
            string whereClause = string.Format(" UpdateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:SyncFromPackPage(ApplicationInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用信息</param>
        public void SyncFromPackPage(ApplicationInfo param)
        {
            provider.SyncFromPackPage(param);
        }
        #endregion
    }
}
