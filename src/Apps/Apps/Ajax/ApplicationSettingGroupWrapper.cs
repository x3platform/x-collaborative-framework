#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ApplicationSettingGroupWrapper.cs
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
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.Util;
    using X3Platform.DigitalNumber;

    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    public class ApplicationSettingGroupWrapper : ContextWrapper
    {
        /// <summary>���ݷ���</summary>
        private IApplicationSettingGroupService service = AppsContext.Instance.ApplicationSettingGroupService;

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
            ApplicationSettingGroupInfo param = new ApplicationSettingGroupInfo();

            string originalName = XmlHelper.Fetch("originalName", doc);

            param = (ApplicationSettingGroupInfo)AjaxUtil.Deserialize(param, doc);

            if (originalName != param.Name)
            {
                if (this.service.IsExistName(param.Name))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�Ѵ�����ͬ�����ơ�\"}}";
                }
            }

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

            ApplicationSettingGroupInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationSettingGroupInfo>(param) + ",");

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

            IList<ApplicationSettingGroupInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationSettingGroupInfo>(list) + ",");

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

            IList<ApplicationSettingGroupInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationSettingGroupInfo>(list) + ",");

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

            string applicationId = XmlHelper.Fetch("applicationId", doc);

            ApplicationFeatureInfo param = new ApplicationFeatureInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.ApplicationId = applicationId;

            param.ParentId = "00000000-0000-0000-0000-000000000000";

            param.Status = 1;

            outString.Append("{\"ajaxStorage\":" + AjaxUtil.Parse<ApplicationFeatureInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetDynamicTreeView(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("getDynamicTreeView")]
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

            //���������ӽڵ�
            string whereClause = string.Format(" ParentId = ##{0}## AND Status = 1 ORDER BY OrderId ", parentId);

            if (treeViewRootTreeNodeId == parentId)
            {
                whereClause = string.Format(" ApplicationId = ##{0}## AND ParentId = ##00000000-0000-0000-0000-000000000000## AND Status = 1 ORDER BY OrderId ", treeViewRootTreeNodeId);
            }

            IList<ApplicationSettingGroupInfo> list = this.service.FindAll(whereClause);

            foreach (ApplicationSettingGroupInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"id\":\"" + item.Id + "\",");
                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson((item.ParentId == treeViewRootTreeNodeId || item.ParentId == "00000000-0000-0000-0000-000000000000") ? "0" : item.ParentId) + "\",");
                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.DisplayName) + "\",");
                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Id).Replace("{treeNodeName}", item.DisplayName)) + "\",");
                outString.Append("\"target\":\"_self\"");
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
