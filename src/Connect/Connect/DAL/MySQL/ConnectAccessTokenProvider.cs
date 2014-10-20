namespace X3Platform.Connect.DAL.MySQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership.Scope;
    using X3Platform.Util;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.IDAL;
    using X3Platform.Connect.Model;
    using X3Platform.DigitalNumber;

    [DataObject]
    public class ConnectAccessTokenProvider : IConnectAccessTokenProvider
    {
        /// <summary>配置</summary>
        private ConnectConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Connect_AccessToken";

        public ConnectAccessTokenProvider()
        {
            this.configuration = ConnectConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:CreateGenericSqlCommand()
        /// <summary>创建通用SQL命令对象</summary>
        public GenericSqlCommand CreateGenericSqlCommand()
        {
            return this.ibatisMapper.CreateGenericSqlCommand();
        }
        #endregion

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ConnectAccessTokenInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="ConnectAccessTokenInfo"/>实例详细信息</param>
        /// <returns><see cref="ConnectAccessTokenInfo"/>实例详细信息</returns>
        public ConnectAccessTokenInfo Save(ConnectAccessTokenInfo param)
        {
            if (!IsExist(param.Id))
            {
                this.Insert(param);
            }
            else
            {
                this.Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(ConnectAccessTokenInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ConnectAccessTokenInfo"/>详细信息</param>
        public void Insert(ConnectAccessTokenInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 函数:Update(ConnectAccessTokenInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ConnectAccessTokenInfo"/>详细信息</param>
        public void Update(ConnectAccessTokenInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public int Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return 1; }

            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                //  删除连接器的附属信息
                args.Add("ConnectAccessTokenId", StringHelper.ToSafeSQL(id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_History_DeleteByConnectAccessTokenId", this.tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Comment_DeleteByConnectAccessTokenId", this.tableName)), args);

                //  删除连接器的实体信息
                args.Add("Id", StringHelper.ToSafeSQL(id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

                this.ibatisMapper.CommitTransaction();
            }
            catch
            {
                this.ibatisMapper.RollBackTransaction();
                throw;
            }

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="param">ConnectAccessTokenInfo Id号</param>
        /// <returns>返回一个实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        public ConnectAccessTokenInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<ConnectAccessTokenInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region 函数:FindOneByAccountId(string appKey, string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        public ConnectAccessTokenInfo FindOneByAccountId(string appKey, string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AppKey", StringHelper.ToSafeSQL(appKey, true));
            args.Add("AccountId", StringHelper.ToSafeSQL(accountId, true));

            return this.ibatisMapper.QueryForObject<ConnectAccessTokenInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByAccountId", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ConnectAccessTokenInfo"/>的详细信息</returns>
        public IList<ConnectAccessTokenInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", query.GetWhereSql());
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<ConnectAccessTokenInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

            args.Add("RowCount", 0);

            IList<ConnectAccessTokenInfo> list = this.ibatisMapper.QueryForList<ConnectAccessTokenInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id"><see cref="ConnectAccessTokenInfo"/>实例详细信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id, true)));

            return Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0 ? false : true;
        }
        #endregion

        #region 函数:IsExist(string appKey, string accountId)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="appKey">应用标识</param>
        /// <param name="accountId">帐号标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string appKey, string accountId)
        {
            if (string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(accountId)) { throw new Exception("应用标识或帐号标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AppKey = '{0}' AND AccountId = '{1}' ", StringHelper.ToSafeSQL(appKey, true), StringHelper.ToSafeSQL(accountId, true)));

            return Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0 ? false : true;
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AppKey", StringHelper.ToSafeSQL(appKey, true));
            args.Add("RefreshToken", StringHelper.ToSafeSQL(refreshToken, true));
            args.Add("ExpireDate", expireDate.ToString("yyyy-MM-dd HH:mm:ss"));
            args.Add("NextRefreshToken", DigitalNumberContext.Generate("Key_32DigitGuid"));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Refresh", tableName)), args);

            return 0;
        }
        #endregion
    }
}
