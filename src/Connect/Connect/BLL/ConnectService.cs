namespace X3Platform.Connect.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.DigitalNumber;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.IDAL;
    using X3Platform.Connect.Model;
    using X3Platform.CacheBuffer;
    using X3Platform.Collections;
    #endregion

    public sealed class ConnectService : IConnectService
    {
        private ConnectConfiguration configuration = null;

        private IConnectProvider provider = null;

        /// <summary>缓存存储</summary>
        private IDictionary<string, ConnectInfo> cacheStorage = new SyncDictionary<string, ConnectInfo>();

        public ConnectService()
        {
            this.configuration = ConnectConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IConnectProvider>(typeof(IConnectProvider));
        }

        #region 索引:this[string appKey]
        /// <summary>索引</summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public ConnectInfo this[string appKey]
        {
            get { return this.FindOneByAppKey(appKey); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ConnectInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="ConnectInfo"/>实例详细信息</param>
        /// <param name="message">数据库操作返回的相关信息</param>
        /// <returns><see cref="ConnectInfo"/>实例详细信息</returns>
        public ConnectInfo Save(ConnectInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new Exception("实例标识不能为空。"); }

            bool isNewObject = !this.IsExist(param.Id);

            string methodName = isNewObject ? "新增" : "编辑";

            if (isNewObject)
            {
                IAccountInfo account = KernelContext.Current.User;

                param.AccountId = account.Id;
                param.AccountName = account.Name;
            }

            // 过滤 Cross Site Script
            param = StringHelper.ToSafeXSS<ConnectInfo>(param);

            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public int Delete(string id)
        {
            return this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">连接器标识</param>
        /// <returns>返回一个实例<see cref="ConnectInfo"/>的详细信息</returns>
        public ConnectInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByAppKey(string appKey)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>返回一个实例<see cref="ConnectInfo"/>的详细信息</returns>
        public ConnectInfo FindOneByAppKey(string appKey)
        {
            ConnectInfo param = null;

            // 初始化缓存
            if (this.cacheStorage.Count == 0)
            {
                IList<ConnectInfo> list = this.FindAll();

                foreach (ConnectInfo item in list)
                {
                    this.cacheStorage.Add(item.AppKey, item);
                }
            }

            // 查找缓存数据
            if (this.cacheStorage.ContainsKey(appKey))
            {
                param = this.cacheStorage[appKey];
            }

            // 如果缓存中未找到相关数据，则查找数据库内容
            return param == null ? this.provider.FindOneByAppKey(appKey) : param;
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ConnectInfo"/>的详细信息</returns>
        public IList<ConnectInfo> FindAll()
        {
            return this.FindAll(new DataQuery() { Limit = 1000 });
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ConnectInfo"/>的详细信息</returns>
        public IList<ConnectInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
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
        /// <returns>返回一个列表<see cref="ConnectInfo"/>实例</returns>
        public IList<ConnectInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表<see cref="ConnectInfo"/>实例</returns>
        public IList<ConnectQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
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

        #region 函数:IsExistAppKey(string appKey)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>布尔值</returns>
        public bool IsExistAppKey(string appKey)
        {
            return (this.FindOneByAppKey(appKey) == null) ? false : true;
        }
        #endregion

        #region 函数:ResetAppSecret(string appKey)
        /// <summary>重置应用链接器的密钥</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns></returns>
        public int ResetAppSecret(string appKey)
        {
            string appSecret = Guid.NewGuid().ToString().Substring(0, 8);

            return this.ResetAppSecret(appKey, appSecret);
        }
        #endregion

        #region 函数:ResetAppSecret(string appKey, string appSecret)
        /// <summary>重置应用链接器的密钥</summary>
        /// <param name="appKey">App Key</param>
        /// <param name="appSecret">App Secret</param>
        /// <returns></returns>
        public int ResetAppSecret(string appKey, string appSecret)
        {
            // 更新缓存信息
            ConnectInfo param = this.FindOneByAppKey(appKey);

            param.AppSecret = appSecret;

            // 更新数据库信息
            return this.provider.ResetAppSecret(appKey, appSecret);
        }
        #endregion
    }
}
