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
    using X3Platform.Tasks.Configuration;

    [LoginFilter]
    public class TaskController : CustomController
    {
        #region 函数:List()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult List()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[TasksConfiguration.ApplicationName];

            // 任务管理 主页
            return View("/views/main/tasks/task-setting-list.cshtml");
        }
        #endregion

        #region 函数:Form()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Form()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[TasksConfiguration.ApplicationName];

            // 加载新的任务编号
            ViewBag.taskCode = DigitalNumberContext.Generate("Key_Timestamp");

            // 任务管理 主页
            return View("/views/main/tasks/task-form.cshtml");
        }
        #endregion
    }
}
