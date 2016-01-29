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
    using X3Platform.Globalization;
    #endregion

    /// <summary></summary>
    public sealed class RoleWrapper : ContextWrapper
    {
        private IRoleService service = MembershipManagement.Instance.RoleService; // 数据服务

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果.</returns>
        public string Save(XmlDocument doc)
        {
            RoleInfo param = new RoleInfo();

            param = AjaxUtil.Deserialize<RoleInfo>(param, doc);

            // param.ExtensionInformation["AuthorityValue"] = XmlHelper.Fetch("authorityValue", doc);

            param.ExtensionInformation["AccountValue"] = XmlHelper.Fetch("accountValue", doc);
            param.ExtensionInformation["OrganizationUnitValue"] = XmlHelper.Fetch("organizationValue", doc);

            string originalName = XmlHelper.Fetch("originalName", doc);

            string originalGlobalName = XmlHelper.Fetch("originalGlobalName", doc);

            // string authorityText = XmlHelper.Fetch("authorityText", doc);

            string memberText = XmlHelper.Fetch("memberText", doc);

            if (string.IsNullOrEmpty(param.Name))
            {
                return "{message:{\"returnCode\":1,\"value\":\"名称不能为空。\"}}";
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                // 新增

                if (this.service.IsExistGlobalName(param.GlobalName))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"此全局名称已存在。\"}}";
                }

                param.Id = Guid.NewGuid().ToString();
            }
            else
            {
                // 修改

                if (param.GlobalName != originalGlobalName)
                {
                    if (this.service.IsExistGlobalName(param.GlobalName))
                    {
                        return "{message:{\"returnCode\":1,\"value\":\"此全局名称已存在。\"}}";
                    }
                }
            }

            // 重置成员关系
            param.ResetMemberRelations(memberText);

            this.service.Save(param);

            return "{message:{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            this.service.Delete(id);

            return GenericException.Serialize(0, I18n.Strings["msg_delete_success"]);
        }
        #endregion

        // -------------------------------------------------------
        // 查询 
        // -------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            IRoleInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IRoleInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllByAccountId(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAllByAccountId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountId = XmlHelper.Fetch("accountId", doc);

            IList<IRoleInfo> list = this.service.FindAllByAccountId(accountId);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IRoleInfo>(list) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllByProjectId(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("findAllByProjectId")]
        public string FindAllByProjectId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string projectId = XmlHelper.Fetch("projectId", doc);

            IList<IRoleInfo> list = this.service.FindAllByProjectId(projectId);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IRoleInfo>(list) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllWithoutMember(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("findAllWithoutMember")]
        public string FindAllWithoutMember(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            bool includeAllRole = Convert.ToBoolean(XmlHelper.Fetch("includeAllRole", doc));

            IList<IRoleInfo> list = this.service.FindAllWithoutMember(length, includeAllRole);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IRoleInfo>(list) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            // 设置当前用户权限
            if (XmlHelper.Fetch("su", doc) == "1")
            {
                paging.Query.Variables["elevatedPrivileges"] = "1";
            }

            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            int rowCount = -1;

            IList<IRoleInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IRoleInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
            // 兼容 ExtJS 设置
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

            return outString.ToString();
        }
        #endregion

        #region 函数:CreateNewObject(XmlDocument doc)
        /// <summary>创建新的对象</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("createNewObject")]
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string organizationId = XmlHelper.Fetch("organizationId", doc);

            RoleInfo param = new RoleInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.OrganizationUnitId = organizationId;

            param.Status = 1;

            param.ModifiedDate = param.CreatedDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IRoleInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:CheckStandardRoleIsCreated(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        [AjaxMethod("checkStandardRoleIsCreated")]
        public string CheckStandardRoleIsCreated(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // 组织标识
            string organizationId = XmlHelper.Fetch("organizationId", doc);
            // 标准角色类型
            string standardRoleType = XmlHelper.Fetch("standardRoleType", doc);
            // 标准角色标识
            string standardRoleIds = XmlHelper.Fetch("standardRoleIds", doc);

            DataTable table = MembershipManagement.Instance.RoleService.GenerateStandardRoleMappingReport(organizationId, standardRoleType, standardRoleIds);

            outString.Append("{\"data\":[");

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

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:QuickCreateRole(XmlDocument doc)
        /// <summary>创建项目类角色</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        [AjaxMethod("quickCreateRole")]
        public string QuickCreateRole(XmlDocument doc)
        {
            string standardRoleType = XmlHelper.Fetch("standardRoleType", doc);

            string organizationId = XmlHelper.Fetch("organizationId", doc);

            string standardRoleId = XmlHelper.Fetch("standardRoleId", doc);

            string roleName = XmlHelper.Fetch("roleName", doc);

            int resultCode = MembershipManagement.Instance.RoleService.QuickCreateRole(standardRoleType, organizationId, standardRoleId, roleName);

            switch (resultCode)
            {
                case 1:
                    return "{\"message\":{\"returnCode\":1,\"value\":\"已存在此角色名称.\"}}";
                case 2:
                    return "{\"message\":{\"returnCode\":2,\"value\":\"不存在相关标准角色信息.\"}}";
                default:
                    return "{\"message\":{\"returnCode\":0,\"value\":\"角色创建成功。\"}}";
            }
        }
        #endregion

        #region 函数:SetProjectRoleMapping(XmlDocument doc)
        /// <summary>设置两个项目之间的角色及人员映射关系</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        [AjaxMethod("setProjectRoleMapping")]
        public string SetProjectRoleMapping(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // 来源项目标识
            string fromProjectId = XmlHelper.Fetch("fromProjectId", doc);
            // 目标项目标识
            string toProjectId = XmlHelper.Fetch("toProjectId", doc);

            DataTable table = MembershipManagement.Instance.RoleService.SetProjectRoleMapping(fromProjectId, toProjectId);

            outString.Append("{\"data\":[");

            foreach (DataRow row in table.Rows)
            {
                outString.Append("{");
                outString.Append("\"fromProjectOrganizationUnitId\":\"" + row["fromProjectOrganizationUnitId"] + "\",");
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

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Rename(XmlDocument doc)
        /// <summary>重命名角色名称</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        [AjaxMethod("rename")]
        public string Rename(XmlDocument doc)
        {
            string roleId = XmlHelper.Fetch("roleId", doc);

            string roleName = XmlHelper.Fetch("roleName", doc);

            int resultCode = MembershipManagement.Instance.RoleService.Rename(roleId, roleName);

            switch (resultCode)
            {
                case 1:
                    return "{\"message\":{\"returnCode\":1,\"value\":\"已存在此角色名称.\"}}";
                case 2:
                    return "{\"message\":{\"returnCode\":2,\"value\":\"不存在相关标准角色信息.\"}}";
                default:
                    return "{\"message\":{\"returnCode\":0,\"value\":\"重命名修改成功。\"}}";
            }
        }
        #endregion

        #region 函数:SetProjectRoleMapping(XmlDocument doc)
        /// <summary>设置两个项目之间的角色及人员映射关系</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        [AjaxMethod("setRelationRange")]
        public string SetRelationRange(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // 帐号标识
            string accountIds = XmlHelper.Fetch("accountIds", doc);
            // 角色标识
            string roleId = XmlHelper.Fetch("roleId", doc);

            // 清理角色相关的帐号
            MembershipManagement.Instance.RoleService.ClearupRelation(roleId);

            // 添加角色关系
            MembershipManagement.Instance.RoleService.AddRelationRange(accountIds, roleId);

            return "{\"message\":{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion

        #region 函数:ReportOrganizationUnitTableHtml(XmlDocument doc)
        /// <summary>获取组织结构</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        [AjaxMethod("reportOrganizationUnitTableHtml")]
        public string ReportOrganizationUnitTableHtml(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            StringBuilder report = new StringBuilder();

            string corporationId = XmlHelper.Fetch("corporationId", doc);

            IList<IOrganizationUnitInfo> list = MembershipManagement.Instance.OrganizationUnitService.FindAllByParentId(corporationId);

            outString.Append("<table style=\"width:100%;border:1px solid #ccc;\" cellPadding=\"0\" cellSpacing=\"0\" >");
            outString.Append("<tr style=\"background-color:#F2F4F6\" >");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >一级部门</td>");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >二级部门</td>");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >三级部门</td>");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >四级部门</td>");
            outString.Append("<td style=\"width:100px;font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >岗位</td>");
            outString.Append("<td style=\"font-weight:bold;border:1px solid #ccc;padding:4px 8px 4px 8px;\" >人员</td>");
            outString.Append("</tr>");

            outString.Append(ParseOrganizationUnitViewHtml(list, 0));

            outString.Append("</table>");

            return "{\"data\":\"" + outString.ToString() + "\", \"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}";
        }
        #endregion

        #region 函数:ReportRoleTreeView(XmlDocument doc)
        /// <summary>获取角色树信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        [AjaxMethod("reportRoleTreeView")]
        public string ReportRoleTreeView(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string corporationLeaderId = XmlHelper.Fetch("corporationLeaderId", doc);

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            int rowCount = -1;

            IList<IRoleInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":[");

            int index = 0;

            foreach (IRoleInfo item in list)
            {
                index = outString.Length;

                outString.Append(AjaxUtil.Parse<IRoleInfo>(item));
                outString.Append(",");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");
outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
            // 兼容 ExtJS 设置
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

            return outString.ToString();
        }
        #endregion

        private string ParseOrganizationUnitViewHtml(IList<IOrganizationUnitInfo> list, int level)
        {
            StringBuilder outString = new StringBuilder();

            IList<IOrganizationUnitInfo> children = null;

            foreach (IOrganizationUnitInfo item in list)
            {
                children = MembershipManagement.Instance.OrganizationUnitService.FindAllByParentId(item.Id);

                outString.Append(ParseRoleViewHtml(item.Id, item.Name, level + 1));

                outString.Append(ParseOrganizationUnitViewHtml(children, level + 1));
            }

            return outString.ToString();
        }

        private string ParseRoleViewHtml(string organizationId, string organizationName, int level)
        {
            int maxLevel = 4;

            if (level > maxLevel)
                return string.Empty;

            StringBuilder outString = new StringBuilder();

            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAllByOrganizationUnitId(organizationId);

            string text = null;

            string previousOrganizationUnitName = null;

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

                    if (organizationName != previousOrganizationUnitName)
                    {
                        columns[level - 1] = organizationName;
                        previousOrganizationUnitName = organizationName;
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

        /// <summary>获取角色对应的人员信息</summary>
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
