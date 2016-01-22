namespace X3Platform.Entities.Mvc.Controllers
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

    using X3Platform.Entities.Model;
    using X3Platform.Entities.Configuration;

    [LoginFilter]
    public class EntitySchemaController : CustomController
    {
        #region 函数:List()
        /// <summary>列表界面</summary>
        /// <returns></returns>
        public ActionResult List()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[EntitiesConfiguration.ApplicationName];

            // 视图
            return View("/views/main/entities/entity-schema-list.cshtml");
        }
        #endregion

        #region 函数:Form(string options)
        /// <summary>提交界面</summary>
        /// <returns></returns>
        public ActionResult Form(string options)
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[EntitiesConfiguration.ApplicationName];

            JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

            string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();

            EntitySchemaInfo param = null;

            if (string.IsNullOrEmpty(id))
            {
                IAccountInfo account = KernelContext.Current.User;

                param = new EntitySchemaInfo();

                param.Id = DigitalNumberContext.Generate("Key_Guid");
                param.Status = 1;
                param.CreatedDate = param.ModifiedDate = DateTime.Now;
            }
            else
            {
                param = EntitiesManagement.Instance.EntitySchemaService.FindOne(id);
            }

            ViewBag.param = param;

            // 视图
            return View("/views/main/entities/entity-schema-form.cshtml");
        }
        #endregion
    }
}
