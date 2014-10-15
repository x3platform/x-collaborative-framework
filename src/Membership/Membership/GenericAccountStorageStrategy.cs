// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Xml;

    using X3Platform.Sessions;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;

    /// <summary></summary>
    public sealed class GenericAccountStorageStrategy : AbstractAccountStorageStrategy
    {
        /// <summary></summary>
        public GenericAccountStorageStrategy() { }

        //#region 方法:GetAccountIdentity()
        ///// <summary>获取帐号标识</summary>
        ///// <returns></returns>
        //public override string GetAccountIdentity()
        //{
        //    string accountIdentityProperty = MembershipConfigurationView.Instance.AccountIdentityCookieToken;

        //    if (HttpContext.Current.Request.Cookies[accountIdentityProperty] == null)
        //    {
        //        return string.Empty;
        //    }
        //    else
        //    {
        //        return HttpContext.Current.Request.Cookies[accountIdentityProperty].Value;
        //    }
        //}
        //#endregion

        //#region 方法:GetAccountIdentity(string key)
        ///// <summary>获取帐号标识</summary>
        ///// <returns></returns>
        //public string GetAccountIdentity(string key)
        //{
        //    return string.Format("{0}_{1}", MembershipConfigurationView.Instance.AccountIdentityCookieToken, key);
        //}
        //#endregion

        //#region 方法:Deserialize(AccountCacheInfo accountCache)
        ///// <summary>反序列化缓存信息信息</summary>
        ///// <returns></returns>
        //public object Deserialize(AccountCacheInfo accountCache)
        //{
        //    if (accountCache == null) { return null; }

        //    AccountInfo account = this.DeserializeObject(accountCache);

        //    return account;
        //}
        //#endregion

        protected override IAccountInfo DeserializeObject(AccountCacheInfo accountCache)
        {
            AccountInfo account = new AccountInfo();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(accountCache.AccountObject);

            account.Id = doc.SelectSingleNode("accountObject/id").InnerText;

            if (doc.SelectSingleNode("accountObject/code") != null)
                account.Code = doc.SelectSingleNode("accountObject/code").InnerText;
            if (doc.SelectSingleNode("accountObject/name") != null)
                account.Name = doc.SelectSingleNode("accountObject/name").InnerText;
            if (doc.SelectSingleNode("accountObject/loginName") != null)
                account.LoginName = doc.SelectSingleNode("accountObject/loginName").InnerText;
            if (doc.SelectSingleNode("accountObject/globalName") != null)
                account.GlobalName = doc.SelectSingleNode("accountObject/globalName").InnerText;
            if (doc.SelectSingleNode("accountObject/displayName") != null)
                account.DisplayName = doc.SelectSingleNode("accountObject/displayName").InnerText;
            if (doc.SelectSingleNode("accountObject/certifiedAvatar") != null)
                account.CertifiedAvatar = doc.SelectSingleNode("accountObject/certifiedAvatar").InnerText;
            if (doc.SelectSingleNode("accountObject/certifiedEmail") != null)
                account.CertifiedEmail = doc.SelectSingleNode("accountObject/certifiedEmail").InnerText;
            if (doc.SelectSingleNode("accountObject/certifiedTelephone") != null)
                account.CertifiedTelephone = doc.SelectSingleNode("accountObject/certifiedTelephone").InnerText;

            return account;
        }

        //#region 方法:Serialize()
        ///// <summary>序列化缓存信息信息</summary>
        ///// <returns></returns>
        //public AccountCacheInfo Serialize()
        //{
        //    AccountCacheInfo accountCache = new AccountCacheInfo();

        //    accountCache.AccountIdentity = this.GetAccountIdentity(this.account.Id);

        //    accountCache.AccountCacheValue = this.account.LoginName;

        //    accountCache.AccountObject = this.SerializeObject(this.account);

        //    accountCache.AccountObjectType = string.Format("{0},{1}", this.account.GetType(), Assembly.GetExecutingAssembly());

        //    accountCache.IP = this.account.IP;

        //    accountCache.BeginDate = DateTime.Now;

        //    accountCache.EndDate = accountCache.BeginDate.AddMonths(3);

        //    accountCache.UpdateDate = DateTime.Now;

        //    return accountCache;
        //}
        //#endregion

        //private string SerializeObject(AccountInfo account)
        //{
        //    StringBuilder outString = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");

        //    outString.Append("<accountObject>");
        //    outString.AppendFormat("<id><![CDATA[{0}]]></id>", account.Id);
        //    outString.AppendFormat("<code><![CDATA[{0}]]></code>", account.Code);
        //    outString.AppendFormat("<name><![CDATA[{0}]]></name>", account.Name);
        //    outString.AppendFormat("<loginName><![CDATA[{0}]]></loginName>", account.LoginName);
        //    outString.AppendFormat("<globalName><![CDATA[{0}]]></globalName>", account.GlobalName);
        //    outString.AppendFormat("<displayName><![CDATA[{0}]]></displayName>", account.DisplayName);
        //    outString.AppendFormat("<certifiedAvatar><![CDATA[{0}]]></certifiedAvatar>", account.CertifiedAvatar);
        //    outString.AppendFormat("<certifiedEmail><![CDATA[{0}]]></certifiedEmail>", account.CertifiedEmail);
        //    outString.AppendFormat("<certifiedTelephone><![CDATA[{0}]]></certifiedTelephone>", account.CertifiedTelephone);
        //    outString.AppendFormat("<ip><![CDATA[{0}]]></ip>", account.IP);
        //    outString.Append("</accountObject>");

        //    return outString.ToString();
        //}
    }
}
