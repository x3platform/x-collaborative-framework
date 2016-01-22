namespace X3Platform.Apps.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CacheBuffer;
    using X3Platform.Spring;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class ApplicationEventService : IApplicationEventService
    {
        /// <summary>数据提供器</summary>
        private IApplicationEventProvider provider = null;

        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        private Dictionary<string, ApplicationInfo> Dictionary = new Dictionary<string, ApplicationInfo>();

        private DateTime actionTime = DateTime.Now;

        #region 构造函数:ApplicationEventService()
        /// <summary>
        /// 构造函数:ApplicationEventService()
        /// </summary>
        public ApplicationEventService()
        {
            configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationEventProvider>(typeof(IApplicationEventProvider));
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(ApplicationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="ApplicationEventInfo"/>详细信息</param>
        /// <returns>ApplicationEventInfo 实例详细信息</returns>
        public ApplicationEventInfo Save(ApplicationEventInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除一组数据记录</summary>
        /// <param name="ids">删除项 Key 的数组</param>
        public void Delete(string id)
        {
            provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">ApplicationEventInfo Id号</param>
        /// <returns>返回一个 ApplicationEventInfo 实例的详细信息</returns>
        public ApplicationEventInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 ApplicationEventInfo 实例的详细信息</returns>
        public IList<ApplicationEventInfo> FindAll()
        {
            return provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 ApplicationEventInfo 实例的详细信息</returns>
        public IList<ApplicationEventInfo> FindAll(string whereClause)
        {
            return provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 ApplicationEventInfo 实例的详细信息</returns>
        public IList<ApplicationEventInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
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
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表<see cref="ApplicationEventInfo"/></returns> 
        public IList<ApplicationEventInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string taskId, string receiverId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="taskId">任务标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string taskId)
        {
            return provider.IsExist(taskId);
        }
        #endregion
    }
}
