namespace X3Platform.Tasks.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using X3Platform.Data;
    using X3Platform.Spring;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IBLL.ITaskCategoryService")]
    public interface ITaskCategoryService
    {
        #region 索引:this[string id]
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TaskCategoryInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(FavoriteInfo param)
        /// <summary>
        /// 保存记录
        /// </summary>
        /// <param name="param">实例详细信息</param>
        /// <returns></returns>
        TaskCategoryInfo Save(TaskCategoryInfo param);
        #endregion

        #region 函数:CanDelete(string id)
        /// <summary>判断新闻类别是否能够被删除</summary>
        /// <param name="id">新闻类别标识</param>
        /// <returns></returns>
        bool CanDelete(string id);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>
        /// 删除记录
        /// </summary>
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
        /// <returns></returns>
        TaskCategoryInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>查询某条记录</summary>
        /// <param name="categoryIndex">类别索引</param>
        /// <returns>返回实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        TaskCategoryInfo FindOneByCategoryIndex(string categoryIndex);
        #endregion

        #region 函数:FindAll()
        /// <summary>
        /// 查询所有相关记录
        /// </summary>
        /// <returns></returns>
        IList<TaskCategoryInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>
        /// 查询所有相关记录
        /// </summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns></returns>
        IList<TaskCategoryInfo> FindAll(string whereClause);
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

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表</returns>
        IList<TaskCategoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>
        /// 查询是否存在相关的记录
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:SetStatus(string id, int status)
        /// <summary>
        /// 停用/启用类别
        /// </summary>
        /// <param name="id">新闻类别标识</param>
        /// <param name="status">1将停用的类别启用，0将在用的类别停用</param>
        /// <returns></returns>
        bool SetStatus(string id, int status);
        #endregion
    }
}
