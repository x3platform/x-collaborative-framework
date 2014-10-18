namespace X3Platform.Membership.Authentication
{
    #region Using Libraries
    using System;
    using System.Net;
    using System.IO;
    using System.Web;

    using X3Platform.DigitalNumber; 
    using X3Platform.Location.IPQuery;
    using X3Platform.Security.Authentication;
    using X3Platform.Sessions;
    using X3Platform.Sessions.Configuration;
    
    using X3Platform.Membership.Model;
    using X3Platform.Membership.Configuration;
    #endregion

    /// <summary>集成单点登录的验证请求管理</summary>
    public class SSOAuthenticationManagement : GenericAuthenticationManagement
    {
        /// <summary>获取认证的用户信息</summary>
        public override IAccountInfo GetAuthUser()
        {
            string accountIdentity = this.GetIdentityValue();

            // 获取帐号信息
            IAccountInfo account = this.GetSessionAccount(accountIdentity);

            if (account == null)
            {
                // 如果当前帐号为空, 则连接到验证服务器验证用户的会话信息.
                string loginName = GetAccountLoginName();

                // 没有登录信息则返回空
                if (string.IsNullOrEmpty(loginName)) { return null; }

                account = MembershipManagement.Instance.AccountService.FindOneByLoginName(loginName);

                if (account != null && account.LoginName == loginName)
                {
                    accountIdentity = string.Format("{0}-{1}", account.Id, DigitalNumberContext.Generate("Key_Session"));


                    HttpAuthenticationCookieSetter.SetUserCookies(accountIdentity);

                    // 记录登录时间
                    if (account.LoginDate.AddHours(8) < DateTime.Now)
                    {
                        MembershipManagement.Instance.AccountService.SetIPAndLoginDate(account.Id, IPQueryContext.GetClientIP(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }
            }

            if (account == null) { return null; }

            // 写入临时存储
            if (!this.cacheStorage.ContainsKey(accountIdentity))
            {
                lock (this.cacheSyncRoot)
                {
                    if (!this.cacheStorage.ContainsKey(accountIdentity))
                    {

                        this.AddSession(accountIdentity, account);
                    }
                }
            }

            return account;
        }

        private string GetAccountLoginName()
        {
            string loginName = string.Empty;

            WebClient web = new WebClient();

            if (HttpContext.Current.Request.Cookies[MembershipConfigurationView.Instance.SsoIdentityCookieToken] != null)
            {
                string account = HttpContext.Current.Request.Cookies[MembershipConfigurationView.Instance.SsoIdentityCookieToken].Value;

                if (!string.IsNullOrEmpty(account))
                {
                    using (Stream stream = web.OpenRead(string.Format(MembershipConfigurationView.Instance.SsoSessionUrl, account)))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            loginName = reader.ReadToEnd();
                            reader.Close();
                        }

                        stream.Close();
                    }
                }
            }

            return loginName;
        }
    }
}