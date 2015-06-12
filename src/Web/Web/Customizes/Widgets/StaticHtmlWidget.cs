namespace X3Platform.Web.Customizes.Widgets
{
  #region Using Libraries
  using System;

  using X3Platform.Util;
  using X3Platform.Velocity;
  using X3Platform.Web.Configuration;
  #endregion

  /// <summary>静态Html代码的窗口部件</summary>
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
        context.Put("widgetHtml", "请编写相关的Html格式编码。");
      }
      else
      {
        context.Put("widgetHtml", this.options["widgetHtml"]);
      }

      return VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/widgets/static-html.vm");
    }
  }
}
