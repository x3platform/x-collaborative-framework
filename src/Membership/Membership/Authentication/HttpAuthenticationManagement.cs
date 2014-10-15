namespace X3Platform.Membership.Authentication
{
    #region Using Libraries
    using System;
    using System.Security;
    using System.Security.Principal;
    using System.Web;

    using X3Platform.Security.Authentication;
    using X3Platform.Sessions;
    using X3Platform.Spring;

    using X3Platform.Membership.Model;
    using X3Platform.Membership.Configuration;
    #endregion

    /// <summary>Http方式的验证请求管理</summary>
    public sealed class HttpAuthenticationManagement : GenericAuthenticationManagement
    {
        /// <summary>获取认证的用户信息</summary>
        public override IAccountInfo GetAuthUser()
        {
            string accountIdentity = this.GetIdentityValue();

            if (string.IsNullOrEmpty(accountIdentity))
            {
                return null;
            }
            else
            {
                if (!this.Dictionary.ContainsKey(accountIdentity))
                {
                    lock (this.cacheSyncRoot)
                    {
                        if (!this.Dictionary.ContainsKey(accountIdentity))
                        {
                            IAccountInfo account = SessionContext.Instance.GetAuthAccount<AccountInfo>(this.strategy, accountIdentity);

                            if (account == null) { return null; }

                            this.AddSession(accountIdentity, account);
                        }
                    }
                }

                return this.Dictionary[accountIdentity];
            }
        }
    }
}