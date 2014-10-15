#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :OrganizationWrapper.cs
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
    using System.Data;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public sealed class RoleWrapper : ContextWrapper
    {
        private IRoleService service = MembershipManagement.Instance.RoleService; // ���ݷ���

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(XmlDocument doc)
        /// <summary>������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������.</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            RoleInfo param = new RoleInfo();

            param = AjaxStorageConvertor.Deserialize<RoleInfo>(param, doc);

            // param.ExtensionInformation["AuthorityValue"] = AjaxStorageConvertor.Fetch("authorityValue", doc);

            param.ExtensionInformation["AccountValue"] = AjaxStorageConvertor.Fetch("accountValue", doc);
            param.ExtensionInformation["OrganizationValue"] = AjaxStorageConvertor.Fetch("organizationValue", doc);

            string originalName = AjaxStorageConvertor.Fetch("originalName", doc);

            string originalGlobalName = AjaxStorageConvertor.Fetch("originalGlobalName", doc);

            // string authorityText = AjaxStorageConvertor.Fetch("authorityText", doc);

            string memberText = AjaxStorageConvertor.Fetch("memberText", doc);

            if (string.IsNullOrEmpty(param.Name))
            {
                return "{message:{\"returnCode\":1,\"value\":\"���Ʋ���Ϊ�ա�\"}}";
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                // ����

                if (this.service.IsExistGlobalName(param.GlobalName))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"��ȫ�������Ѵ��ڡ�\"}}";
                }

                param.Id = Guid.NewGuid().ToString();
            }
            else
            {
                // �޸�

                if (param.GlobalName != originalGlobalName)
                {
                    if (this.service.IsExistGlobalName(param.GlobalName))
                    {
                        return "{message:{\"returnCode\":1,\"value\":\"��ȫ�������Ѵ��ڡ�\"}}";
                    }
                }
            }

            // ���ó�Ա��ϵ
            param.ResetMemberRelations(memberText);

            this.service.Save(param);

            return "{message:{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region 属性:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            this.service.Delete(id);

            return "{message:{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ 
        // -------------------------------------------------------

        #region 属性:FindOne(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = AjaxStorageConvertor.Fetch("id", doc);

            IRoleInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IRoleInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllByAccountId(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string FindAllByAccountId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountId = AjaxStorageConvertor.Fetch("accountId", doc);

            IList<IRoleInfo> list = this.service.FindAllByAccountId(accountId);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IRoleInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllByProjectId(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAllByProjectId")]
        public string FindAllByProjectId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string projectId = AjaxStorageConvertor.Fetch("projectId", doc);

            IList<IRoleInfo> list = this.service.FindAllByProjectId(projectId);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IRoleInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllWithoutMember(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAllWithoutMember")]
        public string FindAllWithoutMember(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(AjaxStorageConvertor.Fetch("length", doc));

            bool includeAllRole = Convert.ToBoolean(AjaxStorageConvertor.Fetch("includeAllRole", doc));

            IList<IRoleInfo> list = this.service.FindAllWithoutMember(length, includeAllRole);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IRoleInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IRoleInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IRoleInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:CreateNewObject(XmlDocument doc)
        /// <summary>�����µĶ���</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("createNewObject")]
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            RoleInfo param = new RoleInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.OrganizationId = organizationId;

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IRoleInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:CheckStandardRoleIsCreated(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("checkStandardRoleIsCreated")]
        public string CheckStandardRoleIsCreated(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // ��֯��ʶ
            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);
            // ��׼��ɫ����
            string standardRoleType = AjaxStorageConvertor.Fetch("standardRoleType", doc);
            // ��׼��ɫ��ʶ
            string standardRoleIds = AjaxStorageConvertor.Fetch("standardRoleIds", doc);

            DataTable table = MembershipManagement.Instance.RoleService.GenerateStandardRoleMappingReport(organizationId, standardRoleType, standardRoleIds);

            outString.Append("{\"ajaxStorage\":[");

            foreach (DataRow row in table.Rows)
            {
                outString.Append("{");
                outString.Append("\"standardRoleId\":\"" + row["standardRoleId"] + "\",");
                outString.Append("\"roleId\":\"" + row["roleId"] + "\",");
                outString.Append("\"roleName\":\"" + row["roleName"] + "\",");
                outString.Append("\"roleIsCreatedValue\":\"" + row["roleIsCreatedValue"] + "\" ");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:QuickCreateRole(XmlDocument doc)
        /// <summary>������Ŀ����ɫ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("quickCreateRole")]
        public string QuickCreateRole(XmlDocument doc)
        {
            string standardRoleType = AjaxStorageConvertor.Fetch("standardRoleType", doc);

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            string standardRoleId = AjaxStorageConvertor.Fetch("standardRoleId", doc);

            string roleName = AjaxStorageConvertor.Fetch("roleName", doc);

            int resultCode = MembershipManagement.Instance.RoleService.QuickCreateRole(standardRoleType, organizationId, standardRoleId, roleName);

            switch (resultCode)
            {
                case 1:
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�Ѵ��ڴ˽�ɫ����.\"}}";
                case 2:
                    return "{\"message\":{\"returnCode\":2,\"value\":\"���������ر�׼��ɫ��Ϣ.\"}}";
                default:
                    return "{\"message\":{\"returnCode\":0,\"value\":\"��ɫ�����ɹ���\"}}";
            }
        }
        #endregion

        #region 属性:SetProjectRoleMapping(XmlDocument doc)
        /// <summary>����������Ŀ֮���Ľ�ɫ����Աӳ����ϵ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("setProjectRoleMapping")]
        public string SetProjectRoleMapping(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // ��Դ��Ŀ��ʶ
            string fromProjectId = AjaxStorageConvertor.Fetch("fromProjectId", doc);
            // Ŀ����Ŀ��ʶ
            string toProjectId = AjaxStorageConvertor.Fetch("toProjectId", doc);

            DataTable table = MembershipManagement.Instance.RoleService.SetProjectRoleMapping(fromProjectId, toProjectId);

            outString.Append("{\"ajaxStorage\":[");

            foreach (DataRow row in table.Rows)
            {
                outString.Append("{");
                outString.Append("\"fromProjectOrganizationId\":\"" + row["fromProjectOrganizationId"] + "\",");
                outString.Append("\"fromProjectRoleId\":\"" + row["fromProjectRoleId"] + "\",");
                outString.Append("\"fromProjectRoleName\":\"" + row["fromProjectRoleName"] + "\",");
                outString.Append("\"fromProjectRoleAccountValue\":\"" + row["fromProjectRoleAccountValue"] + "\",");
                outString.Append("\"fromProjectRoleStandardRoleId\":\"" + row["fromProjectRoleStandardRoleId"] + "\",");
                outString.Append("\"toProjectRoleId\":\"" + row["toProjectRoleId"] + "\",");
                outString.Append("\"toProjectRoleName\":\"" + row["toProjectRoleName"] + "\",");
                outString.Append("\"toProjectRoleAccountValue\":\"" + row["toProjectRoleAccountValue"] + "\" ");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:Rename(XmlDocument doc)
        /// <summary>��������ɫ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("rename")]
        public string Rename(XmlDocument doc)
        {
            string roleId = AjaxStorageConvertor.Fetch("roleId", doc);

            string roleName = AjaxStorageConvertor.Fetch("roleName", doc);

            int resultCode = MembershipManagement.Instance.RoleService.Rename(roleId, roleName);

            switch (resultCode)
            {
                case 1:
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�Ѵ��ڴ˽�ɫ����.\"}}";
                case 2:
                    return "{\"message\":{\"returnCode\":2,\"value\":\"���������ر�׼��ɫ��Ϣ.\"}}";
                default:
                    return "{\"message\":{\"returnCode\":0,\"value\":\"�������޸ĳɹ���\"}}";
            }
        }
        #endregion

        #region 属性:SetProjectRoleMapping(XmlDocument doc)
        /// <summary>����������Ŀ֮���Ľ�ɫ����Աӳ����ϵ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("setRelationRange")]
        public string SetRelationRange(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // �ʺű�ʶ
            string accountIds = AjaxStorageConvertor.Fetch("accountIds", doc);
            // ��ɫ��ʶ
            string roleId = AjaxStorageConvertor.Fetch("roleId", doc);

            // ������ɫ���ص��ʺ�
            MembershipManagement.Instance.RoleService.ClearupRelation(roleId);

            // ���ӽ�ɫ��ϵ
            MembershipManagement.Instance.RoleService.AddRelationRange(accountIds, roleId);

            return "{\"message\":{\"returnCode\":0,\"value\":\"���óɹ���\"}}";
        }
        #endregion

        #region 属性:ReportOrganizationTableHtml(XmlDocument doc)
        /// <summary>��ȡ��֯�ṹ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("reportOrganizationTableHtml")]
        public string ReportOrganizationTableHtml(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            StringBuilder report = new StringBuilder();

            string corporationId = AjaxStorageConvertor.Fetch("corporationId", doc);

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(corporationId);

            outString.Append("<table style=\"width:100%;border:1px solid #ccc;\" cellPadding=\"0\" cellSpacing=\"0\" >");
            outString.Append("<tr style=\"background-color:#F2F4F6\" >");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >һ������</td>");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >��������</td>");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >��������</td>");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >�ļ�����</td>");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >��λ</td>");
            outString.Append("<td style=\"font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >��Ա</td>");
            outString.Append("</tr>");

            outString.Append(ParseOrganizationViewHtml(list, 0));

            outString.Append("</table>");

            return "{\"ajaxStorage\":\"" + outString.ToString() + "\", \"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}";
        }
        #endregion

        #region 属性:ReportRoleTreeView(XmlDocument doc)
        /// <summary>��ȡ��ɫ����Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns> 
        [AjaxMethod("reportRoleTreeView")]
        public string ReportRoleTreeView(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string corporationLeaderId = AjaxStorageConvertor.Fetch("corporationLeaderId", doc);

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IRoleInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":[");

            int index = 0;

            foreach (IRoleInfo item in list)
            {
                index = outString.Length;

                outString.Append(AjaxStorageConvertor.Parse<IRoleInfo>(item));
                outString.Append(",");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        private string ParseOrganizationViewHtml(IList<IOrganizationInfo> list, int level)
        {
            StringBuilder outString = new StringBuilder();

            IList<IOrganizationInfo> children = null;

            foreach (IOrganizationInfo item in list)
            {
                children = MembershipManagement.Instance.OrganizationService.FindAllByParentId(item.Id);

                outString.Append(ParseRoleViewHtml(item.Id, item.Name, level + 1));

                outString.Append(ParseOrganizationViewHtml(children, level + 1));
            }

            return outString.ToString();
        }

        private string ParseRoleViewHtml(string organizationId, string organizationName, int level)
        {
            int maxLevel = 4;

            if (level > maxLevel)
                return string.Empty;

            StringBuilder outString = new StringBuilder();

            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAllByOrganizationId(organizationId);

            string text = null;

            string previousOrganizationName = null;

            string[] columns = new string[maxLevel + 2];

            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = "&nbsp;";
            }

            if (list.Count == 0)
            {
                columns[level - 1] = organizationName;

                text = string.Empty;

                for (int i = 0; i < columns.Length; i++)
                {
                    text += "<td style=\"border:1px solid #ccc;padding:4px 8px 4px 8px;\" >" + columns[i] + "</td>";
                }

                outString.Append("<tr>" + text + "</tr>");
            }
            else
            {
                foreach (IRoleInfo role in list)
                {
                    text = string.Empty;

                    columns[4] = role.Name;

                    columns[5] = ParseAccountViewHtml(role.Id);

                    if (organizationName != previousOrganizationName)
                    {
                        columns[level - 1] = organizationName;
                        previousOrganizationName = organizationName;
                    }
                    else
                    {
                        columns[level - 1] = "&nbsp;";
                    }

                    for (int i = 0; i < columns.Length; i++)
                    {
                        text += "<td style=\"border:1px solid #ccc;padding:4px 8px 4px 8px;\" >" + columns[i] + "</td>";
                    }

                    outString.Append("<tr>" + text + "</tr>");
                }
            }

            return outString.ToString();
        }

        /// <summary>��ȡ��ɫ��Ӧ����Ա��Ϣ</summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private string ParseAccountViewHtml(string roleId)
        {
            StringBuilder outString = new StringBuilder();

            IList<IAccountInfo> list = MembershipManagement.Instance.AccountService.FindAllByRoleId(roleId);

            if (list.Count > 0)
            {
                foreach (IAccountInfo item in list)
                {
                    outString.Append(item.Name + "(" + item.LoginName + "); ");
                }

                return outString.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
