namespace X3Platform.Plugins.Bugs.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using X3Platform.Json;
    using X3Platform.Membership;
    using X3Platform.DigitalNumber;
    using X3Platform.Configuration;
    using X3Platform.Web.Mvc.Controllers;
    using X3Platform.Web.Mvc.Attributes;
    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.Plugins.Bugs.Model;
    using X3Platform.Plugins.Bugs.Configuration;

    [LoginFilter]
    public class BugCategoryController : CustomController
    {
        #region 函数:List()
        /// <summary>列表界面</summary>
        /// <returns></returns>
        public ActionResult List()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

            // 视图
            return View("/views/main/bugs/bug-category-list.cshtml");
        }
        #endregion

        #region 函数:Form(string options)
        /// <summary>提交界面</summary>
        /// <returns></returns>
        public ActionResult Form(string options)
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[BugConfiguration.ApplicationName];

            JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

            string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

            BugCategoryInfo param = null;

            if (string.IsNullOrEmpty(id))
            {
                IAccountInfo account = KernelContext.Current.User;

                param = new BugCategoryInfo();

                param.Id = DigitalNumberContext.Generate("Key_Guid");
                param.AccountId = account.Id;
                param.CreatedDate = param.ModifiedDate = DateTime.Now;
            }
            else
            {
                param = BugContext.Instance.BugCategoryService[id];
            }

            ViewBag.param = param;

            // 视图
            return View("/views/main/bugs/bug-category-form.cshtml");
        }
        #endregion
    }
}
