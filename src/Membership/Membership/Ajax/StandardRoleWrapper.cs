#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardRoleWrapper.cs
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
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
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

            param = (IStandardRoleInfo)AjaxStorageConvertor.Deserialize(param, doc);

            string originalName = AjaxStorageConvertor.Fetch("originalName", doc);

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

            return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string ids = AjaxStorageConvertor.Fetch("ids", doc);

            this.service.Delete(ids);

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
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = AjaxStorageConvertor.Fetch("id", doc);

            IStandardRoleInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            int length = Convert.ToInt32(AjaxStorageConvertor.Fetch("length", doc));

            IList<IStandardRoleInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IStandardRoleInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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
            string id = AjaxStorageConvertor.Fetch("id", doc);

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

            string standardOrganizationId = AjaxStorageConvertor.Fetch("standardOrganizationId", doc);

            IStandardRoleInfo param = new StandardRoleInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            if (!string.IsNullOrEmpty(standardOrganizationId))
            {
                param.StandardOrganizationId = standardOrganizationId;
            }

            param.Status = 1;
            param.UpdateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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

            int standardRoleType = Convert.ToInt32(AjaxStorageConvertor.Fetch("standardRoleType", doc));

            IList<IStandardRoleInfo> list = this.service.GetKeyStandardRoles(standardRoleType);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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
        //    string tree = AjaxStorageConvertor.Fetch("tree", doc);
        //    string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

        //    // 附加属性
        //    string treeViewId = AjaxStorageConvertor.Fetch("treeViewId", doc);
        //    string treeViewName = AjaxStorageConvertor.Fetch("treeViewName", doc);
        //    string treeViewRootTreeNodeId = AjaxStorageConvertor.Fetch("treeViewRootTreeNodeId", doc);

        //    string url = AjaxStorageConvertor.Fetch("url", doc);

        //    // 树形控件默认根节点标识为0, 需要特殊处理.
        //    parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

        //    StringBuilder outString = new StringBuilder();

        //    outString.Append("{\"ajaxStorage\":");
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
