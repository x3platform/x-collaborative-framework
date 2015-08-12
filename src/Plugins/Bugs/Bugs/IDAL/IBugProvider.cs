namespace X3Platform.Plugins.Bugs.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Security.Authority;
    
    using X3Platform.Plugins.Bugs.Configuration;
    using X3Platform.Plugins.Bugs.Model;
    
    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Bugs.IDAL.IBugProvider")]     
    public interface IBugProvider
    {
        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:CreateGenericSqlCommand()
        /// <summary>创建通用SQL命令对象</summary>
        GenericSqlCommand CreateGenericSqlCommand();
        #endregion

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

        #region 函数:Save(BugInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">BugInfo 实例详细信息</param>
        /// <returns>BugInfo 实例详细信息</returns>
        BugInfo Save(BugInfo param);
        #endregion

        #region 函数:Insert(BugInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">BugInfo 实例的详细信息</param>
        void Insert(BugInfo param);
        #endregion

        #region 函数:Update(BugInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">BugInfo 实例的详细信息</param>
        void Update(BugInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        int Delete(string id);
        #endregion

		// -------------------------------------------------------
		// 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">BugInfo Id号</param>
		/// <returns>返回一个 BugInfo 实例的详细信息</returns>
        BugInfo FindOne(string id);
		#endregion

        #region 函数:FindOneByCode(string code)
        /// <summary>查询某条记录</summary>
        /// <param name="code">问题编号</param>
        /// <returns>返回一个 BugInfo 实例的详细信息</returns>
        BugInfo FindOneByCode(string code);
        #endregion

		#region 函数:FindAll(string whereClause,int length)
		/// <summary>查询所有相关记录</summary>
		/// <param name="whereClause">SQL 查询条件</param>
		/// <param name="length">条数</param>
		/// <returns>返回所有 BugInfo 实例的详细信息</returns>
        IList<BugInfo> FindAll(string whereClause, int length);
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
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        IList<BugInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="CostInfo"/></returns>
        IList<BugQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
		/// <param name="param">BugInfo 实例详细信息</param>
		/// <returns>布尔值</returns>
		bool IsExist(string id);
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>查询实体对象的权限信息</summary> 
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName);
        #endregion
	}
}
