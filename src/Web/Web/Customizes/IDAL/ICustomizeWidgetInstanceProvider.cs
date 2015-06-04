namespace X3Platform.Web.Customizes.IDAL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using X3Platform.Data;
  using X3Platform.Spring;
  using X3Platform.Web.Customizes.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Web.Customizes.IDAL.ICustomizeWidgetInstanceProvider")]
    public interface ICustomizeWidgetInstanceProvider
    {
        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">CustomizeWidgetInstanceInfo Id号</param>
        /// <returns>返回一个实例<see cref="CustomizeWidgetInstanceInfo"/>的详细信息</returns>
        CustomizeWidgetInstanceInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="CustomizeWidgetInstanceInfo"/>的详细信息</returns>
        IList<CustomizeWidgetInstanceInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(CustomizeWidgetInstanceInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">CustomizeWidgetInstanceInfo 实例详细信息</param>
        /// <returns>CustomizeWidgetInstanceInfo 实例详细信息</returns>
        CustomizeWidgetInstanceInfo Save(CustomizeWidgetInstanceInfo param);
        #endregion

        #region 函数:Insert(CustomizeWidgetInstanceInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">CustomizeWidgetInstanceInfo 实例的详细信息</param>
        void Insert(CustomizeWidgetInstanceInfo param);
        #endregion

        #region 函数:Update(CustomizeWidgetInstanceInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">CustomizeWidgetInstanceInfo 实例的详细信息</param>
        void Update(CustomizeWidgetInstanceInfo param);
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
        IList<CustomizeWidgetInstanceInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="param">CustomizeWidgetInstanceInfo 实例详细信息</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        /// <summary>删除未绑定的部件实例</summary>
        /// <param name="pageId">页面名称</param>
        /// <param name="bindingWidgetInstanceIds">绑定的部件标识</param>
        /// <returns>布尔值</returns>
        int RemoveUnbound(string pageId, string bindingWidgetInstanceIds);
        #endregion
    }
}
