namespace X3Platform.Membership.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    using X3Platform.Data;
    using X3Platform.Spring;
    
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IAccountBindingService")]
    public interface IAccountBindingService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountBindingInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

		#region 函数:Save(AccountBindingInfo param)
		/// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="AccountBindingInfo"/>详细信息</param>
        /// <returns>实例<see cref="AccountBindingInfo"/>详细信息</returns>
        AccountBindingInfo Save(AccountBindingInfo param);
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
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        AccountBindingInfo FindOne(string id);
		#endregion
        
        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有实例<see cref="AccountBindingInfo"/>的详细信息</returns>
        IList<AccountBindingInfo> FindAll(DataQuery query);
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
        /// <returns>返回一个列表实例<see cref="AccountBindingInfo"/></returns> 
        IList<AccountBindingInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

		#region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
