namespace X3Platform.Web.Customizes.Widgets
{
    #region Using Libraries
    using X3Platform.Util;
    using X3Platform.Velocity;
    #endregion

    /// <summary>IP位置查询窗口部件</summary>
    public sealed class IPSpyWidget : AbstractWidget
    {
        // 接口地址
        // http://ip.taobao.com
        
        public override string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/ip.vm");
        }
    }
}
