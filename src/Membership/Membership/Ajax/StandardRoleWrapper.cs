#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardRoleWrapper.cs
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
    public class StandardRoleWrapper : ContextWrapper
    {
        /// <summary>���ݷ���</summary>
        private IStandardRoleService service = MembershipManagement.Instance.StandardRoleService;

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
            IStandardRoleInfo param = new StandardRoleInfo();

            param = (IStandardRoleInfo)AjaxStorageConvertor.Deserialize(param, doc);

            string originalName = AjaxStorageConvertor.Fetch("originalName", doc);

            if (string.IsNullOrEmpty(param.Name))
            {
                return "{message:{\"returnCode\":1,\"value\":\"���Ʋ���Ϊ�ա�\"}}";
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                // ����

                if (this.service.IsExistName(param.Name))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"�������Ѵ��ڡ�\"}}";
                }

                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }
            else
            {
                // �޸�

                if (param.Name != originalName)
                {
                    if (this.service.IsExistName(param.Name))
                    {
                        return "{message:{\"returnCode\":1,\"value\":\"�������Ѵ��ڡ�\"}}";
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

            IStandardRoleInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(param) + ",");

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

            IList<IStandardRoleInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(list) + ",");

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

            IList<IStandardRoleInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(list) + ",");

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

            string standardOrganizationId = AjaxStorageConvertor.Fetch("standardOrganizationId", doc);

            IStandardRoleInfo param = new StandardRoleInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            if (!string.IsNullOrEmpty(standardOrganizationId))
            {
                param.StandardOrganizationId = standardOrganizationId;
            }

            param.Status = 1;
            param.UpdateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetKeyStandardRoles(XmlDocument doc)
        /// <summary>��ȡ�ؼ���׼��ɫ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        [AjaxMethod("getKeyStandardRoles")]
        public string GetKeyStandardRoles(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int standardRoleType = Convert.ToInt32(AjaxStorageConvertor.Fetch("standardRoleType", doc));

            IList<IStandardRoleInfo> list = this.service.GetKeyStandardRoles(standardRoleType);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardRoleInfo>(list) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion  
   
        // -------------------------------------------------------
        // ���β˵�
        // -------------------------------------------------------

        //#region 属性:GetDynamicTreeView(XmlDocument doc)
        ///// <summary></summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //[AjaxMethod("getDynamicTreeView")]
        //public string GetDynamicTreeView(XmlDocument doc)
        //{
        //    // �����ֶ�
        //    string tree = AjaxStorageConvertor.Fetch("tree", doc);
        //    string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

        //    // ��������
        //    string treeViewId = AjaxStorageConvertor.Fetch("treeViewId", doc);
        //    string treeViewName = AjaxStorageConvertor.Fetch("treeViewName", doc);
        //    string treeViewRootTreeNodeId = AjaxStorageConvertor.Fetch("treeViewRootTreeNodeId", doc);

        //    string url = AjaxStorageConvertor.Fetch("url", doc);

        //    // ���οؼ�Ĭ�ϸ��ڵ���ʶΪ0, ��Ҫ���⴦��.
        //    parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

        //    StringBuilder outString = new StringBuilder();

        //    outString.Append("{\"ajaxStorage\":");
        //    outString.Append("{\"tree\":\"" + tree + "\",");
        //    outString.Append("\"parentId\":\"" + parentId + "\",");
        //    outString.Append("childNodes:[");

        //    string token = null;

        //    if (parentId == "70000000-0000-0000-0000-000000000000")
        //    {
        //        IList<GroupTreeNodeInfo> list = MembershipManagement.Instance.GroupTreeNodeService.FindAllByParentId(parentId);

        //        foreach (GroupTreeNodeInfo item in list)
        //        {
        //            token = "[GroupTreeNode]" + item.Id;

        //            outString.Append("{");
        //            outString.Append("\"id\":\"" + token + "\",");
        //            outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(item.ParentId == treeViewRootTreeNodeId ? "0" : item.ParentId) + "\",");
        //            outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //            outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //            outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Name)) + "\",");
        //            outString.Append("\"target\":\"_self\"");
        //            outString.Append("},");
        //        }
        //    }
        //    else
        //    {
        //        IList<IStandardRoleInfo> list = null;

        //        if (parentId.IndexOf("[GroupTreeNode]") == 0)
        //        {
        //            string whereClause = string.Format(" GroupTreeNodeId = ##{0}## AND ( ParentId IS NULL OR ParentId = ##00000000-0000-0000-0000-000000000000## ) ORDER BY OrderId ", parentId.Replace("[GroupTreeNode]", ""));

        //            list = MembershipManagement.Instance.StandardRoleService.FindAll(whereClause);

        //            foreach (IStandardRoleInfo item in list)
        //            {
        //                token = "[StandardRole]" + item.Id + "[GroupTreeNode]" + item.GroupTreeNodeId;

        //                outString.Append("{");
        //                outString.Append("\"id\":\"" + token + "\",");
        //                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(parentId) + "\",");
        //                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Name)) + "\",");
        //                outString.Append("\"target\":\"_self\"");
        //                outString.Append("},");
        //            }
        //        }
        //        else
        //        {
        //            list = MembershipManagement.Instance.StandardRoleService.FindAllByParentId(parentId.Replace("[StandardRole]", ""));
                  
        //            foreach (IStandardRoleInfo item in list)
        //            {
        //                token = "[StandardRole]" + item.Id + "[GroupTreeNode]" + item.GroupTreeNodeId;

        //                outString.Append("{");
        //                outString.Append("\"id\":\"" + token + "\",");
        //                outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(parentId) + "\",");
        //                outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //                outString.Append("\"title\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        //                outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", token).Replace("{treeNodeName}", item.Name)) + "\",");
        //                outString.Append("\"target\":\"_self\"");
        //                outString.Append("},");
        //            }
        //        }
        //    }

        //    if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
        //        outString = outString.Remove(outString.Length - 1, 1);

        //    outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

        //    return outString.ToString();
        //}
        //#endregion
    }
}
