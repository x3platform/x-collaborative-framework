namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    using System.Data;
    using X3Platform.Globalization;
    #endregion

    /// <summary></summary>
    public class StandardGeneralRoleWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IStandardGeneralRoleService service = MembershipManagement.Instance.StandardGeneralRoleService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            IStandardGeneralRoleInfo param = new StandardGeneralRoleInfo();

            param = (StandardGeneralRoleInfo)AjaxUtil.Deserialize(param, doc);

            this.service.Save(param);

            return GenericException.Serialize(0, I18n.Strings["msg_save_success"]);
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string ids = XmlHelper.Fetch("ids", doc);

            this.service.Delete(ids);

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
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            IStandardGeneralRoleInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardGeneralRoleInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(XmlDocument doc)
        /// <summary>获取列表信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IStandardGeneralRoleInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardGeneralRoleInfo>(list) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetPages(XmlDocument doc)
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

            IList<IStandardGeneralRoleInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardGeneralRoleInfo>(list) + ",");
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

        #region 函数:IsExist(XmlDocument doc)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string IsExist(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            bool result = this.service.IsExist(id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
        }
        #endregion

        #region 函数:CreateNewObject(XmlDocument doc)
        /// <summary>创建新的对象</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string treeViewId = XmlHelper.Fetch("treeViewId", doc);

            string groupTreeNodeId = XmlHelper.Fetch("groupTreeNodeId", doc);

            StandardGeneralRoleInfo param = new StandardGeneralRoleInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.GroupTreeNodeId = string.IsNullOrEmpty(groupTreeNodeId) ? treeViewId : groupTreeNodeId;

            param.Locking = 1;

            param.Status = 1;

            param.ModifiedDate = param.CreatedDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<StandardGeneralRoleInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetMappingTable(XmlDocument doc)
        /// <summary>查找所属组织下的角色和标准通用角色的映射关系</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetMappingTable(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string standardGeneralRoleId = XmlHelper.Fetch("standardGeneralRoleId", doc);

            string organizationId = XmlHelper.Fetch("organizationId", doc);

            DataTable table = this.service.GetMappingTable(standardGeneralRoleId, organizationId);

            outString.Append("{\"data\":" + JsonHelper.ToJosn(table, true, true) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:CreateMappingRelation(XmlDocument doc)
        /// <summary>创建新的对象</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string CreateMappingRelation(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string standardGeneralRoleId = XmlHelper.Fetch("standardGeneralRoleId", doc);

            string organizationId = XmlHelper.Fetch("organizationId", doc);

            StandardGeneralRoleMappingRelationInfo param = new StandardGeneralRoleMappingRelationInfo();

            param.StandardGeneralRoleId = standardGeneralRoleId;

            if (!string.IsNullOrEmpty(param.StandardGeneralRoleId))
            {
                IStandardGeneralRoleInfo standardGeneralRole = MembershipManagement.Instance.StandardGeneralRoleService.FindOne(param.StandardGeneralRoleId);

                param.StandardGeneralRoleName = (standardGeneralRole == null) ? string.Empty : standardGeneralRole.Name;
            }

            param.OrganizationUnitId = organizationId;

            if (!string.IsNullOrEmpty(param.OrganizationUnitId))
            {
                IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService.FindOne(param.OrganizationUnitId);

                param.OrganizationUnitName = (organization == null) ? string.Empty : organization.Name;
            }

            outString.Append("{\"data\":" + AjaxUtil.Parse<StandardGeneralRoleMappingRelationInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindOneMappingRelation(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindOneMappingRelation(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string standardGeneralRoleId = XmlHelper.Fetch("standardGeneralRoleId", doc);

            string organizationId = XmlHelper.Fetch("organizationId", doc);

            IStandardGeneralRoleMappingRelationInfo param = this.service.FindOneMappingRelation(standardGeneralRoleId, organizationId);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardGeneralRoleMappingRelationInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetMappingRelationPages(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetMappingRelationPages(XmlDocument doc)
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

            IList<IStandardGeneralRoleMappingRelationInfo> list = this.service.GetMappingRelationPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardGeneralRoleMappingRelationInfo>(list) + ",");
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

        #region 函数:AddMappingRelation(XmlDocument doc)
        /// <summary>添加映射关系信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string AddMappingRelation(XmlDocument doc)
        {
            string standardGeneralRoleId = XmlHelper.Fetch("standardGeneralRoleId", doc);

            string organizationId = XmlHelper.Fetch("organizationId", doc);

            string roleId = XmlHelper.Fetch("roleId", doc);

            int result = this.service.AddMappingRelation(standardGeneralRoleId, organizationId, roleId);

            if (result == 1)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"已存在相同的映射关系，请删除后再执行此操作。\"}}";
            }
            else if (result == 2)
            {
                return "{\"message\":{\"returnCode\":2,\"value\":\"请确认相关的角色信息是否存在。\"}}";
            }
            else
            {
                return "{\"message\":{\"returnCode\":0,\"value\":\"添加成功。\"}}";
            }
        }
        #endregion

        #region 函数:RemoveMappingRelation(XmlDocument doc)
        /// <summary>移除映射关系信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string RemoveMappingRelation(XmlDocument doc)
        {
            string standardGeneralRoleId = XmlHelper.Fetch("standardGeneralRoleId", doc);

            string organizationId = XmlHelper.Fetch("organizationId", doc);

            this.service.RemoveMappingRelation(standardGeneralRoleId, organizationId);

            return "{\"message\":{\"returnCode\":0,\"value\":\"添加成功。\"}}";
        }
        #endregion
    }
}
