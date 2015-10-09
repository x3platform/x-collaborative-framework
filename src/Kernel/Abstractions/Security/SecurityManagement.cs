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

    /// <summary>安全管理</summary>
    public sealed class SecurityManagement
    {
        //#region 函数:CheckAuthority(IList<AuthorityInfo> authorities)
        ///// <summary>检测用户是否拥有权限</summary>
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

        //#region 函数:CheckAuthority( string[] authorityNames)
        ///// <summary>检测用户是否拥有权限</summary>
        ///// <param name="account"></param>
        ///// <param name="authorityKeys"></param>
        ///// <returns></returns>
        //public static bool CheckAuthority(string[] authorityNames)
        //{
        //    // return Check( null, authorityKeys);
        //    return false;
        //}
        //#endregion

        #region 函数:Check(IAccountInfo account, IRoleInfo[] roles)
        /// <summary>检测用户是否拥有权限</summary>
        /// <param name="account"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool CheckRole(IAccountInfo account, IRoleInfo[] roles)
        {
            return false;
            //return Check(account, roles, null);
        }
        #endregion

        #region 函数:Check(IAccountInfo account, IAuthorizationScope[] authorizationScopes)
        /// <summary>检测用户是否拥有权限</summary>
        /// <param name="account"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool Check(IAccountInfo account, IAuthorizationScope[] authorizationScopes)
        {
            bool authenticated = false;

            foreach (IAuthorizationScope authorizationScope in authorizationScopes)
            {
                IAuthorizationObject authorizationObject = authorizationScope.AuthorizationObject;

                // 空对象: 未找到相关值..
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
                        foreach (IAccountOrganizationUnitRelationInfo relation in account.OrganizationUnitRelations)
                        {
                            if (relation.OrganizationUnitId == authorizationObject.Id)
                            {
                                authenticated = true;
                                break;
                            }
                        }
                        break;
                    // [未实现]
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

        //#region 函数:Check(IAccountInfo account, IRoleInfo[] roles, string[] authorityKeys)
        ///// <summary>检测用户是否拥有权限</summary>
        ///// <param name="account"></param>
        ///// <param name="roles"></param>
        ///// <param name="authorityKeys"></param>
        ///// <returns></returns>
        //public static bool Check(IAccountInfo account, IRoleInfo[] roles, string[] authorityKeys)
        //{
        //    //
        //    // 1.获取帐号的权限
        //    //
        //    // 2.获取的需要的角色范围
        //    //
        //    // 3.需要的权限范围
        //    //
        //    // 4.需要的权限优先级大于需要的角色的优先级.
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
