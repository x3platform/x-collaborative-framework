namespace X3Platform.Membership.Authentication
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Threading;

    using X3Platform.Collections;
    using X3Platform.Security.Authentication;
    using X3Platform.Sessions;
    using X3Platform.Sessions.Configuration;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;
    using Web;
    #endregion

    /// <summary>通用的验证请求管理</summary>
    public abstract class GenericAuthenticationManagement : IAuthenticationManagement
    {
        private object cacheSyncRootObject;

        /// <summary>锁</summary>
        protected object cacheSyncRoot
        {
            get
            {
                if (this.cacheSyncRootObject == null)
                {
                    Interlocked.CompareExchange(ref this.cacheSyncRootObject, new object(), null);
                }

                return this.cacheSyncRootObject;
            }
        }

        /// <summary></summary>
        protected IDictionary<string, IAccountInfo> cacheStorage = new SyncDictionary<string, IAccountInfo>();

        /// <summary>帐号存储策略</summary>
        protected IAccountStorageStrategy strategy = null;

        /// <summary>默认构造函数</summary>
        public GenericAuthenticationManagement()
        {
            this.strategy = new GenericAccountStorageStrategy();
        }

        /// <summary>获取用户存储策略</summary>
        public virtual IAccountStorageStrategy GetAccountStorageStrategy()
        {
            return this.strategy;
        }

        /// <summary>标识名称</summary>
        public virtual string IdentityName
        {
            get { return MembershipConfigurationView.Instance.AccountIdentityCookieToken; }
        }

        /// <summary></summary>
        public virtual string GetIdentityValue()
        {
            string accountIdentityProperty = MembershipConfigurationView.Instance.AccountIdentityCookieToken;

            if (HttpContext.Current.Request.Cookies[accountIdentityProperty] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Request.Cookies[accountIdentityProperty].Value;
            }
        }

        /// <summary></summary>
        public virtual string GetAccessToken()
        {
            // return (HttpContext.Current.Request["accessToken"] == null) ? string.Empty : HttpContext.Current.Request["accessToken"];
            return RequestHelper.Fetch("accessToken", "access_token");
        }

        /// <summary>获取认证的用户信息</summary>
        public abstract IAccountInfo GetAuthUser();

        /// <summary>登录</summary>
        /// <returns></returns>
        public virtual int Login(string loginName, string password, bool isCreatePersistent)
        {
            return 0;
        }

        /// <summary>注销</summary>
        /// <returns></returns>
        public virtual int Logout()
        {
            string accountIdentity = this.GetIdentityValue();

            this.RemoveSession(accountIdentity);

            return 0;
        }

        #region 方法:GetSessions()
        /// <summary></summary>
        /// <returns></returns>
        public IDictionary<string, IAccountInfo> GetSessions()
        {
            return new ReadOnlyDictionary<string, IAccountInfo>(this.cacheStorage);
        }
        #endregion

        #region 方法:AddSession(string sessionId, IAccountInfo account)
        /// <summary>新增会话</summary>
        public void AddSession(string sessionId, IAccountInfo account)
        {
            AddSession(string.Empty, sessionId, account);
        }
        #endregion

        #region 方法:AddSession(string appKey, string sessionId, IAccountInfo account)
        /// <summary>新增会话</summary>
        public void AddSession(string appKey, string sessionId, IAccountInfo account)
        {
            if (account == null)
            {
                this.cacheStorage.Remove(sessionId);
            }
            else if (!this.cacheStorage.ContainsKey(sessionId))
            {
                // 记录用户登录的时间
                account.LoginDate = DateTime.Now;

                this.cacheStorage.Add(sessionId, account);

                if (SessionsConfigurationView.Instance.SessionPersistentMode == "ON")
                {
                    SessionContext.Instance.Write(this.strategy, appKey, sessionId, account);
                }
            }
        }
        #endregion

        #region 方法:RemoveSession(string sessionId)
        /// <summary>移除会话</summary>
        public void RemoveSession(string sessionId)
        {
            this.cacheStorage.Remove(sessionId);

            if (SessionsConfigurationView.Instance.SessionPersistentMode == "ON")
            {
                SessionContext.Instance.AccountCacheService.Delete(sessionId);
            }
        }
        #endregion
        
        #region 方法:GetSessionAccount(string sessionId)
        /// <summary>获取会话帐号信息</summary>
        public IAccountInfo GetSessionAccount(string sessionId)
        {
            IAccountInfo account = null;

            if (this.cacheStorage.ContainsKey(sessionId) && !string.IsNullOrEmpty(sessionId))
            {
                // 字典缓存信息
                account = this.cacheStorage[sessionId];
            }

            if (account == null && SessionsConfigurationView.Instance.SessionPersistentMode == "ON")
            {
                // 持久化缓存信息
                account = SessionContext.Instance.GetAuthAccount<AccountInfo>(this.strategy, sessionId);
            }

            return account;
        }
        #endregion

        #region 方法:ResetSessions()
        /// <summary>重置所有会话</summary>
        public void ResetSessions()
        {
            this.cacheStorage.Clear();
        }
        #endregion
    }
}