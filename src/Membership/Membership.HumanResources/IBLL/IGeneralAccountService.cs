namespace X3Platform.Membership.HumanResources.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;
    using X3Platform.Membership.Model;

    [SpringObject("X3Platform.Membership.HumanResources.IBLL.IGeneralAccountService")]
    public interface IGeneralAccountService
    {
        #region 函数:SetMemberCard(MemberInfo member)
        /// <summary>更新帐户资料</summary>
        /// <returns></returns>
        void SetMemberCard(MemberInfo member);
        #endregion
        
        #region 函数:GetCertifiedAvatar(string accountId)
        ///<summary>获取相关帐号的头像信息</summary>
        ///<param name="accountId">实例的详细信息</param>
        string GetCertifiedAvatar(string accountId);
        #endregion

        #region 函数:GetCertifiedAvatar(IAccountInfo account)
        ///<summary>获取相关帐号的头像信息</summary>
        ///<param name="accountId">实例的详细信息</param>
        string GetCertifiedAvatar(IAccountInfo account);
        #endregion

        #region 函数:ChangePassword(string password, string originalPassword)
        /// <summary>修改密码</summary>
        /// <param name="loginName">登录名</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="originalPassowrd">原始密码</param>
        /// <returns>修改成功, 返回 0, 旧密码不匹配, 返回 1.</returns>
        int ChangePassword(string password, string originalPassword);
        #endregion
    }
}
