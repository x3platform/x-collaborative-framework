#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardOrganizationWrapper.cs
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
    public class StandardOrganizationWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IStandardOrganizationService service = MembershipManagement.Instance.StandardOrganizationService;

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
            StandardOrganizationInfo param = new StandardOrganizationInfo();

            param = (StandardOrganizationInfo)AjaxUtil.Deserialize(param, doc);

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
                    IList<IStandardOrganizationInfo> list = Membership.MembershipManagement.Instance.StandardOrganizationService.FindAllByParentId(param.ParentId);

                    foreach (IStandardOrganizationInfo item in list)
                    {
                        if (item.Name == param.Name)
                        {
                            return "{message:{\"returnCode\":1,\"value\":\"此父级组织下面已已存在相同名称标准组织.\"}}";
                        }
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
            string ids = XmlHelper.Fetch("ids", doc);

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

            string id = XmlHelper.Fetch("id", doc);

            IStandardOrganizationInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<IStandardOrganizationInfo>(param) + ",");

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

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IStandardOrganizationInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<IStandardOrganizationInfo>(list) + ",");

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

            PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IStandardOrganizationInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<IStandardOrganizationInfo>(list) + ",");

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

            string parentId = XmlHelper.Fetch("parentId", doc);

            IStandardOrganizationInfo param = new StandardOrganizationInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            if (!string.IsNullOrEmpty(parentId))
            {
                param.ParentId = parentId;
            }

            param.Status = 1;
            param.UpdateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<IStandardOrganizationInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetStandardOrganizationTypes(XmlDocument doc)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("getStandardOrganizationTypes")]
        public string GetStandardOrganizationTypes(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string combobox = XmlHelper.Fetch("combobox", doc);

            string selectedValue = XmlHelper.Fetch("selectedValue", doc);

            string whereClause = XmlHelper.Fetch("whereClause", doc, "xml");

            IList<SettingInfo> settings = MembershipManagement.Instance.SettingService.FindAllBySettingGroupName("应用管理_协同平台_人员及权限管理_标准角色管理_标准角色类别");

            outString.Append("{\"ajaxStorage\":[");

            foreach (SettingInfo setting in settings)
            {
                outString.Append("{\"text\":\"" + setting.Text + "\",\"value\":\"" + setting.Value + "\",\"selected\":\"" + (selectedValue == setting.Text) + "\"},");
            }

            // int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            // IList<ProjectInfo> list = this.service.FindAll(whereClause, length);

            // Dictionary<string, string> list = new Dictionary<string, string>();

            //list.Add("集团总部", "集团总部");
            //list.Add("地区地产公司", "地区地产公司");
            //list.Add("地区商业公司", "地区商业公司");
            //list.Add("地区物业公司", "地区物业公司");

            //outString.Append("{\"ajaxStorage\":[");

            //foreach (KeyValuePair<string, string> item in list)
            //{
            //    outString.Append("{\"text\":\"" + item.Key + "\",\"value\":\"" + item.Value + "\",\"selected\":\"" + (selectedValue == item.Value) + "\"},");
            //}

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");

            outString.Append("\"combobox\":\"" + combobox + "\",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 树形菜单
        // -------------------------------------------------------

        #region 函数:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getDynamicTreeView")]
        public string GetDynamicTreeView(XmlDocument doc)
        {
            // 必填字段
            string tree = XmlHelper.Fetch("tree", doc);
            string parentId = XmlHelper.Fetch("parentId", doc);

            // 附加属性
            string treeViewId = XmlHelper.Fetch("treeViewId", doc);
            string treeViewName = XmlHelper.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = XmlHelper.Fetch("treeViewRootTreeNodeId", doc);

            string url = XmlHelper.Fetch("url", doc);

            // 树形控件默认根节点标识为0, 需要特殊处理.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"ajaxStorage\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            //查找树的子节点
            string whereClause = string.Format(" [ParentId] = ##{0}## AND [Status] = 1 ORDER BY OrderId, Code ", parentId);

            if (parentId == "70000000-0000-0000-0000-000000000000")
            {
                whereClause = " [ParentId] = ##00000000-0000-0000-0000-000000000001## AND [Status] = 1 ORDER BY OrderId, Code ";
            }

            IList<IStandardOrganizationInfo> list = MembershipManagement.Instance.StandardOrganizationService.FindAll(whereClause);

            foreach (IStandardOrganizationInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + ((parentId == treeViewRootTreeNodeId) ? "0" : StringHelper.ToSafeJson(parentId)) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Id).Replace("{treeNodeName}", item.Name)) + "\",");
                outString.Append("\"target\":\"_self\"");
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
