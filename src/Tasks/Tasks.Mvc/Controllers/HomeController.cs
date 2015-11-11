namespace X3Platform.Tasks.Mvc.Controllers
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
    using System.Collections.Generic;
    using X3Platform.Tasks.Configuration;

    [LoginFilter]
    public class HomeController : CustomController
    {
        #region 函数:Index()
        /// <summary>主页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // 所属应用信息
            ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[TasksConfiguration.ApplicationName];

            // 所属应用信息
            IList<ApplicationSettingInfo> settings = null;

            // 状态
            settings = AppsContext.Instance.ApplicationSettingService.FindAllByApplicationSettingGroupName("应用管理_协同平台_任务管理_状态管理");

            string status = "[";

            foreach (ApplicationSettingInfo setting in settings)
            {
                if (setting.Status == 1)
                {
                    status += "{text:\"" + setting.Text + "\",value:\"" + setting.Value + "\"},";
                }
            }

            status += "{text:'全部',value:''}]";

            // 状态
            settings = AppsContext.Instance.ApplicationSettingService.FindAllByApplicationSettingGroupName("应用管理_协同平台_任务管理_任务类型");

            string type = "[";

            foreach (ApplicationSettingInfo setting in settings)
            {
                if (setting.Status == 1)
                {
                    type += "{text:\"" + setting.Text + "\",value:\"" + setting.Value + "\"},";
                }
            }

            type += "{text:'全部',value:''}]";

            // -------------------------------------------------------
            // 数据加载
            // -------------------------------------------------------

            // 加载当前登录帐户信息
            ViewBag.account = this.Account;
            // 加载当前日期信息
            ViewBag.today = DateTime.Now;
            // 加载待办状态列表
            ViewBag.status = status.Replace("\"", "&quot;");
            // 加载待办类别列表
            ViewBag.type = type.Replace("\"", "&quot;");
            // 客户端定时刷新间隔
            ViewBag.clientRefreshInterval = TasksConfigurationView.Instance.ClientRefreshInterval;

            return View("/views/main/tasks/default.cshtml");
        }
        #endregion
    }
}
