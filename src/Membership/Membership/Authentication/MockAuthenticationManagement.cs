namespace X3Platform.Membership.Authentication
{
    #region Using Libraries
    using System;
    using System.Security;
    using System.Security.Principal;
    using System.Web;

    using X3Platform.Configuration;
    using X3Platform.Sessions;
    using X3Platform.Security.Authentication;
    using X3Platform.Spring;
    using X3Platform.Membership.Model;
    using X3Platform.Membership.Configuration;
    #endregion

    /// <summary>Http方式验证请求管理(测试环境)</summary>
    public sealed class MockAuthenticationManagement : GenericAuthenticationManagement
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

                            this.AddSession(accountIdentity, account);

                            if (account == null) { return null; }
                        }
                    }
                }

                return this.Dictionary[accountIdentity];
            }
        }
    }
}