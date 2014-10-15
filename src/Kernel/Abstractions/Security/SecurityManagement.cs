// =============================================================================
//
// Copyright (c) x3platfrom.com
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

namespace X3Platform.Security
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using X3Platform.Logging;
    using X3Platform.Membership;
    using X3Platform.Security.Authority;
    using X3Platform.Security.Authority.Configuration;

    /// <summary>��ȫ����</summary>
    public sealed class SecurityManagement
    {
        //#region 属性:CheckAuthority(IList<AuthorityInfo> authorities)
        ///// <summary>�����û��Ƿ�ӵ��Ȩ��</summary>
        ///// <param name="account"></param>
        ///// <param name="authorities"></param>
        ///// <returns></returns>
        //public static bool CheckAuthority(IList<AuthorityInfo> authorities)
        //{
        //    //IList<AuthorityInfo> accountAuthorities = GetAuthorities(account);

        //    //authorities = GetIntersectionAuthorities(accountAuthorities, authorities);

        //    //return authorities.Count > 0 ? true : false;
        //    return false;
        //}
        //#endregion

        //#region 属性:CheckAuthority( string[] authorityNames)
        ///// <summary>�����û��Ƿ�ӵ��Ȩ��</summary>
        ///// <param name="account"></param>
        ///// <param name="authorityKeys"></param>
        ///// <returns></returns>
        //public static bool CheckAuthority(string[] authorityNames)
        //{
        //    // return Check( null, authorityKeys);
        //    return false;
        //}
        //#endregion

        #region 属性:Check(IAccountInfo account, IRoleInfo[] roles)
        /// <summary>�����û��Ƿ�ӵ��Ȩ��</summary>
        /// <param name="account"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool CheckRole(IAccountInfo account, IRoleInfo[] roles)
        {
            return false;
            //return Check(account, roles, null);
        }
        #endregion

        #region 属性:Check(IAccountInfo account, IAuthorizationScope[] authorizationScopes)
        /// <summary>�����û��Ƿ�ӵ��Ȩ��</summary>
        /// <param name="account"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool Check(IAccountInfo account, IAuthorizationScope[] authorizationScopes)
        {
            bool authenticated = false;

            foreach (IAuthorizationScope authorizationScope in authorizationScopes)
            {
                IAuthorizationObject authorizationObject = authorizationScope.AuthorizationObject;

                // �ն���: δ�ҵ�����ֵ..
                if (authorizationObject == null)
                    continue;

                switch (authorizationObject.Type)
                {
                    case "account":
                        if (account.Id == authorizationObject.Id)
                            authenticated = true;
                        break;

                    case "role":
                        foreach (IAccountRoleRelationInfo relation in account.RoleRelations)
                        {
                            if (relation.RoleId == authorizationObject.Id)
                            {
                                authenticated = true;
                                break;
                            }
                        }
                        break;

                    case "organization":
                        foreach (IAccountOrganizationRelationInfo relation in account.OrganizationRelations)
                        {
                            if (relation.OrganizationId == authorizationObject.Id)
                            {
                                authenticated = true;
                                break;
                            }
                        }
                        break;
                    // [δʵ��]
                    //case "generalrole":

                    //    foreach (IAuthorizationObject organization.role in account.Roles)
                    //    {
                    //        if (organization.Id == authorizationObject.Id)
                    //        {
                    //            authenticated = true;
                    //            break;
                    //        }
                    //    }
                    //    break;

                    default:
                        break;
                }

                if (authenticated)
                    break;
            }

            return authenticated;
        }
        #endregion

        //#region 属性:Check(IAccountInfo account, IRoleInfo[] roles, string[] authorityKeys)
        ///// <summary>�����û��Ƿ�ӵ��Ȩ��</summary>
        ///// <param name="account"></param>
        ///// <param name="roles"></param>
        ///// <param name="authorityKeys"></param>
        ///// <returns></returns>
        //public static bool Check(IAccountInfo account, IRoleInfo[] roles, string[] authorityKeys)
        //{
        //    //
        //    // 1.��ȡ�ʺŵ�Ȩ��
        //    //
        //    // 2.��ȡ����Ҫ�Ľ�ɫ��Χ
        //    //
        //    // 3.��Ҫ��Ȩ�޷�Χ
        //    //
        //    // 4.��Ҫ��Ȩ�����ȼ�������Ҫ�Ľ�ɫ�����ȼ�.
        //    //

        //    //IList<AuthorityInfo> accountAuthorities = GetAuthorities(account);

        //    //IList<AuthorityInfo> systemAuthorities = GetUnionAuthorities(GetAuthorities(roles), GetAuthorities(authorityKeys));

        //    //IList<AuthorityInfo> authorities = GetIntersectionAuthorities(accountAuthorities, systemAuthorities);

        //    //return authorities.Count > 0 ? true : false;
        //    return false;
        //}
        //#endregion
    }
}
