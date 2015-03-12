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
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    using X3Platform.Util;
    #endregion

    /// <summary></summary>
    public sealed class OrganizationWrapper : ContextWrapper
    {
        private IOrganizationService service = MembershipManagement.Instance.OrganizationService; // 数据服务

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            IOrganizationInfo param = new OrganizationInfo();

            param = (IOrganizationInfo)AjaxUtil.Deserialize<IOrganizationInfo>(param, doc);

            string originalName = XmlHelper.Fetch("originalName", doc);
            string originalGlobalName = XmlHelper.Fetch("originalGlobalName", doc);

            if (string.IsNullOrEmpty(param.Name))
            {
                return "{message:{\"returnCode\":1,\"value\":\"名称不能为空。\"}}";
            }

            if (string.IsNullOrEmpty(param.GlobalName))
            {
                return "{message:{\"returnCode\":1,\"value\":\"全局名称不能为空。\"}}";
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                // 新增

                if (this.service.IsExistGlobalName(param.GlobalName))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"此全局名称已存在。\"}}";
                }

                param.Id = DigitalNumberContext.Generate("Key_Guid");
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

                if (param.Name != originalName)
                {
                    IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(param.ParentId);

                    foreach (IOrganizationInfo item in list)
                    {
                        if (item.Name == param.Name)
                        {
                            return "{message:{\"returnCode\":1,\"value\":\"此父级组织下面已已存在相同名称组织.\"}}";
                        }
                    }
                }
            }

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

            return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
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

            IOrganizationInfo param = MembershipManagement.Instance.OrganizationService.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IOrganizationInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有数据</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAll(whereClause);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IOrganizationInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllByAccountId(XmlDocument doc)
        /// <summary>查询所有数据</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAllByAccountId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountId = XmlHelper.Fetch("accountId", doc);

            IList<IOrganizationInfo> list = this.service.FindAllByAccountId(accountId);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IOrganizationInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllByParentId(XmlDocument doc)
        /// <summary>查询所有数据</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAllByParentId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string parentId = XmlHelper.Fetch("parentId", doc);

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IOrganizationInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllByParentId(XmlDocument doc)
        /// <summary>查询所有数据</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAllByProjectId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string projectId = XmlHelper.Fetch("projectId", doc);

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByProjectId(projectId);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IOrganizationInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out  rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IOrganizationInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
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
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string parentId = XmlHelper.Fetch("parentId", doc);

            OrganizationInfo param = new OrganizationInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.ParentId = parentId;

            // 默认的类型为部门
            param.Type = 1;

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<IOrganizationInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:SetGlobalName(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string SetGlobalName(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);
            string globalName = XmlHelper.Fetch("globalName", doc);

            int reuslt = MembershipManagement.Instance.OrganizationService.SetGlobalName(id, globalName);

            if (reuslt == 1)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"已存在相同全局名称.\"}}";
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}";
        }
        #endregion

        #region 函数:GetCorporationTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetCorporationTreeView(XmlDocument doc)
        {
            // 必填字段
            string tree = XmlHelper.Fetch("tree", doc);
            string parentId = XmlHelper.Fetch("parentId", doc);

            // 附加属性
            string treeViewType = XmlHelper.Fetch("treeViewType", doc);
            string treeViewId = XmlHelper.Fetch("treeViewId", doc);
            string treeViewName = XmlHelper.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = XmlHelper.Fetch("treeViewRootTreeNodeId", doc);

            string url = XmlHelper.Fetch("url", doc);

            // 树形控件默认根节点标识为0, 需要特殊处理.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"data\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            foreach (IOrganizationInfo item in list)
            {
                if (item.Status == 0)
                    continue;

                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.Parent.Id == treeViewRootTreeNodeId ? "0" : item.Parent.Id) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", ((item.Type == 0) ? "[CorporationTreeNode]" : string.Empty) + item.Id).Replace("{treeNodeName}", item.GlobalName)) + "\",");
                outString.Append("\"target\":\"_self\",");
                outString.Append("\"hasChildren\":" + ((item.Type == 0) ? "true" : "false"));
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}