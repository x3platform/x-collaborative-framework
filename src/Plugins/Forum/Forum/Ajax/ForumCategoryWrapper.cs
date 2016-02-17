namespace X3Platform.Plugins.Forum.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Plugins.Forum.IBLL;
    using X3Platform.Plugins.Forum.Model;
    using X3Platform.CategoryIndexes;
    using X3Platform.Globalization; using X3Platform.Messages;
    #endregion

    /// <summary></summary>
    public class ForumCategoryWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IForumCategoryService service = ForumContext.Instance.ForumCategoryService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Save(XmlDocument doc)
        {
            ForumCategoryInfo param = new ForumCategoryInfo();

            param = (ForumCategoryInfo)AjaxUtil.Deserialize(param, doc);
            param.Id = param.Id == "" ? Guid.NewGuid().ToString() : param.Id;

            ForumCategoryInfo info = this.service.FindOneByCategoryIndex(param.CategoryIndex);

            if (!this.service.IsExist(param.Id) && info != null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"已存在该版块！\"}}";
            }

            string authorizationReadScopeObjectText = XmlHelper.Fetch("authorizationReadScopeObjectText", doc);
            string authorizationEditScopeObjectText = XmlHelper.Fetch("authorizationEditScopeObjectText", doc);

            service.BindAuthorizationScopeObjects(param.Id, "应用_通用_查看权限", authorizationReadScopeObjectText);
            service.BindAuthorizationScopeObjects(param.Id, "应用_通用_修改权限", authorizationEditScopeObjectText);

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功.\"}}";
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

            return MessageObject.Stringify("0", I18n.Strings["msg_delete_success"]);
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
            string applicationTag = XmlHelper.Fetch("applicationTag", doc);

            ForumCategoryInfo param = this.service.FindOne(id);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ForumCategoryInfo>(param) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAll(XmlDocument doc)
        /// <summary>获取列表信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = XmlHelper.Fetch("whereClause", doc);
            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));
            string applicationTag = XmlHelper.Fetch("applicationTag", doc);

            IList<ForumCategoryInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<ForumCategoryInfo>(list) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

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

            int rowCount = -1;

            IList<ForumCategoryQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ForumCategoryQueryInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
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
            string applicationTag = XmlHelper.Fetch("applicationTag", doc);

            bool result = this.service.IsExist(id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
        }
        #endregion

        #region 函数:IsAnonymous(XmlDocument doc)
        /// <summary>查询该板块是否允许匿名发布</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string IsAnonymous(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            bool result = this.service.IsAnonymous(id);

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

            ForumCategoryInfo param = new ForumCategoryInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.ModifiedDate = param.CreatedDate = DateTime.Now;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ForumCategoryInfo>(param) + ",");

            outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_create_success"], true) + "}");

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

            string url = XmlHelper.Fetch("url", doc);
            string isCategory = XmlHelper.Fetch("isCategory", doc);
            string isStatus = XmlHelper.Fetch("isStatus", doc);

            string applicationTag = XmlHelper.Fetch("applicationTag", doc);

            // 树形控件默认根节点标识为0, 需要特殊处理.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            ICategoryIndex index = service.FetchCategoryIndex(isStatus);

            outString.Append("{\"data\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            outString.Append(GetDynamicTreeChildNodesView(treeViewRootTreeNodeId, url, index, isCategory));

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"查询成功.\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:GetDynamicTreeChildNodesView(string treeViewRootTreeNodeId, string url, ICategoryIndex index,string isCategory)
        /// <summary></summary>
        /// <param name="treeViewRootTreeNodeId"></param>
        /// <param name="url"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetDynamicTreeChildNodesView(string treeViewRootTreeNodeId, string url, ICategoryIndex index, string isCategory)
        {
            StringBuilder outString = new StringBuilder();

            foreach (ICategoryIndex item in index.ChildNodes)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Value + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson((item.Parent.Value == treeViewRootTreeNodeId) ? "0" : item.Parent.Value) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Text) + "\",");
                if (!item.HasChildren || isCategory == "1")
                {
                    string categoryIndex = item.Value.Substring(5).Replace('_', '\\');
                    ForumCategoryInfo categoryInfo = this.service.FindOneByCategoryIndex(categoryIndex);
                    outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", categoryInfo == null ? "" : categoryInfo.Id).Replace("{treeNodeName}", item.Text).Replace("{treeNodeCategoryIndex}", categoryIndex.Replace("\\", "\\\\"))) + "\",");
                }
                outString.Append("\"target\":\"_self\",");
                outString.Append("\"hasChildren\":\"" + item.HasChildren.ToString().ToLower() + "\",");
                outString.Append("\"ajaxLoading\":\"false\",");
                outString.Append("childNodes:[");
                if (item.HasChildren)
                {
                    outString.Append(GetDynamicTreeChildNodesView(treeViewRootTreeNodeId, url, item, isCategory));
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
