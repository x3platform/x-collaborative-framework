#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardGeneralRoleWrapper.cs
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
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    using System.Data;
    #endregion

    /// <summary></summary>
    public class StandardGeneralRoleWrapper : ContextWrapper
    {
        /// <summary>���ݷ���</summary>
        private IStandardGeneralRoleService service = MembershipManagement.Instance.StandardGeneralRoleService;

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
            IStandardGeneralRoleInfo param = new StandardGeneralRoleInfo();

            param = (StandardGeneralRoleInfo)AjaxStorageConvertor.Deserialize(param, doc);

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

            IStandardGeneralRoleInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardGeneralRoleInfo>(param) + ",");

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

            IList<IStandardGeneralRoleInfo> list = this.service.FindAll(whereClause, length);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardGeneralRoleInfo>(list) + ",");

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
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IStandardGeneralRoleInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardGeneralRoleInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:IsExist(XmlDocument doc)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
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
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string treeViewId = AjaxStorageConvertor.Fetch("treeViewId", doc);

            string groupTreeNodeId = AjaxStorageConvertor.Fetch("groupTreeNodeId", doc);

            StandardGeneralRoleInfo param = new StandardGeneralRoleInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.GroupTreeNodeId = string.IsNullOrEmpty(groupTreeNodeId) ? treeViewId : groupTreeNodeId;

            param.Lock = 1;

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<StandardGeneralRoleInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetMappingTable(XmlDocument doc)
        /// <summary>����������֯�µĽ�ɫ�ͱ�׼ͨ�ý�ɫ��ӳ����ϵ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string GetMappingTable(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string standardGeneralRoleId = AjaxStorageConvertor.Fetch("standardGeneralRoleId", doc);

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            DataTable table = this.service.GetMappingTable(standardGeneralRoleId, organizationId);

            outString.Append("{\"ajaxStorage\":" + JsonHelper.ToJosn(table, true, true) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:CreateMappingRelation(XmlDocument doc)
        /// <summary>�����µĶ���</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string CreateMappingRelation(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string standardGeneralRoleId = AjaxStorageConvertor.Fetch("standardGeneralRoleId", doc);

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            StandardGeneralRoleMappingRelationInfo param = new StandardGeneralRoleMappingRelationInfo();

            param.StandardGeneralRoleId = standardGeneralRoleId;

            if (!string.IsNullOrEmpty(param.StandardGeneralRoleId))
            {
                IStandardGeneralRoleInfo standardGeneralRole = MembershipManagement.Instance.StandardGeneralRoleService.FindOne(param.StandardGeneralRoleId);

                param.StandardGeneralRoleName = (standardGeneralRole == null) ? string.Empty : standardGeneralRole.Name;
            }

            param.OrganizationId = organizationId;

            if (!string.IsNullOrEmpty(param.OrganizationId))
            {
                IOrganizationInfo organization = MembershipManagement.Instance.OrganizationService.FindOne(param.OrganizationId);

                param.OrganizationName = (organization == null) ? string.Empty : organization.Name;
            }

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<StandardGeneralRoleMappingRelationInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindOneMappingRelation(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string FindOneMappingRelation(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string standardGeneralRoleId = AjaxStorageConvertor.Fetch("standardGeneralRoleId", doc);

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            IStandardGeneralRoleMappingRelationInfo param = this.service.FindOneMappingRelation(standardGeneralRoleId, organizationId);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardGeneralRoleMappingRelationInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:GetMappingRelationPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string GetMappingRelationPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IStandardGeneralRoleMappingRelationInfo> list = this.service.GetMappingRelationPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IStandardGeneralRoleMappingRelationInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:AddMappingRelation(XmlDocument doc)
        /// <summary>����ӳ����ϵ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string AddMappingRelation(XmlDocument doc)
        {
            string standardGeneralRoleId = AjaxStorageConvertor.Fetch("standardGeneralRoleId", doc);

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            string roleId = AjaxStorageConvertor.Fetch("roleId", doc);

            int result = this.service.AddMappingRelation(standardGeneralRoleId, organizationId, roleId);

            if (result == 1)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"�Ѵ�����ͬ��ӳ����ϵ����ɾ������ִ�д˲�����\"}}";
            }
            else if (result == 2)
            {
                return "{\"message\":{\"returnCode\":2,\"value\":\"��ȷ�����صĽ�ɫ��Ϣ�Ƿ����ڡ�\"}}";
            }
            else
            {
                return "{\"message\":{\"returnCode\":0,\"value\":\"���ӳɹ���\"}}";
            }
        }
        #endregion

        #region 属性:RemoveMappingRelation(XmlDocument doc)
        /// <summary>�Ƴ�ӳ����ϵ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز�������</returns>
        public string RemoveMappingRelation(XmlDocument doc)
        {
            string standardGeneralRoleId = AjaxStorageConvertor.Fetch("standardGeneralRoleId", doc);

            string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            this.service.RemoveMappingRelation(standardGeneralRoleId, organizationId);

            return "{\"message\":{\"returnCode\":0,\"value\":\"���ӳɹ���\"}}";
        }
        #endregion
    }
}
