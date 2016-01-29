namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
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
    public class StandardRoleWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IStandardRoleService service = MembershipManagement.Instance.StandardRoleService;

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
            IStandardRoleInfo param = new StandardRoleInfo();

            param = (IStandardRoleInfo)AjaxUtil.Deserialize(param, doc);

            string originalName = XmlHelper.Fetch("originalName", doc);

            if (string.IsNullOrEmpty(param.Name))
            {
                return "{message:{\"returnCode\":1,\"value\":\"名称不能为空。\"}}";
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                // 新增

                if (this.service.IsExistName(param.Name))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"�������Ѵ��ڡ�\"}}";
                }

                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }
            else
            {
                // 修改

                if (param.Name != originalName)
                {
                    if (this.service.IsExistName(param.Name))
                    {
                        return "{message:{\"returnCode\":1,\"value\":\"此名称已存在。\"}}";
                    }
                }
            }

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

            IStandardRoleInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardRoleInfo>(param) + ",");

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

            IList<IStandardRoleInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardRoleInfo>(list) + ",");

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

            IList<IStandardRoleInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardRoleInfo>(list) + ",");
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
        [AjaxMethod("isExist")]
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
        [AjaxMethod("createNewObject")]
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string standardOrganizationUnitId = XmlHelper.Fetch("standardOrganizationUnitId", doc);

            IStandardRoleInfo param = new StandardRoleInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            if (!string.IsNullOrEmpty(standardOrganizationUnitId))
            {
                param.StandardOrganizationUnitId = standardOrganizationUnitId;
            }

            param.Status = 1;
            param.ModifiedDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardRoleInfo>(param) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetKeyStandardRoles(XmlDocument doc)
        /// <summary>获取关键标准角色</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("getKeyStandardRoles")]
        public string GetKeyStandardRoles(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int standardRoleType = Convert.ToInt32(XmlHelper.Fetch("standardRoleType", doc));

            IList<IStandardRoleInfo> list = this.service.GetKeyStandardRoles(standardRoleType);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IStandardRoleInfo>(list) + ",");

            outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion  
   
        // -------------------------------------------------------
        // 树形菜单
        // -------------------------------------------------------

        //#region 函数:GetDynamicTreeView(XmlDocument doc)
        ///// <summary></summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //[AjaxMethod("getDynamicTreeView")]
        //public string GetDynamicTreeView(XmlDocument doc)
        //{
        //    // 必填字段
        //    string tree = XmlHelper.Fetch("tree", doc);
        //    string parentId = XmlHelper.Fetch("parentId", doc);

        //    // 附加属性
        //    string treeViewId = XmlHelper.Fetch("treeViewId", doc);
        //    string treeViewName = XmlHelper.Fetch("treeViewName", doc);
        //    string treeViewRootTreeNodeId = XmlHelper.Fetch("treeViewRootTreeNodeId", doc);

        //    string url = XmlHelper.Fetch("url", doc);

        //    // 树形控件默认根节点标识为0, 需要特殊处理.
        //    parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

        //    StringBuilder outString = new StringBuilder();

        //    outString.Append("{\"data\":");
        //    outString.Append("{\"tree\":\"" + tree + "\",");
        //    outString.Append("\"parentId\":\"" + parentId + "\",");
        //    outString.Append("childNodes:[");

        //    string token = null;

        //    if (parentId == "70000000-0000-0000-0000-000000000000")
        //    {
        //        IList<GroupTreeNodeInfo> list = MembershipManagement.Instance.GroupTreeNodeService.FindAllByParentId(parentId);

        //        foreach (GroupTreeNodeInfo item in list)
        //        {
        //            token = "[GroupTreeNode]" + item.Id;

        //            outString.Append("{");
        //            outString.Append("\"id\":\"" + token + "\",");
        //            outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.ParentId == treeViewRootTreeNodeId ? "0" : item.ParentId) + "\",");
        //            outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //            outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //            outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Name)) + "\",");
        //            outString.Append("\"target\":\"_self\"");
        //            outString.Append("},");
        //        }
        //    }
        //    else
        //    {
        //        IList<IStandardRoleInfo> list = null;

        //        if (parentId.IndexOf("[GroupTreeNode]") == 0)
        //        {
        //            string whereClause = string.Format(" GroupTreeNodeId = ##{0}## AND ( ParentId IS NULL OR ParentId = ##00000000-0000-0000-0000-000000000000## ) ORDER BY OrderId ", parentId.Replace("[GroupTreeNode]", ""));

        //            list = MembershipManagement.Instance.StandardRoleService.FindAll(whereClause);

        //            foreach (IStandardRoleInfo item in list)
        //            {
        //                token = "[StandardRole]" + item.Id + "[GroupTreeNode]" + item.GroupTreeNodeId;

        //                outString.Append("{");
        //                outString.Append("\"id\":\"" + token + "\",");
        //                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(parentId) + "\",");
        //                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Name)) + "\",");
        //                outString.Append("\"target\":\"_self\"");
        //                outString.Append("},");
        //            }
        //        }
        //        else
        //        {
        //            list = MembershipManagement.Instance.StandardRoleService.FindAllByParentId(parentId.Replace("[StandardRole]", ""));
                  
        //            foreach (IStandardRoleInfo item in list)
        //            {
        //                token = "[StandardRole]" + item.Id + "[GroupTreeNode]" + item.GroupTreeNodeId;

        //                outString.Append("{");
        //                outString.Append("\"id\":\"" + token + "\",");
        //                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(parentId) + "\",");
        //                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Name)) + "\",");
        //                outString.Append("\"target\":\"_self\"");
        //                outString.Append("},");
        //            }
        //        }
        //    }

        //    if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
        //        outString = outString.Remove(outString.Length - 1, 1);

        //    outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

        //    return outString.ToString();
        //}
        //#endregion
    }
}
