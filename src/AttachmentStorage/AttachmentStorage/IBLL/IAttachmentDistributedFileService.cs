// =============================================================================
//
// Copyright (c) 2007 X3Platform
//
// FileName     :
//
// Description  :
//
// Author       :X3Platform
//
// Date         :2007-07-08
//
// =============================================================================

namespace X3Platform.AttachmentStorage.IBLL
{
    using System.Collections.Generic;

    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.AttachmentStorage.IBLL.IAttachmentDistributedFileService")]
    public interface IAttachmentDistributedFileService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DistributedFileInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(DistributedFileInfo param)
        ///<summary>保存记录</summary>
        ///<param name="param">DistributedFileInfo 实例详细信息</param>
        ///<returns>DistributedFileInfo 实例详细信息</returns>
        DistributedFileInfo Save(DistributedFileInfo param);
        #endregion

        #region 函数:Delete(string id)
        ///<summary>删除记录</summary>
        ///<param name="id">标识</param>
        void Delete(string id);
        #endregion
        
        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        ///<summary>查询某条记录</summary>
        ///<param name="id">DistributedFileInfo Id号</param>
        ///<returns>返回一个 DistributedFileInfo 实例的详细信息</returns>
        DistributedFileInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        ///<summary>查询所有相关记录</summary>
        ///<returns>返回所有 DistributedFileInfo 实例的详细信息</returns>
        IList<DistributedFileInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<returns>返回所有 DistributedFileInfo 实例的详细信息</returns>
        IList<DistributedFileInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有 DistributedFileInfo 实例的详细信息</returns>
        IList<DistributedFileInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        IList<DistributedFileInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        ///<summary>查询是否存在相关的记录</summary>
        ///<param name="id">DistributedFileInfo 实例详细信息</param>
        ///<returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
