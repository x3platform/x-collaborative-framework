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
    public class CustomizeContentService : SecurityObject, ICustomizeContentService
    {
        private ICustomizeContentProvider provider = null;

        #region 构造函数:CustomizeContentService()
        /// <summary>构造函数</summary>
        public CustomizeContentService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = WebConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(WebConfiguration.APP_NAME_CUSTOMIZES, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ICustomizeContentProvider>(typeof(ICustomizeContentProvider));
        }
        #endregion

        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CustomizeContentInfo this[string index]
        {
            get { return this.FindOne(index); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(CustomizeContentInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">CustomizeContentInfo 实例详细信息</param>
        /// <returns>CustomizeContentInfo 实例详细信息</returns>
        public CustomizeContentInfo Save(CustomizeContentInfo param)
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
        /// <param name="id">CustomizeContentInfo Id号</param>
        /// <returns>返回一个 CustomizeContentInfo 实例的详细信息</returns>
        public CustomizeContentInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">页面名称</param>
        /// <returns>返回一个 CustomizeContentInfo 实例的详细信息</returns>
        public CustomizeContentInfo FindOneByName(string name)
        {
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 CustomizeContentInfo 实例的详细信息</returns>
        public IList<CustomizeContentInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 CustomizeContentInfo 实例的详细信息</returns>
        public IList<CustomizeContentInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 CustomizeContentInfo 实例的详细信息</returns>
        public IList<CustomizeContentInfo> FindAll(string whereClause, int length)
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
        public IList<CustomizeContentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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

        #region 函数:GetHtml(string name)
        /// <summary>获取Html文本</summary>
        /// <param name="name">区域划分模板名称</param>
        /// <returns>Html文本</returns>
        public string GetHtml(string name)
        {
            return this.provider.GetHtml(name);
        }
        #endregion
    }
}
