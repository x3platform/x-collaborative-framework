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

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;
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
        protected IDictionary<string, IAccountInfo> Dictionary = new SyncDictionary<string, IAccountInfo>();

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

            SessionContext.Instance.AccountCacheService.Delete(accountIdentity);

            return 0;
        }

        #region 方法:GetSessions()
        /// <summary></summary>
        /// <returns></returns>
        public IDictionary<string, IAccountInfo> GetSessions()
        {
            return new ReadOnlyDictionary<string, IAccountInfo>(this.Dictionary);
        }
        #endregion

        #region 方法:AddSession(string sessionId, IAccountInfo account)
        /// <summary>新增会话</summary>
        public void AddSession(string sessionId, IAccountInfo account)
        {
            if (account == null)
            {
                this.Dictionary.Remove(sessionId);
            }
            else
            {
                // 记录用户登录的时间
                if (account.LoginDate.AddHours(2) < DateTime.Now)
                {
                    account.LoginDate = DateTime.Now;
                }

                if (!this.Dictionary.ContainsKey(sessionId))
                {
                    this.Dictionary.Add(sessionId, account);
                }
            }
        }
        #endregion

        #region 方法:RemoveSession(string sessionId)
        /// <summary>移除会话</summary>
        public void RemoveSession(string sessionId)
        {
            this.Dictionary.Remove(sessionId);
        }
        #endregion

        #region 方法:ResetSessions()
        /// <summary>重置所有会话</summary>
        public void ResetSessions()
        {
            this.Dictionary.Clear();
        }
        #endregion
    }
}