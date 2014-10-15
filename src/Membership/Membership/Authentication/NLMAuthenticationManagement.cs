namespace X3Platform.Membership.Authentication
{
    #region Using Libraries
    using System;
    using System.Web;

    using X3Platform.DigitalNumber;
    using X3Platform.Sessions;
    using X3Platform.Location.IPQuery;
    using X3Platform.Security.Authentication;

    using X3Platform.Membership.Model;
    using X3Platform.Membership.Configuration;
    #endregion

    /// <summary>NLM方式的验证请求管理</summary>
    public class NLMAuthenticationManagement : GenericAuthenticationManagement
    {
        /// <summary></summary>
        public string GetIdentityValue()
        {
            string accountIdentityProperty = MembershipConfigurationView.Instance.AccountIdentityCookieToken;

            if (HttpContext.Current.Request.Cookies[accountIdentityProperty] == null
                || string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[accountIdentityProperty].Value))
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    string longinName = HttpContext.Current.User.Identity.Name;

                    int index = longinName.IndexOf("\\") + 1;

                    longinName = longinName.Substring(index, longinName.Length - index);

                    IAccountInfo account = MembershipManagement.Instance.AccountService.FindOneByLoginName(longinName);

                    if (account == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        string accountIdentity = string.Format("{0}-{1}", account.Id, DigitalNumberContext.Generate("Key_Session"));

                        SessionContext.Instance.Write(this.strategy, accountIdentity, account);

                        HttpAuthenticationCookieSetter.SetUserCookies(accountIdentity);

                        // 记录登录时间
                        if (account.LoginDate.AddHours(8) < DateTime.Now)
                        {
                            MembershipManagement.Instance.AccountService.SetIPAndLoginDate(account.Id, IPQueryContext.GetClientIP(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        }

                        return accountIdentity;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return HttpContext.Current.Request.Cookies[accountIdentityProperty].Value;
            }
        }

        /// <summary>获取认证的用户信息</summary>
        public override IAccountInfo GetAuthUser()
        {
            string accountIdentity = this.GetIdentityValue();

            IAccountInfo account = SessionContext.Instance.GetAuthAccount<AccountInfo>(strategy, accountIdentity);

            if (account == null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string longinName = HttpContext.Current.User.Identity.Name;

                int index = longinName.IndexOf("\\") + 1;

                longinName = longinName.Substring(index, longinName.Length - index);

                account = MembershipManagement.Instance.AccountService.FindOneByLoginName(longinName);

                if (account == null)
                {
                    return null;
                }
                else
                {
                    SessionContext.Instance.Write(this.strategy, accountIdentity, account);

                    HttpAuthenticationCookieSetter.SetUserCookies(accountIdentity);

                    // 记录登录时间
                    if (account.LoginDate.AddHours(8) < DateTime.Now)
                    {
                        MembershipManagement.Instance.AccountService.SetIPAndLoginDate(account.Id, IPQueryContext.GetClientIP(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
            }

            if (account != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                HttpAuthenticationCookieSetter.SetUserCookies(accountIdentity);

                return account;
            }
            else
            {
                return null;
            }
        }
    }
}