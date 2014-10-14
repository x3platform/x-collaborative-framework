#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2012 Elane, ruany@chinasic.com
//
// FileName     :NavigationPortalShortcutGroupWrapper.cs
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
    public class NavigationPortalShortcutGroupWrapper : ContextWrapper
    {
        /// <summary>���ݷ���</summary>
        private INavigationPortalShortcutGroupService service = NavigationContext.Instance.NavigationPortalShortcutGroupService;

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(XmlDocument doc)
        /// <summary>������¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            NavigationPortalShortcutGroupInfo param = new NavigationPortalShortcutGroupInfo();

            param = (NavigationPortalShortcutGroupInfo)AjaxStorageConvertor.Deserialize(param, doc);

            this.service.Save(param);

            return "{\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region ����:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string ids = AjaxStorageConvertor.Fetch("ids", doc);

            this.service.Delete(ids);

            return "{message:{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = AjaxStorageConvertor.Fetch("id", doc);

            NavigationPortalShortcutGroupInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<NavigationPortalShortcutGroupInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:FindAll(XmlDocument doc)
        /// <summary>��ȡ�б���Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            int length = Convert.ToInt32(AjaxStorageConvertor.Fetch("length", doc));

            IList<NavigationPortalShortcutGroupInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<NavigationPortalShortcutGroupInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<NavigationPortalShortcutGroupInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<NavigationPortalShortcutGroupInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:IsExist(XmlDocument doc)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("isExist")]
        public string IsExist(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            bool result = this.service.IsExist(id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
        }
        #endregion

        #region ����:CreateNewObject(XmlDocument doc)
        /// <summary>�����µĶ���</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("createNewObject")]
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            NavigationPortalShortcutGroupInfo param = new NavigationPortalShortcutGroupInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<NavigationPortalShortcutGroupInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getDynamicTreeView")]
        public string GetOrgDynamicTreeView(XmlDocument doc)
        {
            // �����ֶ�
            string tree = AjaxStorageConvertor.Fetch("tree", doc);
            string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

            // ��������
            string treeViewId = AjaxStorageConvertor.Fetch("treeViewId", doc);
            string treeViewName = AjaxStorageConvertor.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = AjaxStorageConvertor.Fetch("treeViewRootTreeNodeId", doc);

            string url = AjaxStorageConvertor.Fetch("url", doc);

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
                string shortcutGroupWhereClause = string.Format(" [PortalId] =##{0}## AND [Status]=1 order by [OrderId]", parentId.Replace("[PortalId]", ""));

                IList<NavigationPortalShortcutGroupInfo> shortcutGrouplist = NavigationContext.Instance.NavigationPortalShortcutGroupService.FindAll(shortcutGroupWhereClause);

                if (shortcutGrouplist.Count > 0)
                {
                    foreach (NavigationPortalShortcutGroupInfo item in shortcutGrouplist)
                    {
                        token = "[ShortcutGroupId]" + item.Id;
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
