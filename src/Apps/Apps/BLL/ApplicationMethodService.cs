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
        /// <summary>数据提供器</summary>
        private IApplicationMethodProvider provider = null;

        /// <summary>缓存存储</summary>
        private Dictionary<string, ApplicationMethodInfo> dict = new Dictionary<string, ApplicationMethodInfo>();

        #region 构造函数:ApplicationMethodService()
        /// <summary>构造函数</summary>
        public ApplicationMethodService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = AppsConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

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
            this.provider.Save(param);

            if (param != null && this.dict.ContainsKey(param.Name))
            {
                // 同步到缓存
                this.dict[param.Name] = param;
            }

            return param;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        public void Delete(string id)
        {
            ApplicationMethodInfo param = this.FindOne(id);

            if (param != null && this.dict.ContainsKey(param.Name))
            {
                this.dict.Remove(param.Name);
            }

            provider.Delete(id);
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
            if (this.dict.Count == 0)
            {
                IList<ApplicationMethodInfo> list = this.provider.FindAll(new DataQuery());

                foreach (ApplicationMethodInfo item in list)
                {
                    if (this.dict.ContainsKey(item.Name))
                    {
                        KernelContext.Log.Warn(string.Format("method:{0}", item.Name) + " is exists.");
                    }
                    else
                    {
                        this.dict.Add(item.Name, item);
                    }
                }
            }

            // 查找缓存数据
            if (this.dict.ContainsKey(name))
            {
                param = this.dict[name];
            }

            // 如果缓存中未找到相关数据，则查找数据库内容
            return param == null ? this.provider.FindOneByName(name) : param;
        }
        #endregion

        #region 函数:FindAllByApplicationId(string applicationId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationId">应用唯一标识</param>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public IList<ApplicationMethodInfo> FindAllByApplicationId(string applicationId)
        {
            return this.provider.FindAllByApplicationId(applicationId);
        }
        #endregion


        #region 函数:FindAllByApplicationName(string applicationName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationName">应用名称</param>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        public IList<ApplicationMethodInfo> FindAllByApplicationName(string applicationName)
        {
            return this.provider.FindAllByApplicationName(applicationName);
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
            DataQuery query = new DataQuery();

            query.Variables["scence"] = "FetchNeededSyncData";

            query.Where.Add("BeginDate", beginDate);
            query.Where.Add("EndDate", endDate);

            return this.provider.FindAll(query);
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
