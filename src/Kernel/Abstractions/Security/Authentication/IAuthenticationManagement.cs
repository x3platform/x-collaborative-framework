namespace X3Platform.Security.Authentication
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.Security.Principal;

    using X3Platform.Membership;
    using X3Platform.Spring;
    using X3Platform.Sessions;
    #endregion

    /// <summary>验证请求</summary>
    [SpringObject("X3Platform.Security.Authentication.IAuthenticationManagement")]
    public interface IAuthenticationManagement
    {
        /// <summary>获取用户存储策略</summary>
        IAccountStorageStrategy GetAccountStorageStrategy();

        /// <summary>标识的名字</summary>
        string IdentityName { get; }

        /// <summary>获取用户标识的值</summary>
        string GetIdentityValue();

        /// <summary>获取认证的用户信息</summary>
        IAccountInfo GetAuthUser();

        #region 方法:Login(string loginName, string password, bool isCreatePresistent)
        /// <summary>登录</summary>
        /// <returns></returns>
        int Login(string loginName, string password, bool isCreatePresistent);
        #endregion

        #region 方法:Logout()
        /// <summary>注销</summary>
        /// <returns></returns>
        int Logout();
        #endregion

        #region 方法:GetSessions()
        /// <summary>获取当前会话集合</summary>
        /// <returns></returns>
        IDictionary<string, IAccountInfo> GetSessions();
        #endregion

        #region 方法:AddSession(string sessionId, IAccountInfo account)
        /// <summary>新增会话</summary>
        void AddSession(string sessionId, IAccountInfo account);
        #endregion

        #region 方法:RemoveSession(string sessionId)
        /// <summary>移除会话</summary>
        void RemoveSession(string sessionId);
        #endregion

        #region 方法:ResetSessions()
        /// <summary>重置所有会话</summary>
        void ResetSessions();
        #endregion
    }
}