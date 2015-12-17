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
        private IMemberService service = MembershipManagement.Instance.MemberService; // 数据服务

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            MemberInfo param = new MemberInfo();

            param = (MemberInfo)AjaxUtil.Deserialize<MemberInfo>(param, doc);

            this.service.Save(param);

            return "{message:{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string ids = XmlHelper.Fetch("ids", doc);

            this.service.Delete(ids);

            return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = XmlHelper.Fetch("id", doc);

            IMemberInfo param = this.service.FindOne(id);

            if (param == null) { return "{\"message\":{\"returnCode\":0,\"value\":\"未找到相关对象。\"}}"; }

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(param) + ",");

            outString.Insert(outString.Length - 2, string.Format(",\"account\":{0}", AjaxUtil.Parse<IAccountInfo>(param.Account)));

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("findOneByAccountId")]
        public string FindOneByAccountId(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountId = XmlHelper.Fetch("accountId", doc);

            IMemberInfo param = this.service.FindOneByAccountId(accountId);

            if (param == null) { return "{\"message\":{\"returnCode\":0,\"value\":\"未找到相关对象。\"}}"; }

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(param) + ",");

            outString.Insert(outString.Length - 2, string.Format(",\"account\":{0}", AjaxUtil.Parse<IAccountInfo>(param.Account)));

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllWithoutDefaultOrganizationUnit(XmlDocument doc)
        /// <summary>查询所有没有默认组织的用户信息</summary>
        /// <returns>返回一个相关的实例列表.</returns> 
        [AjaxMethod("findAllWithoutDefaultOrganizationUnit")]
        public string FindAllWithoutDefaultOrganizationUnit(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IMemberInfo> list = this.service.FindAllWithoutDefaultOrganizationUnit(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllWithoutDefaultJob(XmlDocument doc)
        /// <summary>查询所有没有默认职位的用户信息</summary>
        /// <returns>返回一个相关的实例列表.</returns> 
        [AjaxMethod("findAllWithoutDefaultJob")]
        public string FindAllWithoutDefaultJob(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IMemberInfo> list = this.service.FindAllWithoutDefaultJob(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllWithoutDefaultAssignedJob(XmlDocument doc)
        /// <summary>查询所有没有默认岗位的用户信息</summary>
        /// <returns>返回一个相关的实例列表.</returns> 
        [AjaxMethod("findAllWithoutDefaultAssignedJob")]
        public string FindAllWithoutDefaultAssignedJob(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IMemberInfo> list = this.service.FindAllWithoutDefaultAssignedJob(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindAllWithoutDefaultRole(XmlDocument doc)
        /// <summary>查询所有没有默认角色的用户信息</summary>
        /// <returns>返回一个相关的实例列表.</returns> 
        [AjaxMethod("findAllWithoutDefaultRole")]
        public string FindAllWithoutDefaultRole(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

            IList<IMemberInfo> list = this.service.FindAllWithoutDefaultRole(length);

            outString.Append("{\"data\":" + AjaxUtil.Parse<IMemberInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns> 
        public string GetPaging(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

            // 设置当前用户权限
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
            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"},");
            // 兼容 ExtJS 设置
            outString.Append("\"metaData\":{\"root\":\"data\",\"idProperty\":\"id\",\"totalProperty\":\"total\",\"successProperty\":\"success\",\"messageProperty\": \"message\"},");
            outString.Append("\"total\":" + paging.RowCount + ",");
            outString.Append("\"success\":1,");
            outString.Append("\"msg\":\"success\"}");

            return outString.ToString();
        }
        #endregion

        #region 函数:SetDefaultRole(XmlDocument doc)
        /// <summary>查询所有没有默认角色的用户信息</summary>
        /// <returns>返回一个相关的实例列表.</returns> 
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

            // 记录帐号操作日志
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "设置联系方式", "【" + account.Name + "】更新了自己的联系信息，【IP:" + IPQueryContext.GetClientIP() + "】。", account.Id);

            return "{message:{\"returnCode\":0,\"value\":\"设置联系信息成功。\"}}";
        }
        #endregion

        #region 函数:SetDefaultRole(XmlDocument doc)
        /// <summary>查询所有没有默认角色的用户信息</summary>
        /// <returns>返回一个相关的实例列表.</returns> 
        [AjaxMethod("setDefaultRole")]
        public string SetDefaultRole(XmlDocument doc)
        {
            string accountId = XmlHelper.Fetch("accountId", doc);

            string roleId = XmlHelper.Fetch("roleId", doc);

            int resultCode = this.service.SetDefaultRole(accountId, roleId);

            return "{message:{\"returnCode\":0,\"value\":\"设置默认角色成功。\"}}";
        }
        #endregion

        #region 函数:Read(XmlDocument doc)
        /// <summary>读取缓存数据</summary>
        /// <returns></returns>
        [AjaxMethod("read")]
        public string Read(XmlDocument doc)
        {
            string key = XmlHelper.Fetch("key", doc);

            StringBuilder outString = new StringBuilder();

            IAccountStorageStrategy strategy = KernelContext.Current.AuthenticationManagement.GetAccountStorageStrategy();

            IAccountInfo param = strategy.Deserialize(SessionContext.Instance.AccountCacheService.Read(key));

            outString.Append("{\"data\":" + AjaxUtil.Parse<IAccountInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 注册帐号
        // -------------------------------------------------------

        #region 函数:Register(XmlDocument doc)
        /// <summary>注册帐号</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns></returns>
        public string Register(XmlDocument doc)
        {
            IAccountInfo param = new AccountInfo();

            // Registration 注册类型: email | mobile | default
            string registration = XmlHelper.Fetch("registration", doc);
            // 登陆名
            string loginName = XmlHelper.Fetch("loginName", doc);
            // 姓名
            string name = XmlHelper.Fetch("name", doc);
            // 手机号码
            string mobile = XmlHelper.Fetch("mobile", doc);
            // 邮箱
            string email = XmlHelper.Fetch("email", doc);
            // 密码
            string password = XmlHelper.Fetch("password", doc);

            string code = XmlHelper.Fetch("code", doc);

            if (registration == "mail")
            {
                if (string.IsNullOrEmpty(email))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"必须填写电子邮箱。\"}}";
                }

                if (MembershipManagement.Instance.AccountService.IsExistCertifiedEmail(email))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"此邮箱已经存在。\"}}";
                }

                if (!VerificationCodeContext.Instance.VerificationCodeService.Validate("Mail", email, "用户注册", code))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"邮件验证码错误。\"}}";
                }

                param.LoginName = email;

                param.DisplayName = ((AccountInfo)param).Name = ((AccountInfo)param).GlobalName = email;

                param.CertifiedEmail = email;

                if (MembershipManagement.Instance.AccountService.IsExistLoginName(param.LoginName))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"此登录名已经存在。\"}}";
                }
            }
            else if (registration == "mobile")
            {
                if (string.IsNullOrEmpty(mobile))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"必须填写手机号码。\"}}";
                }

                if (MembershipManagement.Instance.AccountService.IsExistCertifiedMobile(mobile))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"此手机号码已经存在。\"}}";
                }

                if (!VerificationCodeContext.Instance.VerificationCodeService.Validate("Mobile", mobile, "用户注册", code))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"短信验证码错误。\"}}";
                }

                param.LoginName = mobile;

                param.DisplayName = ((AccountInfo)param).Name = mobile;

                param.CertifiedMobile = mobile;

                if (MembershipManagement.Instance.AccountService.IsExistLoginName(param.LoginName))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"此登录名已经存在。\"}}";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(name))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"必须填写登录名和全局名称。\"}}";
                }

                if (MembershipManagement.Instance.AccountService.IsExistLoginNameAndGlobalName(loginName, name))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"此登录名已经存在。\"}}";
                }

                param.LoginName = loginName;

                ((AccountInfo)param).GlobalName = name;

                if (MembershipManagement.Instance.AccountService.IsExistLoginNameAndGlobalName(param.LoginName, param.GlobalName))
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"此登录名已经存在。\"}}";
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
                    // 设置帐号在线状态信息

                    string accountIdentity = DigitalNumberContext.Generate("Key_Guid");

                    SessionContext.Instance.Write(KernelContext.Current.AuthenticationManagement.GetAccountStorageStrategy(), accountIdentity, param);

                    HttpAuthenticationCookieSetter.SetUserCookies(accountIdentity);
                }
            }
            // this.RegisterMember(param.Id, doc);

            return "{\"message\":{\"returnCode\":0,\"value\":\"帐号注册成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 登录帐号
        // -------------------------------------------------------

        #region 函数:Auth(XmlDocument doc)
        /// <summary>验证</summary>
        public string Auth(XmlDocument doc)
        {
            // -------------------------------------------------------
            // 验证码 验证
            // -------------------------------------------------------

            // 帐号信息
            IAccountInfo account = null;

            // 用户信息
            IMemberInfo member = null;

            string loginName = XmlHelper.Fetch("loginName", doc);

            string password = XmlHelper.Fetch("password", doc);

            switch (KernelConfigurationView.Instance.AuthenticationManagementType)
            {
                // Http 方式验证 (生产环境)
                case "X3Platform.Membership.Authentication.HttpAuthenticationManagement,X3Platform.Membership":

                    string serverValidateCode = (HttpContext.Current.Session["ServerValidateCode"] == null ? string.Empty : HttpContext.Current.Session["AdminCheckCode"].ToString());

                    string clientValidateCode = XmlHelper.Fetch("validatecode", doc);

                    if (string.IsNullOrEmpty(clientValidateCode))
                    {
                        // -*- 不设置验证码 -*- 

                        // 3.验证码失效.
                        // return "{\"message\":{\"returnCode\":3,\"value\":\"验证码失效。\"}}";
                    }
                    else if (clientValidateCode != serverValidateCode.ToUpper())
                    {
                        // 2.验证码不匹配.
                        return "{\"message\":{\"returnCode\":2,\"value\":\"验证码不匹配。\"}}";
                    }

                    account = MembershipManagement.Instance.AccountService.LoginCheck(loginName, password);
                    break;

                // Http 方式验证 (测试环境)
                case "X3Platform.Membership.Authentication.MockAuthenticationManagement,X3Platform.Membership":
                    if (password == MembershipConfigurationView.Instance.MockAuthenticationPassword)
                    {
                        account = MembershipManagement.Instance.AccountService.FindOneByLoginName(loginName);
                    }
                    else
                    {
                        return "{\"message\":{\"returnCode\":1,\"value\":\"密码错误, 系统当前的验证方式是模拟测试验证, 请从联系管理员获取测试密码。\"}}";
                    }

                    break;

                case "X3Platform.Membership.Authentication.NLMAuthenticationManagement,X3Platform.Membership":
                    return "{\"message\":{\"returnCode\":1,\"value\":\"系统当前的验证方式是Windows集成验证, 请使用Windows验证方式登录。\"}}";

                case "X3Platform.Membership.Authentication.SSOAuthenticationManagement,X3Platform.Membership":
                    return "{\"message\":{\"returnCode\":1,\"value\":\"系统当前的验证方式是单点登录验证, 请从门户登录。\"}}";
                default:
                    return "{\"message\":{\"returnCode\":1,\"value\":\"系统未设置任何登录方式, 请从联系管理员。\"}}";
            }

            if (account == null)
            {
                // 1.用户名和密码不正确.
                return "{\"message\":{\"returnCode\":1,\"value\":\"用户名和密码不正确。\"}}";
            }
            else
            {
                if (account.Status == 0)
                {
                    // 2.用户名和密码不正确.
                    return "{\"message\":{\"returnCode\":1,\"value\":\"此帐号被禁用，如有问题请联系提供管理员。\"}}";
                }

                MembershipManagement.Instance.AccountService.SetIPAndLoginDate(account.Id, IPQueryContext.GetClientIP(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                member = this.service.FindOne(account.Id);

                // 4.此帐号无此权限,请联系管理员。
                if (member == null)
                {
                    return "{\"message\":{\"returnCode\":4,\"value\":\"此帐号无此权限,请联系管理员。\"}}";
                }

                // 设置帐号在线状态信息

                member.Account.LoginName = loginName;

                member.Account.IP = IPQueryContext.GetClientIP();

                string accountIdentity = string.Format("{0}-{1}", account.Id, DigitalNumberContext.Generate("Key_Session"));

                KernelContext.Current.AuthenticationManagement.AddSession(string.Empty, accountIdentity, account);

                HttpAuthenticationCookieSetter.SetUserCookies(accountIdentity);

                // 设置本地登录帐号
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("session-local-account", "{\"id\":\"" + account.Id + "\",\"name\":\"" + HttpUtility.UrlEncode(account.Name) + "\",\"loginName\":\"" + account.LoginName + "\"}"));
                // 设置本地服务器状态
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("session-local-status", "1"));

                MembershipManagement.Instance.AccountLogService.Log(account.Id, "membership.member.quit", string.Format("【{0}】在 {1} 登录了系统。【IP:{2}】", ((IAuthorizationObject)member.Account).Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), member.Account.IP));

                FormsAuthentication.SetAuthCookie(loginName, false);
            }

            return "{\"message\":{\"returnCode\":0,\"value\":\"登录成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 退出帐号
        // -------------------------------------------------------

        #region 函数:Quit(XmlDocument doc)
        /// <summary>退出</summary>
        public string Quit(XmlDocument doc)
        {
            string identityName = KernelContext.Current.AuthenticationManagement.IdentityName;

            // 获取当前用户信息
            IAccountInfo account = KernelContext.Current.User;

            KernelContext.Current.AuthenticationManagement.Logout();

            // -------------------------------------------------------
            // Session
            // -------------------------------------------------------

            HttpContext.Current.Session.Clear();

            // Mono 2.10.9 版本的 InProc 模式下调用 Session.Abandon() 会引发如下错误 
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

            // 记录帐号操作日志
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "membership.member.quit", string.Format("【{0}】在 {1} 退出了系统。【IP:{2}】", account.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), IPQueryContext.GetClientIP()));

            return "{\"message\":{\"returnCode\":0,\"value\":\"已成功退出。\"}}";
        }
        #endregion
    }
}
