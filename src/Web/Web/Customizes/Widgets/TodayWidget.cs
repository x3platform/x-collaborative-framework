namespace X3Platform.Web.Customizes.Widgets
{
    #region Using Libraries
    using X3Platform.Velocity;
    using X3Platform.Util;
  using X3Platform.Web.Configuration;
    #endregion

    /// <summary>������Ϣ���ڲ���</summary>
    public sealed class TodayWidget : AbstractWidget
    {
        public override string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            return VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/widgets/today.vm");
        }
    }
}
