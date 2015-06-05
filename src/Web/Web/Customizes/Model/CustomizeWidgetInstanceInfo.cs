namespace X3Platform.Web.Customizes.Model
{
  using System;

  /// <summary>页面信息</summary>
  [Serializable]
  public class CustomizeWidgetInstanceInfo : EntityClass
  {
    public CustomizeWidgetInstanceInfo()
    {
    }

    #region 属性:Id
    private string m_Id;

    /// <summary>标识</summary>
    public string Id
    {
      get { return m_Id; }
      set { m_Id = value; }
    }
    #endregion

    #region 属性:Page
    private CustomizePageInfo m_Page = null;

    /// <summary>部件</summary>
    public CustomizePageInfo Page
    {
      get
      {
        if (!string.IsNullOrEmpty(this.PageId))
        {
          m_Page = CustomizeContext.Instance.CustomizePageService[this.PageId];
        }

        return m_Page;
      }
    }
    #endregion

    #region 属性:PageId
    private string m_PageId;

    /// <summary>页面标识</summary>
    public string PageId
    {
      get { return m_PageId; }
      set { m_PageId = value; }
    }
    #endregion

    #region 属性:PageName
    private string m_PageName;

    /// <summary>页面名称</summary>
    public string PageName
    {
      get { return m_PageName; }
      set { m_PageName = value; }
    }
    #endregion

    #region 属性:Widget
    private CustomizeWidgetInfo m_Widget = null;

    /// <summary>部件</summary>
    public CustomizeWidgetInfo Widget
    {
      get
      {
        if (!string.IsNullOrEmpty(this.WidgetId))
        {
          m_Widget = CustomizeContext.Instance.CustomizeWidgetService[this.WidgetId];
        }

        return m_Widget;
      }
    }
    #endregion

    #region 属性:WidgetId
    private string m_WidgetId;

    /// <summary>部件标识</summary>
    public string WidgetId
    {
      get { return m_WidgetId; }
      set { m_WidgetId = value; }
    }
    #endregion

    #region 属性:WidgetName
    private string m_WidgetName;

    /// <summary>部件名称</summary>
    public string WidgetName
    {
      get { return m_WidgetName; }
      set { m_WidgetName = value; }
    }
    #endregion

    #region 属性:Title
    private string m_Title;

    /// <summary>标题</summary>
    public string Title
    {
      get { return m_Title; }
      set { m_Title = value; }
    }
    #endregion

    #region 属性:Height
    private int m_Height;

    /// <summary>高度</summary>
    public int Height
    {
      get { return m_Height; }
      set { m_Height = value; }
    }
    #endregion

    #region 属性:Width
    private int m_Width;

    /// <summary>宽度</summary>
    public int Width
    {
      get { return m_Width; }
      set { m_Width = value; }
    }
    #endregion

    #region 属性:Html
    private string m_Html = string.Empty;

    /// <summary>Html代码</summary>
    public string Html
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_Html) && this.Widget != null)
        {
          IWidget widget = (IWidget)KernelContext.CreateObject(this.Widget.ClassName);

          if (widget == null)
          {
            this.m_Html = "<div style=\"padding:10px;\">未找到相关对象【" + this.Widget.ClassName + "】信息，请联系管理员检查相关系统配置。</div>";
          }
          else
          {
            widget.Load(this.Options);

            this.m_Html = widget.ParseHtml();
          }
        }

        return this.m_Html.Replace("\n", string.Empty).Replace("\r", string.Empty);
      }
    }
    #endregion

    #region 属性:Options
    private string m_Options;

    /// <summary>配置</summary>
    public string Options
    {
      get { return m_Options; }
      set { m_Options = value; }
    }
    #endregion

    #region 属性:UpdateDate
    private DateTime m_UpdateDate;

    /// <summary>修改时间</summary>
    public DateTime UpdateDate
    {
      get { return m_UpdateDate; }
      set { m_UpdateDate = value; }
    }
    #endregion

    #region 属性:CreateDate
    private DateTime m_CreateDate;

    /// <summary>创建时间</summary>
    public DateTime CreateDate
    {
      get { return m_CreateDate; }
      set { m_CreateDate = value; }
    }
    #endregion

    // -------------------------------------------------------
    // 设置 EntityClass 标识
    // -------------------------------------------------------

    #region 属性:EntityId
    /// <summary>实体对象标识</summary>
    public override string EntityId
    {
      get { return this.Id; }
    }
    #endregion
  }
}
