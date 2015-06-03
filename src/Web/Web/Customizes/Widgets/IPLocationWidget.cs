namespace X3Platform.Web.Customizes.Widgets
{
    #region Using Libraries
    using X3Platform.Util;
    using X3Platform.Velocity;
    #endregion

    /// <summary>IPλ�ò�ѯ���ڲ���</summary>
    public sealed class IPSpyWidget : AbstractWidget
    {
        // �ӿڵ�ַ
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
