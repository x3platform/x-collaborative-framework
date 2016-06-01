namespace X3Platform.Membership.HumanResources.Ajax
{
    using System;
    using System.Net.Mail;
    using System.Web;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Location.IPQuery;
    using X3Platform.Membership.Model;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.Membership.HumanResources.IBLL;
    using X3Platform.Membership.HumanResources.Model;
    using X3Platform.Util;
    using X3Platform.Email.Client;
    using X3Platform.Security.VerificationCode;
    using X3Platform.DigitalNumber;
    using X3Platform.Security;
    using X3Platform.TemplateContent;
    using X3Platform.Json;
    using X3Platform.SMS.Client;
    using X3Platform.Security.VerificationCode.Configuration;
    using X3Platform.Security.Configuration;
    using X3Platform.Globalization; using X3Platform.Messages;

    public class GeneralAccountWrapper : ContextWrapper
    {
        private IGeneralAccountService service = HumanResourceManagement.Instance.GeneralAccountService;

        //-------------------------------------------------------
        // 保存 删除
        //-------------------------------------------------------

        #region 函数:SetMemberCard(XmlDocument doc)
        /// <summary>设置员工卡片信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SetMemberCard(XmlDocument doc)
        {
            IAccountInfo account = KernelContext.Current.User;

            MemberInfo member = new MemberInfo();

            MemberExtensionInformation memberProperties = new MemberExtensionInformation();

            member = (MemberInfo)AjaxUtil.Deserialize(member, doc);

            member.ExtensionInformation.Load(doc);

            // 更新自己的帐号信息
            member.Id = member.AccountId = account.Id;

            if (string.IsNullOrEmpty(member.AccountId))
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"必须填写相关帐号标识。\"}}";
            }
            else
            {
                member.Account.IdentityCard = XmlHelper.Fetch("identityCard", doc);
            }

            this.service.SetMemberCard(member);

            // 记录帐号操作日志
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "hr.general.setMemberCard", "【" + account.Name + "】更新了自己的个人信息，【IP:" + IPQueryContext.GetClientIP() + "】。", account.Id);

            return MessageObject.Stringify("0", I18n.Strings["msg_save_success"]);
        }
        #endregion

        #region 函数:GetCertifiedAvatar(XmlDocument doc)
        /// <summary>获取头像信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetCertifiedAvatar(XmlDocument doc)
        {
            string accountId = XmlHelper.Fetch("accountId", doc);

            IAccountInfo account = null;

            if (string.IsNullOrEmpty(accountId))
            {
                account = KernelContext.Current.User;
            }
            else
            {
                account = MembershipManagement.Instance.AccountService[accountId];
            }

            string avatar_120x120 = string.Empty;

            if (string.IsNullOrEmpty(account.CertifiedAvatar))
            {
                avatar_120x120 = AttachmentStorageConfigurationView.Instance.VirtualUploadFolder + "avatar/default_120x120.png";
            }
            else
            {
                avatar_120x120 = account.CertifiedAvatar.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.VirtualUploadFolder);
            }

            return avatar_120x120;
        }
        #endregion

        #region 函数:ChangePassword(XmlDocument doc)
        /// <summary>修改密码</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string ChangePassword(XmlDocument doc)
        {
            string password = XmlHelper.Fetch("password", doc);

            string originalPassword = XmlHelper.Fetch("originalPassword", doc);

            int result = service.ChangePassword(password, originalPassword);

            if (result == 0)
            {
                return "{message:{\"returnCode\":0,\"value\":\"修改成功。\"}}";
            }
            else
            {
                return "{message:{\"returnCode\":1,\"value\":\"修改失败, 用户或密码错误.\"}}";
            }
        }
        #endregion

        #region 函数:ChangeLoginName(XmlDocument doc)
        /// <summary>修改密码</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string ChangeLoginName(XmlDocument doc)
        {
            string loginName = XmlHelper.Fetch("loginName", doc);
            
            int result = service.ChangeLoginName(loginName);

            if (result == 0)
            {
                return MessageObject.Stringify("0", "修改成功");
            }
            else
            {
                return "{message:{\"returnCode\":1,\"value\":\"修改失败, 用户或密码错误.\"}}";
            }
        }
        #endregion

        #region 函数:ForgotPassword(XmlDocument doc)
        /// <summary>忘记密码</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string ForgotPassword(XmlDocument doc)
        {
            string mobile = XmlHelper.Fetch("mobile", doc);
            string email = XmlHelper.Fetch("email", doc);

            IAccountInfo account = null;

            if (!string.IsNullOrEmpty(mobile))
            {
                account = MembershipManagement.Instance.AccountService.FindOneByCertifiedMobile(mobile);

                if (account != null)
                {
                    VerificationCodeInfo verificationCode = VerificationCodeContext.Instance.VerificationCodeService.Create("Mobile", mobile, "忘记密码");

                    EmailClientContext.Instance.Send(mobile, "密码找回", string.Format("尊敬的用户，\r\n您好，您正在通过忘记密码功能找回密码。当前验证码:{0}, \r\nIP:{1}\r\nDate:{2} ", verificationCode.Code, IPQueryContext.GetClientIP(), DateTime.Now.ToString()));

                    return "{message:{\"returnCode\":0,\"value\":\"验证码发送成功。\"}}";
                }
                else
                {
                    return "{message:{\"returnCode\":1,\"value\":\"验证码发送失败，不存在的手机号码。\"}}";
                }
            }
            else if (!string.IsNullOrEmpty(email))
            {
                account = MembershipManagement.Instance.AccountService.FindOneByCertifiedEmail(email);

                if (account != null)
                {
                    VerificationCodeInfo verificationCode = VerificationCodeContext.Instance.VerificationCodeService.Create("Mail", email, "忘记密码");

                    IPQueryContext.GetClientIP();

                    // 您好 {date} 您通过了忘记密码功能找回密码。验证码: {code}
                    // 
                    // IP 地址
                    EmailClientContext.Instance.Send(email, "密码找回", string.Format("尊敬的用户，\r\n您好，您正在通过忘记密码功能找回密码。当前验证码:{0}, \r\nIP:{1}\r\nDate:{2} ", verificationCode.Code, IPQueryContext.GetClientIP(), DateTime.Now.ToString()));

                    return "{message:{\"returnCode\":0,\"value\":\"验证码发送成功。\"}}";
                }
                else
                {
                    return "{message:{\"returnCode\":1,\"value\":\"验证码发送失败，不存在的邮箱地址。\"}}";
                }
            }

            return "{message:{\"returnCode\":2,\"value\":\"必须填写手机号码或邮箱地址。\"}}";
        }
        #endregion

        #region 函数:SetPassword(XmlDocument doc)
        /// <summary>忘记密码</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SetPassword(XmlDocument doc)
        {
            // Registration 注册类型: email | mobile | default
            string registration = XmlHelper.Fetch("registration", doc);
            // 手机号码
            string mobile = XmlHelper.Fetch("mobile", doc);
            // 邮箱地址
            string email = XmlHelper.Fetch("email", doc);
            // 验证码
            string code = XmlHelper.Fetch("code", doc);
            // 密码
            string password = XmlHelper.Fetch("password", doc);

            VerificationCodeInfo verificationCode = null;

            IAccountInfo account = null;

            if (registration == "mail")
            {
                verificationCode = VerificationCodeContext.Instance.VerificationCodeService.FindOne(VerificationObjectType.Mail.ToString(), email, "忘记密码");
            }
            else if (registration == "mobile")
            {
                verificationCode = VerificationCodeContext.Instance.VerificationCodeService.FindOne(VerificationObjectType.Mobile.ToString(), mobile, "忘记密码");
            }

            if (verificationCode.Code != code)
            {
                return "{message:{\"returnCode\":2,\"value\":\"验证码错误。\"}}";
            }

            if (registration == "mail")
            {
                account = MembershipManagement.Instance.AccountService.FindOneByCertifiedEmail(email);
            }
            else if (registration == "mobile")
            {
                account = MembershipManagement.Instance.AccountService.FindOneByCertifiedMobile(mobile);
            }

            if (account != null)
            {
                // 生成一个包含大写字母、小写字母、数字、特殊字符的八位长度的字符串
                /*
                string password = Guid.NewGuid().ToString().Substring(0, 5);

                Random random = new Random();

                password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("!@#$", 1));

                password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("abcdefghijkmnpqrstuvwxyz", 1));

                password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("ABCDEFGHJKLMNPQRSTUVWXYZ", 1));

                MembershipManagement.Instance.AccountService.SetPassword(account.Id, Encrypter.EncryptSHA1(password));
                */

                // 通过了{date}忘记密码功能找回密码
                // IP 地址
                // EmailClientContext.Instance.Send(email, "密码找回", "新的密码:" + password);

                MembershipManagement.Instance.AccountService.SetPassword(account.Id, password);

                return "{message:{\"returnCode\":0,\"value\":\"密码设置成功。\"}}";
            }
            else
            {
                return "{message:{\"returnCode\":1,\"value\":\"不存在的用户信息。\"}}";
            }

        }
        #endregion

        #region 函数:SendVerificationMail(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SendVerificationMail(XmlDocument doc)
        {
            if (SecurityConfigurationView.Instance.CaptchaMode == "ON")
            {
                // 验证码
                string captcha = XmlHelper.Fetch("captcha", doc);

                if (HttpContext.Current.Session["captcha"] == null || captcha != HttpContext.Current.Session["captcha"].ToString())
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"验证码错误。\"}}";
                }
            }

            // 检查验证码发送时间
            if (HttpContext.Current.Session["VerificationCodeSendTime"] != null && HttpContext.Current.Session["VerificationCodeSendTime"] is DateTime)
            {
                DateTime time = (DateTime)HttpContext.Current.Session["VerificationCodeSendTime"];

                if (time.AddSeconds(VerificationCodeConfigurationView.Instance.SendInterval) > DateTime.Now)
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"发送太频繁，请稍后再试。\"}}";
                }
            }

            // 邮件地址
            string email = XmlHelper.Fetch("email", doc);
            // 验证类型
            string validationType = XmlHelper.Fetch("validationType", doc);

            VerificationCodeInfo verificationCode = VerificationCodeContext.Instance.VerificationCodeService.Create("Mail", email, validationType, IPQueryContext.GetClientIP());

            VerificationCodeTemplateInfo template = VerificationCodeContext.Instance.VerificationCodeTemplateService.FindOne("Mail", validationType);

            JsonData options = JsonMapper.ToObject(template.Options);

            string content = TemplateContentContext.Instance.TemplateContentService.GetHtml(template.TemplateContentName);

            EmailClientContext.Instance.Send(email, JsonHelper.GetDataValue(options, "subject"), string.Format(content, verificationCode.Code), EmailFormat.Html);

            HttpContext.Current.Session["VerificationCodeSendTime"] = DateTime.Now;

            return "{\"message\":{\"returnCode\":0,\"value\":\"发送成功。\"}}";
        }
        #endregion

        #region 函数:SendVerificationSMS(XmlDocument doc)
        /// <summary></summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SendVerificationSMS(XmlDocument doc)
        {
            if (SecurityConfigurationView.Instance.CaptchaMode == "ON")
            {
                // 验证码
                string captcha = XmlHelper.Fetch("captcha", doc);

                if (HttpContext.Current.Session["captcha"] == null || captcha != HttpContext.Current.Session["captcha"].ToString())
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"验证码错误。\"}}";
                }
            }

            // 检查验证码发送时间
            if (HttpContext.Current.Session["VerificationCodeSendTime"] != null && HttpContext.Current.Session["VerificationCodeSendTime"] is DateTime)
            {
                DateTime time = (DateTime)HttpContext.Current.Session["VerificationCodeSendTime"];

                if (time.AddSeconds(VerificationCodeConfigurationView.Instance.SendInterval) > DateTime.Now)
                {
                    return "{\"message\":{\"returnCode\":1,\"value\":\"发送太频繁，请稍后再试。\"}}";
                }
            }

            // 手机号码
            string phoneNumber = XmlHelper.Fetch("phoneNumber", doc);
            // 验证类型
            string validationType = XmlHelper.Fetch("validationType", doc);

            // 发送短信
            SMSContext.Instance.SMSService.Send(phoneNumber, validationType);

            HttpContext.Current.Session["VerificationCodeSendTime"] = DateTime.Now;

            return "{\"message\":{\"returnCode\":0,\"value\":\"发送成功。\"}}";
        }
        #endregion
    }
}