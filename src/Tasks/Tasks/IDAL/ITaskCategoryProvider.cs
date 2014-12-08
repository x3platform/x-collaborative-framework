namespace X3Platform.Tasks.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IDAL.ITaskCategoryProvider")]
    public interface ITaskCategoryProvider
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

        #region 函数:Save(TaskCategoryInfo param)
        /// <summary>
        /// 保存记录
        /// </summary>
        /// <param name="param">实例详细信息</param>
        /// <returns></returns>
        TaskCategoryInfo Save(TaskCategoryInfo param);
        #endregion

        #region 函数:Insert(TaskCategoryInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例的详细信息</param>
        void Insert(TaskCategoryInfo param);
        #endregion

        #region 函数:Update(TaskCategoryInfo param)
        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="param">实例的详细信息</param>
        void Update(TaskCategoryInfo param);
        #endregion

        #region 函数:CanDelete(string id)
        /// <summary>判断新闻类别是否能够被删除</summary>
        /// <param name="id">新闻类别标识</param>
        /// <returns></returns>
        bool CanDelete(string id);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">新闻类别标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>
        /// 查询某条记录
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例的详细信息</returns>
        TaskCategoryInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>查询某条记录</summary>
        /// <param name="categoryIndex">类别索引</param>
        /// <returns>返回实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        TaskCategoryInfo FindOneByCategoryIndex(string categoryIndex);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>
        /// 查询所有相关记录
        /// </summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns></returns>
        IList<TaskCategoryInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllQueryObject(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        IList<TaskCategoryInfo> FindAllQueryObject(string whereClause, int length);
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
        /// <returns></returns>
        IList<TaskCategoryInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:SetStatus(string id, int status)
        /// <summary>设置类别状态(停用/启用)</summary>
        /// <param name="id">新闻类别标识</param>
        /// <param name="status">1 将停用的类别启用，0 将在用的类别停用</param>
        /// <returns></returns>
        bool SetStatus(string id, int status);
        #endregion
    }
}
