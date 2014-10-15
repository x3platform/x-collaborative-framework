// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using X3Platform.Apps.IDAL;
using X3Platform.Apps.Model;
using X3Platform.Apps.Configuration;
using X3Platform.CacheBuffer;
using X3Platform.Spring;
using X3Platform.Apps.IBLL;

namespace X3Platform.Apps.BLL
{
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

        #region 函数:Delete(string ids)
        /// <summary>删除一组数据记录</summary>
        /// <param name="ids">删除项 Key 的数组</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
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

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy)
        /// <summary>
        /// 数据表 高效分页函数.
        /// </summary>
        /// <param name="startIndex">开始行索引数,由0开始统计.</param>
        /// <param name="pageSize">页面大小.</param>
        /// <param name="whereClause">WHERE 查询条件.</param>
        /// <param name="orderBy">ORDER BY 排序条件.</param>
        /// <returns>返回一个 ApplicationEventInfo 列表实例.</returns> 
        public IList<ApplicationEventInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
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
