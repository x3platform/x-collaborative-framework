using System;
using System.Reflection;

using X3Platform.Logging;
using X3Platform.Plugins;
using X3Platform.Spring;

using X3Platform.Web.Customize;
using System.Xml;
using System.Net;
using System.Text;
using X3Platform.Util;
using X3Platform.Velocity;

namespace X3Platform.Web.Customize.Widgets
{
    /// <summary>ÊÕ²Ø¼Ð²¿¼þ</summary>
    public sealed class FavoriteWidget : IWidget
    {
        public void Load(string configuration)
        {

        }

        public string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/favorite.vm");
        }
    }
}
