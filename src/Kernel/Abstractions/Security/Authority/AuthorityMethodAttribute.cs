using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.Membership;
using System.Runtime.Remoting.Messaging;

namespace X3Platform.Security.Authority
{
    /// <summary>��Ȩ�޵ķ���</summary>
    public sealed class AuthorityMethodAttribute : SecurityMethodAttribute
    {
        /// <summary>Ȩ���б�</summary>
        private IList<AuthorityInfo> authorities;

        #region ����:Account
        /// <summary>��ǰ�ʺ���Ϣ</summary>
        protected override IAccountInfo Account
        {
            get { return KernelContext.Current.User; }
        }
        #endregion

        #region ���캯��:AuthorityMethodAttribute(IRoleInfo[] roles)
        /// <summary>���캯��</summary>
        /// <param name="roles">��ɫ����</param>
        public AuthorityMethodAttribute(IRoleInfo[] roles)
            : this(roles, null)
        {
        }
        #endregion

        #region ���캯��:AuthorityMethodAttribute(string authorityKeys)
        /// <summary>���캯��</summary>
        /// <param name="authorityKeys">Ȩ��ֵ,����Զ��ŷֿ�</param>
        public AuthorityMethodAttribute(string authorityKeys)
            : this(null, authorityKeys)
        {
        }
        #endregion

        #region ���캯��:AuthorityMethodAttribute(IRoleInfo[] roles, string authorityKeys)
        /// <summary>���캯��</summary>
        /// <param name="roles">��ɫ����</param>
        /// <param name="authorityKeys">Ȩ��ֵ,����Զ��ŷֿ�</param>
        public AuthorityMethodAttribute(IRoleInfo[] roles, string authorityKeys)
        {
            Parse(roles, authorityKeys.Split(new char[','], StringSplitOptions.RemoveEmptyEntries));
        }
        #endregion

        #region ����:Parse(IRoleInfo[] roles, string[] authorityKeys)
        /// <summary>����Ȩ���б�</summary>
        /// <param name="roles">��ɫ����</param>
        /// <param name="authorityKeys">Ȩ��ֵ,����Զ��ŷֿ�</param>
        private void Parse(IRoleInfo[] roles, string[] authorityKeys)
        {
            this.authorities = AuthorityChecker.GetUnionAuthorities(AuthorityChecker.GetAuthorities(roles), AuthorityChecker.GetAuthorities(authorityKeys));
        }
        #endregion

        #region ����:Check(IMethodCallMessage methodCallMessage)
        /// <summary>���Ȩ��</summary>
        /// <param name="methodCallMessage">����</param>
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
