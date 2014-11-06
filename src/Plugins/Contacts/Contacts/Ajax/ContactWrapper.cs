#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ContactWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Contacts.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.CategoryIndexes;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;
    
    using X3Platform.Plugins.Contacts.IBLL;
    using X3Platform.Plugins.Contacts.Model;
    #endregion

    public class ContactWrapper : ContextWrapper
    {
        private IContactService service = ContactContext.Instance.ContactService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            ContactInfo param = new ContactInfo();

            param = (ContactInfo)AjaxUtil.Deserialize(param, doc);
            param.Id = param.Id == "" ? DigitalNumberContext.Generate("Key_Guid") : param.Id;
            param.AccountId = KernelContext.Current.User.Id;
            //param.Tags = string.Empty;
            //param.IconPath = string.Empty;
            param.UpdateDate = System.DateTime.Now;

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Delete(XmlDocument doc)
        {
            string ids = XmlHelper.Fetch("ids", doc);

            this.service.Delete(ids);

            return "{\"message\":{\"returnCode\":0,\"value\":\"删除成功。\"}}";
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

            ContactInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ContactInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Query(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            IList<ContactInfo> list = this.service.FindAll();

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ContactInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:Query(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Query(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<ContactInfo> list = this.service.Query(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ContactInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:CreateNewObject(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            ContactInfo param = new ContactInfo();
            
            param.Id = DigitalNumberContext.Generate("Key_Guid");
            
            param.CreateDate = param.UpdateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ContactInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetDynamicTreeView(XmlDocument doc)
        {
            // 必填字段
            string tree = XmlHelper.Fetch("tree", doc);
            string parentId = XmlHelper.Fetch("parentId", doc);

            // 附加属性
            string treeViewId = XmlHelper.Fetch("treeViewId", doc);
            string treeViewName = XmlHelper.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = XmlHelper.Fetch("treeViewRootTreeNodeId", doc);

            // 类别索引前缀
            string prefixCategoryIndex = XmlHelper.Fetch("prefixCategoryIndex", doc);

            string url = XmlHelper.Fetch("url", doc);

            // 树形控件默认根节点标识为0, 需要特殊处理.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            ICategoryIndex index = this.service.FetchCategoryIndex(KernelContext.Current.User.Id, prefixCategoryIndex);

            outString.Append("{\"ajaxStorage\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            outString.Append(this.GetDynamicTreeChildNodesView(treeViewRootTreeNodeId, url, index));

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"查询成功.\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetDynamicTreeChildNodesView(string treeViewRootTreeNodeId, string url, ICategoryIndex index)
        /// <summary></summary>
        /// <param name="treeViewRootTreeNodeId"></param>
        /// <param name="url"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetDynamicTreeChildNodesView(string treeViewRootTreeNodeId, string url, ICategoryIndex index)
        {
            StringBuilder outString = new StringBuilder();

            foreach (ICategoryIndex item in index.ChildNodes)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Value + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson((item.Parent.Value == treeViewRootTreeNodeId) ? "0" : item.Parent.Value) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Text) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Value).Replace("{treeNodeName}", item.Text)) + "\",");
                outString.Append("\"target\":\"_self\",");
                outString.Append("\"hasChildren\":\"" + item.HasChildren.ToString().ToLower() + "\",");
                outString.Append("\"ajaxLoading\":\"false\",");
                outString.Append("childNodes:[");
                if (item.HasChildren)
                {
                    outString.Append(GetDynamicTreeChildNodesView(treeViewRootTreeNodeId, url, item));
                }
                outString.Append("]");

                outString.Append("},");
            }

            if (outString.Length > 1 && outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            return outString.ToString();
        }
        #endregion
    }
}