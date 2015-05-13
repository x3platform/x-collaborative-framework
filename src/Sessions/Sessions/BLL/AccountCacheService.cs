namespace X3Platform.Sessions.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.CacheBuffer;
    using X3Platform.Collections;
    using X3Platform.Membership;
    using X3Platform.Spring;
    using X3Platform.Security;
    using X3Platform.Security.Authority;

    using X3Platform.Sessions.Configuration;
    using X3Platform.Sessions.IBLL;
    using X3Platform.Sessions.IDAL;
    using Common.Logging;
    #endregion

    /// <summary></summary>
    public class AccountCacheService : IAccountCacheService
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SessionsConfiguration configuration = null;

        private IAccountCacheProvider provider = null;

        private IDictionary<string, AccountCacheInfo> cacheStorage = null;

        #region 构造函数:AccountCacheService()
        /// <summary>构造函数</summary>
        public AccountCacheService()
        {
            this.configuration = SessionsConfigurationView.Instance.Configuration;

            this.cacheStorage = new SyncDictionary<string, AccountCacheInfo>();

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(SessionsConfiguration.ApplicationName, springObjectFile);

            this.provider = objectBuilder.GetObject<IAccountCacheProvider>(typeof(IAccountCacheProvider));
        }
        #endregion

        #region 索引:this[string accountIdentity]
        /// <summary>缓存的索引</summary>
        /// <param name="accountIdentity">缓存标识</param>
        /// <returns></returns>
        public AccountCacheInfo this[string accountIdentity]
        {
            get { return this.Read(accountIdentity); }
        }
        #endregion

        #region 属性:GetAuthAccount(IAccountStorageStrategy strategy)
        /// <summary>获取当前帐号缓存信息</summary>
        /// <param name="strategy">帐号存储策略</param>
        public IAccountInfo GetAuthAccount(IAccountStorageStrategy strategy, string accountIdentity)
        {
            AccountCacheInfo param = this.Read(accountIdentity);

            return strategy.Deserialize(param);
        }
        #endregion

        #region 函数:Authorize(string accountIdentity)
        /// <summary>查找授权信息</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <returns></returns>
        public bool Authorize(string accountIdentity) { return false; }
        #endregion

        #region 函数:Authorize(string accountIdentity, string appKey)
        /// <summary>查找授权信息</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <param name="appKey">App Key</param>
        /// <returns></returns>
        public bool Authorize(string accountIdentity, string appKey) { return false; }
        #endregion

        /// <summary></summary>
        /// <param name="accountIdentity"></param>
        /// <returns></returns>
        public AccountCacheInfo Read(string accountIdentity)
        {
            // 过滤空值
            if (string.IsNullOrEmpty(accountIdentity)) { return null; }

            if (this.cacheStorage.ContainsKey(accountIdentity))
            {
                return this.cacheStorage[accountIdentity];
            }
            else
            {
                AccountCacheInfo accountCacheInfo = this.FindByAccountIdentity(accountIdentity);

                if (accountCacheInfo != null && !this.cacheStorage.ContainsKey(accountIdentity))
                {
                    this.cacheStorage.Add(accountIdentity, accountCacheInfo);
                }

                return accountCacheInfo;
            }
        }

        /// <summary></summary>
        /// <param name="accountCacheValue"></param>
        /// <returns></returns>
        public AccountCacheInfo ReadWithAccountCacheValue(string accountCacheValue)
        {
            return this.FindByAccountCacheValue(accountCacheValue);
        }

        #region 函数:Write(IAccountStorageStrategy strategy, string appKey, string accountIdentity, IAccountInfo account)
        ///<summary>写入信息</summary>
        ///<param name="strategy">策略</param>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <param name="account">帐号信息</param>
        ///<returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        public void Write(IAccountStorageStrategy strategy, string appKey, string accountIdentity, IAccountInfo account)
        {
            // 过滤空值
            if (string.IsNullOrEmpty(accountIdentity)) { return; }

            AccountCacheInfo param = strategy.Serialize(appKey, accountIdentity, account);

            param.UpdateDate = DateTime.Now;

            // 更新字典信息
            if (cacheStorage.ContainsKey(param.AccountIdentity))
            {
                cacheStorage[param.AccountIdentity] = param;
            }
            else
            {
                cacheStorage.Add(param.AccountIdentity, param);
            }

            // 更新数据库信息
            if (this.IsExist(param.AccountIdentity))
            {
                this.Update(param);
            }
            else
            {
                param.BeginDate = DateTime.Now;
                param.EndDate = DateTime.Now.AddHours(SessionsConfigurationView.Instance.SessionTimeLimit);

                this.Insert(param);
            }
        }
        #endregion

        #region 函数:FindByAccountIdentity(string accountIdentity)
        /// <summary>根据查找某条记录</summary>
        /// <param name="accountIdentity">帐号会话唯一标识</param>
        /// <returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        public AccountCacheInfo FindByAccountIdentity(string accountIdentity)
        {
            return this.provider.FindByAccountIdentity(accountIdentity);
        }
        #endregion

        #region 函数:FindByAccountCacheValue(string cacheValue)
        ///<summary>根据缓存的值查找某条记录</summary>
        ///<param name="cacheValue">缓存的值</param>
        ///<returns>返回一个 AccountCacheInfo 实例的详细信息</returns>
        public AccountCacheInfo FindByAccountCacheValue(string cacheValue)
        {
            return this.provider.FindByAccountCacheValue(cacheValue);
        }
        #endregion

        #region 函数:Dump()
        /// <summary>转储所有记录信息</summary>
        ///<returns>返回一个实例<see cref="AccountCacheInfo"/>的详细信息</returns>
        public IList<AccountCacheInfo> Dump()
        {
            return this.provider.Dump();
        }
        #endregion

        #region 函数:Insert(AccountCacheInfo param)
        ///<summary>添加记录</summary>
        ///<param name="param">实例<see cref="AccountCacheInfo"/>的详细信息</param>
        public void Insert(AccountCacheInfo param)
        {
            this.provider.Insert(param);
        }
        #endregion

        #region 函数:Update(AccountCacheInfo param)
        ///<summary>修改记录</summary>
        ///<param name="param">实例<see cref="AccountCacheInfo"/>的详细信息</param>
        public void Update(AccountCacheInfo param)
        {
            this.provider.Update(param);
        }
        #endregion

        #region 函数:Delete(string accountIdentity)
        ///<summary>删除记录</summary>
        ///<param name="accountIdentity">帐号标识</param>
        public int Delete(string accountIdentity)
        {
            return this.provider.Delete(accountIdentity);
        }
        #endregion

        #region 函数:IsExist(string accountIdentity)
        ///<summary>检测记录是否存在</summary>
        public bool IsExist(string accountIdentity)
        {
            return this.provider.IsExist(accountIdentity);
        }
        #endregion

        #region 函数:IsExistAccountCacheValue(string accountIdentity)
        ///<summary>检测记录是否存在</summary>
        public bool IsExistAccountCacheValue(string accountIdentity)
        {
            return this.provider.IsExistAccountCacheValue(accountIdentity);
        }
        #endregion

        #region 函数:Clear(DateTime expiryTime)
        /// <summary>清理过期时间之前的缓存记录</summary>
        /// <param name="expiryTime">过期时间</param>
        public int Clear(DateTime expiryTime)
        {
            return this.provider.Clear(expiryTime);
        }
        #endregion

        #region 函数:Clear()
        ///<summary>清空缓存记录</summary>
        public int Clear()
        {
            return this.provider.Clear(DateTime.Now);
        }
        #endregion
    }
}
