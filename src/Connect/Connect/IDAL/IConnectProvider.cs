namespace X3Platform.Connect.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Security.Authority;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Connect.IDAL.IConnectProvider")]
    public interface IConnectProvider
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

        #region 函数:Save(ConnectInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="ConnectInfo"/>实例详细信息</param>
        /// <returns><see cref="ConnectInfo"/>实例详细信息</returns>
        ConnectInfo Save(ConnectInfo param);
        #endregion

        #region 函数:Insert(ConnectInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param"><see cref="ConnectInfo"/>实例的详细信息</param>
        void Insert(ConnectInfo param);
        #endregion

        #region 函数:Update(ConnectInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param"><see cref="ConnectInfo"/>实例的详细信息</param>
        void Update(ConnectInfo param);
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
        /// <param name="id">ConnectInfo Id号</param>
        /// <returns>返回一个<see cref="ConnectInfo"/>实例的详细信息</returns>
        ConnectInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByAppKey(string appKey)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>返回一个实例<see cref="ConnectInfo"/>的详细信息</returns>
        ConnectInfo FindOneByAppKey(string appKey);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有<see cref="ConnectInfo"/>实例的详细信息</returns>
        IList<ConnectInfo> FindAll(DataQuery query);
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
        /// <returns>返回一个列表实例<see cref="ConnectInfo" /></returns> 
        IList<ConnectInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ConnectQueryInfo" /></returns> 
        IList<ConnectQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="param"><see cref="ConnectInfo"/>实例详细信息</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistAppKey(string appKey)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>布尔值</returns>
        bool IsExistAppKey(string appKey);
        #endregion

        #region 函数:ResetAppSecret(string appKey)
        /// <summary>重置应用链接器的密钥</summary>
        /// <param name="appKey">App Key</param>
        /// <param name="appSecret">App Secret</param>
        /// <returns></returns>
        int ResetAppSecret(string appKey, string appSecret);
        #endregion
    }
}
