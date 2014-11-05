namespace X3Platform.Security.Authority
{
    using System.Collections.Generic;

    using X3Platform.Membership;

    /// <summary>Ȩ�޼�����</summary>
    public sealed class AuthorityChecker
    {
        //#region 属性:HasOrganization(string accountId, string organizationNames)
        ///// <summary>
        ///// �����û��Ƿ�ӵ��Ȩ��
        ///// </summary>
        ///// <param name="accountId"></param>
        ///// <param name="organizationNames"></param>
        ///// <returns></returns>
        //public static bool HasOrganization(string accountId, string organizationNames)
        //{
        //    return AuthorityContext.Instance.AuthorityService.HasAuthorizationObject(accountId, "Organization", organizationNames);
        //}
        //#endregion

        //#region 属性:HasRole(string accountId, string roleNames)
        ///// <summary>
        ///// �����û��Ƿ�ӵ��Ȩ��
        ///// </summary>
        ///// <param name="accountId"></param>
        ///// <param name="roleName"></param>
        ///// <returns></returns>
        //public static bool HasRole(string accountId, string roleNames)
        //{
        //    return AuthorityContext.Instance.AuthorityService.HasAuthorizationObject(accountId, "Role", roleNames);
        //}
        //#endregion

        //#region 属性:Check(IAccountInfo account, string[] roleName)
        ///// <summary>
        ///// �����û��Ƿ�ӵ��Ȩ��
        ///// </summary>
        ///// <param name="account"></param>
        ///// <param name="authorityKeys"></param>
        ///// <returns></returns>
        //public static bool HasAuthority(string accountId, string authorityNames)
        //{
        //    return AuthorityContext.Instance.AuthorityService.HasAuthorizationObject(accountId, "Authority", authorityNames);
        //}
        //#endregion

        ////#region 属性:Check(IAccountInfo account, IRoleInfo[] roles, string[] authorityKeys)
        /////// <summary>
        /////// �����û��Ƿ�ӵ��Ȩ��
        /////// </summary>
        /////// <param name="account"></param>
        /////// <param name="roles"></param>
        /////// <param name="authorityKeys"></param>
        /////// <returns></returns>
        ////public static bool Check(IAccountInfo account, IRoleInfo[] roles, string[] authorityKeys)
        ////{
        ////    //
        ////    // 1.��ȡ�ʺŵ�Ȩ��
        ////    //
        ////    // 2.��ȡ����Ҫ�Ľ�ɫ��Χ
        ////    //
        ////    // 3.��Ҫ��Ȩ�޷�Χ
        ////    //
        ////    // 4.��Ҫ��Ȩ�����ȼ�������Ҫ�Ľ�ɫ�����ȼ�.
        ////    //

        ////    IList<AuthorityInfo> accountAuthorities = GetAuthorities(account);

        ////    IList<AuthorityInfo> systemAuthorities = GetUnionAuthorities(GetAuthorities(roles), GetAuthorities(authorityKeys));

        ////    IList<AuthorityInfo> authorities = GetIntersectionAuthorities(accountAuthorities, systemAuthorities);

        ////    return authorities.Count > 0 ? true : false;
        ////}
        ////#endregion

        #region 属性:GetAuthorities(IAccountInfo account)
        /// <summary>��ȡȨ���б�</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static IList<AuthorityInfo> GetAuthorities(IAccountInfo account)
        {
            IRoleInfo[] roles = new IRoleInfo[account.RoleRelations.Count];

            for (int i = 0; i < roles.Length; i++)
            {
                roles[i] = account.RoleRelations[i].GetRole();
            }

            return GetAuthorities(roles);
        }
        #endregion

        #region 属性:GetAuthorities(IRoleInfo[] roles)
        /// <summary>��ȡȨ���б�</summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static IList<AuthorityInfo> GetAuthorities(IRoleInfo[] roles)
        {
            if (roles == null)
                return new List<AuthorityInfo>();

            IList<AuthorityInfo> listA = new List<AuthorityInfo>();

            IList<AuthorityInfo> listB;

            for (int i = 0; i < roles.Length; i++)
            {
                // listB =  roles[i].Authorities;
                listB = new List<AuthorityInfo>();

                listA = GetUnionAuthorities(listA, listB);
            }

            return listA;
        }
        #endregion

        #region 属性:GetAuthorities(string[] authorityKeys)
        /// <summary>��ȡȨ���б�</summary>
        /// <param name="authorityKeys"></param>
        /// <returns></returns>
        public static IList<AuthorityInfo> GetAuthorities(string[] authorityKeys)
        {
            if (authorityKeys == null)
                return new List<AuthorityInfo>();

            IList<AuthorityInfo> list = new List<AuthorityInfo>();

            foreach (string authorityKey in authorityKeys)
            {
                AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityKey];

                if (authority != null)
                {
                    list.Add(authority);
                }
            }

            return list;
        }
        #endregion

        #region 属性:GetIntersectionAuthorities(IList<AuthorityInfo> listA, params IList<AuthorityInfo>[] listB)
        /// <summary>��ȡ����Ȩ���б��Ľ���</summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <returns></returns>
        public static IList<AuthorityInfo> GetIntersectionAuthorities(IList<AuthorityInfo> listA, params IList<AuthorityInfo>[] listB)
        {
            IList<AuthorityInfo> authorities = null;

            AuthorityInfo authority = null;

            foreach (IList<AuthorityInfo> list in listB)
            {
                authorities = new List<AuthorityInfo>();

                for (int x = 0; x < listA.Count; x++)
                {
                    authority = listA[x];

                    if (authority == null)
                        continue;

                    for (int y = 0; y < list.Count; y++)
                    {
                        if (authority == list[y])
                        {
                            authorities.Add(authority);
                        }
                    }
                }
            }

            return authorities;
        }
        #endregion

        #region 属性:GetUnionAuthorities(IList<AuthorityInfo> listA, params IList<AuthorityInfo>[] listB)
        /// <summary>��ȡ����Ȩ���б��Ĳ���</summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <returns></returns>
        public static IList<AuthorityInfo> GetUnionAuthorities(IList<AuthorityInfo> listA, params IList<AuthorityInfo>[] listB)
        {
            IList<AuthorityInfo> authorities = null;

            AuthorityInfo authority = null;

            for (int i = 0; i < listA.Count; i++)
            {
                authority = listA[i];

                if (authority == null)
                    continue;

                authorities.Add(authority);
            }

            foreach (IList<AuthorityInfo> list in listB)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    authority = list[i];

                    if (authority == null)
                        continue;

                    if (!authorities.Contains(authority))
                    {
                        authorities.Add(authority);
                    }
                }
            }

            return listA;
        }
        #endregion
    }
}
