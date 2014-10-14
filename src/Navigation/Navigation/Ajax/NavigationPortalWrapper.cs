#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2012 Elane, ruany@chinasic.com
//
// FileName     :NavigationPortalWrapper.cs
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
    using X3Platform.Membership;
    using X3Platform.Membership.Model;
    using X3Platform.Util;

    using X3Platform.Navigation.IBLL;
    using X3Platform.Navigation.Model;
    #endregion

    /// <summary></summary>
    public class NavigationPortalWrapper : ContextWrapper
    {
        /// <summary>���ݷ���</summary>
        private INavigationPortalService service = NavigationContext.Instance.NavigationPortalService;

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
            NavigationPortalInfo param = new NavigationPortalInfo();

            param = (NavigationPortalInfo)AjaxStorageConvertor.Deserialize(param, doc);

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

            NavigationPortalInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<NavigationPortalInfo>(param) + ",");

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

            IList<NavigationPortalInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<NavigationPortalInfo>(list) + ",");

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

            IList<NavigationPortalInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<NavigationPortalInfo>(list) + ",");

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

            NavigationPortalInfo param = new NavigationPortalInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<NavigationPortalInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:getCombobox(XmlDocument doc)
        [AjaxMethod("getCombobox")]
        public string GetCombobox(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();
            string combobox = AjaxStorageConvertor.Fetch("combobox", doc);
            string selectedValue = AjaxStorageConvertor.Fetch("selectedValue", doc);
            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);
            if (string.IsNullOrEmpty(selectedValue))
            {
                selectedValue = "-1";
            }
            if (whereClause.ToUpper().IndexOf("ORDER") == -1)
            {
                whereClause = whereClause + " ORDER BY OrderId ";
            }
            IList<NavigationPortalInfo> list = this.service.FindAll(whereClause, 0);
            outString.Append("{\"ajaxStorage\":[");
            foreach (NavigationPortalInfo item in list)
            {
                outString.Append(string.Concat(new object[] { "{\"text\":\"", item.Text, "\",\"value\":\"", item.Id, "\",\"selected\":\"", selectedValue == item.Id, "\"}," }));
            }
            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }
            outString.Append("],");
            outString.Append("\"combobox\":\"" + combobox + "\",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");
            return outString.ToString();
        }
        #endregion

        #region ����:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getDynamicTreeView")]
        public string GetDynamicTreeView(XmlDocument doc)
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
                string whereClause = string.Format(" Status = 1 ORDER BY OrderId ");

                IList<NavigationPortalInfo> list = this.service.FindAll(whereClause, 0);

                foreach (NavigationPortalInfo item in list)
                {
                    token = "[OrgId]" + item.Id;
                    outString.Append("{");
                    outString.Append("\"id\":\"" + token + "\",");
                    outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson("0") + "\",");
                    outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Text) + "\",");
                    outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Description) + "\",");
                    outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Text)) + "\",");
                    outString.Append("\"target\":\"_self\"");
                    outString.Append("},");
                }
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ�.\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:GetOrgDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getOrgDynamicTreeView")]
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

            if (parentId == "70000000-0000-0000-0000-000000000000")//������֯�ĸ��ڵ�
            {

                string whereClause = string.Format(" ParentId = ##00000000-0000-0000-0000-000000000000## AND Status  =1 ORDER BY OrderId ");

                IList<IOrganizationInfo> list = Membership.MembershipManagement.Instance.OrganizationService.FindAll(whereClause);

                foreach (IOrganizationInfo item in list)
                {
                    token = "[OrgId]" + item.Id;
                    outString.Append("{");
                    outString.Append("\"id\":\"" + token + "\",");
                    outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson("0") + "\",");
                    outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                    outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                    outString.Append("\"target\":\"_self\"");
                    outString.Append("},");
                }
            }
            else
            {
                //������֯�е��ӽڵ�
                string orgWhereClause = string.Format(" ParentId = ##{0}## AND Status = 1 ORDER BY OrderId ", parentId.Replace("[OrgId]", ""));

                IList<IOrganizationInfo> orgList = Membership.MembershipManagement.Instance.OrganizationService.FindAll(orgWhereClause);

                if (orgList.Count > 0)
                {
                    foreach (IOrganizationInfo item in orgList)
                    {
                        token = "[OrgId]" + item.Id;
                        outString.Append("{");
                        outString.Append("\"id\":\"" + token + "\",");
                        outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.ParentId == treeViewRootTreeNodeId ? "0" : ("[OrgId]" + item.ParentId)) + "\",");
                        outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                        outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
                        outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Name)) + "\",");
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
