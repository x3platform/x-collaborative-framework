namespace X3Platform.Plugins.Forum.Customizes
{
  #region Using Libraries
  using System.Collections.Generic;

  using X3Platform.Ajax.Json;
  using X3Platform.Util;
  using X3Platform.Velocity;
  using X3Platform.Web.Customizes;

  using X3Platform.Plugins.Forum.Model;
  using X3Platform.Web.Configuration;
  #endregion

  /// <summary>Forum Widget</summary>
  public sealed class ForumWidget : AbstractWidget
  {
    public override string ParseHtml()
    {
      string widgetRuntimeId = StringHelper.ToGuid();

      VelocityContext context = new VelocityContext();

      context.Put("widgetRuntimeId", widgetRuntimeId);

      context.Put("height", (this.Height == 0 ? "height:auto;" : "height:" + this.Height + "px;"));
      context.Put("width", (this.Width == 0 ? "width:auto;" : "width:" + this.Width + "px;"));

      context.Put("categoryIndex", this.options["categoryIndex"]);

      // 设置最大行数
      int maxRowCount;

      int.TryParse(this.options["maxRowCount"], out maxRowCount);

      if (maxRowCount < 1)
      {
        maxRowCount = 1;
      }

      if (maxRowCount > 100)
      {
        maxRowCount = 100;
      }

      context.Put("maxRowCount", maxRowCount);

      // 设置最大标题长度
      int maxLength;

      int.TryParse(this.options["maxLength"], out maxLength);

      context.Put("maxLength", maxLength);

      return VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/widgets/forum.vm");
    }
  }
}
