namespace X3Platform.Apps.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    public class ApplicationPackageLogService : IApplicationPackageLogService
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IApplicationPackageLogProvider provider = null;

        #region 构造函数:ApplicationPackageLogService()
        /// <summary>构造函数</summary>
        public ApplicationPackageLogService()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationPackageLogProvider>(typeof(IApplicationPackageLogProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationPackageLogInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationPackageLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationPackageLogInfo"/>详细信息</returns>
        public ApplicationPackageLogInfo Save(ApplicationPackageLogInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        #region 函数:DeleteByApplicationId(string applicationId)
        /// <summary>删除某个应用的同步数据包发送记录</summary>
        /// <param name="applicationId">应用标识</param>
        public void DeleteByApplicationId(string applicationId)
        {
            this.provider.DeleteByApplicationId(applicationId);
        }
        #endregion

        #region 函数:DeleteAll()
        /// <summary>删除全部应用的同步数据包发送记录</summary>
        public void DeleteAll()
        {
            this.provider.DeleteAll();
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        public ApplicationPackageLogInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        public IList<ApplicationPackageLogInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        public IList<ApplicationPackageLogInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        public IList<ApplicationPackageLogInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationPackageLogInfo"/></returns>
        public IList<ApplicationPackageLogInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:GetLatestPackageLogByApplicationId(string applicationId)
        /// <summary>根据应用标识查询最新的应用包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        public ApplicationPackageLogInfo GetLatestPackageLogByApplicationId(string applicationId)
        {
            return this.provider.GetLatestPackageLogByApplicationId(applicationId);
        }
        #endregion
    }
}
