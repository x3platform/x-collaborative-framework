using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.Membership;
using System.Runtime.Remoting.Messaging;

namespace X3Platform.Security.Authority
{
    /// <summary>带权限的方法</summary>
    public sealed class AuthorityMethodAttribute : SecurityMethodAttribute
    {
        /// <summary>权限列表</summary>
        private IList<AuthorityInfo> authorities;

        #region 属性:Account
        /// <summary>当前帐号信息</summary>
        protected override IAccountInfo Account
        {
            get { return KernelContext.Current.User; }
        }
        #endregion

        #region 构造函数:AuthorityMethodAttribute(IRoleInfo[] roles)
        /// <summary>构造函数</summary>
        /// <param name="roles">角色数组</param>
        public AuthorityMethodAttribute(IRoleInfo[] roles)
            : this(roles, null)
        {
        }
        #endregion

        #region 构造函数:AuthorityMethodAttribute(string authorityKeys)
        /// <summary>构造函数</summary>
        /// <param name="authorityKeys">权限值,多个以逗号分开</param>
        public AuthorityMethodAttribute(string authorityKeys)
            : this(null, authorityKeys)
        {
        }
        #endregion

        #region 构造函数:AuthorityMethodAttribute(IRoleInfo[] roles, string authorityKeys)
        /// <summary>构造函数</summary>
        /// <param name="roles">角色数组</param>
        /// <param name="authorityKeys">权限值,多个以逗号分开</param>
        public AuthorityMethodAttribute(IRoleInfo[] roles, string authorityKeys)
        {
            Parse(roles, authorityKeys.Split(new char[','], StringSplitOptions.RemoveEmptyEntries));
        }
        #endregion

        #region 函数:Parse(IRoleInfo[] roles, string[] authorityKeys)
        /// <summary>解析权限列表</summary>
        /// <param name="roles">角色数组</param>
        /// <param name="authorityKeys">权限值,多个以逗号分开</param>
        private void Parse(IRoleInfo[] roles, string[] authorityKeys)
        {
            this.authorities = AuthorityChecker.GetUnionAuthorities(AuthorityChecker.GetAuthorities(roles), AuthorityChecker.GetAuthorities(authorityKeys));
        }
        #endregion

        #region 函数:Check(IMethodCallMessage methodCallMessage)
        /// <summary>检测权限</summary>
        /// <param name="methodCallMessage">参数</param>
        /// <returns></returns>
        public override bool Check(IMethodCallMessage methodCallMessage)
        {
            string authorityNames = string.Empty;

            foreach (AuthorityInfo authority in this.authorities)
            {
                authorityNames += authority.Name + ",";
            }

            // return AuthorityChecker.HasAuthority(this.Account.Id, authorityNames.TrimEnd(new char[] { ',' }));
            return false;
        }
        #endregion
    }
}
