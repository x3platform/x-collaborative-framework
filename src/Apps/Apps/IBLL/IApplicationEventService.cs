namespace X3Platform.Apps.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Apps.IBLL.IApplicationEventService")]
    public interface IApplicationEventService
    {
        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationEventInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="ApplicationEventInfo"/>详细信息</param>
        /// <returns>ApplicationEventInfo 实例详细信息</returns>
        ApplicationEventInfo Save(ApplicationEventInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识信息,多个以逗号分开.</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string taskId, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="id">ApplicationEventInfo 标识</param>
        /// <returns>返回一个 ApplicationEventInfo 实例的详细信息</returns>
        ApplicationEventInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 ApplicationEventInfo 实例的详细信息</returns>
        IList<ApplicationEventInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 ApplicationEventInfo 实例的详细信息</returns>
        IList<ApplicationEventInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 ApplicationEventInfo 实例的详细信息</returns>
        IList<ApplicationEventInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>
        /// 数据表 高效分页函数.
        /// </summary>
        /// <param name="startIndex">开始行索引数,由0开始统计.</param>
        /// <param name="pageSize">页面大小.</param>
        /// <param name="whereClause">WHERE 查询条件.</param>
        /// <param name="orderBy">ORDER BY 排序条件.</param>
        /// <returns>返回一个 ApplicationEventInfo 列表实例.</returns> 
        IList<ApplicationEventInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string taskId, string receiverId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <param name="receiverId">接收者标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
