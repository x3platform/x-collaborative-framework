namespace X3Platform.Sessions.DAL.MySQL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Storages;
    using X3Platform.Util;

    using X3Platform.Sessions;
    using X3Platform.Sessions.Configuration;
    using X3Platform.Sessions.IDAL;
    using Common.Logging;
    #endregion

    /// <summary>帐号缓存数据提供者</summary>
    [DataObject]
    public class AccountCacheProvider : IAccountCacheProvider
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>配置</summary>
        private SessionsConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private Dictionary<string, ISqlMapper> ibatisMappers = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_AccountCache";

        /// <summary>数据存储架构标识</summary>
        private string storageSchemaId = "01-14-Session";

        /// <summary>数据存储策略</summary>
        private IStorageStrategy storageStrategy = null;

        #region 构造函数:AccountCacheProvider()
        /// <summary>构造函数</summary>
        public AccountCacheProvider()
        {
            this.configuration = SessionsConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMappers = StorageContext.Instance.CreateSqlMappers(this.storageSchemaId, this.ibatisMapping);

            this.storageStrategy = StorageContext.Instance.StorageSchemaService[this.storageSchemaId].GetStrategyClass();
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindByAccountIdentity(string accountIdentity)
        /// <summary>根据查找某条记录</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        public AccountCacheInfo FindByAccountIdentity(string accountIdentity)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountIdentity", StringHelper.ToSafeSQL(accountIdentity, true));

            return this.ibatisMappers[storageNode.Name].QueryForObject<AccountCacheInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindByAccountIdentity", tableName)), args);
        }
        #endregion

        #region 函数:FindByAccountCacheValue(string accountCacheValue)
        /// <summary>查找某条记录</summary>
        /// <param name="accountCacheValue">帐号缓存的值</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        public AccountCacheInfo FindByAccountCacheValue(string accountCacheValue)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountCacheValue", StringHelper.ToSafeSQL(accountCacheValue, true));

            return this.ibatisMappers[storageNode.Name].QueryForObject<AccountCacheInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindByAccountCacheValue", tableName)), args);
        }
        #endregion

        #region 函数:Dump()
        /// <summary>转储所有记录信息</summary>
        /// <returns>返回一个<see cref="AccountCacheInfo"/>列表</returns>
        public IList<AccountCacheInfo> Dump()
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            return this.ibatisMappers[storageNode.Name].QueryForList<AccountCacheInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_Dump", tableName)), args);
        }
        #endregion

        #region 函数:Insert(AccountCacheInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="AccountCacheInfo"/>的详细信息</param>
        public void Insert(AccountCacheInfo param)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            this.ibatisMappers[storageNode.Name].Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(AccountCacheInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="AccountCacheInfo"/>的详细信息</param>
        public void Update(AccountCacheInfo param)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            this.ibatisMappers[storageNode.Name].Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string accountIdentity)
        /// <summary>删除记录</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        public int Delete(string accountIdentity)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Node", accountIdentity);

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountIdentity < '{0}' ", StringHelper.ToSafeSQL(accountIdentity, true)));

            this.ibatisMappers[storageNode.Name].Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:IsExist(string accountIdentity)
        /// <summary>检测记录是否存在</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        public bool IsExist(string accountIdentity)
        {
            if (string.IsNullOrEmpty(accountIdentity)) { throw new Exception("帐号缓存唯一标识不能为空。"); }

            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountIdentity = '{0}' ", StringHelper.ToSafeSQL(accountIdentity, true)));

            return (Convert.ToInt32(this.ibatisMappers[storageNode.Name].QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistAccountCacheValue(string accountCacheValue)
        /// <summary>检测记录是否存在</summary>
        /// <param name="accountCacheValue">帐号缓存值</param>
        public bool IsExistAccountCacheValue(string accountCacheValue)
        {
            if (string.IsNullOrEmpty(accountCacheValue)) { throw new Exception("帐号缓存值不能为空。"); }

            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountCacheValue = '{0}' ", StringHelper.ToSafeSQL(accountCacheValue, true)));

            return ((int)this.ibatisMappers[storageNode.Name].QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 函数:Clear(DateTime expiryTime)
        /// <summary>清理过期时间之前的缓存记录</summary>
        /// <param name="expiryTime">过期时间</param>
        public int Clear(DateTime expiryTime)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Node");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" EndDate < '{0}' ", expiryTime.ToString("yyyy-MM-dd HH:mm:ss")));

            this.ibatisMappers[storageNode.Name].Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

            return 0;
        }
        #endregion
    }
}
