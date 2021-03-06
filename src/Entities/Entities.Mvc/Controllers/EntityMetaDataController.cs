﻿namespace X3Platform.Entities.Mvc.Controllers
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
    using X3Platform.Util;

    [LoginFilter]
    public class EntityMetaDataController : CustomController
    {
        #region 函数:List()
        /// <summary>列表界面</summary>
        /// <returns></returns>
        public ActionResult List()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[EntitiesConfiguration.ApplicationName];

            // 视图
            return View("/views/main/entities/entity-metadata-list.cshtml");
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

            string id = JsonHelper.GetDataValue(request, "id");
            string entitySchemaId = JsonHelper.GetDataValue(request, "entitySchemaId");

            EntityMetaDataInfo param = null;

            if (string.IsNullOrEmpty(id))
            {
                IAccountInfo account = KernelContext.Current.User;

                param = new EntityMetaDataInfo();

                param.Id = DigitalNumberContext.Generate("Key_Guid");
                param.EntitySchemaId = entitySchemaId;
                param.Status = 1;
                param.CreatedDate = param.ModifiedDate = DateTime.Now;
            }
            else
            {
                param = EntitiesManagement.Instance.EntityMetaDataService.FindOne(id);
            }

            ViewBag.param = param;

            // 视图
            return View("/views/main/entities/entity-metadata-form.cshtml");
        }
        #endregion
    }
}
