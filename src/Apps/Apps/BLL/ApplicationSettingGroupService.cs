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
    using X3Platform.CacheBuffer;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class ApplicationSettingGroupService : IApplicationSettingGroupService
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IApplicationSettingGroupProvider provider = null;

        /// <summary>缓存存储</summary>
        private Dictionary<string, ApplicationSettingGroupInfo> Dictionary = new Dictionary<string, ApplicationSettingGroupInfo>();

        #region 构造函数:ApplicationSettingGroupService()
        /// <summary>构造函数</summary>
        public ApplicationSettingGroupService()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationSettingGroupProvider>(typeof(IApplicationSettingGroupProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationSettingGroupInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationSettingGroupInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingGroupInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationSettingGroupInfo"/>详细信息</returns>
        public ApplicationSettingGroupInfo Save(ApplicationSettingGroupInfo param)
        {
            param = provider.Save(param);

            // 更新缓存数据
            if (this.Dictionary.ContainsKey(param.Id))
            {
                this.Dictionary[param.Id] = param;
            }

            return param;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
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
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        public ApplicationSettingGroupInfo FindOne(string id)
        {
            ApplicationSettingGroupInfo param = null;

            // 初始化缓存
            if (this.Dictionary.Count == 0)
            {
                IList<ApplicationSettingGroupInfo> list = this.FindAll();

                foreach (ApplicationSettingGroupInfo item in list)
                {
                    this.Dictionary.Add(item.Id, item);
                }
            }

            // 查找缓存数据
            if (this.Dictionary.ContainsKey(id))
            {
                param = this.Dictionary[id];
            }

            // 如果缓存中未找到相关数据，则查找数据库内容
            return param == null ? provider.FindOne(id) : param;
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        public IList<ApplicationSettingGroupInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        public IList<ApplicationSettingGroupInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        public IList<ApplicationSettingGroupInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="parentId">父级对象的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationSettingGroupInfo> FindAllByParentId(string parentId)
        {
            return provider.FindAllByParentId(parentId);
        }
        #endregion

        #region 函数:FindAllByApplicationId(string applicationId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        public IList<ApplicationSettingGroupInfo> FindAllByApplicationId(string applicationId)
        {
            return provider.FindAllByApplicationId(applicationId);
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
        /// <returns>返回一个列表实例<see cref="ApplicationSettingGroupInfo"/></returns>
        public IList<ApplicationSettingGroupInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">应用名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        ///<summary>获取需要同步的数据</summary>
        ///<param name="param">应用参数分组信息</param>
        public IList<ApplicationSettingGroupInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        {
            string whereClause = string.Format(" ModifiedDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:SyncFromPackPage(ApplicationSettingGroupInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用参数分组信息</param>
        public void SyncFromPackPage(ApplicationSettingGroupInfo param)
        {
            provider.SyncFromPackPage(param);
        }
        #endregion
    }
}
