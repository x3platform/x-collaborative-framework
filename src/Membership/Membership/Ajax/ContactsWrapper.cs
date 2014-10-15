#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :AccountWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Configuration;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public sealed class ContactsWrapper : ContextWrapper
    {
        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindAll(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();
            // ��ϵ������
            ContactType contactType = (ContactType)Enum.Parse(typeof(ContactType), AjaxStorageConvertor.Fetch("contactType", doc));
            // ��������ֹ�Ķ���
            int includeProhibited = Convert.ToInt32(AjaxStorageConvertor.Fetch("includeProhibited", doc));
            // ��Ȩ�������ƹؼ���ƥ��
            string key = AjaxStorageConvertor.Fetch("key", doc);

            string whereClause = null;

            outString.Append("{\"ajaxStorage\":[");

            if ((contactType & ContactType.Account) == ContactType.Account)
            {
                whereClause = string.Format("( ( T.Name LIKE ##%{0}%## OR T.LoginName LIKE ##%{0}%## ) {1} )", key, (includeProhibited == 1 ? string.Empty : "AND Status = 1"));

                outString.Append(FormatAccount(MembershipManagement.Instance.AccountService.FindAll(whereClause), includeProhibited));
            }

            if ((contactType & ContactType.Organization) == ContactType.Organization)
            {
                whereClause = string.Format("( T.Name LIKE ##%{0}%## {1} )", key, (includeProhibited == 1 ? string.Empty : "AND Status = 1"));

                outString.Append(FormatOrganization(MembershipManagement.Instance.OrganizationService.FindAll(whereClause), includeProhibited));
            }

            if ((contactType & ContactType.Role) == ContactType.Role)
            {
                whereClause = string.Format("( T.Name LIKE ##%{0}%## {1} )", key, (includeProhibited == 1 ? string.Empty : "AND Status = 1"));

                outString.Append(FormatRole(MembershipManagement.Instance.RoleService.FindAll(whereClause), includeProhibited));
            }

            if ((contactType & ContactType.Group) == ContactType.Group)
            {
                whereClause = string.Format("( T.Name LIKE ##%{0}%## {1} )", key, (includeProhibited == 1 ? string.Empty : "AND Status = 1"));

                outString.Append(FormatGroup(MembershipManagement.Instance.GroupService.FindAll(whereClause), includeProhibited));
            }

            // ��׼��֯
            if ((contactType & ContactType.StandardOrganization) == ContactType.StandardOrganization)
            {
                whereClause = string.Format("( T.Name LIKE ##%{0}%## {1} )", key, (includeProhibited == 1 ? string.Empty : "AND Status = 1"));

                outString.Append(FormatStandardOrganization(MembershipManagement.Instance.StandardOrganizationService.FindAll(whereClause)));
            }

            if ((contactType & ContactType.StandardRole) == ContactType.StandardRole)
            {
                whereClause = string.Format("( T.Name LIKE ##%{0}%## {1} )", key, (includeProhibited == 1 ? string.Empty : "AND ( Status = 1 AND T.Lock = 1 )"));

                outString.Append(FormatStandardRole(MembershipManagement.Instance.StandardRoleService.FindAll(whereClause)));
            }

            if ((contactType & ContactType.GeneralRole) == ContactType.GeneralRole)
            {
                whereClause = string.Format("( T.Name LIKE ##%{0}%## {1} )", key, (includeProhibited == 1 ? string.Empty : "AND Status = 1"));

                outString.Append(FormatGeneralRole(MembershipManagement.Instance.GeneralRoleService.FindAll(whereClause)));
            }

            if ((contactType & ContactType.Contact) == ContactType.Contact)
            {
                //
                // ��ϵ��[δ����]
                //

                //whereClause = string.Format(" T.Name LIKE ##%{0}%## ", key);

                //if (KernelConfigurationView.Instance.RuntimePattern == "Longfor")
                //{
                //    whereClause = string.Format(" T.PostName LIKE ##%{0}%## ", key);
                //}

                //outString.Append(FormatRole(MembershipManagement.Instance.RoleService.FindAll(whereClause)));
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("],");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:View(XmlDocument doc)
        /// <summary>Ԥ����ɫ����֯��Ӧ��ʵ����Ա</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("view")]
        public string View(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            IList<IAccountInfo> list = null;

            string key = AjaxStorageConvertor.Fetch("key", doc);

            string[] keyArray = key.Split(',');

            string[] temp = null;

            ContactType contactType;

            int viewCount = 0;

            int maxViewCount = 5;

            foreach (string keyItem in keyArray)
            {
                temp = keyItem.Split('#');

                if (temp.Length != 3)
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"��ѯ������ʽ����ȷ��\"}}";
                }

                // ���α��ܽ�ɫ��Ա��Ϣ
                if (MembershipConfigurationView.Instance.Configuration.Keys["ProhibitedPreviewObjects"].Value.IndexOf(temp[2]) > -1)
                {
                    return "{\"message\":{\"returnCode\":0,\"value\":\"\"}}";
                }

                switch (temp[0].ToLower())
                {
                    case "account":
                        return "{\"message\":{\"returnCode\":0,\"value\":\"\"}}";

                    case "organization":
                        contactType = ContactType.Organization;
                        break;

                    case "role":
                    default:
                        contactType = ContactType.Role;
                        break;

                }

                if (contactType == ContactType.Organization)
                {
                    list = MembershipManagement.Instance.AccountService.FindAllByOrganizationId(temp[1]);

                    foreach (IAccountInfo item in list)
                    {
                        if (item.Status == 0)
                            continue;

                        outString.AppendFormat("{0};", item.Name);

                        viewCount++;

                        if (viewCount == maxViewCount)
                        {
                            return "{\"message\":{\"returnCode\":0,\"value\":\"" + StringHelper.ToSafeJson(outString.ToString()) + "...\"}}";
                        }
                    }
                }

                if (contactType == ContactType.Role)
                {
                    list = MembershipManagement.Instance.AccountService.FindAllByRoleId(temp[1]);

                    foreach (IAccountInfo item in list)
                    {
                        if (item.Status == 0)
                            continue;

                        outString.AppendFormat("{0};", item.Name);

                        viewCount++;

                        if (viewCount == maxViewCount)
                        {
                            return "{\"message\":{\"returnCode\":0,\"value\":\"" + StringHelper.ToSafeJson(outString.ToString()) + "...\"}}";
                        }
                    }
                }
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + StringHelper.ToSafeJson(outString.ToString()) + "\"}}";
        }
        #endregion

        #region 属性:FindAllByOrganizationId(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAllByOrganizationId")]
        public string FindAllByOrganizationId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // ��ϵ������
            ContactType contactType = (ContactType)Enum.Parse(typeof(ContactType), AjaxStorageConvertor.Fetch("contactType", doc));
            // ��������ֹ�Ķ���
            int includeProhibited = Convert.ToInt32(AjaxStorageConvertor.Fetch("includeProhibited", doc));

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            // �Զ�ת�����ҵĹ�˾
            if (organizationId == "20000000-0000-0000-0000-000000000000")
            {
                IAccountInfo account = KernelContext.Current.User;

                organizationId = MembershipManagement.Instance.MemberService[account.Id].Corporation.Id;
            }

            // 0 ȫ�� 1 2 4 8;

            outString.Append("{\"ajaxStorage\":[");

            if ((contactType & ContactType.Organization) == ContactType.Organization)
            {
                outString.Append(FormatOrganization(MembershipManagement.Instance.OrganizationService[organizationId]));

                outString.Append(FormatOrganization(MembershipManagement.Instance.OrganizationService.FindAllByParentId(organizationId), includeProhibited));
            }

            if ((contactType & ContactType.AssignedJob) == ContactType.AssignedJob)
            {
                outString.Append(FormatAssignedJob(MembershipManagement.Instance.AssignedJobService.FindAllByOrganizationId(organizationId), includeProhibited));
            }

            if ((contactType & ContactType.Role) == ContactType.Role)
            {
                outString.Append(FormatRole(MembershipManagement.Instance.RoleService.FindAllByOrganizationId(organizationId), includeProhibited));
            }

            if ((contactType & ContactType.Account) == ContactType.Account)
            {
                outString.Append(FormatAccount(MembershipManagement.Instance.AccountService.FindAllByOrganizationId(organizationId, true), includeProhibited));
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("],");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllByStandardOrganizationId(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAllByStandardOrganizationId")]
        public string FindAllByStandardOrganizationId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // ��ϵ������
            ContactType contactType = (ContactType)Enum.Parse(typeof(ContactType), AjaxStorageConvertor.Fetch("contactType", doc));
            // ��������ֹ�Ķ���
            int includeProhibited = Convert.ToInt32(AjaxStorageConvertor.Fetch("includeProhibited", doc));

            string standardOrganizationId = AjaxStorageConvertor.Fetch("standardOrganizationId", doc);

            // 0 ȫ�� 1 2 4 8;

            outString.Append("{\"ajaxStorage\":[");

            if ((contactType & ContactType.StandardOrganization) == ContactType.StandardOrganization)
            {
                outString.Append(FormatStandardOrganization(MembershipManagement.Instance.StandardOrganizationService[standardOrganizationId]));

                outString.Append(FormatStandardOrganization(MembershipManagement.Instance.StandardOrganizationService.FindAllByParentId(standardOrganizationId)));
            }

            if ((contactType & ContactType.StandardRole) == ContactType.StandardRole)
            {
                outString.Append(FormatStandardRole(MembershipManagement.Instance.StandardRoleService.FindAllByStandardOrganizationId(standardOrganizationId)));
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("],");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllByGroupNodeId(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAllByGroupNodeId")]
        public string FindAllByGroupNodeId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string groupType = AjaxStorageConvertor.Fetch("groupType", doc);
            // ��������ֹ�Ķ���
            int includeProhibited = Convert.ToInt32(AjaxStorageConvertor.Fetch("includeProhibited", doc));

            string groupTreeNodeId = AjaxStorageConvertor.Fetch("groupTreeNodeId", doc);

            // 0 ȫ�� 1 2 4 8;

            outString.Append("{\"ajaxStorage\":[");

            switch (groupType)
            {
                case "group":
                    outString.Append(FormatGroup(MembershipManagement.Instance.GroupService.FindAllByGroupTreeNodeId(groupTreeNodeId), includeProhibited));
                    break;
                case "general-role":
                    outString.Append(FormatGeneralRole(MembershipManagement.Instance.GeneralRoleService.FindAllByGroupTreeNodeId(groupTreeNodeId)));
                    break;
                case "standard-role":
                    IList<IStandardOrganizationInfo> standardOrganizations = null;

                    if (groupTreeNodeId.IndexOf("[GroupTreeNode]") == 0)
                    {
                        string whereClause = string.Format(" GroupTreeNodeId = ##{0}## AND ( ParentId IS NULL OR ParentId = ##00000000-0000-0000-0000-000000000000## )  ", groupTreeNodeId.Replace("[GroupTreeNode]", ""));

                        standardOrganizations = MembershipManagement.Instance.StandardOrganizationService.FindAll(whereClause);
                    }
                    else
                    {
                        standardOrganizations = MembershipManagement.Instance.StandardOrganizationService.FindAllByParentId(groupTreeNodeId.Replace("[StandardRole]", ""));
                    }

                    outString.Append(FormatStandardOrganization(standardOrganizations));

                    break;
                case "standard-general-role":
                    outString.Append(FormatStandardGeneralRole(MembershipManagement.Instance.StandardGeneralRoleService.FindAllByGroupTreeNodeId(groupTreeNodeId)));
                    break;
                case "workflow-role":
                    outString.Append(FormatWorkflowRole(groupTreeNodeId));
                    break;
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("],");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatOrganization(IList<IOrganizationInfo> list, int includeProhibited)
        /// <summary>��ʽ������</summary>
        /// <param name="list"></param>
        /// <param name="includeProhibited"></param>
        /// <returns>���ز�������</returns>
        private string FormatOrganization(IList<IOrganizationInfo> list, int includeProhibited)
        {
            StringBuilder outString = new StringBuilder();

            // ����
            foreach (IOrganizationInfo item in list)
            {
                // ���˽��õĶ���
                if (includeProhibited == 0 && item.Status == 0)
                {
                    continue;
                }

                // ����ȫ������Ϊ�յĶ���
                if (string.IsNullOrEmpty(item.GlobalName))
                    continue;

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[����]" + StringHelper.ToSafeJson(item.GlobalName) + "\",");
                outString.Append("\"type\":\"organization\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatOrganization(IOrganizationInfo item)
        /// <summary>��ʽ������</summary>
        /// <param name="item"></param>
        /// <returns>���ز�������</returns>
        private string FormatOrganization(IOrganizationInfo item)
        {
            StringBuilder outString = new StringBuilder();

            // ���˽��õĶ���
            if (item.Status == 0) { return string.Empty; }

            // ����ȫ������Ϊ�յĶ���
            if (string.IsNullOrEmpty(item.GlobalName))
                return string.Empty;

            outString.Append("{");
            outString.Append("\"id\":\"" + item.Id + "\",");
            outString.Append("\"name\":\"[����]" + StringHelper.ToSafeJson(item.GlobalName) + "\",");
            outString.Append("\"type\":\"organization\" ");
            outString.Append("},");

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatRole(IList<IAssignedJobInfo> list, int includeProhibited)
        /// <summary>��ʽ������</summary>
        /// <param name="list">�б�</param>
        /// <param name="includeProhibited">�Ƿ����������ö���</param>
        /// <returns>���ز�������</returns>
        private string FormatAssignedJob(IList<IAssignedJobInfo> list, int includeProhibited)
        {
            StringBuilder outString = new StringBuilder();

            // ��λ
            foreach (IAssignedJobInfo item in list)
            {
                // ���˽��õĶ���
                if (includeProhibited == 0 && item.Status == 0) { continue; }

                // ����ȫ������Ϊ�յĶ���
                if (string.IsNullOrEmpty(item.Name)) { continue; }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[��λ]" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"type\":\"assignedJob\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatRole(IList<IRoleInfo> list, int includeProhibited)
        /// <summary>��ʽ������</summary>
        /// <param name="list">�б�</param>
        /// <param name="includeProhibited">�Ƿ����������ö���</param>
        /// <returns>���ز�������</returns>
        private string FormatRole(IList<IRoleInfo> list, int includeProhibited)
        {
            StringBuilder outString = new StringBuilder();

            // ����
            foreach (IRoleInfo item in list)
            {
                // ���˽��õĶ���
                if (includeProhibited == 0 && item.Status == 0)
                {
                    continue;
                }

                // ����ȫ������Ϊ�յĶ���
                if (string.IsNullOrEmpty(item.GlobalName))
                {
                    continue;
                }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[��ɫ]" + StringHelper.ToSafeJson(item.GlobalName) + "\",");
                outString.Append("\"type\":\"role\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatAccount(IList<IAccountInfo> list, int includeProhibited)
        /// <summary>��ʽ������</summary>
        /// <param name="list">�б�</param>
        /// <param name="includeProhibited">�Ƿ����������ö���</param>
        /// <returns>���ز�������</returns>
        private string FormatAccount(IList<IAccountInfo> list, int includeProhibited)
        {
            StringBuilder outString = new StringBuilder();

            // ��Ա����
            foreach (IAccountInfo item in list)
            {
                // ���˽��õĶ���
                if (includeProhibited == 0 && item.Status == 0)
                {
                    continue;
                }

                // ����ȫ������Ϊ�յĶ���
                if (string.IsNullOrEmpty(item.GlobalName))
                {
                    continue;
                }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[��Ա]" + StringHelper.ToSafeJson(item.GlobalName) + "\",");
                outString.Append("\"type\":\"account\",");
                outString.Append("\"status\":\"" + item.Status + "\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatGroup(IList<GroupInfo> list, int includeProhibited)
        /// <summary>��ʽ������</summary>
        /// <param name="list">�б�</param>
        /// <param name="includeProhibited">�Ƿ����������ö���</param>
        /// <returns>���ز�������</returns>
        private string FormatGroup(IList<IGroupInfo> list, int includeProhibited)
        {
            StringBuilder outString = new StringBuilder();

            // ����
            foreach (IGroupInfo item in list)
            {
                // ���˽��õĶ���
                if (includeProhibited == 0 && item.Status == 0)
                {
                    continue;
                }

                // ����ȫ������Ϊ�յĶ���
                if (string.IsNullOrEmpty(item.GlobalName))
                {
                    continue;
                }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[Ⱥ��]" + StringHelper.ToSafeJson(item.GlobalName) + "\",");
                outString.Append("\"type\":\"group\",");
                outString.Append("\"status\":\"" + item.Status + "\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatGeneralRole(IList<GeneralRoleInfo> list)
        /// <summary>��ʽ������</summary>
        /// <param name="list">�б�</param>
        /// <returns>���ز�������</returns>
        private string FormatGeneralRole(IList<GeneralRoleInfo> list)
        {
            StringBuilder outString = new StringBuilder();

            // ����
            foreach (GeneralRoleInfo item in list)
            {
                // ���˽��õĶ���
                if (item.Status == 0) { continue; }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[ͨ�ý�ɫ]" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"type\":\"generalRole\",");
                outString.Append("\"status\":\"" + item.Status + "\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatStandardOrganization(IList<IStandardOrganizationInfo> list)
        /// <summary>��ʽ������</summary>
        /// <param name="list"></param>
        /// <returns>���ز�������</returns>
        private string FormatStandardOrganization(IList<IStandardOrganizationInfo> list)
        {
            StringBuilder outString = new StringBuilder();

            // ����
            foreach (IStandardOrganizationInfo item in list)
            {
                // ���˽��õĶ���
                if (item.Status == 0)
                {
                    continue;
                }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[��׼����]" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"type\":\"standardOrganization\",");
                outString.Append("\"status\":\"" + item.Status + "\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatStandardOrganization(IStandardOrganizationInfo item)
        /// <summary>��ʽ������</summary>
        /// <param name="item"></param>
        /// <returns>���ز�������</returns>
        private string FormatStandardOrganization(IStandardOrganizationInfo item)
        {
            StringBuilder outString = new StringBuilder();

            // ���˽��õĶ���
            if (item.Status == 0)
                return string.Empty;

            outString.Append("{");
            outString.Append("\"id\":\"" + item.Id + "\",");
            outString.Append("\"name\":\"[��׼����]" + StringHelper.ToSafeJson(item.Name) + "\",");
            outString.Append("\"type\":\"standardOrganization\" ");
            outString.Append("},");

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatStandardRole(IList<IStandardRoleInfo> list)
        /// <summary>��ʽ������</summary>
        /// <param name="list"></param>
        /// <returns>���ز�������</returns>
        private string FormatStandardRole(IList<IStandardRoleInfo> list)
        {
            StringBuilder outString = new StringBuilder();

            // ����
            foreach (IStandardRoleInfo item in list)
            {
                // ���˽��õĶ���
                if (item.Lock == 0 || item.Status == 0)
                    continue;

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[��׼��ɫ]" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"type\":\"standardRole\",");
                outString.Append("\"status\":\"" + item.Status + "\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatStandardGeneralRole(IList<IStandardGeneralRoleInfo> list)
        /// <summary>��ʽ������</summary>
        /// <param name="list"></param>
        /// <returns>���ز�������</returns>
        private string FormatStandardGeneralRole(IList<IStandardGeneralRoleInfo> list)
        {
            StringBuilder outString = new StringBuilder();

            // ����
            foreach (IStandardGeneralRoleInfo item in list)
            {
                // ���˽��õĶ���
                if (item.Status == 0) { continue; }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"[��׼ͨ�ý�ɫ]" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"type\":\"standardGeneralRole\",");
                outString.Append("\"status\":\"" + item.Status + "\" ");
                outString.Append("},");
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:FormatWorkflowRole(string groupTreeNodeId)
        /// <summary>��ʽ������</summary>
        /// <param name="groupTreeNodeId"></param>
        /// <returns>���ز�������</returns>
        private string FormatWorkflowRole(string groupTreeNodeId)
        {
            StringBuilder outString = new StringBuilder();

            switch (groupTreeNodeId)
            {
                case "60000000-0000-0000-0001-000000000000":

                    outString.Append("{");
                    outString.Append("\"id\":\"00000000-0000-0000-0000-000000000000\",");
                    outString.Append("\"name\":\"[���̽�ɫ]���̷�����\",");
                    outString.Append("\"type\":\"initiator\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0001-000000000090\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["PriorityLeader90DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]���Ų��Ÿ�����/������˾������(�ż������˽�ɫ)\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["PriorityLeader90DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"priorityLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");

                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0001-000000000080\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["PriorityLeader80DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]�������ĸ�����/����ְ�ܲ��Ÿ�����(�˼������˽�ɫ)\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["PriorityLeader80DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"priorityLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    break;

                case "60000000-0000-0000-0002-000000000000":

                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0002-000000000001\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["ForwardLeader01DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]��˾������(����һ���쵼)\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["ForwardLeader01DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"forwardLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0002-000000000002\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["ForwardLeader02DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]���Ÿ�����(���������쵼)\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["ForwardLeader02DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"forwardLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0002-000000000003\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["ForwardLeader03DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]���ĸ�����(���������쵼)\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["ForwardLeader03DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"forwardLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0002-000000000004\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["ForwardLeader04DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]С�鸺����(�����ļ��쵼)\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["ForwardLeader04DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"forwardLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    break;

                case "60000000-0000-0000-0003-000000000000":

                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0003-000000000001\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["BackwardLeader01DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]ֱ���쵼(����һ���쵼)\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["BackwardLeader01DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"backwardLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0003-000000000002\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["BackwardLeader02DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]���������쵼\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["BackwardLeader02DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"backwardLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0003-000000000003\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["BackwardLeader03DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]���������쵼\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["BackwardLeader03DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"backwardLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    outString.Append("{");
                    outString.Append("\"id\":\"60000000-0000-0000-0003-000000000004\",");
                    if (MembershipConfigurationView.Instance.Configuration.Keys["BackwardLeader04DisplayName"] == null)
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]�����ļ��쵼\",");
                    }
                    else
                    {
                        outString.Append("\"name\":\"[���̽�ɫ]" + MembershipConfigurationView.Instance.Configuration.Keys["BackwardLeader04DisplayName"].Value + "\",");
                    }
                    outString.Append("\"type\":\"backwardLeader\",");
                    outString.Append("\"status\":\"1\" ");
                    outString.Append("},");
                    break;

                default:
                    break;
            }

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // ���β˵�
        // -------------------------------------------------------

        #region 属性:GetTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getTreeView")]
        public string GetTreeView(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            outString.Append(this.GetTreeView(organizationId));

            if (outString.ToString().Substring(outString.Length - 2, 2) == "]}")
            {
                outString = outString.Remove(outString.Length - 2, 2);
            }

            outString.Append("],\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetTreeView(string organizationId)
        /// <summary></summary>
        public string GetTreeView(string organizationId)
        {
            StringBuilder outString = new StringBuilder();

            string childNodes = string.Empty;

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(organizationId);

            outString.Append("{\"ajaxStorage\":[");

            foreach (IOrganizationInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");

                childNodes = GetTreeView(item.Id);

                childNodes = childNodes.Replace("{\"ajaxStorage\":[", "[").Replace("]}", "]");

                outString.Append("\"childNodes\":" + childNodes + ", ");

                outString.Append("\"status\":\"" + item.Status + "\" ");

                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("]}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getDynamicTreeView")]
        public string GetDynamicTreeView(XmlDocument doc)
        {
            // �����ֶ�
            string tree = AjaxStorageConvertor.Fetch("tree", doc);
            string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

            // ��������
            string treeViewType = AjaxStorageConvertor.Fetch("treeViewType", doc);
            string treeViewId = AjaxStorageConvertor.Fetch("treeViewId", doc);
            string treeViewName = AjaxStorageConvertor.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = AjaxStorageConvertor.Fetch("treeViewRootTreeNodeId", doc);

            string url = AjaxStorageConvertor.Fetch("url", doc);

            // ���οؼ�Ĭ�ϸ��ڵ���ʶΪ0, ��Ҫ���⴦��.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"ajaxStorage\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            IAccountInfo account = KernelContext.Current.User;

            switch (treeViewType)
            {
                case "my-corporation":

                    IMemberInfo member = MembershipManagement.Instance.MemberService[account.Id];

                    if (member.Corporation == null)
                    {
                        parentId = "00000000-0000-0000-0000-000000000001";
                    }
                    else
                    {
                        treeViewRootTreeNodeId = MembershipManagement.Instance.MemberService[account.Id].Corporation.Id;
                    }

                    if (parentId == "20000000-0000-0000-0000-000000000000")
                    {
                        parentId = treeViewRootTreeNodeId;
                    }

                    outString.Append(GetTreeViewWithOrganization(parentId, url, treeViewRootTreeNodeId));
                    break;

                case "group":
                case "general-role":
                    outString.Append(GetTreeViewWithGroupTreeNode(parentId, url, treeViewRootTreeNodeId));
                    break;
                case "standard-organization":
                    outString.Append(GetTreeViewWithStandardOrganizationTreeNode(parentId, url, treeViewRootTreeNodeId));
                    break;
                case "standard-role":
                    outString.Append(GetTreeViewWithStandardOrganizationTreeNode(parentId, url, treeViewRootTreeNodeId));
                    break;
                case "standard-general-role":
                    outString.Append(GetTreeViewWithGroupTreeNode(parentId, url, treeViewRootTreeNodeId));
                    break;
                case "workflow-role":
                    outString.Append(GetTreeViewWithWorkflowRole(parentId, url, treeViewRootTreeNodeId));
                    break;

                case "organization":
                default:
                    outString.Append(GetTreeViewWithOrganization(parentId, url, treeViewRootTreeNodeId));
                    break;
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        private string GetTreeViewWithGroupTreeNode(string parentId, string url, string treeViewRootTreeNodeId)
        {
            IList<GroupTreeNodeInfo> list = MembershipManagement.Instance.GroupTreeNodeService.FindAllByParentId(parentId);

            StringBuilder outString = new StringBuilder();

            foreach (GroupTreeNodeInfo item in list)
            {
                if (item.Status == 0) { continue; }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.ParentId == treeViewRootTreeNodeId ? "0" : item.ParentId) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Id).Replace("{treeNodeName}", item.Name)) + "\",");
                outString.Append("\"target\":\"_self\"");
                outString.Append("},");
            }

            return outString.ToString();
        }

        private string GetTreeViewWithWorkflowRole(string parentId, string url, string treeViewRootTreeNodeId)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("{");
            outString.Append("\"id\":\"60000000-0000-0000-0001-000000000000\",");
            outString.Append("\"parentId\":\"0\",");
            outString.Append("\"name\":\"���̽�ɫ\",");
            outString.Append("\"title\":\"���̽�ɫ\",");
            outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", "60000000-0000-0000-0001-000000000000")) + "\",");
            outString.Append("\"target\":\"_self\"");
            outString.Append("},");

            outString.Append("{");
            outString.Append("\"id\":\"60000000-0000-0000-0002-000000000000\",");
            outString.Append("\"parentId\":\"0\",");
            outString.Append("\"name\":\"�����쵼\",");
            outString.Append("\"title\":\"�����쵼\",");
            outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", "60000000-0000-0000-0002-000000000000")) + "\",");
            outString.Append("\"target\":\"_self\"");
            outString.Append("},");

            outString.Append("{");
            outString.Append("\"id\":\"60000000-0000-0000-0003-000000000000\",");
            outString.Append("\"parentId\":\"0\",");
            outString.Append("\"name\":\"�����쵼\",");
            outString.Append("\"title\":\"�����쵼\",");
            outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", "60000000-0000-0000-0003-000000000000")) + "\",");
            outString.Append("\"target\":\"_self\"");
            outString.Append("}");

            return outString.ToString();
        }

        private string GetTreeViewWithOrganization(string parentId, string url, string treeViewRootTreeNodeId)
        {
            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            StringBuilder outString = new StringBuilder();

            foreach (IOrganizationInfo item in list)
            {
                if (item.Status == 0) { continue; }

                //if (AppsSecurity.IsAdministrator(KernelContext.Current.User, "Membership")) 
                //&& item.Name == "��������")
                //    continue;

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.Parent.Id == treeViewRootTreeNodeId ? "0" : item.Parent.Id) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Id).Replace("{treeNodeName}", item.Name)) + "\",");
                outString.Append("\"target\":\"_self\"");
                outString.Append("},");
            }

            return outString.ToString();
        }

        private string GetTreeViewWithStandardOrganizationTreeNode(string parentId, string url, string treeViewRootTreeNodeId)
        {
            StringBuilder outString = new StringBuilder();

            IList<IStandardOrganizationInfo> list = MembershipManagement.Instance.StandardOrganizationService.FindAllByParentId(parentId);

            foreach (IStandardOrganizationInfo item in list)
            {
                if (item.Status == 0) { continue; }

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.ParentId == treeViewRootTreeNodeId ? "0" : item.ParentId) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Id).Replace("{treeNodeName}", item.Name)) + "\",");
                outString.Append("\"target\":\"_self\"");
                outString.Append("},");
            }

            return outString.ToString();
        }
    }
}