namespace X3Platform.Apps.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    
    using X3Platform.Apps.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Apps.IBLL.IApplicationService")]
    public interface IApplicationService
    {
        #region 索引:this[string index]
        /// <summary>缓存的索引</summary>
        /// <param name="applicationName">缓存标识</param>
        /// <returns></returns>
        ApplicationInfo this[string applicationName] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="ApplicationInfo"/>详细信息</param>
        /// <returns>ApplicationInfo 实例详细信息</returns>
        ApplicationInfo Save(ApplicationInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识信息, 多个以逗号分开.</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">ApplicationInfo Id号</param>
        /// <returns>返回一个 ApplicationInfo 实例的详细信息</returns>
        ApplicationInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByApplicationName(string applicationName)
        /// <summary>查询某条记录</summary>
        /// <param name="applicationName">applicationName</param>
        /// <returns>返回一个 ApplicationInfo 实例的详细信息</returns>
        ApplicationInfo FindOneByApplicationName(string applicationName);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 AuthorityInfo 实例的详细信息</returns>
        IList<ApplicationInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 AuthorityInfo 实例的详细信息</returns>
        IList<ApplicationInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="ApplicationInfo"/>实例的详细信息</returns>
        IList<ApplicationInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>根据帐号所属的标准角色信息对应的应用系统的功能点, 查询此帐号有权限启用的应用系统信息.</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="ApplicationInfo"/>实例的详细信息</returns>
        IList<ApplicationInfo> FindAllByAccountId(string accountId);
        #endregion

        #region 函数:FindAllByRoleIds(string roleIds)
        /// <summary>根据角色所属的标准角色信息对应的应用系统的功能点, 查询此帐号有权限启用的应用系统信息.</summary>
        /// <param name="roleIds">角色标识</param>
        /// <returns>返回所有<see cref="ApplicationInfo"/>实例的详细信息</returns>
        IList<ApplicationInfo> FindAllByRoleIds(string roleIds);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表<see cref="ApplicationInfo"/></returns> 
        IList<ApplicationInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">应用标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">应用名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:HasAuthority(IAccountInfo account, string applicationId, string authorityName)
        /// <summary>判断用户是否拥应用有权限信息</summary>
        /// <param name="account">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns>布尔值</returns>
        bool HasAuthority(IAccountInfo account, string applicationId, string authorityName);
        #endregion

        #region 函数:HasAuthority(string accountId, string applicationId, string authorityName)
        /// <summary>判断用户是否拥应用有权限信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns>布尔值</returns>
        bool HasAuthority(string accountId, string applicationId, string authorityName);
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string applicationId, string authorityName, string scopeText)
        /// <summary>配置应用的权限信息</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        void BindAuthorizationScopeObjects(string applicationId, string authorityName, string scopeText);
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string applicationId, string authorityName)
        /// <summary>查询应用的权限信息</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string applicationId, string authorityName);
        #endregion

        #region 函数:IsAdministrator(IAccountInfo account, string applicationId)
        /// <summary>判断用户是否是应用的默认管理员</summary>
        /// <param name="account">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <returns>布尔值</returns>
        bool IsAdministrator(IAccountInfo account, string applicationId);
        #endregion

        #region 函数:IsReviewer(IAccountInfo account, string applicationId)
        /// <summary>判断用户是否是应用的默认审查员</summary>
        /// <param name="account">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <returns>布尔值</returns>
        bool IsReviewer(IAccountInfo account, string applicationId);
        #endregion

        #region 函数:IsMember(IAccountInfo account, string applicationId)
        /// <summary>判断用户是否是应用的默认可访问成员</summary>
        /// <param name="account">帐号</param>
        /// <param name="applicationId">应用的标识</param>
        /// <returns>布尔值</returns>
        bool IsMember(IAccountInfo account, string applicationId);
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        ///<summary>获取需要同步的数据</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        IList<ApplicationInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:SyncFromPackPage(ApplicationInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用信息</param>
        void SyncFromPackPage(ApplicationInfo param);
        #endregion
    }
}