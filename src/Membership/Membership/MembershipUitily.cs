// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :MembershipUitily.cs
//
// Description  :MembershipUitily
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    using X3Platform.Membership;
    using X3Platform.Membership.Model;
    using X3Platform.Membership.Scope;

    /// <summary>��Ա��Ȩ�޹�������</summary>
    public sealed class MembershipUitily
    {
        /// <summary>��ȡ�û���Ϣ</summary>
        /// <param name="scopeText">��Χ�ı�����</param>
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

        #region 属性:GetAccount(string id)
        /// <summary>��ȡ�ʻ���Ϣ</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IAccountInfo GetAccount(string id)
        {
            IAccountInfo target = MembershipManagement.Instance.AccountService[id];

            return (target.Status == 1) ? target : null;
        }
        #endregion

        #region 属性:GetCorporation(string organizationId)
        /// <summary>��ȡ��˾��Ϣ</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IOrganizationInfo GetCorporation(string id)
        {
            IOrganizationInfo target = MembershipManagement.Instance.OrganizationService[id];

            return (target.Type == 0 && target.Status == 1) ? target : null;
        }
        #endregion

        #region 属性:GetDepartment(string id)
        /// <summary></summary>
        public static IOrganizationInfo GetDepartment(string id)
        {
            IOrganizationInfo target = MembershipManagement.Instance.OrganizationService[id];

            return (target.Type == 1 && target.Status == 1) ? target : null;
        }
        #endregion

        #region 属性:GetGroup(string id)
        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IGroupInfo GetGroup(string id)
        {
            IGroupInfo target = MembershipManagement.Instance.GroupService[id];

            return (target.Status == 1) ? target : null;
        }
        #endregion

        #region 属性:GetAccounts(string ids)
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
                // ���˶���Ϊ�ջ��߽��õĶ�����
                if (account != null && account.Status == 1)
                {
                    list.Add(account);
                }
            }

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // ��֯�ܹ�
        // -------------------------------------------------------

        #region 属性:GetDepartments(string ids)
        /// <summary>��ȡ��Ӧ��Ⱥ����Ϣ</summary>
        /// <param name="ids">Ⱥ����ʶ�������Զ��Ÿ���</param>
        /// <returns></returns>
        public static IList<IOrganizationInfo> GetDepartments(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' });

            List<IOrganizationInfo> list = new List<IOrganizationInfo>();

            IOrganizationInfo item = null;

            foreach (string key in keys)
            {
                item = MembershipManagement.Instance.OrganizationService[key];

                // ���˶���Ϊ�ջ��߽��õĶ�����
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 属性:GetDepartmentAccounts(string ids)
        /// <summary>
        /// ȡ�ò����µ������û�������id��ͬ���û�
        /// </summary>
        /// <param name="ids">���ű�ʶ, ������","�ָ�.</param>
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

        #region 属性:GetDepartmentLeaderAccounts(string ids)
        /// <summary>ȡ�ò����쵼������Id��ͬ���û�</summary>
        /// <param name="ids">���ű�ʶ��������","�ָ�.</param>
        /// <param name="level">�㼶</param>
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

        #region 属性:GetDepartmentChiefLeaderAccounts(string ids)
        /// <summary>ȡ�ò������쵼������id��ͬ���û���</summary>
        /// <param name="ids">���ű�ʶ��������","�ָ���</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetDepartmentChiefLeaderAccounts(string ids)
        {
            return GetDepartmentLeaderAccounts(ids, 1);
        }
        #endregion

        #region 属性:GetDepartmentChiefLeaderAccounts(string ids)
        /// <summary>ȡ�ò������쵼������id��ͬ���û���</summary>
        /// <param name="organizationIds">���ű�ʶ��������","�ָ���</param>
        /// <param name="minPriority">��СȨ��</param>
        /// <param name="maxPriority">����Ȩ��</param>
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

        #region 属性:GetDepartmentAccounts(string organizationIds)
        /// <summary>ȡ�ò����µ������û�������id��ͬ���û�</summary>
        /// <param name="organizationIds">���ű�ʶ, ������","�ָ�.</param>
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
        // Ⱥ��
        // -------------------------------------------------------

        #region 属性:GetGroups(string ids)
        /// <summary>��ȡ��Ӧ��Ⱥ����Ϣ</summary>
        /// <param name="ids">Ⱥ����ʶ�������Զ��Ÿ���</param>
        /// <returns></returns>
        public static IList<IGroupInfo> GetGroups(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IGroupInfo> list = new List<IGroupInfo>();

            IGroupInfo item = null;

            foreach (string key in keys)
            {
                item = MembershipManagement.Instance.GroupService[key];

                // ���˶���Ϊ�ջ��߽��õĶ�����
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 属性:GetGroupAccounts(string ids)
        /// <summary>��ȡ��Ӧ��Ⱥ�����û���Ϣ������id��ͬ���û�</summary>
        /// <param name="ids">Ⱥ���ı�ʶ�������ö��ŷָ�</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetGroupAccounts(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IAccountInfo> list = new List<IAccountInfo>();

            foreach (string key in keys)
            {
                // ���ˣ�1.����Ϊ�� 2.����״̬Ϊ���� 3.�����ظ���
                GetUnionAccounts(list,
                    MembershipManagement.Instance.AccountService.FindAllByGroupId(key).Where(item
                        => item != null && item.Status == 1).ToList());
            }

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // ��ɫ
        // -------------------------------------------------------

        #region 属性:GetRoles(string ids)
        /// <summary>��ȡ��Ӧ�Ľ�ɫ��Ϣ</summary>
        /// <param name="ids">��ɫ��ʶ�������Զ��Ÿ���</param>
        /// <returns></returns>
        public IList<IRoleInfo> GetRoles(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IRoleInfo> list = new List<IRoleInfo>();

            IRoleInfo item = null;

            for (int i = 0; i < keys.Length; i++)
            {
                item = MembershipManagement.Instance.RoleService[keys[i]];

                // ���˶���Ϊ�ջ��߽��õĶ�����
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 属性:GetRoleAccounts(string ids)
        /// <summary>��ȡ��Ӧ�Ľ�ɫ���û���Ϣ������id��ͬ���û�</summary>
        /// <param name="ids">Ⱥ���ı�ʶ�������ö��ŷָ�</param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetRoleAccounts(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IAccountInfo> list = new List<IAccountInfo>();

            foreach (string key in keys)
            {
                // ���ˣ�1.����Ϊ�� 2.����״̬Ϊ���� 3.�����ظ���
                list = GetUnionAccounts(list,
                    MembershipManagement.Instance.AccountService.FindAllByRoleId(key).Where(item
                        => item != null && item.Status == 1).ToList());
            }

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // ��׼��֯
        // -------------------------------------------------------

        #region 属性:GetStandardOrganizations(string ids)
        /// <summary>��ȡ��Ӧ�Ľ�ɫ��Ϣ</summary>
        /// <param name="ids">��ɫ��ʶ�������Զ��Ÿ���</param>
        /// <returns></returns>
        public IList<IStandardOrganizationInfo> GetStandardOrganizations(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IStandardOrganizationInfo> list = new List<IStandardOrganizationInfo>();

            IStandardOrganizationInfo item = null;

            for (int i = 0; i < keys.Length; i++)
            {
                item = MembershipManagement.Instance.StandardOrganizationService[keys[i]];

                // ���˶���Ϊ�ջ��߽��õĶ�����
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 属性:GetStandardRoleAccounts(string ids)
        /// <summary>��ȡ��Ӧ�Ľ�ɫ���û���Ϣ������id��ͬ���û�</summary>
        /// <param name="ids">Ⱥ���ı�ʶ�������ö��ŷָ�</param>
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
        // ��׼��ɫ
        // -------------------------------------------------------

        #region 属性:GetStandardRoles(string ids)
        /// <summary>��ȡ��Ӧ�Ľ�ɫ��Ϣ</summary>
        /// <param name="ids">��ɫ��ʶ�������Զ��Ÿ���</param>
        /// <returns></returns>
        public IList<IStandardRoleInfo> GetStandardRoles(string ids)
        {
            string[] keys = ids.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            IList<IStandardRoleInfo> list = new List<IStandardRoleInfo>();

            IStandardRoleInfo item = null;

            for (int i = 0; i < keys.Length; i++)
            {
                item = MembershipManagement.Instance.StandardRoleService[keys[i]];

                // ���˵�����Ϊ�ջ��߽��õĶ�����
                if (item != null && item.Status == 1)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region 属性:GetStandardRoleAccounts(string ids)
        /// <summary>��ȡ��Ӧ�Ľ�ɫ���û���Ϣ������id��ͬ���û�</summary>
        /// <param name="ids">Ⱥ���ı�ʶ�������ö��ŷָ�</param>
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
        //���ߺ���
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

        #region 属性:GetIntersectionAccounts(List<AccountInfo> AccountsA, params List<AccountInfo>[] AccountsB)
        /// <summary>
        /// �õ��û������Ľ����������ɸ���������ͬ���û�
        /// </summary>
        /// <param name="list"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static IList<IAccountInfo> GetIntersectionAccounts(IList<IAccountInfo> list, params IList<IAccountInfo>[] lists)
        {
            //
            // �Ե�һ���û���IList<Ϊ�ȶԶ���
            // ÿ�αȶ���֮�����鸳ֵ���¸�ֵ����һ���û���list.
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

                // ���򽻼�
                list = ToAccounts(result);
            }

            return list;
        }
        #endregion

        #region 属性:GetUnionAccounts(List<AccountInfo> AccountsA, params List<AccountInfo>[] AccountsB)
        /// <summary>�õ��û������Ĳ����������ɸ����������в�ͬ���û�</summary>
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
        // ��ɫ���ĺ���
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

        #region 属性:GetRoleIds(IRoleInfo[] roles)
        /// <summary>���ݽ�ɫ������Ϣ��ȡ���صĽ�ɫ��ʶ</summary>
        /// <param name="roles">��ɫ������Ϣ</param>
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

        #region 属性:GetRoleNames(IRoleInfo[] roles)
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

        #region 属性:GetIntersectionRoles(IRoleInfo[] rolesA, params IRoleInfo[][] rolesB)
        /// <summary>�õ���ɫ�����Ľ����������ɸ���������ͬ�Ľ�ɫ</summary>
        /// <param name="list"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static IList<IRoleInfo> GetIntersectionRoles(IList<IRoleInfo> list, params IList<IRoleInfo>[] lists)
        {
            //
            // �Ե�һ���û���IListΪ�ȶԶ���
            // ÿ�αȶ���֮�����鸳ֵ���¸�ֵ����һ���û���list.
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

        #region 属性:GetUnionRoles(IList<IRoleInfo> list, params IList<IRoleInfo>[] lists)
        /// <summary>�õ��û������Ĳ����������ɸ����������в�ͬ���û�</summary>
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