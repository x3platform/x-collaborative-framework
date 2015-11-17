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
    public class CustomizeWidgetService : SecurityObject, ICustomizeWidgetService
    {
        private ICustomizeWidgetProvider provider = null;

        #region 构造函数:CustomizeWidgetService()
        /// <summary>构造函数</summary>
        public CustomizeWidgetService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = WebConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(WebConfiguration.APP_NAME_CUSTOMIZES, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ICustomizeWidgetProvider>(typeof(ICustomizeWidgetProvider));
        }
        #endregion

        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CustomizeWidgetInfo this[string index]
        {
            get { return this.FindOne(index); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(CustomizeWidgetInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">CustomizeWidgetInfo 实例详细信息</param>
        /// <returns>CustomizeWidgetInfo 实例详细信息</returns>
        public CustomizeWidgetInfo Save(CustomizeWidgetInfo param)
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
        /// <param name="id">CustomizeWidgetInfo Id号</param>
        /// <returns>返回一个 CustomizeWidgetInfo 实例的详细信息</returns>
        public CustomizeWidgetInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">页面名称</param>
        /// <returns>返回一个 CustomizeWidgetInfo 实例的详细信息</returns>
        public CustomizeWidgetInfo FindOneByName(string name)
        {
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 CustomizeWidgetInfo 实例的详细信息</returns>
        public IList<CustomizeWidgetInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 CustomizeWidgetInfo 实例的详细信息</returns>
        public IList<CustomizeWidgetInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 CustomizeWidgetInfo 实例的详细信息</returns>
        public IList<CustomizeWidgetInfo> FindAll(string whereClause, int length)
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
        public IList<CustomizeWidgetInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out  rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">部件标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">页面名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 函数:GetOptionHtml(string id)
        /// <summary>获取属性编辑框Html文本</summary>
        /// <param name="id">标识</param>
        /// <returns>Html文本</returns>
        public string GetOptionHtml(string id)
        {
            return this.provider.GetOptionHtml(id);
        }
        #endregion
    }
}
