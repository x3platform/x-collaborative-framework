namespace X3Platform.Web.Customizes.BLL
{
    using System;
    using System.Collections.Generic;
    using X3Platform.Data;
    using X3Platform.Security;
    using X3Platform.Spring;
    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customizes.IBLL;
    using X3Platform.Web.Customizes.IDAL;
    using X3Platform.Web.Customizes.Model;

    /// <summary>页面</summary>
    [SecurityClass]
    public class CustomizeWidgetInstanceService : SecurityObject, ICustomizeWidgetInstanceService
    {
        private ICustomizeWidgetInstanceProvider provider = null;

        #region 构造函数:CustomizeWidgetInstanceService()
        /// <summary>构造函数</summary>
        public CustomizeWidgetInstanceService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = WebConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(WebConfiguration.APP_NAME_CUSTOMIZES, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ICustomizeWidgetInstanceProvider>(typeof(ICustomizeWidgetInstanceProvider));
        }
        #endregion

        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CustomizeWidgetInstanceInfo this[string index]
        {
            get { return this.FindOne(index); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(CustomizeWidgetInstanceInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">CustomizeWidgetInstanceInfo 实例详细信息</param>
        /// <returns>CustomizeWidgetInstanceInfo 实例详细信息</returns>
        public CustomizeWidgetInstanceInfo Save(CustomizeWidgetInstanceInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">CustomizeWidgetInstanceInfo Id号</param>
        /// <returns>返回一个 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
        public CustomizeWidgetInstanceInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
        public IList<CustomizeWidgetInstanceInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
        public IList<CustomizeWidgetInstanceInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 CustomizeWidgetInstanceInfo 实例的详细信息</returns>
        public IList<CustomizeWidgetInstanceInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个 WorkflowCollectorInfo 列表实例</returns>
        public IList<CustomizeWidgetInstanceInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out  rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">登录名信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:GetOptionHtml(string id)
        /// <summary>获取属性编辑框Html文本</summary>
        /// <param name="id">标识</param>
        /// <returns>Html文本</returns>
        public string GetOptionHtml(string id)
        {
            CustomizeWidgetInstanceInfo param = this.FindOne(id);

            return CustomizeContext.Instance.CustomizeWidgetService.GetOptionHtml(param.WidgetId);
        }
        #endregion

        #region 函数:SetPageAndWidget(CustomizeWidgetInstanceInfo param, string pageId, string widgetName)
        /// <summary>设置实例所在的页面和部件类型</summary>
        /// <param name="param">CustomizeWidgetInstanceInfo 实例详细信息</param>
        /// <param name="pageId">页面标识</param>
        /// <param name="widgetName">部件名称</param>
        /// <returns>部件实例信息</returns>
        public CustomizeWidgetInstanceInfo SetPageAndWidget(CustomizeWidgetInstanceInfo param, string pageId, string widgetName)
        {
            CustomizePageInfo page = CustomizeContext.Instance.CustomizePageService.FindOne(pageId);

            CustomizeWidgetInfo widget = CustomizeContext.Instance.CustomizeWidgetService.FindOneByName(widgetName);

            param.PageId = (page == null) ? string.Empty : page.Id;
            // param.PageName = pageId;

            param.WidgetId = (widget == null) ? string.Empty : widget.Id;
            param.WidgetName = widgetName;

            return param;
        }
        #endregion

        #region 函数:RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        /// <summary>删除未绑定的部件实例</summary>
        /// <param name="pageId">页面名称</param>
        /// <param name="bindingWidgetInstanceIds">绑定的部件标识</param>
        /// <returns>布尔值</returns>
        public int RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        {
            return this.provider.RemoveUnbound(pageId, bindingWidgetInstanceIds);
        }
        #endregion
    }
}
