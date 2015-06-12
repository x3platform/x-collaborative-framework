namespace X3Platform.Tasks.Customizes
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Web;

    using X3Platform.Membership;
    using X3Platform.Velocity;
    using X3Platform.Util;
    using X3Platform.Web.Customizes;

    using X3Platform.Ajax.Json;
    using X3Platform.Configuration;
    using X3Platform.Tasks.Model;
    using X3Platform.Apps;
    using X3Platform.Tasks.Configuration;
  using X3Platform.Web.Configuration;
    #endregion

    /// <summary>任务管理窗口部件</summary>
    public sealed class TaskWidget : AbstractWidget
    {
        /// <summary>解析为Html格式数据</summary>
        public override string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            context.Put("application", AppsContext.Instance.ApplicationService[TasksConfiguration.ApplicationName]);

            context.Put("height", (this.Height == 0 ? "height:auto;" : "height:" + this.Height + "px;"));
            context.Put("width", (this.Width == 0 ? "width:auto;" : "width:" + this.Width + "px;"));

            return VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/widgets/tasks.vm");
        }
    }
}
