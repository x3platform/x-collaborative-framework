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
    #endregion

    /// <summary>测试环境的验证请求管理</summary>
    /// <remarks>单元测试环境模拟测试帐号信息通过测试</remarks>
    public sealed class TestingAuthenticationManagement : GenericAuthenticationManagement
    {
        // 默认测试帐号的唯一标识
        private const string DEFAULT_ACCOUNT_ID = "52cf89ba-7db5-4e64-9c64-3c868b6e7a99";
        // 默认测试帐号的名称
        private const string DEFAULT_ACCOUNT_NAME = "内置测试帐号";

        /// <summary>当前登录帐号信息</summary>
        private IAccountInfo account = null;

        /// <summary>获取认证的帐号信息</summary>
        public override IAccountInfo GetAuthUser()
        {
            // 获取帐号信息
            if (this.account == null)
            {
                IAccountInfo param = new AccountInfo();

                param.Id = DEFAULT_ACCOUNT_ID;
                param.Name = DEFAULT_ACCOUNT_NAME;

                this.account = param;
            }

            return account;
        }

        /// <summary>修改当前登录帐号信息</summary>
        /// <param name="accountId">帐号的唯一标识</param>
        public void ChangeAuthUser(string accountId)
        {
            this.account = MembershipManagement.Instance.AccountService[accountId];
        }

    }
}