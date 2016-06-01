namespace X3Platform.Membership.HumanResources.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.Membership.Model;
    using X3Platform.Spring;

    using X3Platform.Membership.HumanResources.Configuration;
    using X3Platform.Membership.HumanResources.IBLL;

    /// <summary></summary>
    public class GeneralAccountService : IGeneralAccountService
    {
        #region 函数:SetMemberCard(MemberInfo param)
        ///<summary>设置个人信息卡片</summary>
        ///<param name="param">实例的详细信息</param>
        public void SetMemberCard(MemberInfo param)
        {
            if (string.IsNullOrEmpty(param.AccountId))
            {
                throw new Exception("【帐号标识(AccountId)】必须填写。");
            }

            param.Id = param.AccountId;

            MembershipManagement.Instance.AccountService.Save(param.Account);

            MembershipManagement.Instance.MemberService.Save(param);
        }
        #endregion

        #region 函数:GetCertifiedAvatar(string accountId)
        ///<summary>获取相关帐号的头像信息</summary>
        ///<param name="accountId">实例的详细信息</param>
        public string GetCertifiedAvatar(string accountId)
        {
            if (string.IsNullOrEmpty(accountId)) { return string.Empty; }

            IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

            return GetCertifiedAvatar(account);
        }
        #endregion

        #region 函数:GetCertifiedAvatar(IAccountInfo account)
        ///<summary>获取相关帐号的头像信息</summary>
        ///<param name="accountId">实例的详细信息</param>
        public string GetCertifiedAvatar(IAccountInfo account)
        {
            if (account == null) { return string.Empty; }

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

        #region 函数:ChangePassword(string password, string originalPassword)
        public int ChangePassword(string password, string originalPassword)
        {
            IAccountInfo account = KernelContext.Current.User;

            return MembershipManagement.Instance.AccountService.ChangePassword(account.LoginName, password, originalPassword);
        }
        #endregion

        #region 函数:ChangeLoginName(string loginName)
        public int ChangeLoginName(string loginName)
        {
            IAccountInfo account = KernelContext.Current.User;

            IAccountInfo newAccount = MembershipManagement.Instance.AccountService.FindOneByLoginName(loginName);

            if (newAccount == null)
            {
                MembershipManagement.Instance.AccountService.SetLoginName(account.Id, loginName);
            }
            else if (newAccount.Id == account.Id)
            {
                // 当前登陆名和 不进行操作
            }
            else
            {
                throw new GenericException("1", "已存在登陆名");
            }

            return 0;
        }
        #endregion
    }
}