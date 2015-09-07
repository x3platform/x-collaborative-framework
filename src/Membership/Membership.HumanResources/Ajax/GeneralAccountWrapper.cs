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
  using X3Platform.Email.Client;
  using System;
  using X3Platform.Security.VerificationCode;
  using X3Platform.DigitalNumber;

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

    #region 函数:ForgotPassword(XmlDocument doc)
    /// <summary>忘记密码</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string ForgotPassword(XmlDocument doc)
    {
      string email = XmlHelper.Fetch("email", doc);

      IAccountInfo account = MembershipManagement.Instance.AccountService.FindOneByCertifiedEmail(email);

      if (account != null)
      {
        VerificationCodeInfo verificationCode = VerificationCodeContext.Instance.VerificationCodeService.Create("Mail", email, "forgot_password");
        IPQueryContext.GetClientIP();
        // 您好 {date} 您通过了忘记密码功能找回密码。验证码: {code}
        // 
        // IP 地址
        EmailClientContext.Instance.Send(email, "密码找回", string.Format("您好，您通过了忘记密码功能找回密码。验证码:{0}, \r\nIP:{1}\r\nDate:{2} ", verificationCode.Code, IPQueryContext.GetClientIP(), DateTime.Now.ToString()));

        return "{message:{\"returnCode\":0,\"value\":\"邮件发送成功。\"}}";
      }
      else
      {
        return "{message:{\"returnCode\":1,\"value\":\"邮件发送失败，不存在的邮箱地址。\"}}";
      }
    }
    #endregion

    #region 函数:SetPassword(XmlDocument doc)
    /// <summary>忘记密码</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string SetPassword(XmlDocument doc)
    {
      // 邮箱地址
      string email = XmlHelper.Fetch("email", doc);
      // 验证码
      string code = XmlHelper.Fetch("code", doc);

      VerificationCodeInfo verificationCode = VerificationCodeContext.Instance.VerificationCodeService.FindOne("Mail", email, "ForgotPassword");
      
      if (verificationCode.Code != code)
      {
        return "{message:{\"returnCode\":1,\"value\":\"邮件发送失败，不存在的邮箱地址。\"}}";
      }

      IAccountInfo account = MembershipManagement.Instance.AccountService.FindOneByCertifiedEmail(email);

      if (account != null)
      {
        // 生成一个包含大写字母、小写字母、数字、特殊字符的八位长度的字符串
        string password = Guid.NewGuid().ToString().Substring(0, 5);

        Random random = new Random();

        password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("!@#$", 1));

        password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("abcdefghijkmnpqrstuvwxyz", 1));

        password = password.Insert(random.Next(password.Length), StringHelper.ToRandom("ABCDEFGHJKLMNPQRSTUVWXYZ", 1));

        MembershipManagement.Instance.AccountService.SetPassword(account.Id, password);
        // 通过了{date}忘记密码功能找回密码
        // IP 地址
        EmailClientContext.Instance.Send(email, "密码找回", "新的密码:" + password);

        return "{message:{\"returnCode\":0,\"value\":\"邮件发送成功。\"}}";
      }
      else
      {
        return "{message:{\"returnCode\":1,\"value\":\"邮件发送失败，不存在的邮箱地址。\"}}";
      }
    }
    #endregion
  }
}