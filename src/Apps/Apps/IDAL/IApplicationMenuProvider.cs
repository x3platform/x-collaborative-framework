namespace X3Platform.Apps.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Apps.Model;
    using X3Platform.Membership.Scope;
    using X3Platform.Membership;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Apps.IDAL.IApplicationMenuProvider")]
    public interface IApplicationMenuProvider
    {
        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        void BeginTransaction();
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        void CommitTransaction();
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationMenuInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationMenuInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationMenuInfo"/>详细信息</returns>
        ApplicationMenuInfo Save(ApplicationMenuInfo param);
        #endregion

        #region 函数:Insert(ApplicationMenuInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationMenuInfo"/>详细信息</param>
        void Insert(ApplicationMenuInfo param);
        #endregion

        #region 函数:Update(ApplicationMenuInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationMenuInfo"/>详细信息</param>
        void Update(ApplicationMenuInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationMenuInfo"/>的详细信息</returns>
        ApplicationMenuInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationMenuInfo"/>的详细信息</returns>
        IList<ApplicationMenuInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllQueryObject(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationMenuQueryInfo"/>的详细信息</returns>
        IList<ApplicationMenuQueryInfo> FindAllQueryObject(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationMenuInfo"/></returns>
        IList<ApplicationMenuInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy,out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationMenuQueryInfo"/></returns>
        IList<ApplicationMenuQueryInfo> GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        // -------------------------------------------------------
        // 授权范围管理
        // -------------------------------------------------------

        #region 函数:HasAuthority(string entityId, string authorityName, IAccountInfo account)
        /// <summary>判断用户是否拥有实体对象的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号信息</param>
        /// <returns>布尔值</returns>
        bool HasAuthority(string entityId, string authorityName, IAccountInfo account);
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>绑定实体对象的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText);
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string applicationId, string authorityName)
        /// <summary>查询实体对象的权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName);
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(ApplicationMenuInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用菜单信息</param>
        void SyncFromPackPage(ApplicationMenuInfo param);
        #endregion
    }
}
