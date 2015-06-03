namespace X3Platform.Web.Customizes.Widgets
{
    #region Using Libraries
    using X3Platform.Util;
    using X3Platform.Velocity;
    #endregion

    /// <summary>天气窗口部件</summary>
    public sealed class WeatherWidget : AbstractWidget
    {
        public override string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/weather.vm");
        }
    }
}
