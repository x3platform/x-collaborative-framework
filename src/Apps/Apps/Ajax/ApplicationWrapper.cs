#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ApplicationWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Apps.Ajax
{
    #region Using Libraries
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    public class ApplicationWrapper : ContextWrapper
    {
        private IApplicationService service = AppsContext.Instance.ApplicationService;

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(XmlDocument doc)
        /// <summary>������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string Save(XmlDocument doc)
        {
            ApplicationInfo param = new ApplicationInfo();

            string administratorScopeText = XmlHelper.Fetch("administratorScopeText", doc);

            string reviewerScopeText = XmlHelper.Fetch("reviewerScopeText", doc);

            string memberScopeText = XmlHelper.Fetch("memberScopeText", doc);

            string originalApplicationName = XmlHelper.Fetch("originalApplicationName", doc);

            param = (ApplicationInfo)AjaxUtil.Deserialize(param, doc);

            param.AccountId = string.IsNullOrEmpty(param.AccountId) ? KernelContext.Current.User.Id : param.AccountId;

            if (originalApplicationName != param.ApplicationName)
            {
                if (this.service.IsExistName(param.ApplicationName))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�Ѵ�����ͬ�����ơ�\"}}";
                }
            }

            param = this.service.Save(param);

            this.service.BindAuthorizationScopeObjects(param.Id, "Ӧ��_Ĭ��_����Ա", administratorScopeText);

            this.service.BindAuthorizationScopeObjects(param.Id, "Ӧ��_Ĭ��_����Ա", reviewerScopeText);

            this.service.BindAuthorizationScopeObjects(param.Id, "Ӧ��_Ĭ��_�ɷ��ʳ�Ա", memberScopeText);

            return "{\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region 属性:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string Delete(XmlDocument doc)
        {
            string ids = XmlHelper.Fetch("ids", doc);

            this.service.Delete(ids);

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

            string id = XmlHelper.Fetch("id", doc);

            ApplicationInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationInfo>(param) + ",");

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
        public string Query(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            int rowCount = -1;

            IList<ApplicationInfo> list = AppsContext.Instance.ApplicationService.Query(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationInfo>(list) + ",");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"metaData\":{\"root\":\"ajaxStorage\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"���ҳɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetDynamicTreeView(XmlDocument doc)
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

            // ���οؼ�Ĭ�ϸ��ڵ���ʶΪ0, ��Ҫ���⴦��.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"ajaxStorage\":");
            outString.Append("{\"tree\":\"" + tree + "\",");
            outString.Append("\"parentId\":\"" + parentId + "\",");
            outString.Append("childNodes:[");

            // ���������ӽڵ�
            string whereClause = string.Format(" ParentId = ##{0}## AND Status = 1 ORDER BY OrderId, Code ", parentId);

            IList<ApplicationInfo> list = this.service.FindAll(whereClause);

            foreach (ApplicationInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.ParentId == treeViewRootTreeNodeId ? "0" : item.ParentId) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.ApplicationDisplayName) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Id).Replace("{treeNodeName}", item.ApplicationDisplayName)) + "\",");
                outString.Append("\"target\":\"_self\"");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetExtTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string GetExtTreeView(XmlDocument doc)
        {
            // �����ֶ�
            string tree = XmlHelper.Fetch("tree", doc);
            string parentId = XmlHelper.Fetch("parentId", doc);

            // ��������
            string treeViewId = XmlHelper.Fetch("treeViewId", doc);
            string treeViewName = XmlHelper.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = XmlHelper.Fetch("treeViewRootTreeNodeId", doc);

            string url = XmlHelper.Fetch("url", doc);

            // ���οؼ�Ĭ�ϸ��ڵ���ʶΪ0, ��Ҫ���⴦��.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"childNodes\":[");

            // ԭʼ�ĸ������ݽڵ���ʶ
            string originalParentId = (treeViewRootTreeNodeId == parentId) ? "00000000-0000-0000-0000-000000000000" : parentId;

            string whereClause = string.Format(" ParentId = ##{0}## AND Status = 1 ORDER BY OrderId, Code ", originalParentId);

            IList<ApplicationInfo> list = this.service.FindAll(whereClause);

            foreach (ApplicationInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson((item.ParentId == treeViewRootTreeNodeId || item.ParentId == "00000000-0000-0000-0000-000000000000") ? "0" : item.ParentId) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.ApplicationDisplayName) + "\",");
                outString.Append("\"leaf\":" + (item.HasChildren ? 0 : 1) + ",");
                outString.Append("\"target\":\"_self\"");
                outString.Append("},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
