namespace X3Platform.Plugins.Bugs.Mvc.Controllers
{
  using System;
  using System.Web.Mvc;
  using System.Collections.Generic;

  using X3Platform.Json;
  using X3Platform.Membership;
  using X3Platform.DigitalNumber;
  using X3Platform.Configuration;
  using X3Platform.Web.Mvc.Controllers;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  using X3Platform.Docs;
  using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Membership.Model;
  using X3Platform.Plugins.Bugs.Configuration;

  [LoginFilter]
  public class HomeController : CustomController
  {
    #region 函数:Index()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

      // 管理员身份标记
      bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, application.ApplicationName);

      // 视图
      return View("/views/main/bugs/bug-list.cshtml");
    }
    #endregion

    #region 函数:Form(string options)
    /// <summary>表单内容界面</summary>
    /// <returns></returns>
    public ActionResult Form(string options)
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

      // 管理员身份标记
      bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, application.ApplicationName);

      // -------------------------------------------------------
      // 业务数据处理
      // -------------------------------------------------------

      JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

      // 实体数据标识
      string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();
      // 文档编辑模式
      DocEditMode docEditMode = DocEditMode.Unkown;
      // 实体数据信息
      BugInfo param = null;

      if (string.IsNullOrEmpty(id))
      {
        param = new BugInfo();

        param.Id = DigitalNumberContext.Generate("Table_Bug_Key_Id");

        // 设置编辑模式【新建】
        docEditMode = DocEditMode.New;
      }
      else
      {
        param = BugContext.Instance.BugService.FindOne(id);

        if (param == null)
        {
          ApplicationError.Write(404);
        }

        // 设置编辑模式【编辑】
        docEditMode = DocEditMode.Edit;
      }

      // -------------------------------------------------------
      // 数据加载
      // -------------------------------------------------------

      ViewBag.Title = string.Format("{0}-{1}-{2}", (string.IsNullOrEmpty(param.Title) ? "新问题" : param.Title), application.ApplicationDisplayName, this.SystemName);

      // 加载当前业务实体数据
      ViewBag.entityClassName = KernelContext.ParseObjectType(param.GetType());
      // 加载当前业务实体数据
      ViewBag.param = param;
      // 加载当前文档编辑模式
      ViewBag.docEditMode = docEditMode;

      // 视图
      return View("/views/main/bugs/bug-form.cshtml");
    }
    #endregion

    #region 函数:Detail(string options)
    /// <summary>详细内容界面</summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public ActionResult Detail(string options)
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

      // 管理员身份标记
      bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, application.ApplicationName);

      JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

      // 实体数据标识
      string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();
      // 实体数据编码
      string code = !request.Keys.Contains("code") ? string.Empty : request["code"].ToString();

      BugInfo param = null;

      if (!string.IsNullOrEmpty(id))
      {
        param = BugContext.Instance.BugService.FindOne(id);
      }
      else if (!string.IsNullOrEmpty(code))
      {
        param = BugContext.Instance.BugService.FindOneByCode(code);
      }

      if (param == null)
      {
        ApplicationError.Write(404);
      }

      // -------------------------------------------------------
      // 数据加载
      // -------------------------------------------------------

      ViewBag.Title = string.Format("{0}-{1}-{2}", param.Title, application.ApplicationDisplayName, this.SystemName);

      // 加载数据表前缀
      ViewBag.dataTablePrefix = BugConfigurationView.Instance.DataTablePrefix;
      // 加载当前业务实体数据
      ViewBag.entityClassName = KernelContext.ParseObjectType(param.GetType());
      // 加载当前业务实体数据
      ViewBag.param = param;
      // 加载当前用户详细信息
      ViewBag.member = MembershipManagement.Instance.MemberService[this.Account.Id];

      return View("/views/main/bugs/bug-detail.cshtml");
    }
    #endregion
  }
}
