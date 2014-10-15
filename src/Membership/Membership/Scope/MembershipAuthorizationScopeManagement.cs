// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :MembershipAuthorizationScopeManagement.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.Scope
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    using X3Platform.Membership;
    using X3Platform.Membership.Model;
    using X3Platform.Security.Authority;

    /// <summary>Ȩ�޷�Χ������</summary>
    public sealed class MembershipAuthorizationScopeManagement
    {
        // 
        // MembershipAuthorizationScopeObject
        //

        /// <summary></summary>
        /// <param name="authorizationScopeObjects"></param>
        /// <param name="scopeText"></param>
        public static void BindAuthorizationScopeObjects(IList<MembershipAuthorizationScopeObject> authorizationScopeObjects, string scopeText)
        {
            IList<MembershipAuthorizationScopeObject> list = GetAuthorizationScopeObjects(scopeText);

            authorizationScopeObjects.Clear();

            foreach (MembershipAuthorizationScopeObject item in list)
            {
                authorizationScopeObjects.Add(new MembershipAuthorizationScopeObject(item.AuthorizationObjectType, item.AuthorizationObjectId, item.AuthorizationObjectDescription));
            }
        }

        /// <summary></summary>
        /// <param name="scopeText"></param>
        /// <returns></returns>
        public static IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string scopeText)
        {
            IList<MembershipAuthorizationScopeObject> scopeArray = new List<MembershipAuthorizationScopeObject>();

            if (!string.IsNullOrEmpty(scopeText))
            {
                string[] scope = scopeText.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string scopeItemText in scope)
                {
                    scopeArray.Add(new MembershipAuthorizationScopeObject(scopeItemText));
                }
            }

            return scopeArray;
        }

        /// <summary></summary>
        /// <param name="authorizationScopeObjects"></param>
        /// <returns></returns>
        public static string GetAuthorizationScopeObjectText(IList<MembershipAuthorizationScopeObject> authorizationScopeObjects)
        {
            StringBuilder outString = new StringBuilder();

            foreach (MembershipAuthorizationScopeObject authorizationScopeObject in authorizationScopeObjects)
            {
                if (!string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectType)
                    && !string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectId))
                {
                    outString.Append(authorizationScopeObject);
                }
            }

            return outString.ToString().TrimEnd(',');
        }

        /// <summary></summary>
        /// <param name="authorizationScopeObjects"></param>
        /// <returns></returns>
        public static string GetAuthorizationScopeObjectView(IList<MembershipAuthorizationScopeObject> authorizationScopeObjects)
        {
            StringBuilder outString = new StringBuilder();

            foreach (MembershipAuthorizationScopeObject authorizationScopeObject in authorizationScopeObjects)
            {
                if (!string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectType)
                    && !string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectId)
                    && !string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectDescription))
                {
                    outString.AppendFormat("{0};", authorizationScopeObject.AuthorizationObjectDescription);
                }
            }

            return outString.ToString();
        }

        /// <summary>��֤</summary>
        /// <param name="scopes"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool Authenticate(IList<MembershipAuthorizationScopeObject> scopes, IAccountInfo account)
        {
            MembershipAuthorizationScopeObject authorizationScopeObject = null;

            IList<MembershipAuthorizationScopeObject> list = MembershipManagement.Instance.AccountService.GetAuthorizationScopeObjects(account);

            foreach (MembershipAuthorizationScopeObject item in list)
            {
                authorizationScopeObject = scopes.Where(scope => scope.AuthorizationObjectType == item.AuthorizationObjectType
                     && scope.AuthorizationObjectId == item.AuthorizationObjectId).FirstOrDefault();

                if (authorizationScopeObject != null)
                {
                    return true;
                }
            }

            return false;
        }

        // 
        // IAuthorizationScope
        //

        /// <summary></summary>
        /// <param name="authorizationScopes"></param>
        /// <returns></returns>
        public static string GetAuthorizationScopeText(IList<IAuthorizationScope> authorizationScopes)
        {
            StringBuilder outString = new StringBuilder();

            foreach (IAuthorizationScope authorizationScope in authorizationScopes)
            {
                //[�ݴ�] authorizationScope.AuthorizationObject != null , Ȩ�޶�����ɾ��, authorizationScope.AuthorizationObject ���ؿ�.
                if (authorizationScope.AuthorizationObject != null)
                {
                    outString.AppendFormat("{0}#{1}#{2};",
                        authorizationScope.AuthorizationObject.Type.ToLower(),
                        authorizationScope.AuthorizationObject.Id,
                        authorizationScope.AuthorizationObject.Name);
                }
            }

            return outString.ToString();
        }

        /// <summary></summary>
        /// <param name="authorizationScopes"></param>
        /// <returns></returns>
        public static string GetAuthorizationScopeView(IList<IAuthorizationScope> authorizationScopes)
        {
            StringBuilder outString = new StringBuilder();

            foreach (IAuthorizationScope authorizationScope in authorizationScopes)
            {
                //[�ݴ�] authorizationScope.AuthorizationObject != null , Ȩ�޶�����ɾ��, authorizationScope.AuthorizationObject ���ؿ�.
                if (authorizationScope.AuthorizationObject != null)
                {
                    outString.AppendFormat("{0};",
                       authorizationScope.AuthorizationObject.Name);
                }
            }

            return outString.ToString();
        }

        /// <summary></summary>
        /// <param name="entity"></param>
        /// <param name="authority"></param>
        /// <param name="authorizationScopes"></param>
        /// <param name="scopeText"></param>
        public static void BindAuthorizationScope(EntityClass entity, AuthorityInfo authority, IList<IAuthorizationScope> authorizationScopes, string scopeText)
        {
            IList<MembershipAuthorizationScopeObject> list = GetAuthorizationScopeObjects(scopeText);

            authorizationScopes.Clear();

            foreach (MembershipAuthorizationScopeObject item in list)
            {
                authorizationScopes.Add(new MembershipAuthorizationScope(entity, authority, item.GetAuthorizationObject()));
            }
        }

        /// <summary>��֤</summary>
        /// <param name="scopes"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool Authenticate(IList<IAuthorizationScope> scopes, IAccountInfo account)
        {
            IList<MembershipAuthorizationScopeObject> list = MembershipManagement.Instance.AccountService.GetAuthorizationScopeObjects(account);

            foreach (MembershipAuthorizationScopeObject item in list)
            {
                foreach (IAuthorizationScope scope in scopes)
                {
                    if (scope.AuthorizationObject.Type == item.AuthorizationObjectType
                        && scope.AuthorizationObject.Id == item.AuthorizationObjectId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}