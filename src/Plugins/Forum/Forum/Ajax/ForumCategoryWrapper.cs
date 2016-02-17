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
        /// <summary>���ݷ���</summary>
        private IForumCategoryService service = ForumContext.Instance.ForumCategoryService;

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(XmlDocument doc)
        /// <summary>�����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        public string Save(XmlDocument doc)
        {
            ForumCategoryInfo param = new ForumCategoryInfo();

            param = (ForumCategoryInfo)AjaxUtil.Deserialize(param, doc);
            param.Id = param.Id == "" ? Guid.NewGuid().ToString() : param.Id;

            ForumCategoryInfo info = this.service.FindOneByCategoryIndex(param.CategoryIndex);

            if (!this.service.IsExist(param.Id) && info != null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"�Ѵ��ڸð�飡\"}}";
            }

            string authorizationReadScopeObjectText = XmlHelper.Fetch("authorizationReadScopeObjectText", doc);
            string authorizationEditScopeObjectText = XmlHelper.Fetch("authorizationEditScopeObjectText", doc);

            service.BindAuthorizationScopeObjects(param.Id, "Ӧ��_ͨ��_�鿴Ȩ��", authorizationReadScopeObjectText);
            service.BindAuthorizationScopeObjects(param.Id, "Ӧ��_ͨ��_�޸�Ȩ��", authorizationEditScopeObjectText);

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"����ɹ�.\"}}";
        }
        #endregion

        #region ����:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        public string Delete(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            this.service.Delete(id);

            return MessageObject.Stringify("0", I18n.Strings["msg_delete_success"]);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
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

        #region ����:FindAll(XmlDocument doc)
        /// <summary>��ȡ�б���Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
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
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            int rowCount = -1;

            IList<ForumCategoryQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":" + AjaxUtil.Parse<ForumCategoryQueryInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"},");
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

            return outString.ToString();
        }
        #endregion

        #region ����:IsExist(XmlDocument doc)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        public string IsExist(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);
            string applicationTag = XmlHelper.Fetch("applicationTag", doc);

            bool result = this.service.IsExist(id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
        }
        #endregion

        #region ����:IsAnonymous(XmlDocument doc)
        /// <summary>��ѯ�ð���Ƿ�������������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        public string IsAnonymous(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            bool result = this.service.IsAnonymous(id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
        }
        #endregion

        #region ����:CreateNewObject(XmlDocument doc)
        /// <summary>�����µĶ���</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
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

        #region ����:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetDynamicTreeView(XmlDocument doc)
        {
            // �����ֶ�
            string tree = XmlHelper.Fetch("tree", doc);
            string parentId = XmlHelper.Fetch("parentId", doc);

            // ��������
            string treeViewId = XmlHelper.Fetch("treeViewId", doc);
            string treeViewName = XmlHelper.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = XmlHelper.Fetch("treeViewRootTreeNodeId", doc);

            string url = XmlHelper.Fetch("url", doc);
            string isCategory = XmlHelper.Fetch("isCategory", doc);
            string isStatus = XmlHelper.Fetch("isStatus", doc);

            string applicationTag = XmlHelper.Fetch("applicationTag", doc);

            // ���οؼ�Ĭ�ϸ��ڵ��ʶΪ0, ��Ҫ���⴦��.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            ICategoryIndex index = service.FetchCategoryIndex(isStatus);

            outString.Append("{\"data\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            outString.Append(GetDynamicTreeChildNodesView(treeViewRootTreeNodeId, url, index, isCategory));

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ�.\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:GetDynamicTreeChildNodesView(string treeViewRootTreeNodeId, string url, ICategoryIndex index,string isCategory)
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
