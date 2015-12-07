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
  using System.Web;
  using X3Platform.Util;

  [LoginFilter]
  public class ForumThreadController : CustomController
  {
    #region 函数:List()
    /// <summary>列表</summary>
    /// <returns></returns>
    public ActionResult List()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

      bool isAdminToken = ViewBag.isAdminToken = AppsSecurity.IsAdministrator(this.Account, ForumConfiguration.ApplicationName);

      return View("/views/main/forum/forum-thread-list.cshtml");
    }
    #endregion

    #region 函数:Form(string options)
    /// <summary>表单内容界面</summary>
    /// <returns></returns>
    public ActionResult Form(string options)
    {
      // 防止浏览器读取本地缓存数据，强制页面读取服务器端最新数据。
      Response.Cache.SetCacheability(HttpCacheability.NoCache);

      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[ForumConfiguration.ApplicationName];

      JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

      string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

      ForumThreadInfo param = null;

      if (string.IsNullOrEmpty(id))
      {
        IAccountInfo account = KernelContext.Current.User;

        string categoryId = !request.Keys.Contains("categoryId") ? string.Empty : request["categoryId"].ToString();

        ForumCategoryInfo category = ForumContext.Instance.ForumCategoryService.FindOne(categoryId);

        param = new ForumThreadInfo();

        param.Id = DigitalNumberContext.Generate("Key_Guid");
        param.AccountId = account.Id;
        param.AccountName = account.Name;
        param.CategoryId = category == null ? string.Empty : category.Id;
        param.CategoryIndex = category == null ? string.Empty : category.CategoryIndex;
        param.TopExpiryDate = DateHelper.DefaultTime;
        param.HotExpiryDate = DateHelper.DefaultTime;
        param.Status = -1;
        param.CreatedDate = param.ModifiedDate = DateTime.Now;
      }
      else
      {
        param = ForumContext.Instance.ForumThreadService.FindOne(id);
      }

      // 加载当前业务实体数据
      ViewBag.entityClassName = KernelContext.ParseObjectType(param.GetType());
      // 加载当前业务实体数据
      ViewBag.param = param;

      // 视图
      return View("/views/main/forum/forum-thread-form.cshtml");
    }
    #endregion
  }
}
