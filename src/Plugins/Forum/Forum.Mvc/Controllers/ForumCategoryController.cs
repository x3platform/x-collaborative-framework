namespace X3Platform.Plugins.Forum.Mvc.Controllers
{
  using System;
  using System.Web.Mvc;

  using X3Platform.Json;
  using X3Platform.Membership;
  using X3Platform.DigitalNumber;
  using X3Platform.Configuration;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Web.Mvc.Controllers;

  using X3Platform.Apps;
  using X3Platform.Apps.Model;

  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Plugins.Forum.Configuration;

  [LoginFilter]
  public class ForumCategoryController : CustomController
  {
    #region 函数:List()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult List()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

      // -------------------------------------------------------
      // 身份验证
      // -------------------------------------------------------

      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      {
        ApplicationError.Write(401);
      }

      return View("/views/main/forum/forum-category-list.cshtml");
    }
    #endregion

    #region 函数:Form(string options)
    /// <summary>提交界面</summary>
    /// <returns></returns>
    public ActionResult Form(string options)
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

      // -------------------------------------------------------
      // 身份验证
      // -------------------------------------------------------

      if (!AppsSecurity.IsAdministrator(KernelContext.Current.User, application.ApplicationName))
      {
        ApplicationError.Write(401);
      }

      JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

      string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

      ForumCategoryInfo param = null;

      if (string.IsNullOrEmpty(id))
      {
        IAccountInfo account = KernelContext.Current.User;

        param = new ForumCategoryInfo();

        param.Id = DigitalNumberContext.Generate("Key_Guid");
        param.AccountId = account.Id;
        param.CreateDate = param.UpdateDate = DateTime.Now;
      }
      else
      {
        param = ForumContext.Instance.ForumCategoryService.FindOne(id);
      }

      ViewBag.param = param;

      return View("/views/main/forum/forum-category-form.cshtml");
    }
    #endregion
  }
}
