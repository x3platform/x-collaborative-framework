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
        /// <summary>���ݷ���</summary>
        private IStandardOrganizationService service = MembershipManagement.Instance.StandardOrganizationService;

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
            StandardOrganizationInfo param = new StandardOrganizationInfo();

            param = (StandardOrganizationInfo)AjaxStorageConvertor.Deserialize(param, doc);

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
                    IList<IStandardOrganizationInfo> list = Membership.MembershipManagement.Instance.StandardOrganizationService.FindAllByParentId(param.ParentId);

                    foreach (IStandardOrganizationInfo item in list)
                    {
                        if (item.Name == param.Name)
                        {
                            return "{message:{\"returnCode\":1,\"value\":\"�˸�����֯�������Ѵ�����ͬ���Ʊ�׼��֯.\"}}";
                        }
                    }
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
            string ids = AjaxStorageConvertor.Fetch("ids", doc);

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

            string id = AjaxStorageConvertor.Fetch("id", doc);

            IStandardOrganizationInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardOrganizationInfo>(param) + ",");

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

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            int length = Convert.ToInt32(AjaxStorageConvertor.Fetch("length", doc));

            IList<IStandardOrganizationInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardOrganizationInfo>(list) + ",");

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

            IList<IStandardOrganizationInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardOrganizationInfo>(list) + ",");

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
            string id = AjaxStorageConvertor.Fetch("id", doc);

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

            string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

            IStandardOrganizationInfo param = new StandardOrganizationInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            if (!string.IsNullOrEmpty(parentId))
            {
                param.ParentId = parentId;
            }

            param.Status = 1;
            param.UpdateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardOrganizationInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetStandardOrganizationTypes(XmlDocument doc)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("getStandardOrganizationTypes")]
        public string GetStandardOrganizationTypes(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string combobox = AjaxStorageConvertor.Fetch("combobox", doc);

            string selectedValue = AjaxStorageConvertor.Fetch("selectedValue", doc);

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc, "xml");

            IList<SettingInfo> settings = MembershipManagement.Instance.SettingService.FindAllBySettingGroupName("Ӧ�ù���_Эͬƽ̨_��Ա��Ȩ�޹���_��׼��ɫ����_��׼��ɫ����");

            outString.Append("{\"ajaxStorage\":[");

            foreach (SettingInfo setting in settings)
            {
                outString.Append("{\"text\":\"" + setting.Text + "\",\"value\":\"" + setting.Value + "\",\"selected\":\"" + (selectedValue == setting.Text) + "\"},");
            }

            // int length = Convert.ToInt32(AjaxStorageConvertor.Fetch("length", doc));

            // IList<ProjectInfo> list = this.service.FindAll(whereClause, length);

            // Dictionary<string, string> list = new Dictionary<string, string>();

            //list.Add("�����ܲ�", "�����ܲ�");
            //list.Add("�����ز���˾", "�����ز���˾");
            //list.Add("������ҵ��˾", "������ҵ��˾");
            //list.Add("������ҵ��˾", "������ҵ��˾");

            //outString.Append("{\"ajaxStorage\":[");

            //foreach (KeyValuePair<string, string> item in list)
            //{
            //    outString.Append("{\"text\":\"" + item.Key + "\",\"value\":\"" + item.Value + "\",\"selected\":\"" + (selectedValue == item.Value) + "\"},");
            //}

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");

            outString.Append("\"combobox\":\"" + combobox + "\",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // ���β˵�
        // -------------------------------------------------------

        #region 属性:GetDynamicTreeView(XmlDocument doc)
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

            //���������ӽڵ�
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

            outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion
    }
}
