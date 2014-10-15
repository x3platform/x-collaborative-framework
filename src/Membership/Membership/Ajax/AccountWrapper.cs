#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :AccountWrapper.cs
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
    using System.Data;
    using System.Xml;
    using System.Text;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    using X3Platform.Location.IPQuery;
    #endregion

    /// <summary></summary>
    public sealed class AccountWrapper : ContextWrapper
    {
        private IAccountService service = MembershipManagement.Instance.AccountService; // ���ݷ���

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
            AccountInfo param = new AccountInfo();

            param = (AccountInfo)AjaxStorageConvertor.Deserialize(param, doc);

            string originalName = AjaxStorageConvertor.Fetch("originalName", doc);

            string originalGlobalName = AjaxStorageConvertor.Fetch("originalGlobalName", doc);

            string organizationText = AjaxStorageConvertor.Fetch("organizationText", doc);

            string roleText = AjaxStorageConvertor.Fetch("roleText", doc);

            string groupText = AjaxStorageConvertor.Fetch("groupText", doc);

            if (string.IsNullOrEmpty(param.LoginName))
            {
                return "{message:{\"returnCode\":1,\"value\":\"��¼������Ϊ�ա�\"}}";
            }

            if (string.IsNullOrEmpty(param.Id))
            {
                // ����

                if (this.service.IsExistGlobalName(param.GlobalName))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"��ȫ�������Ѵ��ڡ�\"}}";
                }

                if (this.service.IsExistLoginNameAndGlobalName(param.LoginName, param.Name))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"���ʺŻ������Ѵ��ڡ�\"}}";
                }

                param.Id = Guid.NewGuid().ToString();
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
                    if (this.service.IsExistName(param.Name))
                    {
                        return "{message:{\"returnCode\":1,\"value\":\"�������Ѵ��ڡ�\"}}";
                    }
                }
            }

            param.ResetRelations("organization", organizationText);
            param.ResetRelations("role", roleText);
            param.ResetRelations("group", groupText);

            // ��ȡԭʼ������Ϣ
            IAccountInfo originalObject = this.service.FindOne(param.Id);
            // �����Ƿ����ڵĶ������ж��Ƿ��½�����
            bool isNewObject = originalObject == null ? true : false;

            this.service.Save(param);

            // ����û��Ĭ��, �����������ڵ�һ����ɫΪĬ�Ͻ�ɫ��
            if (!MembershipManagement.Instance.RoleService.HasDefaultRelation(param.Id))
            {
                if (param.RoleRelations.Count > 0)
                {
                    MembershipManagement.Instance.MemberService.SetDefaultRole(param.Id, param.RoleRelations[0].RoleId);
                }
            }

            // ��¼�ʺŲ�����־
            string optionName = isNewObject ? "�½�" : "�༭";

            IAccountInfo optionAccount = KernelContext.Current.User;

            string description = "��" + optionAccount.Name + "��" + optionName + "���ʺš�" + param.Name + "����";

            if (optionName == "�༭")
            {
                description = "��" + optionAccount.Name + "���༭���û���" + param.Name + "�����ʺ���Ϣ��";
            }

            MembershipManagement.Instance.AccountLogService.Log(param.Id, optionName, originalObject, description, optionAccount.Id);

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

            // ��ȡԭʼ������Ϣ
            IAccountInfo originalObject = this.service.FindOne(id);

            this.service.Delete(id);

            // ��¼�ʺŲ�����־
            string optionName = "ɾ��";

            IAccountInfo optionAccount = KernelContext.Current.User;

            string description = "��" + optionAccount.Name + "��ɾ�����ʺš�" + originalObject.Name + "����";

            MembershipManagement.Instance.AccountLogService.Log(originalObject.Id, optionName, originalObject, description, optionAccount.Id);

            return "{message:{\"returnCode\":0,\"value\":\"ɾ���ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(XmlDocument doc)
        /// <summary>��ȡ������Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = AjaxStorageConvertor.Fetch("id", doc);

            IAccountInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAll(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <returns>����һ�����ص�ʵ���б�.</returns> 
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            IList<IAccountInfo> list = this.service.FindAll(whereClause);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:FindAllWithoutMemberInfo(XmlDocument doc)
        /// <summary>��ѯ��������</summary>
        /// <returns>����һ�����ص�ʵ���б�.</returns> 
        [AjaxMethod("findAllWithoutMemberInfo")]
        public string FindAllWithoutMemberInfo(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(AjaxStorageConvertor.Fetch("length", doc));

            IList<IAccountInfo> list = this.service.FindAllWithoutMemberInfo(length);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns></returns> 
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IAccountInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountInfo>(list) + ",");

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

            // string organizationId = AjaxStorageConvertor.Fetch("organizationId", doc);

            IAccountInfo param = new AccountInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:ChangeBasicInfo(XmlDocument doc)
        /// <summary></summary>
        [AjaxMethod("changeBasicInfo")]
        public string ChangeBasicInfo(XmlDocument doc)
        {
            AccountInfo param = new AccountInfo();

            param = (AccountInfo)AjaxStorageConvertor.Deserialize(param, doc);

            // �����ʺ�

            if (param.Name != (string)param.Properties["OriginalName"])
            {
                if (string.IsNullOrEmpty(param.LoginName) || this.service.IsExistName(param.Name))
                {
                    return "{message:{\"returnCode\":1,\"value\":\"�������Ѵ��ڡ�\"}}";
                }
            }

            // �������� 

            this.service.ChangeBasicInfo(param);

            return "{message:{\"returnCode\":0,\"value\":\"�޸ĳɹ���\"}}";
        }
        #endregion

        #region 属性:ChangePassword(string xml)
        /// <summary></summary>
        [AjaxMethod("changePassword")]
        public string ChangePassword(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            string loginName = AjaxStorageConvertor.Fetch("loginName", doc);

            string password = AjaxStorageConvertor.Fetch("password", doc);

            string originalPassword = AjaxStorageConvertor.Fetch("originalPassword", doc);

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOneByLoginName(loginName);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"δ���⵽��ǰ�û��ĺϷ���Ϣ��\"}}";
            }

            if (optionAccount.LoginName == loginName)
            {
                int result = this.service.ChangePassword(loginName, password, originalPassword);

                if (result == 0)
                {
                    // ��¼�ʺŲ�����־
                    MembershipManagement.Instance.AccountLogService.Log(optionAccount.Id, "�޸�����", "�û���" + optionAccount.Name + "���޸������ɹ���");

                    return "{message:{\"returnCode\":0,\"value\":\"�޸ĳɹ���\"}}";
                }
                else
                {
                    // ��¼�ʺŲ�����־
                    MembershipManagement.Instance.AccountLogService.Log(optionAccount.Id, "�޸�����", "�û���" + optionAccount.Name + "���޸�����ʧ�ܣ���IP:" + IPQueryContext.GetClientIP() + "����");

                    return "{message:{\"returnCode\":1,\"value\":\"�޸�ʧ��, �û�����������.\"}}";
                }
            }
            else
            {
                // ��¼�ʺŲ�����־
                MembershipManagement.Instance.AccountLogService.Log(account.Id, "�޸�����", "��" + optionAccount.Name + "�������޸���һ���û���" + account.Name + "�������룬��IP:" + IPQueryContext.GetClientIP() + "����", optionAccount.Id);

                return "{message:{\"returnCode\":1,\"value\":\"�޸�ʧ��, �û�����������.\"}}";
            }
        }
        #endregion

        #region 属性:SearchRoleLevel(string whereClause)
        /// <summary>��ѯ��������</summary>
        /// <returns>����һ�����ص�ʵ���б�.</returns> 
        public string SearchRoleLevel(string whereClause)
        {
            try
            {
                StringBuilder outString = new StringBuilder();

                whereClause = " CateId='b70b1f6c-3c75-44b6-815e-bff04233cb26' AND Name LIKE '%" + whereClause + "%' ORDER BY No ";

                IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAll(whereClause);

                outString.Append("{\"ajaxStorage\":{\"list\":[");

                foreach (IOrganizationInfo item in list)
                {
                    outString.Append("{");
                    outString.Append("\"Id\":\"" + item.Id + "\",");
                    outString.Append("\"Name\":\"" + item.Name + "\",");
                    //outString.Append("\"parentId\":\"" + item.ParentId + "\",");
                    //outString.Append("\"cateId\":\"" + item.CateId + "\",");
                    outString.Append("\"status\":\"" + item.Status + "\" ");
                    outString.Append("},");
                }

                if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                    outString = outString.Remove(outString.Length - 1, 1);

                outString.Append("]}}");

                return outString.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region 属性:FindParentRoles(string accountId)
        /// <summary>��ѯ��������</summary>
        /// <returns>����һ�����ص�ʵ���б�.</returns> 
        public string FindParentRoles(string accountId)
        {
            // 1.����Ĭ�ϸ�λ
            //
            // 2.�����ϼ���λ
            //

            StringBuilder outString = new StringBuilder();

            //string whereClause = string.Format(" T.Id='{0}' AND T.IsDefault=1 ", id);

            IList<IRoleInfo> list = null;

            IList<IRoleInfo> myRoles = MembershipManagement.Instance.RoleService.FindAllByAccountId(accountId);

            if (myRoles.Count > 0)
            {
                //
                // list = MembershipManagement.Instance.Role.FindParentRoles(myRoles[0].Id);
            }

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IRoleInfo>(list) + ",");

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

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(id);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"δ���⵽��ǰ�û��ĺϷ���Ϣ��\"}}";
            }

            int reuslt = this.service.SetGlobalName(id, globalName);

            MembershipManagement.Instance.AccountLogService.Log(account.Id, "����ȫ������", "��" + optionAccount.Name + "�����û���" + account.Name + "����ȫ����������Ϊ��" + globalName + "������IP:" + IPQueryContext.GetClientIP() + "����", optionAccount.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"����ȫ�����Ƴɹ���\"}}";
        }
        #endregion

        #region 属性:SetPassword(XmlDocument doc)
        /// <summary>��������</summary>
        [AjaxMethod("setPassword")]
        public string SetPassword(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            string password = AjaxStorageConvertor.Fetch("password", doc);

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(id);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"δ���⵽��ǰ�û��ĺϷ���Ϣ��\"}}";
            }

            this.service.SetPassword(id, password);

            // ��¼�ʺŲ�����־
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "��������", "��" + optionAccount.Name + "�������������û���" + account.Name + "�������룬��IP:" + IPQueryContext.GetClientIP() + "����", optionAccount.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"���������ɹ���\"}}";
        }
        #endregion

        #region 属性:SetLoginName(XmlDocument doc)
        /// <summary></summary>
        [AjaxMethod("setLoginName")]
        public string SetLoginName(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            string loginName = AjaxStorageConvertor.Fetch("loginName", doc);

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(id);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"δ���⵽��ǰ�û��ĺϷ���Ϣ��\"}}";
            }

            if (string.IsNullOrEmpty(loginName))
            {
                return "{message:{\"returnCode\":1,\"value\":\"��¼������Ϊ�ա�\"}}";
            }
            else if (!this.service.IsExistLoginName(loginName))
            {
                this.service.SetLoginName(id, loginName);

                // ��¼�ʺŲ�����־
                MembershipManagement.Instance.AccountLogService.Log(account.Id, "���õ�¼��", "��" + optionAccount.Name + "�����û���" + account.Name + "���ĵ�¼������Ϊ��" + loginName + "������IP:" + IPQueryContext.GetClientIP() + "����", optionAccount.Id);

                return "{message:{\"returnCode\":0,\"value\":\"���õ�¼���ɹ���\"}}";
            }
            else
            {
                return "{message:{\"returnCode\":1,\"value\":\"�Ѵ��ڴ˵�¼����\"}}";
            }
        }
        #endregion

        #region 属性:SetStatus(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [AjaxMethod("setStatus")]
        public string SetStatus(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            int status = Convert.ToInt32(AjaxStorageConvertor.Fetch("status", doc));

            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOne(id);

            IAccountInfo optionAccount = KernelContext.Current.User;

            if (account == null || optionAccount == null)
            {
                return "{message:{\"returnCode\":1,\"value\":\"δ���⵽��ǰ�û��ĺϷ���Ϣ��\"}}";
            }

            int reuslt = this.service.SetStatus(id, status);

            MembershipManagement.Instance.AccountLogService.Log(account.Id, "�����ʺ�״̬", "��" + optionAccount.Name + "�����û���" + account.Name + "����״̬����Ϊ��" + (status == 0 ? "����" : "����") + "������IP:" + IPQueryContext.GetClientIP() + "����", optionAccount.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"�����ʺ�״̬�ɹ���\"}}";
        }
        #endregion

        #region 属性:GetAuthorizationObjects(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns></returns> 
        [AjaxMethod("getAuthorizationObjects")]
        public string GetAuthorizationObjects(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            DataTable table = MembershipManagement.Instance.AuthorizationObjectService.Filter(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":[");

            foreach (DataRow row in table.Rows)
            {
                outString.Append("{");
                outString.Append("\"authorizationObjectId\":\"" + StringHelper.ToSafeJson(row["AuthorizationObjectId"].ToString()) + "\",");
                outString.Append("\"authorizationObjectName\":\"" + StringHelper.ToSafeJson(row["AuthorizationObjectName"].ToString()) + "\",");
                outString.Append("\"authorizationObjectType\":\"" + StringHelper.ToSafeJson(row["AuthorizationObjectType"].ToString()) + "\",");
                outString.Append("\"accountId\":\"" + StringHelper.ToSafeJson(row["AccountId"].ToString()) + "\",");
                outString.Append("\"accountGlobalName\":\"" + StringHelper.ToSafeJson(row["AccountGlobalName"].ToString()) + "\",");
                outString.Append("\"accountLoginName\":\"" + StringHelper.ToSafeJson(row["AccountLoginName"].ToString()) + "\"},");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion
    }
}