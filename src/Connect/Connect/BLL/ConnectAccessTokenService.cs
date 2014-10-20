namespace X3Platform.Connect.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.DigitalNumber;
    using X3Platform.Membership;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.IDAL;
    using X3Platform.Connect.Model;
    using X3Platform.Location.IPQuery;
    #endregion

    public sealed class ConnectAccessTokenService : IConnectAccessTokenService
    {
        private ConnectConfiguration configuration = null;

        private IConnectAccessTokenProvider provider = null;

        public ConnectAccessTokenService()
        {
            this.configuration = ConnectConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IConnectAccessTokenProvider>(typeof(IConnectAccessTokenProvider));
        }

        public ConnectAccessTokenInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ConnectAccessTokenInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="ConnectAccessTokenInfo"/>实例详细信息</param>
        /// <param name="message">数据库操作返回的相关信息</param>
        /// <returns><see cref="ConnectAccessTokenInfo"/>实例详细信息</returns>
        public ConnectAccessTokenInfo Save(ConnectAccessTokenInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new NullReferenceException("实例标识不能为空。"); }

            // 过滤 Cross Site Script
            param = StringHelper.ToSafeXSS<ConnectAccessTokenInfo>(param);

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
        /// <returns>返回一个实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        public ConnectAccessTokenInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByAccountId(string appKey, string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        public ConnectAccessTokenInfo FindOneByAccountId(string appKey, string accountId)
        {
            return this.provider.FindOneByAccountId(appKey, accountId);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        public IList<ConnectAccessTokenInfo> FindAll()
        {
            return this.FindAll(new DataQuery() { Length = 1000 });
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        public IList<ConnectAccessTokenInfo> FindAll(DataQuery query)
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
        /// <returns>返回一个列表实例</returns>
        public IList<ConnectAccessTokenInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">会员标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExist(string appKey, string accountId)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string appKey, string accountId)
        {
            return this.provider.IsExist(appKey, accountId);
        }
        #endregion

        #region 函数:Write(string appKey, string accountId)
        /// <summary>写入的帐号的访问令牌信息</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns></returns>
        public int Write(string appKey, string accountId)
        {
            ConnectAccessTokenInfo param = this.FindOneByAccountId(appKey, accountId);

            if (param == null)
            {
                param = new ConnectAccessTokenInfo();

                param.Id = DigitalNumberContext.Generate("Key_32DigitGuid");
                param.AppKey = appKey;
                param.AccountId = accountId;
                param.ExpireDate = DateTime.Now.AddSeconds(ConnectConfigurationView.Instance.SessionTimeLimit);
                param.RefreshToken = DigitalNumberContext.Generate("Key_32DigitGuid");

                this.Save(param);
            }
            else
            {
                this.Refesh(appKey, param.RefreshToken, DateTime.Now.AddSeconds(ConnectConfigurationView.Instance.SessionTimeLimit));
            }

            return 0;
        }
        #endregion

        #region 函数:Refesh(string appKey, string refreshToken, DateTime expireDate)
        /// <summary>刷新帐号的访问令牌</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="refreshToken">刷新令牌</param>
        /// <param name="expireDate">过期时间</param>
        /// <returns></returns>
        public int Refesh(string appKey, string refreshToken, DateTime expireDate)
        {
            return this.provider.Refesh(appKey, refreshToken, expireDate);
        }
        #endregion
    }
}
