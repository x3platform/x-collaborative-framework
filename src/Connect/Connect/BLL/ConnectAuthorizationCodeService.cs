namespace X3Platform.Connect.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.DigitalNumber;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.IDAL;
    using X3Platform.Connect.Model;
    using X3Platform.Membership;
    #endregion

    public sealed class ConnectAuthorizationCodeService : IConnectAuthorizationCodeService
    {
        private ConnectConfiguration configuration = null;

        private IConnectAuthorizationCodeProvider provider = null;

        public ConnectAuthorizationCodeService()
        {
            this.configuration = ConnectConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IConnectAuthorizationCodeProvider>(typeof(IConnectAuthorizationCodeProvider));
        }

        public ConnectAuthorizationCodeInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ConnectAuthorizationCodeInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="ConnectAuthorizationCodeInfo" />实例详细信息</param>
        /// <param name="message">数据库操作返回的相关信息</param>
        /// <returns><see cref="ConnectAuthorizationCodeInfo" />实例详细信息</returns>
        public ConnectAuthorizationCodeInfo Save(ConnectAuthorizationCodeInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new NullReferenceException("实例标识不能为空。"); }

            // 过滤 Cross Site Script
            param = StringHelper.ToSafeXSS<ConnectAuthorizationCodeInfo>(param);

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
        /// <returns>返回一个实例<see cref="ConnectAuthorizationCodeInfo"/>的详细信息</returns>
        public ConnectAuthorizationCodeInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByAccountId(string appKey, string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个实例<see cref="ConnectAuthorizationCodeInfo"/>的详细信息</returns>
        public ConnectAuthorizationCodeInfo FindOneByAccountId(string appKey, string accountId)
        {
            return this.provider.FindOneByAccountId(appKey, accountId);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ConnectAuthorizationCodeInfo"/>的详细信息</returns>
        public IList<ConnectAuthorizationCodeInfo> FindAll()
        {
            return this.FindAll(new DataQuery() { Length = 1000 });
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ConnectAuthorizationCodeInfo"/>的详细信息</returns>
        public IList<ConnectAuthorizationCodeInfo> FindAll(DataQuery query)
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
        public IList<ConnectAuthorizationCodeInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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

        #region 函数:GetAuthorizationCode(string appKey, IAccountInfo account)
        /// <summary>获取帐号的授权码</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>授权码</returns>
        public string GetAuthorizationCode(string appKey, IAccountInfo account)
        {
            ConnectAuthorizationCodeInfo code = this.FindOneByAccountId(appKey, account.Id);

            return code == null ? string.Empty : code.Id;
        }
        #endregion
    }
}
