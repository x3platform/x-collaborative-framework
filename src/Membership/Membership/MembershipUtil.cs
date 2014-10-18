namespace X3Platform.Membership
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    using X3Platform.Membership;
    using X3Platform.Membership.Model;
    using X3Platform.Membership.Scope;

    /// <summary>人员及权限管理工具类</summary>
    public sealed class MembershipUitily
    {
        /// <summary>获取用户信息</summary>
        /// <param name="scopeText">范围文本数据</param>
        /// <returns></returns>
        public static IList<IAccountInfo> ParseAccounts(string scopeText)
        {
            IList<IAccountInfo> objects = new List<IAccountInfo>();

            IList<IAccountInfo> accounts = new List<IAccountInfo>();

            string[] list = scopeText.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < list.Length; i++)
            {
                string[] temp = list[i].Split('#');

                if (temp.Length < 2)
                    continue;

                switch (temp[0].ToLower())
                {
                    case "account":
                        accounts = GetAccounts(temp[1]);
                        break;
                    case "org":
                    case "organization":
                        accounts = GetDepartmentAccounts(temp[1]);
                        break;
                    case "group":
                        accounts = GetGroupAccounts(temp[1]);
                        break;
                    case "role":
                        accounts = GetRoleAccounts(temp[1]);
                        break;
                    case "standardrole":
                        accounts = GetStandardRoleAccounts(temp[1]);
                        break;
                    case "standardorganization":
                        accounts = GetStandardOrganizationAccounts(temp[1]);
                        break;
                    default:
                        break;
                }

                objects = GetUnionAccounts(objects, accounts);
            }

            return objects;
        }

        #region 函数:GetAccount(string id)
        /// <summary>获取帐户信息</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IAccountInfo GetAccount(string id)
        {
            IAccountInfo target = MembershipManagement.Instance.AccountService[id];

            return (target.Status == 1) ? target : null;
        }
        #endregion

        #region 函数:GetCorporation(string organizationId)
        /// <summary>获取公司信息</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IOrganizationInfo GetCorporation(string id)
        {
            IOrganizationInfo target = MembershipManagement.Instance.OrganizationService[id];

            return (target.Type == 0 && target.Status == 1) ? target : null;
        }
        #endregion

        #region 函数:GetDepartment(string id)
        /// <summary></summary>
        public static IOrganizationInfo GetDepartment(string id)
        {
            IOrganizationInfo target = MembershipManagement.Instance.OrganizationService[id];

            return (target.Type == 1 && target.Status == 1) ? target : null;
        }
        #endregion

        #region 函数:GetGroup(string id)
        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IGroupInfo GetGroup(string id)
        {
            IGroupInfo target = MembershipManagement.Instance.GroupService[id];

            return (target.Status == 1) ? target : null;
        }
        #endregion

        #region 函数:GetAccounts(string ids)
        /// <summary></summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetAccounts(string ids)
        {
            string whereClause = string.Format(" Id IN (##{0}##) ", ids.Replace(",", "##,##"));

            IList<IAccountInfo> list = new List<IAccountInfo>();

            IList<IAccountInfo> accounts = MembershipManagement.Instance.AccountService.FindAll(whereClause);

            foreach (IAccountInfo account in accounts)
            {
                // 过滤对象为空或者禁用的对象。
                if (account != null && account.Status == 1)
                {
                    list.Add(account);
                }
            }

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 组织架构
        // -------------------------------------------------------

        #region 函数:GetDepartments(string ids)
        /// <summary>获取对应的群组信息</summary>
        /// <param name="ids">群组标识，多个以逗号隔开</param>
        /// <returns></returns>
        public static IList<IOrganizationInfo> GetDepartments(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' });

            List<IOrganizationInfo> list = new List<IOrganizationInfo>();

            IOrganizationInfo item = null;

            foreach (string key in keys)
            {
                item = MembershipManagement.Instance.OrganizationService[key];

                // 过滤对象为空或者禁用的对象。
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 函数:GetDepartmentAccounts(string ids)
        /// <summary>
        /// 取得部门下的所有用户，屏蔽id相同的用户
        /// </summary>
        /// <param name="ids">部门标识, 多个用","分隔.</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetDepartmentAccounts(string ids)
        {
            IList<IOrganizationInfo> departmentArray = GetDepartments(ids);

            IList<IAccountInfo> objectArray = new List<IAccountInfo>();

            for (int x = 0; x < departmentArray.Count; x++)
            {
                if (departmentArray[x] == null)
                    continue;

                IList<IAccountInfo> members = MembershipManagement.Instance.AccountService.FindAllByOrganizationId(departmentArray[x].Id);

                for (int y = 0; y < members.Count; y++)
                {
                    if (members[y] == null)
                        continue;
                    objectArray.Add(members[y]);
                }
            }

            return objectArray;
        }
        #endregion

        #region 函数:GetDepartmentLeaderAccounts(string ids)
        /// <summary>取得部门领导，屏蔽Id相同的用户</summary>
        /// <param name="ids">部门标识，多个用","分隔.</param>
        /// <param name="level">层级</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetDepartmentLeaderAccounts(string ids, int level)
        {
            IList<IOrganizationInfo> departmentArray = GetDepartments(ids);

            IList<IAccountInfo> objectArray = new List<IAccountInfo>();

            for (int x = 0; x < departmentArray.Count; x++)
            {
                if (departmentArray[x] == null)
                    continue;

                IList<IAccountInfo> leaders = MembershipManagement.Instance.AccountService.FindForwardLeaderAccountsByOrganizationId(departmentArray[x].Id, level);

                for (int y = 0; y < leaders.Count; y++)
                {
                    if (leaders[y] == null)
                        continue;
                    objectArray.Add(leaders[y]);
                }
            }

            return MembershipUitily.ToAccounts(objectArray);
        }
        #endregion

        #region 函数:GetDepartmentChiefLeaderAccounts(string ids)
        /// <summary>取得部门正领导，屏蔽id相同的用户。</summary>
        /// <param name="ids">部门标识，多个用","分隔。</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetDepartmentChiefLeaderAccounts(string ids)
        {
            return GetDepartmentLeaderAccounts(ids, 1);
        }
        #endregion

        #region 函数:GetDepartmentChiefLeaderAccounts(string ids)
        /// <summary>取得部门正领导，屏蔽id相同的用户。</summary>
        /// <param name="organizationIds">部门标识，多个用","分隔。</param>
        /// <param name="minPriority">最小权重</param>
        /// <param name="maxPriority">最大权重</param>
        /// <returns></returns>
        public static IList<IRoleInfo> GetDepartmentLeadersBetweenPriority(string organizationIds, int minPriority, int maxPriority)
        {
            IList<IRoleInfo> result = new List<IRoleInfo>();

            string[] keys = organizationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in keys)
            {
                IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAllBetweenPriority(key, minPriority, maxPriority);

                result = GetUnionRoles(result, list);
            }

            return result;
        }
        #endregion

        #region 函数:GetDepartmentAccounts(string organizationIds)
        /// <summary>取得部门下的所有用户，屏蔽id相同的用户</summary>
        /// <param name="organizationIds">部门标识, 多个用","分隔.</param>
        /// <param name="minPriority"></param>
        /// <param name="maxPriority"></param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetDepartmentLeaderAccountsBetweenPriority(string organizationIds, int minPriority, int maxPriority)
        {
            IList<IRoleInfo> leaders = GetDepartmentLeadersBetweenPriority(organizationIds, minPriority, maxPriority);

            IList<IAccountInfo> result = new List<IAccountInfo>();

            for (int x = 0; x < leaders.Count; x++)
            {
                if (leaders[x] == null)
                    continue;

                IList<IAccountInfo> members = MembershipManagement.Instance.AccountService.FindAllByRoleId(leaders[x].Id);

                for (int y = 0; y < members.Count; y++)
                {
                    if (members[y] == null)
                        continue;
                    result.Add(members[y]);
                }
            }

            return result;
        }
        #endregion

        // -------------------------------------------------------
        // 群组
        // -------------------------------------------------------

        #region 函数:GetGroups(string ids)
        /// <summary>获取对应的群组信息</summary>
        /// <param name="ids">群组标识，多个以逗号隔开</param>
        /// <returns></returns>
        public static IList<IGroupInfo> GetGroups(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IGroupInfo> list = new List<IGroupInfo>();

            IGroupInfo item = null;

            foreach (string key in keys)
            {
                item = MembershipManagement.Instance.GroupService[key];

                // 过滤对象为空或者禁用的对象。
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 函数:GetGroupAccounts(string ids)
        /// <summary>获取对应的群组的用户信息，屏蔽id相同的用户</summary>
        /// <param name="ids">群组的标识，多个用逗号分隔</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetGroupAccounts(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IAccountInfo> list = new List<IAccountInfo>();

            foreach (string key in keys)
            {
                // 过滤：1.对象为空 2.对象状态为禁用 3.对象重复。
                GetUnionAccounts(list,
                    MembershipManagement.Instance.AccountService.FindAllByGroupId(key).Where(item
                        => item != null && item.Status == 1).ToList());
            }

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 角色
        // -------------------------------------------------------

        #region 函数:GetRoles(string ids)
        /// <summary>获取对应的角色信息</summary>
        /// <param name="ids">角色标识，多个以逗号隔开</param>
        /// <returns></returns>
        public IList<IRoleInfo> GetRoles(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IRoleInfo> list = new List<IRoleInfo>();

            IRoleInfo item = null;

            for (int i = 0; i < keys.Length; i++)
            {
                item = MembershipManagement.Instance.RoleService[keys[i]];

                // 过滤对象为空或者禁用的对象。
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 函数:GetRoleAccounts(string ids)
        /// <summary>获取对应的角色的用户信息，屏蔽id相同的用户</summary>
        /// <param name="ids">群组的标识，多个用逗号分隔</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetRoleAccounts(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IAccountInfo> list = new List<IAccountInfo>();

            foreach (string key in keys)
            {
                // 过滤：1.对象为空 2.对象状态为禁用 3.对象重复。
                list = GetUnionAccounts(list,
                    MembershipManagement.Instance.AccountService.FindAllByRoleId(key).Where(item
                        => item != null && item.Status == 1).ToList());
            }

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 标准组织
        // -------------------------------------------------------

        #region 函数:GetStandardOrganizations(string ids)
        /// <summary>获取对应的角色信息</summary>
        /// <param name="ids">角色标识，多个以逗号隔开</param>
        /// <returns></returns>
        public IList<IStandardOrganizationInfo> GetStandardOrganizations(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IStandardOrganizationInfo> list = new List<IStandardOrganizationInfo>();

            IStandardOrganizationInfo item = null;

            for (int i = 0; i < keys.Length; i++)
            {
                item = MembershipManagement.Instance.StandardOrganizationService[keys[i]];

                // 过滤对象为空或者禁用的对象。
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 函数:GetStandardRoleAccounts(string ids)
        /// <summary>获取对应的角色的用户信息，屏蔽id相同的用户</summary>
        /// <param name="ids">群组的标识，多个用逗号分隔</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetStandardOrganizationAccounts(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IAccountInfo> list = new List<IAccountInfo>();

            string roleIds = null;

            foreach (string key in keys)
            {
                roleIds = string.Empty;

                MembershipManagement.Instance.RoleService.FindAllByStandardOrganizationId(key)
                     .ToList()
                     .ForEach(role => roleIds += role.Id + ",");

                list = GetUnionAccounts(list, GetRoleAccounts(roleIds.TrimEnd(new char[] { ',' })));
            }

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 标准角色
        // -------------------------------------------------------

        #region 函数:GetStandardRoles(string ids)
        /// <summary>获取对应的角色信息</summary>
        /// <param name="ids">角色标识，多个以逗号隔开</param>
        /// <returns></returns>
        public IList<IStandardRoleInfo> GetStandardRoles(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IStandardRoleInfo> list = new List<IStandardRoleInfo>();

            IStandardRoleInfo item = null;

            for (int i = 0; i < keys.Length; i++)
            {
                item = MembershipManagement.Instance.StandardRoleService[keys[i]];

                // 过滤掉对象为空或者禁用的对象。
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 函数:GetStandardRoleAccounts(string ids)
        /// <summary>获取对应的角色的用户信息，屏蔽id相同的用户</summary>
        /// <param name="ids">群组的标识，多个用逗号分隔</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetStandardRoleAccounts(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IAccountInfo> list = new List<IAccountInfo>();

            string roleIds = null;

            foreach (string key in keys)
            {
                roleIds = string.Empty;

                MembershipManagement.Instance.RoleService.FindAllByStandardRoleId(key)
                     .ToList()
                     .ForEach(role => roleIds += role.Id + ",");

                list = GetUnionAccounts(list, GetRoleAccounts(roleIds.TrimEnd(new char[] { ',' })));
            }

            return list;
        }
        #endregion

        // ------------------------------------------------------------------------------------------
        //工具函数
        // ------------------------------------------------------------------------------------------

        /// <summary></summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public static IList<IAccountInfo> ToAccounts(Dictionary<int, IAccountInfo> accounts)
        {
            List<IAccountInfo> objects = new List<IAccountInfo>();

            foreach (KeyValuePair<int, IAccountInfo> entry in accounts)
            {
                objects.Add(entry.Value);
            }

            return objects;
        }

        /// <summary></summary>
        public static IList<IAccountInfo> ToAccounts(IList<IAccountInfo> accounts)
        {
            return accounts;
        }

        /// <summary></summary>
        public static IList<IAccountInfo> ToAccounts(string ids)
        {
            return GetAccounts(ids);
        }

        /// <summary></summary>
        public static IList<IAccountInfo> ToAccounts(IAccountInfo Account)
        {
            IList<IAccountInfo> objects = new List<IAccountInfo>();

            objects.Add(Account);

            return objects;
        }

        /// <summary></summary>
        public static string GetIdArray(List<IAccountInfo> list)
        {
            StringBuilder outString = new StringBuilder();

            foreach (IAccountInfo item in list)
            {
                if (item == null)
                    continue;

                outString.Append(item.Id + ",");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            return outString.ToString();
        }

        /// <summary></summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetNameArray(IList<IAccountInfo> list)
        {
            StringBuilder outString = new StringBuilder();

            foreach (IAccountInfo item in list)
            {
                if (item == null) { continue; }

                outString.Append(item.Name + ",");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            return outString.ToString();
        }

        #region 函数:GetIntersectionAccounts(List<AccountInfo> AccountsA, params List<AccountInfo>[] AccountsB)
        /// <summary>
        /// 得到用户数组的交集，即若干个数组中相同的用户
        /// </summary>
        /// <param name="list"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetIntersectionAccounts(IList<IAccountInfo> list, params IList<IAccountInfo>[] lists)
        {
            //
            // 以第一个用户组IList<为比对对象
            // 每次比对完之后把组赋值重新赋值给第一个用户组list.
            //

            IList<IAccountInfo> result = null;

            // AccountInfo account = null;

            foreach (IList<IAccountInfo> targetList in lists)
            {
                result = new List<IAccountInfo>();

                foreach (IAccountInfo item in list)
                {
                    if (item == null)
                        continue;

                    foreach (IAccountInfo targetObject in targetList)
                    {
                        if (item == targetObject)
                        {
                            result.Add(item);
                            continue;
                        }
                    }
                }

                // 单个交集
                list = ToAccounts(result);
            }

            return list;
        }
        #endregion

        #region 函数:GetUnionAccounts(List<AccountInfo> AccountsA, params List<AccountInfo>[] AccountsB)
        /// <summary>得到用户数组的并集，即若干个数组中所有不同的用户</summary>
        /// <param name="list"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetUnionAccounts(IList<IAccountInfo> list, params IList<IAccountInfo>[] lists)
        {
            IList<IAccountInfo> result = new List<IAccountInfo>();

            foreach (IAccountInfo item in list)
            {
                if (item == null)
                    continue;

                result.Add(item);
            }

            foreach (IList<IAccountInfo> targetList in lists)
            {
                foreach (IAccountInfo targetObject in targetList)
                {
                    if (targetObject == null)
                        continue;

                    if (!result.Contains(targetObject))
                        result.Add(targetObject);
                }
            }

            return result;
        }
        #endregion

        // ------------------------------------------------------------------------------------------
        // 角色类的函数
        // ------------------------------------------------------------------------------------------

        /// <summary></summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static IRoleInfo[] ToRoleArray(IList<IRoleInfo> roles)
        {
            IRoleInfo[] roleArray = new IRoleInfo[roles.Count];

            for (int index = 0; index < roleArray.Length; index++)
            {
                roleArray[index] = (IRoleInfo)roles[index];
            }

            return roleArray;
        }

        #region 函数:GetRoleIds(IRoleInfo[] roles)
        /// <summary>根据角色数组信息获取相关的角色标识</summary>
        /// <param name="roles">角色数组信息</param>
        /// <summary></summary>
        public static string GetRoleIds(IRoleInfo[] roles)
        {
            string ids = string.Empty;

            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null) { continue; }

                ids += roles[i].Id + ",";
            }

            return ids.TrimEnd(new char[] { ',' });
        }
        #endregion

        #region 函数:GetRoleNames(IRoleInfo[] roles)
        /// <summary></summary>
        public static string GetRoleNames(IRoleInfo[] roles)
        {
            string names = string.Empty;

            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                    continue;

                names = roles[i].Name + ",";
            }

            return names.TrimEnd(new char[] { ',' });
        }
        #endregion

        #region 函数:GetIntersectionRoles(IRoleInfo[] rolesA, params IRoleInfo[][] rolesB)
        /// <summary>得到角色数组的交集，即若干个数组中相同的角色</summary>
        /// <param name="list"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static IList<IRoleInfo> GetIntersectionRoles(IList<IRoleInfo> list, params IList<IRoleInfo>[] lists)
        {
            //
            // 以第一个用户组IList为比对对象
            // 每次比对完之后把组赋值重新赋值给第一个用户组list.
            //

            IList<IRoleInfo> result = null;

            // AccountInfo account = null;

            foreach (IList<IAccountInfo> targetList in lists)
            {
                result = new List<IRoleInfo>();

                foreach (IRoleInfo item in list)
                {
                    if (item == null)
                        continue;

                    foreach (IRoleInfo targetObject in targetList)
                    {
                        if (item == targetObject)
                        {
                            result.Add(item);
                            continue;
                        }
                    }
                }

                list = result;
            }

            return list;
        }
        #endregion

        #region 函数:GetUnionRoles(IList<IRoleInfo> list, params IList<IRoleInfo>[] lists)
        /// <summary>得到用户数组的并集，即若干个数组中所有不同的用户</summary>
        /// <param name="list"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static IList<IRoleInfo> GetUnionRoles(IList<IRoleInfo> list, params IList<IRoleInfo>[] lists)
        {
            IList<IRoleInfo> result = new List<IRoleInfo>();

            foreach (IRoleInfo item in list)
            {
                if (item == null)
                    continue;

                result.Add(item);
            }

            foreach (IList<IRoleInfo> targetList in lists)
            {
                foreach (IRoleInfo targetObject in targetList)
                {
                    if (targetObject == null)
                        continue;

                    if (!result.Contains(targetObject))
                        result.Add(targetObject);
                }
            }

            return result;
        }
        #endregion
    }
}