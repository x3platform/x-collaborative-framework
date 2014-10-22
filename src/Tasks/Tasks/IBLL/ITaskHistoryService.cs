namespace X3Platform.Tasks.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IBLL.ITaskHistoryService")]
    public interface ITaskHistoryService
    {
        #region 索引:this[string id, string receiverId]
        /// <summary>索引</summary>
        /// <param name="id">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns></returns>
        TaskHistoryItemInfo this[string id, string receiverId] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(TaskHistoryItemInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例详细信息</param>
        /// <returns>返回实例<see cref="TaskHistoryItemInfo"/></returns>
        TaskHistoryItemInfo Save(TaskHistoryItemInfo param);
        #endregion

        #region 函数:Delete(string id, string receiverId)
        /// <summary>删除记录</summary>
        /// <param name="id">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        void Delete(string id, string receiverId);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id, string receiverId)
        /// <summary>查询某条记录</summary>
        /// <param name="id">任务标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>返回实例<see cref="TaskHistoryItemInfo"/></returns>
        TaskHistoryItemInfo FindOne(string id, string receiverId);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="receiverId">接收人标识</param>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
        IList<TaskHistoryItemInfo> GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id, string receiverId)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <param name="receiverId">接收人标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id, string receiverId);
        #endregion
    }
}
