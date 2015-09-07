namespace X3Platform.Entities.Mvc.Controllers
{
  using System;
  using System.Web.Mvc;

  using X3Platform.Json;
  using X3Platform.Membership;
  using X3Platform.Configuration;
  using X3Platform.DigitalNumber;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Web.Mvc.Controllers;

  using X3Platform.Apps;
  using X3Platform.Apps.Model;

  [LoginFilter]
  public class HomeController : CustomController
  {
    #region 函数:Index()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      return Redirect("/entities/entity-schema/list");
    }
    #endregion
  }
}
