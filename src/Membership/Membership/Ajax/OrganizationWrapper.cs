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
        private IOrganizationService service = MembershipManagement.Instance.OrganizationService; // ���ݷ���

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
            IOrganizationInfo param = new OrganizationInfo();

            param = (IOrganizationInfo)AjaxStorageConvertor.Deserialize<IOrganizationInfo>(param, doc);
            
            string originalName = AjaxStorageConvertor.Fetch("originalName", doc);
            string originalGlobalName = AjaxStorageConvertor.Fetch("originalGlobalName", doc);
            
            if (string.IsNullOrEmpty(param.Name))
            {
                return "{message:{\"returnCode\":1,\"value\":\"���Ʋ���Ϊ�ա�\"}}";
            }

            if (string.IsNullOrEmpty(param.GlobalName))
            {
                return "{message:{\"returnCode\":1,\"value\":\"ȫ�����Ʋ���Ϊ�ա�\"}}";
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                // ����

                if (this.service.IsExistGlobalName(param.GlobalName))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"��ȫ�������Ѵ��ڡ�\"}}";
                }

                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }
            else
            {
                // �޸�

                if (param.GlobalName != originalGlobalName)
                {
                    if (this.service.IsExistGlobalName(param.GlobalName))
                    {
                        return "{message:{\"returnCode\":1,\"value\":\"��ȫ�������Ѵ��ڡ�\"}}";
                    }
                }

                if (param.Name != originalName)
                {
                    IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(param.ParentId);

                    foreach (IOrganizationInfo item in list)
                    {
                        if (item.Name == param.Name)
                        {
                            return "{message:{\"returnCode\":1,\"value\":\"�˸�����֯�������Ѵ�����ͬ������֯.\"}}";
                        }
                    }
                }
            }

            this.service.Save(param);

            return "{message:{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
        }
        #endregion

        #region 属性:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            this.service.Delete(id);

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

            string id = AjaxStorageConvertor.Fetch("id", doc);

            IOrganizationInfo param = MembershipManagement.Instance.OrganizationService.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IOrganizationInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ��������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAll(whereClause);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IOrganizationInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllByAccountId(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAllByAccountId")]
        public string FindAllByAccountId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountId = AjaxStorageConvertor.Fetch("accountId", doc);

            IList<IOrganizationInfo> list = this.service.FindAllByAccountId(accountId);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IOrganizationInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllByParentId(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAllByParentId")]
        public string FindAllByParentId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IOrganizationInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllByParentId(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("findAllByProjectId")]
        public string FindAllByProjectId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string projectId = AjaxStorageConvertor.Fetch("projectId", doc);

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByProjectId(projectId);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IOrganizationInfo>(list) + ",");

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

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out  rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IOrganizationInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
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

            string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

            OrganizationInfo param = new OrganizationInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.ParentId = parentId;

            // Ĭ�ϵ�����Ϊ����
            param.Type = 1;

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IOrganizationInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:SetGlobalName(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("setGlobalName")]
        public string SetGlobalName(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);
            string globalName = AjaxStorageConvertor.Fetch("globalName", doc);

            int reuslt = MembershipManagement.Instance.OrganizationService.SetGlobalName(id, globalName);

            if (reuslt == 1)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"�Ѵ�����ͬȫ������.\"}}";
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}";
        }
        #endregion

        #region 属性:GetCorporationTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getCorporationTreeView")]
        public string GetCorporationTreeView(XmlDocument doc)
        {
            // �����ֶ�
            string tree = AjaxStorageConvertor.Fetch("tree", doc);
            string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

            // ��������
            string treeViewType = AjaxStorageConvertor.Fetch("treeViewType", doc);
            string treeViewId = AjaxStorageConvertor.Fetch("treeViewId", doc);
            string treeViewName = AjaxStorageConvertor.Fetch("treeViewName", doc);
            string treeViewRootTreeNodeId = AjaxStorageConvertor.Fetch("treeViewRootTreeNodeId", doc);

            string url = AjaxStorageConvertor.Fetch("url", doc);

            // ���οؼ�Ĭ�ϸ��ڵ���ʶΪ0, ��Ҫ���⴦��.
            parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"ajaxStorage\":");
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

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion
    }
}