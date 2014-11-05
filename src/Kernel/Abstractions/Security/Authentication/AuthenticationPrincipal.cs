using System;
using System.Security;
using System.Security.Principal;

using X3Platform.Membership;

namespace X3Platform.Security.Authentication
{
    /// <summary></summary>
    public class AuthenticationPrincipal : IPrincipal
    {
        private IAccountInfo account;

        /// <summary>初始化 若成员通过授权则创建.</summary>
        /// <param name="account">成员信息</param>
        public AuthenticationPrincipal(IAccountInfo account)
        {
            if (account != null && account.IsAuthenticated)
            {
                this.account = account;
            }
            else
            {
                throw new System.Security.SecurityException("创建成员凭证失败.");
            }
        }

        /// <summary>返回一个现实IIdentity接口的 IAccountInfo对象</summary>
        public IIdentity Identity
        {
            get { return this.account; }
        }

        /// <summary>验证是否是角色</summary>
        /// <param name="role">角色名称</param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            foreach (IAccountRoleRelationInfo relation in this.account.RoleRelations)
            {
                if (relation.RoleGlobalName == role)
                {
                    return true;
                }
            }

            return false;
        }
    }
}