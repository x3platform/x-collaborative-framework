namespace X3Platform.AttachmentStorage.IDAL
{
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.Data;

    /// <summary>附件存储</summary>
    [SpringObject("X3Platform.AttachmentStorage.IDAL.IAttachmentDistributedFileProvider")]
    public interface IAttachmentDistributedFileProvider
	{
		// -------------------------------------------------------
		// 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">DistributedFileInfo Id号</param>
		/// <returns>返回一个<see cref="DistributedFileInfo"/>实例的详细信息</returns>
        DistributedFileInfo FindOne(string id);
		#endregion

		#region 函数:FindAll(string whereClause,int length)
		/// <summary>查询所有相关记录</summary>
		/// <param name="query">数据查询参数</param>
		/// <param name="length">条数</param>
		/// <returns>返回所有<see cref="DistributedFileInfo"/>实例的详细信息</returns>
        IList<DistributedFileInfo> FindAll(DataQuery query);
		#endregion

		// -------------------------------------------------------
        // 保存 添加 修改 删除
		// -------------------------------------------------------

        #region 函数:Save(DistributedFileInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="DistributedFileInfo"/>实例详细信息</param>
        /// <returns><see cref="DistributedFileInfo"/>实例详细信息</returns>
        DistributedFileInfo Save(DistributedFileInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
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
        IList<DistributedFileInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
		/// <param name="param"><see cref="DistributedFileInfo"/>实例详细信息</param>
		/// <returns>布尔值</returns>
		bool IsExist(string id);
		#endregion
	}
}
