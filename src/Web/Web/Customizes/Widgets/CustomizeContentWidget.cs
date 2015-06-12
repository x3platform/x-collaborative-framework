namespace X3Platform.Web.Customizes.Widgets
{
  #region Using Libraries
  using System;

  using X3Platform.Util;
  using X3Platform.Velocity;
  using X3Platform.Web.Configuration;
  #endregion

  /// <summary>�Զ������ݵĴ��ڲ���</summary>
  public sealed class CustomizeContentWidget : AbstractWidget
  {
    /// <summary></summary>
    public override string ParseHtml()
    {
      string widgetRuntimeId = StringHelper.ToGuid();

      VelocityContext context = new VelocityContext();

      context.Put("widgetRuntimeId", widgetRuntimeId);

      context.Put("height", (this.Height == 0 ? "height:auto;" : "height:" + this.Height + "px;"));
      context.Put("width", (this.Width == 0 ? "width:auto;" : "width:" + this.Width + "px;"));
      context.Put("contentName", this.options["contentName"]);

      return VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/widgets/customize-content.vm");
    }
  }
}
