namespace X3Platform.Web.Customizes.Widgets
{
  #region Using Libraries
  using System;

  using X3Platform.Util;
  using X3Platform.Velocity;
  using X3Platform.Web.Configuration;
  #endregion

  /// <summary>��̬Html����Ĵ��ڲ���</summary>
  public sealed class StaticHtmlWidget : AbstractWidget
  {
    /// <summary></summary>
    public override string ParseHtml()
    {
      string widgetRuntimeId = StringHelper.ToGuid();

      VelocityContext context = new VelocityContext();

      context.Put("widgetRuntimeId", widgetRuntimeId);

      context.Put("height", (this.Height == 0 ? "height:auto;" : "height:" + this.Height + "px;"));
      context.Put("width", (this.Width == 0 ? "width:auto;" : "width:" + this.Width + "px;"));

      if (string.IsNullOrEmpty(this.options["widgetHtml"]))
      {
        context.Put("widgetHtml", "���д��ص�Html��ʽ���롣");
      }
      else
      {
        context.Put("widgetHtml", this.options["widgetHtml"]);
      }

      return VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/widgets/static-html.vm");
    }
  }
}
