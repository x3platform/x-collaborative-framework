namespace X3Platform.Web.Customizes.Widgets
{
    #region Using Libraries
    using X3Platform.Util;
    using X3Platform.Velocity;
    #endregion

    /// <summary>�ֻ���λ�ò�ѯ���ڲ���</summary>
    public sealed class MobileSpyWidget : AbstractWidget
    {
        public override string ParseHtml()
        {
            // http://www.woaic.com/2013/03/389

            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/mobile.vm");
        }
    }
}
