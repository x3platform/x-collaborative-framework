#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2012 Elane, ruany@chinasic.com
//
// FileName     :NavigationPortalSidebarItemGroupWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Navigation.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Navigation.IBLL;
    using X3Platform.Navigation.Model;
    #endregion

    /// <summary></summary>
    public class NavigationPortalSidebarItemGroupWrapper : ContextWrapper
    {
        /// <summary>���ݷ���</summary>
        private INavigationPortalSidebarItemGroupService service = NavigationContext.Instance.NavigationPortalSidebarItemGroupService;

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(XmlDocument doc)
        /// <summary>������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            NavigationPortalSidebarItemGroupInfo param = new NavigationPortalSidebarItemGroupInfo();

            param = (NavigationPortalSidebarItemGroupInfo)AjaxUtil.Deserialize(param, doc);

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region 属性:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("delete")]
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

            NavigationPortalSidebarItemGroupInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<NavigationPortalSidebarItemGroupInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAll(XmlDocument doc)
        /// <summary>��ȡ�б���Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = XmlHelper.Fetch("whereClause", doc);

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<NavigationPortalSidebarItemGroupInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<NavigationPortalSidebarItemGroupInfo>(list) + ",");

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
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<NavigationPortalSidebarItemGroupInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<NavigationPortalSidebarItemGroupInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:IsExist(XmlDocument doc)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("isExist")]
        public string IsExist(XmlDocument doc)
        {
            string id = XmlHelper.Fetch("id", doc);

            bool result = this.service.IsExist(id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
        }
        #endregion

        #region 属性:CreateNewObject(XmlDocument doc)
        /// <summary>�����µĶ���</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("createNewObject")]
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            NavigationPortalSidebarItemGroupInfo param = new NavigationPortalSidebarItemGroupInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<NavigationPortalSidebarItemGroupInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getDynamicTreeView")]
        public string GetOrgDynamicTreeView(XmlDocument doc)
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

            string token = null;

            if (parentId == "00000000-0000-0000-0000-000000000000")//������֯�ĸ��ڵ�
            {
                string whereClause = string.Format(" [Status]=1 order by [OrderId]");

                IList<NavigationPortalInfo> list = NavigationContext.Instance.NavigationPortalService.FindAll(whereClause);

                foreach (NavigationPortalInfo item in list)
                {
                    token = "[PortalId]" + item.Id;
                    outString.Append("{");
                    outString.Append("\"id\":\"" + token + "\",");
                    outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson("0") + "\",");
                    outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Text) + "\",");
                    outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Text) + "\",");
                    outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Text)) + "\",");
                    outString.Append("\"target\":\"_self\"");
                    outString.Append("},");
                }
            }
            else
            {
                //������֯�е��ӽڵ�
                string itemGroupWhereClause = string.Format(" [PortalId] =##{0}## AND [Status]=1 order by [OrderId]", parentId.Replace("[PortalId]", ""));

                IList<NavigationPortalSidebarItemGroupInfo> itemGrouplist = NavigationContext.Instance.NavigationPortalSidebarItemGroupService.FindAll(itemGroupWhereClause);

                if (itemGrouplist.Count > 0)
                {
                    foreach (NavigationPortalSidebarItemGroupInfo item in itemGrouplist)
                    {
                        token = "[ItemGroupId]" + item.Id;
                        outString.Append("{");
                        outString.Append("\"id\":\"" + token + "\",");
                        outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.PortalId == treeViewRootTreeNodeId ? "0" : ("[PortalId]" + item.PortalId)) + "\",");
                        outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Text) + "\",");
                        outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Text) + "\",");
                        outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Text).Replace("{portalId}", item.PortalId).Replace("{portalName}", item.PortalName)) + "\",");
                        outString.Append("\"target\":\"_self\"");
                        outString.Append("},");
                    }
                }
            }


            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ�.\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
