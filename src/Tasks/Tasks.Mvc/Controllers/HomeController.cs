﻿namespace X3Platform.Tasks.Mvc.Controllers
{
  using System;
  using System.Web.Mvc;

  using X3Platform.Tasks;
  using X3Platform.Json;
  using X3Platform.Membership;
  using X3Platform.DigitalNumber;
  using X3Platform.Configuration;
  using X3Platform.Web.Mvc.Controllers;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Apps;
  using X3Platform.Apps.Model;

  [LoginFilter]
  public class HomeController : CustomController
  {
    private string APPLICATION_NAME = "Tasks";

    #region 函数:Index()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[APPLICATION_NAME];

      return View("/views/main/tasks/default.cshtml");
    }
    #endregion
  }
}
