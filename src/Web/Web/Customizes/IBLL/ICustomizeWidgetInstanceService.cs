namespace X3Platform.Web.Customizes.IBLL
{
    #region Using Libraries
  using System.Collections.Generic;
  using X3Platform.Data;
  using X3Platform.Spring;
  using X3Platform.Web.Customizes.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Web.Customizes.IBLL.ICustomizeWidgetInstanceService")]
    public interface ICustomizeWidgetInstanceService
    {
        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        CustomizeWidgetInstanceInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(CustomizeWidgetInstanceInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">CustomizeWidgetInstanceInfo 实例详细信息</param>
        /// <returns>CustomizeWidgetInstanceInfo 实例详细信息</returns>
        CustomizeWidgetInstanceInfo Save(CustomizeWidgetInstanceInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="keys">标识,多个以逗号隔开</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">CustomizeWidgetInstanceInfo Id号</param>
        /// <returns>返回一个 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
        CustomizeWidgetInstanceInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
        IList<CustomizeWidgetInstanceInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
        IList<CustomizeWidgetInstanceInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
        IList<CustomizeWidgetInstanceInfo> FindAll(string whereClause, int length);
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
        /// <param name="id">CustomizeWidgetInstanceInfo 实例详细信息</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetOptionHtml(string id)
        /// <summary>获取属性编辑框Html文本</summary>
        /// <param name="id">标识</param>
        /// <returns>Html文本</returns>
        string GetOptionHtml(string id);
        #endregion

        #region 函数:SetPageAndWidget(CustomizeWidgetInstanceInfo param, string pageId, string widgetName)
        /// <summary>设置实例所在的页面和部件类型</summary>
        /// <param name="param">CustomizeWidgetInstanceInfo 实例详细信息</param>
        /// <param name="pageId">页面标识</param>
        /// <param name="widgetName">部件名称</param>
        /// <returns>部件实例信息</returns>
        CustomizeWidgetInstanceInfo SetPageAndWidget(CustomizeWidgetInstanceInfo param, string pageId, string widgetName);
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
