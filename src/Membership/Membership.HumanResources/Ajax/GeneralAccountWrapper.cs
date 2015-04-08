namespace X3Platform.Membership.HumanResources.Ajax
{
    using System.Xml;
    using X3Platform.Ajax;
    using X3Platform.Location.IPQuery;
    using X3Platform.Membership.Model;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.Membership.HumanResources.IBLL;
    using X3Platform.Membership.HumanResources.Model;
    using X3Platform.Util;

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
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "设置个人信息", "【" + account.Name + "】更新了自己的个人信息，【IP:" + IPQueryContext.GetClientIP() + "】。", account.Id);

            return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:GetCertifiedAvatar(XmlDocument doc)
        /// <summary>设置员工卡片信息</summary>
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
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("changePassword")]
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
    }
}