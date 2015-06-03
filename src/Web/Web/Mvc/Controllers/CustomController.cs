namespace X3Platform.Web.Mvc.Controllers
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Web.Mvc;

  using X3Platform.Configuration;
  using X3Platform.Membership;

  using X3Platform.Web.Builder;
  using X3Platform.Web.UserAgents;
  using X3Platform.Security;
  using X3Platform.DigitalNumber;
  using X3Platform.Util;
  using X3Platform.Web.Configuration;
  #endregion

  public class CustomController : Controller
  {
    /// <summary>初始化时间，主要用于计算页面加载速度</summary>
    protected DateTime initializedTime;

    #region 属性:Account
    private IAccountInfo m_Account = null;

    /// <summary>帐户信息</summary>
    public IAccountInfo Account { get { return this.m_Account; } }
    #endregion

    #region 属性:Client
    private ClientInfo m_Client = null;

    /// <summary>客户端信息</summary>
    public ClientInfo Client { get { return this.m_Client; } }
    #endregion

    #region 属性:DeviceType
    private DeviceType m_DeviceType = DeviceType.Other;

    /// <summary>设备类型</summary>
    public DeviceType DeviceType { get { return this.m_DeviceType; } }
    #endregion

    #region 属性:Scripts
    private IList<string> m_Scripts = null;

    /// <summary>外部脚本链接集合</summary>
    public IList<string> Scripts
    {
      get
      {
        if (m_Scripts == null)
        {
          m_Scripts = new List<string>();
        }
        return m_Scripts;
      }
    }
    #endregion

    #region 属性:Styles
    private IList<string> m_Styles = null;

    /// <summary>外部样式链接集合</summary>
    public IList<string> Styles
    {
      get
      {
        if (m_Styles == null)
        {
          m_Styles = new List<string>();
        }

        return m_Styles;
      }
    }
    #endregion

    /// <summary></summary>
    /// <param name="filterContext"></param>
    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      base.OnActionExecuting(filterContext);

      // 记录页面初始化时间
      this.initializedTime = DateTime.Now;

      ViewData["themeName"] = WebConfigurationView.Instance.ThemeName;

      ViewData["client"] = this.m_Client = X3Platform.Web.UserAgents.Parser.GetDefault().Parse(Request.UserAgent);
      ViewData["deviceType"] = this.m_DeviceType = DeviceTypeParser.Parse(this.Client);
      ViewData["account"] = this.m_Account = KernelContext.Current.User;

      Response.Cookies["device-type"].Value = this.m_DeviceType.ToString();
      Response.Cookies["device-type"].HttpOnly = true;

      // 当前年份信息
      ViewData["year"] = this.initializedTime.Year;
    }

    protected override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      // 系统名称
      ViewData["systemName"] = KernelConfigurationView.Instance.SystemName;
      // 系统状态
      ViewData["systemStatus"] = KernelConfigurationView.Instance.SystemStatus;
      // 版本
      ViewData["version"] = KernelConfigurationView.Instance.Version;
      // 默认域名
      ViewData["domain"] = KernelConfigurationView.Instance.Domain;
      // 身份标识名称
      ViewData["identityName"] = KernelContext.Current.AuthenticationManagement.IdentityName;
      // 给Session对象赋值，固定取得SessionID
      HttpContext.Session["__session__ticket__"] = this.initializedTime;
      // 会话标识
      ViewData["session"] = HttpContext.Session.SessionID;
      // 时间间隔
      ViewData["timeSpan"] = DateHelper.GetTimeSpan(this.initializedTime);

      // -------------------------------------------------------
      // 生成签名
      // -------------------------------------------------------

      ViewData["clientId"] = KernelConfigurationView.Instance.ApplicationClientId;
      // 时间戳
      ViewData["timestamp"] = DateHelper.GetTimestamp();
      // 随机数
      ViewData["nonce"] = DigitalNumberContext.Generate("Key_Nonce");
      // 签名
      ViewData["clientSignature"] = Encrypter.EncryptSHA1(Encrypter.SortAndConcat(
          KernelConfigurationView.Instance.ApplicationClientSecret,
          ViewData["timestamp"].ToString(),
          ViewData["nonce"].ToString()));

      base.OnActionExecuted(filterContext);
    }

    /// <summary>自动定位到设备相关的目录</summary>
    public virtual string LocateFolder(string folderName)
    {
      if (this.DeviceType == DeviceType.Mobile)
      {
        return folderName + "-mobile";
      }
      else if (this.DeviceType == DeviceType.Tablet)
      {
        return folderName + "-tablet";
      }
      else
      {
        return folderName;
      }
    }
  }
}
