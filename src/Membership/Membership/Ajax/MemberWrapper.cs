namespace X3Platform.Membership.Ajax
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Configuration;
    using X3Platform.DigitalNumber;
    using X3Platform.Location.IPQuery;
    using X3Platform.Logging;
    using X3Platform.Sessions;
    using X3Platform.Util;

    using X3Platform.Membership.Authentication;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    using X3Platform.Security.VerificationCode;

    /// <summary></summary>
    public sealed class MemberWrapper : ContextWrapper
    {
        private IMemberService service = MembershipManagement.Instance.MemberService; // ���ݷ���

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(XmlDocument doc)
        /// <summary>�����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            MemberInfo param = new MemberInfo();

            param = (MemberInfo)AjaxUtil.Deserialize<MemberInfo>(param, doc);

            this.service.Save(param);

            return "{message:{\"returnCode\":0,\"value\":\"����ɹ���\"}}";
        }
        #endregion

        #region ����:Delete(XmlDocument doc)
        /// <summary>ɾ����¼</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
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

        #region ����:FindOne(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            IMemberInfo param = this.service.FindOne(id);

            if (param == null) { return "{\"message\":{\"returnCode\":0,\"value\":\"δ�ҵ���ض���\"}}"; }

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(param) + ",");

            outString.Insert(outString.Length - 2, string.Format(",\"account\":{0}", AjaxUtil.Parse<IAccountInfo>(param.Account)));

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:FindOne(XmlDocument doc)
        /// <summary>��ȡ��ϸ��Ϣ</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns>
        [AjaxMethod("findOneByAccountId")]
        public string FindOneByAccountId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountId = XmlHelper.Fetch("accountId", doc);

            IMemberInfo param = this.service.FindOneByAccountId(accountId);

            if (param == null) { return "{\"message\":{\"returnCode\":0,\"value\":\"δ�ҵ���ض���\"}}"; }

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(param) + ",");

            outString.Insert(outString.Length - 2, string.Format(",\"account\":{0}", AjaxUtil.Parse<IAccountInfo>(param.Account)));

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:FindAllWithoutDefaultOrganizationUnit(XmlDocument doc)
        /// <summary>��ѯ����û��Ĭ����֯���û���Ϣ</summary>
        /// <returns>����һ����ص�ʵ���б�.</returns> 
        [AjaxMethod("findAllWithoutDefaultOrganizationUnit")]
        public string FindAllWithoutDefaultOrganizationUnit(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IMemberInfo> list = this.service.FindAllWithoutDefaultOrganizationUnit(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:FindAllWithoutDefaultJob(XmlDocument doc)
        /// <summary>��ѯ����û��Ĭ��ְλ���û���Ϣ</summary>
        /// <returns>����һ����ص�ʵ���б�.</returns> 
        [AjaxMethod("findAllWithoutDefaultJob")]
        public string FindAllWithoutDefaultJob(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IMemberInfo> list = this.service.FindAllWithoutDefaultJob(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:FindAllWithoutDefaultAssignedJob(XmlDocument doc)
        /// <summary>��ѯ����û��Ĭ�ϸ�λ���û���Ϣ</summary>
        /// <returns>����һ����ص�ʵ���б�.</returns> 
        [AjaxMethod("findAllWithoutDefaultAssignedJob")]
        public string FindAllWithoutDefaultAssignedJob(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IMemberInfo> list = this.service.FindAllWithoutDefaultAssignedJob(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region ����:FindAllWithoutDefaultRole(XmlDocument doc)
        /// <summary>��ѯ����û��Ĭ�Ͻ�ɫ���û���Ϣ</summary>
        /// <returns>����һ����ص�ʵ���б�.</returns> 
        [AjaxMethod("findAllWithoutDefaultRole")]
        public string FindAllWithoutDefaultRole(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IMemberInfo> list = this.service.FindAllWithoutDefaultRole(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(XmlDocument doc)
        /// <summary>��ȡ��ҳ����</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns>���ز������</returns> 
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            // ���õ�ǰ�û�Ȩ��
            if (XmlHelper.Fetch("su", doc) == "1")
            {
                paging.Query.Variables["elevatedPrivileges"] = "1";
            }

            paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

            int rowCount = -1;

            IList<IMemberInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

            paging.RowCount = rowCount;

            outString.Append("{\"data\":[");

            int index = 0;

            foreach (IMemberInfo item in list)
            {
                index = outString.Length;

                outString.Append(AjaxUtil.Parse<IMemberInfo>(item));

                outString.Insert(index + 1, string.Format("\"account\":{0},", AjaxUtil.Parse<IAccountInfo>(item.Account)));

                outString.Append(",");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
                outString = outString.Remove(outString.Length - 1, 1);

            outString.Append("],");
            outString.Append("\"paging\":" + paging + ",");
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"},");
            // ���� ExtJS ����
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

            return outString.ToString();
        }
        #endregion

        #region ����:SetDefaultRole(XmlDocument doc)
        /// <summary>��ѯ����û��Ĭ�Ͻ�ɫ���û���Ϣ</summary>
        /// <returns>����һ����ص�ʵ���б�.</returns> 
        public string SetContactCard(XmlDocument doc)
        {
            Dictionary<string, string> contactItems = new Dictionary<string, string>();

            contactItems.Add("Mobile", XmlHelper.Fetch("mobile", doc));
            contactItems.Add("Telephone", XmlHelper.Fetch("telephone", doc));
            contactItems.Add("QQ", XmlHelper.Fetch("qq", doc));
            contactItems.Add("MSN", XmlHelper.Fetch("msn", doc));
            contactItems.Add("Email", XmlHelper.Fetch("email", doc));
            contactItems.Add("Rtx", XmlHelper.Fetch("rtx", doc));
            contactItems.Add("PostCode", XmlHelper.Fetch("postCode", doc));
            contactItems.Add("Address", XmlHelper.Fetch("address", doc));

            IAccountInfo account = KernelContext.Current.User;

            this.service.SetContactCard(account.Id, contactItems);

            // ��¼�ʺŲ�����־
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "������ϵ��ʽ", "��" + account.Name + "���������Լ�����ϵ��Ϣ����IP:" + IPQueryContext.GetClientIP() + "����", account.Id);

            return "{message:{\"returnCode\":0,\"value\":\"������ϵ��Ϣ�ɹ���\"}}";
        }
        #endregion

        #region ����:SetDefaultRole(XmlDocument doc)
        /// <summary>��ѯ����û��Ĭ�Ͻ�ɫ���û���Ϣ</summary>
        /// <returns>����һ����ص�ʵ���б�.</returns> 
        [AjaxMethod("setDefaultRole")]
        public string SetDefaultRole(XmlDocument doc)
        {
            string accountId = XmlHelper.Fetch("accountId", doc);

            string roleId = XmlHelper.Fetch("roleId", doc);

            int resultCode = this.service.SetDefaultRole(accountId, roleId);

            return "{message:{\"returnCode\":0,\"value\":\"����Ĭ�Ͻ�ɫ�ɹ���\"}}";
        }
        #endregion

        #region ����:Read(XmlDocument doc)
        /// <summary>��ȡ��������</summary>
        /// <returns></returns>
        [AjaxMethod("read")]
        public string Read(XmlDocument doc)
        {
            string key = XmlHelper.Fetch("key", doc);

            StringBuilder outString = new StringBuilder();

            IAccountStorageStrategy strategy = KernelContext.Current.AuthenticationManagement.GetAccountStorageStrategy();

            IAccountInfo param = strategy.Deserialize(SessionContext.Instance.AccountCacheService.Read(key));

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"��ѯ�ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // ע���ʺ�
        // -------------------------------------------------------

        #region ����:Register(XmlDocument doc)
        /// <summary>ע���ʺ�</summary>
        /// <param name="doc">Xml �ĵ�����</param>
        /// <returns></returns>
        public string Register(XmlDocument doc)
        {
            IAccountInfo param = new AccountInfo();

            // Registration ע������: email | mobile | default
            string registration = XmlHelper.Fetch("registration", doc);
            // ��½��
            string loginName = XmlHelper.Fetch("loginName", doc);
            // ����
            string name = XmlHelper.Fetch("name", doc);
            // �ֻ�����
            string mobile = XmlHelper.Fetch("mobile", doc);
            // ����
            string email = XmlHelper.Fetch("email", doc);
            // ����
            string password = XmlHelper.Fetch("password", doc);

            string code = XmlHelper.Fetch("code", doc);

            if (registration == "mail")
            {
                if (string.IsNullOrEmpty(email))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"������д�������䡣\"}}";
                }

                if (MembershipManagement.Instance.AccountService.IsExistCertifiedEmail(email))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�������Ѿ����ڡ�\"}}";
                }

                if (!VerificationCodeContext.Instance.VerificationCodeService.Validate("Mail", email, "�û�ע��", code))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�ʼ���֤�����\"}}";
                }

                param.LoginName = email;

                param.DisplayName = ((AccountInfo)param).Name = ((AccountInfo)param).GlobalName = email;

                param.CertifiedEmail = email;

                if (MembershipManagement.Instance.AccountService.IsExistLoginName(param.LoginName))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�˵�¼���Ѿ����ڡ�\"}}";
                }
            }
            else if (registration == "mobile")
            {
                if (string.IsNullOrEmpty(mobile))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"������д�ֻ����롣\"}}";
                }

                if (MembershipManagement.Instance.AccountService.IsExistCertifiedMobile(mobile))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"���ֻ������Ѿ����ڡ�\"}}";
                }

                if (!VerificationCodeContext.Instance.VerificationCodeService.Validate("Mobile", mobile, "�û�ע��", code))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"������֤�����\"}}";
                }

                param.LoginName = mobile;

                param.DisplayName = ((AccountInfo)param).Name = mobile;

                param.CertifiedMobile = mobile;

                if (MembershipManagement.Instance.AccountService.IsExistLoginName(param.LoginName))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�˵�¼���Ѿ����ڡ�\"}}";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(name))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"������д��¼����ȫ�����ơ�\"}}";
                }

                if (MembershipManagement.Instance.AccountService.IsExistLoginNameAndGlobalName(loginName, name))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�˵�¼���Ѿ����ڡ�\"}}";
                }

                param.LoginName = loginName;

                ((AccountInfo)param).GlobalName = name;

                if (MembershipManagement.Instance.AccountService.IsExistLoginNameAndGlobalName(param.LoginName, param.GlobalName))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"�˵�¼���Ѿ����ڡ�\"}}";
                }
            }

            param.Id = DigitalNumberContext.Generate("Key_Guid");
            param.LoginDate = new DateTime(2000, 1, 1);
            param.Status = 1;
            param.IP = IPQueryContext.GetClientIP();

            param = MembershipManagement.Instance.AccountService.Save(param);

            if (param != null)
            {
                MembershipManagement.Instance.AccountService.SetPassword(param.Id, password);

                var result = this.service.Save(new MemberInfo() { Id = param.Id, AccountId = param.Id, Mobile = mobile });

                if (result != null)
                {
                    // �����ʺ�����״̬��Ϣ

                    string accountIdentity = DigitalNumberContext.Generate("Key_Guid");

                    SessionContext.Instance.Write(KernelContext.Current.AuthenticationManagement.GetAccountStorageStrategy(), accountIdentity, param);

                    HttpAuthenticationCookieSetter.SetUserCookies(accountIdentity);
                }
            }
            // this.RegisterMember(param.Id, doc);

            return "{\"message\":{\"returnCode\":0,\"value\":\"�ʺ�ע��ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // ��¼�ʺ�
        // -------------------------------------------------------

        #region ����:Auth(XmlDocument doc)
        /// <summary>��֤</summary>
        public string Auth(XmlDocument doc)
        {
            // -------------------------------------------------------
            // ��֤�� ��֤
            // -------------------------------------------------------

            // �ʺ���Ϣ
            IAccountInfo account = null;

            // �û���Ϣ
            IMemberInfo member = null;

            string loginName = XmlHelper.Fetch("loginName", doc);

            string password = XmlHelper.Fetch("password", doc);

            switch (KernelConfigurationView.Instance.AuthenticationManagementType)
            {
                // Http ��ʽ��֤ (��������)
                case "X3Platform.Membership.Authentication.HttpAuthenticationManagement,X3Platform.Membership":

                    string serverValidateCode = (HttpContext.Current.Session["ServerValidateCode"] == null ? string.Empty : HttpContext.Current.Session["AdminCheckCode"].ToString());

                    string clientValidateCode = XmlHelper.Fetch("validatecode", doc);

                    if (string.IsNullOrEmpty(clientValidateCode))
                    {
                        // -*- ��������֤�� -*- 

                        // 3.��֤��ʧЧ.
                        // return "{\"message\":{\"returnCode\":3,\"value\":\"��֤��ʧЧ��\"}}";
                    }
                    else if (clientValidateCode != serverValidateCode.ToUpper())
                    {
                        // 2.��֤�벻ƥ��.
                        return "{\"message\":{\"returnCode\":2,\"value\":\"��֤�벻ƥ�䡣\"}}";
                    }

                    account = MembershipManagement.Instance.AccountService.LoginCheck(loginName, password);
                    break;

                // Http ��ʽ��֤ (���Ի���)
                case "X3Platform.Membership.Authentication.MockAuthenticationManagement,X3Platform.Membership":
                    if (password == MembershipConfigurationView.Instance.MockAuthenticationPassword)
                    {
                        account = MembershipManagement.Instance.AccountService.FindOneByLoginName(loginName);
                    }
                    else
                    {
                        return "{\"message\":{\"returnCode\":1,\"value\":\"�������, ϵͳ��ǰ����֤��ʽ��ģ�������֤, �����ϵ����Ա��ȡ�������롣\"}}";
                    }

                    break;

                case "X3Platform.Membership.Authentication.NLMAuthenticationManagement,X3Platform.Membership":
                    return "{\"message\":{\"returnCode\":1,\"value\":\"ϵͳ��ǰ����֤��ʽ��Windows������֤, ��ʹ��Windows��֤��ʽ��¼��\"}}";

                case "X3Platform.Membership.Authentication.SSOAuthenticationManagement,X3Platform.Membership":
                    return "{\"message\":{\"returnCode\":1,\"value\":\"ϵͳ��ǰ����֤��ʽ�ǵ����¼��֤, ����Ż���¼��\"}}";
                default:
                    return "{\"message\":{\"returnCode\":1,\"value\":\"ϵͳδ�����κε�¼��ʽ, �����ϵ����Ա��\"}}";
            }

            if (account == null)
            {
                // 1.�û��������벻��ȷ.
                return "{\"message\":{\"returnCode\":1,\"value\":\"�û��������벻��ȷ��\"}}";
            }
            else
            {
                if (account.Status == 0)
                {
                    // 2.�û��������벻��ȷ.
                    return "{\"message\":{\"returnCode\":1,\"value\":\"���ʺű����ã�������������ϵ�ṩ����Ա��\"}}";
                }

                MembershipManagement.Instance.AccountService.SetIPAndLoginDate(account.Id, IPQueryContext.GetClientIP(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                member = this.service.FindOne(account.Id);

                // 4.���ʺ��޴�Ȩ��,����ϵ����Ա��
                if (member == null)
                {
                    return "{\"message\":{\"returnCode\":4,\"value\":\"���ʺ��޴�Ȩ��,����ϵ����Ա��\"}}";
                }

                // �����ʺ�����״̬��Ϣ

                member.Account.LoginName = loginName;

                member.Account.IP = IPQueryContext.GetClientIP();

                string accountIdentity = string.Format("{0}-{1}", account.Id, DigitalNumberContext.Generate("Key_Session"));

                KernelContext.Current.AuthenticationManagement.AddSession(string.Empty, accountIdentity, account);

                HttpAuthenticationCookieSetter.SetUserCookies(accountIdentity);

                // ���ñ��ص�¼�ʺ�
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("session-local-account", "{\"id\":\"" + account.Id + "\",\"name\":\"" + HttpUtility.UrlEncode(account.Name) + "\",\"loginName\":\"" + account.LoginName + "\"}"));
                // ���ñ��ط�����״̬
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("session-local-status", "1"));

                MembershipManagement.Instance.AccountLogService.Log(account.Id, "membership.member.quit", string.Format("��{0}���� {1} ��¼��ϵͳ����IP:{2}��", ((IAuthorizationObject)member.Account).Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), member.Account.IP));

                FormsAuthentication.SetAuthCookie(loginName, false);
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"��¼�ɹ���\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // �˳��ʺ�
        // -------------------------------------------------------

        #region ����:Quit(XmlDocument doc)
        /// <summary>�˳�</summary>
        public string Quit(XmlDocument doc)
        {
            string identityName = KernelContext.Current.AuthenticationManagement.IdentityName;

            // ��ȡ��ǰ�û���Ϣ
            IAccountInfo account = KernelContext.Current.User;

            KernelContext.Current.AuthenticationManagement.Logout();

            // -------------------------------------------------------
            // Session
            // -------------------------------------------------------

            HttpContext.Current.Session.Clear();

            // Mono 2.10.9 �汾�� InProc ģʽ�µ��� Session.Abandon() ���������´��� 
            // System.NullReferenceException: Object reference not set to an instance of an object at 
            // System.Web.SessionState.SessionInProcHandler.GetItemInternal (System.Web.HttpContext context, System.String id, System.Boolean& locked, System.TimeSpan& lockAge, System.Object& lockId, System.Web.SessionState.SessionStateActions& actions, Boolean exclusive)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                HttpContext.Current.Session.Abandon();
            }

            HttpAuthenticationCookieSetter.ClearUserCookies();

            HttpContext.Current.Response.Cookies[identityName].Value = null;
            HttpContext.Current.Response.Cookies[identityName].Expires = DateTime.Now.AddDays(-1);

            // -------------------------------------------------------
            // IIdentity
            // -------------------------------------------------------

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }

            // ��¼�ʺŲ�����־
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "membership.member.quit", string.Format("��{0}���� {1} �˳���ϵͳ����IP:{2}��", account.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), IPQueryContext.GetClientIP()));

            return "{\"message\":{\"returnCode\":0,\"value\":\"�ѳɹ��˳���\"}}";
        }
        #endregion
    }
}
