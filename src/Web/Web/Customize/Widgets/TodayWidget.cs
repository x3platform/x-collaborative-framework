namespace X3Platform.Web.Customize.Widgets
{
    #region Using Libraries
    using X3Platform.Velocity;
    using X3Platform.Util;
    #endregion

    /// <summary>今日信息窗口部件</summary>
    public sealed class TodayWidget : AbstractWidget
    {
        public override string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/today.vm");
        }
    }
}
