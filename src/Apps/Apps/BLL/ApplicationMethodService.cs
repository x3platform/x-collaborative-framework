namespace X3Platform.Apps.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using X3Platform.Ajax.Json;
    using X3Platform.CacheBuffer;
    using X3Platform.Membership;
    using X3Platform.Spring;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    public class ApplicationMethodService : IApplicationMethodService
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IApplicationMethodProvider provider = null;

        /// <summary>缓存存储</summary>
        private Dictionary<string, ApplicationMethodInfo> Dictionary = new Dictionary<string, ApplicationMethodInfo>();

        #region 构造函数:ApplicationMethodService()
        /// <summary>构造函数</summary>
        public ApplicationMethodService()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationMethodProvider>(typeof(IApplicationMethodProvider));
        }
        #endregion

        #region 索引:this[string name]
        /// <summary>索引</summary>
        /// <param name="name">方法名称</param>
        /// <returns></returns>
        public ApplicationMethodInfo this[string name]
        {
            get { return this.FindOneByName(name); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationMethodInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationMethodInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationMethodInfo"/>详细信息</returns>
        public ApplicationMethodInfo Save(ApplicationMethodInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
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
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public ApplicationMethodInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">名称</param>
        /// <returns>返回实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public ApplicationMethodInfo FindOneByName(string name)
        {
            ApplicationMethodInfo param = null;

            // 初始化缓存
            if (this.Dictionary.Count == 0)
            {
                IList<ApplicationMethodInfo> list = this.FindAll();

                foreach (ApplicationMethodInfo item in list)
                {
                    this.Dictionary.Add(item.Name, item);
                }
            }

            // 查找缓存数据
            if (this.Dictionary.ContainsKey(name))
            {
                param = this.Dictionary[name];
            }

            // 如果缓存中未找到相关数据，则查找数据库内容
            return param == null ? this.provider.FindOneByName(name) : param;
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public IList<ApplicationMethodInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public IList<ApplicationMethodInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public IList<ApplicationMethodInfo> FindAll(string whereClause, int length)
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
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationMethodInfo"/></returns>
        public IList<ApplicationMethodInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
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

        #region 函数:IsExistCode(string code)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="code">代码</param>
        /// <returns>布尔值</returns>
        public bool IsExistCode(string code)
        {
            return this.provider.IsExistCode(code);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        ///<summary>获取需要同步的数据</summary>
        ///<param name="param">应用参数信息</param>
        public IList<ApplicationMethodInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        {
            string whereClause = string.Format(" UpdateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:SyncFromPackPage(ApplicationSettingInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用方法信息</param>
        public void SyncFromPackPage(ApplicationMethodInfo param)
        {
            provider.SyncFromPackPage(param);
        }
        #endregion
    }
}
