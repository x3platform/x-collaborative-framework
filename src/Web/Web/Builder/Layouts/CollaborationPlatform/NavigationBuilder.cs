namespace X3Platform.Web.Builder.Layouts.CollaborationPlatform
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using X3Platform.Apps;
  using X3Platform.Apps.Model;
  using X3Platform.Membership;
  using X3Platform.Velocity;
  using X3Platform.Web.Builder.ILayouts;
  using X3Platform.Web.Configuration;
  #endregion

  /// <summary></summary>
  public class NavigationBuilder : CommonBuilder, INavigationBuilder
  {
    #region 函数:GetStartMenu(IAccountInfo account)
    public string GetStartMenu(IAccountInfo account)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##StartMenu## AND Status = 1 ORDER BY OrderId ", Guid.Empty);

      outString.Append("<!-- 开始菜单【开始】-->");

      AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
      {
        //
        // 菜单 DisplayType = "MenuItem" , 菜单项.
        // 菜单 Status = 0 , 禁用.
        //
        //if (item.DisplayType == "MenuItem")
        //{
        //}
        if (item.DisplayType == "MenuSplitLine")
        {
          outString.Append("<li class=\"line\">-*- 华丽的分隔线 -*-</li>");
        }
        else
        {
          string dropdownMenu = GetStartDropdownMenu(item.Id);

          outString.AppendFormat("<li><a href=\"{2}\" title=\"{1}\" target=\"{3}\">{0}",
              item.Name,
              item.Description,
              item.Url,
              string.IsNullOrEmpty(item.Target) ? "_self" : item.Target);

          if (string.IsNullOrEmpty(dropdownMenu))
          {
            // outString.Append("</a></li>\r\n");
            outString.Append("<!--[if gte IE 7]><!--></a><!--<![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]><table><tr><td><![endif]-->\r\n");
            // outString.AppendFormat("<ul>{0}</ul>\r\n", links);
            outString.Append("<!--[if lte IE 6]></td></tr></table><![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]></a><![endif]-->\r\n");

            outString.Append("</li>\r\n");
          }
          else
          {
            outString.Append("<!--[if gte IE 7]><!--></a><!--<![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]><table><tr><td><![endif]-->\r\n");
            outString.AppendFormat("<ul>{0}</ul>\r\n", dropdownMenu);
            outString.Append("<!--[if lte IE 6]></td></tr></table><![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]></a><![endif]-->\r\n");

            outString.Append("</li>\r\n");
          }
        }
      });

      outString.Append("<!-- 开始菜单【结束】-->");

      return outString.ToString();
    }
    #endregion

    #region 私有函数:GetStartDropdownMenu(string menuId)
    private string GetStartDropdownMenu(string menuId)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##StartMenu## AND Status = 1 ORDER BY OrderId ", menuId);

      AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
      {
        if (item.DisplayType == "MenuSplitLine")
        {
          outString.Append("<li class=\"line\">-*- 华丽的分隔线 -*-</li>");
        }
        else
        {
          string dropdownMenu = GetStartDropdownMenu(item.Id);

          outString.AppendFormat("<li><a href=\"{2}\" title=\"{1}\" target=\"{3}\">{0}",
              item.Name,
              item.Description,
              item.Url,
              string.IsNullOrEmpty(item.Target) ? "_self" : item.Target);

          if (string.IsNullOrEmpty(dropdownMenu))
          {
            outString.Append("</a></li>\r\n");
          }
          else
          {
            outString.Append("<!--[if gte IE 7]><!--></a><!--<![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]><table><tr><td><![endif]-->\r\n");
            outString.AppendFormat("<ul>{0}</ul>\r\n", dropdownMenu);
            outString.Append("<!--[if lte IE 6]></td></tr></table><![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]></a><![endif]-->\r\n");

            outString.Append("</li>\r\n");
          }
        }
      });

      return outString.ToString();
    }
    #endregion

    #region 函数:GetTopMenu(IAccountInfo account)
    /// <summary>获取顶部菜单信息</summary>
    /// <param name="account"></param>
    /// <param name="topMenu"></param>
    /// <returns></returns>
    public string GetTopMenu(IAccountInfo account)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##TopMenu## AND Status = 1 ORDER BY OrderId ", Guid.Empty);

      outString.Append("<!-- 顶部菜单【开始】-->");

      AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
      {
        //
        // 菜单 DisplayType = "MenuItem" , 菜单项.
        // 菜单 Status = 0 , 禁用.
        //
        if (item.DisplayType == "MenuItem")
        {
          outString.AppendFormat("<li><a href=\"{2}\" title=\"{1}\" target=\"{3}\">{0}",
              item.Name,
              item.Description,
              item.Url,
              string.IsNullOrEmpty(item.Target) ? "_self" : item.Target);

          string dropdownMenu = GetTopDropdownMenu(item.Id);

          if (string.IsNullOrEmpty(dropdownMenu))
          {
            outString.Append("</a></li>\r\n");
          }
          else
          {
            outString.Append("<!--[if gte IE 7]><!--></a><!--<![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]><table><tr><td><![endif]-->\r\n");
            outString.AppendFormat("<ul>{0}</ul>\r\n", dropdownMenu);
            outString.Append("<!--[if lte IE 6]></td></tr></table><![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]></a><![endif]-->\r\n");

            outString.Append("</li>\r\n");
          }
        }
      });

      outString.Append("<!-- 顶部菜单【结束】-->");

      VelocityContext context = new VelocityContext();

      context.Put("hostName", HostName);

      context.Put("account", account);

      context.Put("startMenu", GetStartMenu(account));

      context.Put("topMenu", outString.ToString());

      return VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/layouts/navigation-top-menu.vm");
    }
    #endregion

    #region 私有函数:GetTopDropdownMenu(string menuId)
    private string GetTopDropdownMenu(string menuId)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##TopMenu## AND Status = 1 ORDER BY OrderId ", menuId);

      AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
      {
        if (item.DisplayType == "MenuSplitLine")
        {
          outString.Append("<li class=\"line\">-*- 华丽的分隔线 -*-</li>");
        }
        else
        {
          // outString.AppendFormat("<li><a href=\"{2}\" title=\"{1}\" target=\"{3}\">{0}</a></li>\r\n",
          // item.Name,
          // item.Description,
          // item.Url,
          // string.IsNullOrEmpty(item.Target) ? "_self" : item.Target);

          string dropdownMenu = GetTopDropdownMenu(item.Id);

          outString.AppendFormat("<li><a href=\"{2}\" title=\"{1}\" target=\"{3}\">{0}",
              item.Name,
              item.Description,
              item.Url,
              string.IsNullOrEmpty(item.Target) ? "_self" : item.Target);

          if (string.IsNullOrEmpty(dropdownMenu))
          {
            outString.Append("</a></li>\r\n");
          }
          else
          {
            outString.Append("<!--[if gte IE 7]><!--></a><!--<![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]><table><tr><td><![endif]-->\r\n");
            outString.AppendFormat("<ul>{0}</ul>\r\n", dropdownMenu);
            outString.Append("<!--[if lte IE 6]></td></tr></table><![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]></a><![endif]-->\r\n");

            outString.Append("</li>\r\n");
          }
        }
      });

      return outString.ToString();
    }
    #endregion

    #region 函数:GetBottomMenu(IAccountInfo account)
    public string GetBottomMenu(IAccountInfo account)
    {
      VelocityContext context = new VelocityContext();

      context.Put("hostName", HostName);

      context.Put("year", DateTime.Now.Year);

      context.Put("account", account);

      return VelocityManager.Instance.Merge(context, "themes/" + WebConfigurationView.Instance.ThemeName + "/layouts/NavigationBuilder.GetBottomMenu.vm");
    }
    #endregion

    public string GetShortcutMenu(IAccountInfo account)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##ShortcutMenu## AND Status = 1 ORDER BY OrderId ", Guid.Empty);

      AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
      {
        //
        // 菜单 DisplayType = "MenuItem" , 菜单项.
        // 菜单 Status = 0 , 禁用.
        //
        //if (item.DisplayType == "MenuItem")
        //{
        //}
        if (item.DisplayType == "MenuSplitLine")
        {
          outString.Append("<li class=\"line\">-*- 华丽的分隔线 -*-</li>");
        }
        else
        {
          string dropdownMenu = GetStartDropdownMenu(item.Id);

          outString.AppendFormat("<li><a href=\"{2}\" title=\"{1}\" target=\"{3}\">{0}",
              item.Name,
              item.Description,
              item.Url,
              string.IsNullOrEmpty(item.Target) ? "_self" : item.Target);

          if (string.IsNullOrEmpty(dropdownMenu))
          {
            // outString.Append("</a></li>\r\n");
            outString.Append("<!--[if gte IE 7]><!--></a><!--<![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]><table><tr><td><![endif]-->\r\n");
            // outString.AppendFormat("<ul>{0}</ul>\r\n", links);
            outString.Append("<!--[if lte IE 6]></td></tr></table><![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]></a><![endif]-->\r\n");

            outString.Append("</li>\r\n");
          }
          else
          {
            outString.Append("<!--[if gte IE 7]><!--></a><!--<![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]><table><tr><td><![endif]-->\r\n");
            outString.AppendFormat("<ul>{0}</ul>\r\n", dropdownMenu);
            outString.Append("<!--[if lte IE 6]></td></tr></table><![endif]-->\r\n");

            outString.Append("<!--[if lte IE 6]></a><![endif]-->\r\n");

            outString.Append("</li>\r\n");
          }
        }
      });

      return outString.ToString();
    }

    public string GetApplicationMenu(IAccountInfo account, string applicationName)
    {
      return GetApplicationMenu(account, applicationName, string.Empty);
    }

    public string GetApplicationMenu(IAccountInfo account, string applicationName, string parentMenuFullPath)
    {
      return GetApplicationMenu(account, applicationName, new Dictionary<string, string>() { { "parentMenuFullPath", parentMenuFullPath } });
    }

    public string GetApplicationMenu(IAccountInfo account, string applicationName, Dictionary<string, string> options)
    {
      StringBuilder outString = new StringBuilder();

      ApplicationInfo application = AppsContext.Instance.ApplicationService[applicationName];

      string parentMenuId = GetOptionValue(options, "parentMenuId");

      string parentMenuFullPath = GetOptionValue(options, "parentMenuFullPath");

      string currentMenuId = GetOptionValue(options, "currentMenuId");

      string currentMenuFullPath = GetOptionValue(options, "currentMenuFullPath");

      string whereClause = string.Empty;

      if (!string.IsNullOrEmpty(parentMenuId))
      {
        whereClause = string.Format(" ApplicationId = ##{0}## AND ParentId=##{1}## AND MenuType = ##ApplicationMenu## AND Status = 1 ORDER BY OrderId ", application.Id, parentMenuId);
      }
      else if (!string.IsNullOrEmpty(parentMenuFullPath))
      {
        whereClause = string.Format(" ApplicationId = ##{0}## AND FullPath Like ##{1}%## AND MenuType = ##ApplicationMenu## AND Status = 1 ORDER BY OrderId ", application.Id, parentMenuFullPath);
      }
      else
      {
        whereClause = string.Format(" ApplicationId = ##{0}## AND ParentId=##{1}## AND MenuType = ##ApplicationMenu## AND Status = 1 ORDER BY OrderId ", application.Id, Guid.Empty);
      }

      IList<ApplicationMenuInfo> list = AppsContext.Instance.ApplicationMenuService.FindAll(whereClause);

      outString.Append("<!-- 应用菜单【开始】-->");
      outString.Append("<div id=\"windowApplicationMenuContainer\" class=\"x-ui-pkg-menu-slide-menu-container\">");
      outString.Append("<div id=\"windowApplicationMenuWrapper\" class=\"x-ui-pkg-menu-slide-menu-wrapper\">");
      outString.Append("<div class=\"x-ui-pkg-menu-slide-menu-submenu first-child\" >");
      outString.Append("<span>" + application.ApplicationDisplayName + "</span>");

      foreach (ApplicationMenuInfo item in list)
      {
        if (item.DisplayType == "MenuGroup")
        {
          outString.Append("</div>");
          outString.Append("<div class=\"x-ui-pkg-menu-slide-menu-submenu\" >");
          outString.Append("<span>" + item.Name + "</span>");
        }
        else if (item.DisplayType == "MenuSplitLine")
        {
          // 默认情况下不显示
        }
        else
        {
          if (string.IsNullOrEmpty(item.IconPath))
          {
            // item.IconPath = "/resources/images/icon/icon-menu-default.gif";
            item.IconPath = "fa fa-bars";
          }

          string icon = "<img src=\"/resources/images/transparent.gif\" style=\"background: #fff url(" + item.IconPath + ") no-repeat right; width:16px; height:16px;\" />";

          if (item.IconPath.IndexOf("fa ") == 0 || item.IconPath.IndexOf("glyphicon ") == 0)
          {
            icon = "<i class=\"" + item.IconPath + "\" ></i> ";
          }

          if (currentMenuId == item.Id || currentMenuFullPath == item.FullPath)
          {
            outString.AppendFormat("<a id=\"{0}\" href=\"{1}\" target=\"{2}\" class=\"current\" >{3} {4}</a>", item.Id, ReplaceOptionValue(options, item.Url), item.Target, icon, item.Name);
          }
          else
          {
            outString.AppendFormat("<a id=\"{0}\" href=\"{1}\" target=\"{2}\" >{3} {4}</a>", item.Id, ReplaceOptionValue(options, item.Url), item.Target, icon, item.Name);
          }
        }
      }

      outString.Append("</div>");
      outString.Append("</div>");
      outString.Append("</div>");

      outString.Append("<div id=\"windowApplicationMenuHandleBar\" class=\"x-ui-pkg-menu-slide-menu-handle-bar\">");
      outString.Append("</div>");

      outString.Append("<!-- 应用菜单【结束】-->");

      return outString.ToString();
    }

    public string GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName)
    {
      return string.Empty;
    }
  }
}