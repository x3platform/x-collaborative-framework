using System;
using System.Reflection;

using X3Platform.Logging;
using X3Platform.Plugins;
using X3Platform.Spring;

using X3Platform.Web.Customize;
using X3Platform.Velocity;
using X3Platform.Util;

namespace X3Platform.Web.Customize.Widgets
{
    /// <summary>Bug��������</summary>
    public sealed class TodayWidget : IWidget
    {
        public void Load(string configuration)
        {
        }

        public string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/today.vm");
        }
    }
}
