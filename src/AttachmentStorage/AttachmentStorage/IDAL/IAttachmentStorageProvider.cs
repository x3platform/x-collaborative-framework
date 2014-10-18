#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IAttachmentStorageProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.AttachmentStorage.IDAL
{
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.Data;

    /// <summary>附件存储</summary>
    [SpringObject("X3Platform.AttachmentStorage.IDAL.IAttachmentStorageProvider")]     
    public interface IAttachmentStorageProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(IAttachmentFileInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IAttachmentFileInfo"/>详细信息</param>
        /// <returns>实例<see cref="IAttachmentFileInfo"/>详细信息</returns>
        IAttachmentFileInfo Save(IAttachmentFileInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
        #endregion
        
		// -------------------------------------------------------
		// 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">IAttachmentFileInfo Id号</param>
		/// <returns>返回一个 实例<see cref="IAttachmentFileInfo"/>的详细信息</returns>
        IAttachmentFileInfo FindOne(string id);
		#endregion

		#region 函数:FindAll(string whereClause,int length)
		/// <summary>查询所有相关记录</summary>
		/// <param name="query">数据查询参数</param>
		/// <param name="length">条数</param>
		/// <returns>返回所有 实例<see cref="IAttachmentFileInfo"/>的详细信息</returns>
        IList<IAttachmentFileInfo> FindAll(DataQuery query);
		#endregion

        #region 函数:FindAllByEntityId(string entityClassName, string entityId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <returns>返回所有 实例<see cref="IAttachmentFileInfo"/>的详细信息</returns>
        IList<IAttachmentFileInfo> FindAllByEntityId(string entityClassName, string entityId);
        #endregion
   
		// -------------------------------------------------------
		// 自定义功能
		// -------------------------------------------------------

        #region 属性:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        IList<IAttachmentFileInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
		/// <param name="param">实例<see cref="IAttachmentFileInfo"/>详细信息</param>
		/// <returns>布尔值</returns>
		bool IsExist(string id);
		#endregion

        #region 函数:Rename(string id, string name)
        /// <summary>重命名</summary>
        /// <param name="id">附件标识</param>
        /// <param name="name">新的附件名称</param>
        void Rename(string id, string name);
        #endregion
	}
}
