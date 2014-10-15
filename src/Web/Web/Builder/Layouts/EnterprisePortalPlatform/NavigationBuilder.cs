#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :NavigationBuilder.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Web.Builder.Layouts.EnterprisePortalPlatform
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Web;

    using X3Platform.Apps;
    using X3Platform.Apps.Model;
    using X3Platform.Membership;
    using X3Platform.Navigation;
    using X3Platform.Navigation.Model;
    using X3Platform.Util;
    using X3Platform.Velocity;
    using X3Platform.Web.Configuration;
    using X3Platform.Web.Builder.ILayouts;
    using X3Platform.Membership.Model;
    using X3Platform.Membership.Authentication;
    #endregion

    /// <summary>��ҵ�Ż�ƽ̨����������</summary>
    public class NavigationBuilder : CommonBuilder, INavigationBuilder
    {
        #region 属性:GetStartMenu(IAccountInfo account)
        public string GetStartMenu(IAccountInfo account)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##StartMenu## AND Status = 1 ORDER BY OrderId ", Guid.Empty);

            outString.Append("<!-- ��ʼ�˵�����ʼ��-->");

            AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
            {
                //
                // �˵� DisplayType = "MenuItem" , �˵���. 
                // �˵� Status = 0 , ����.
                //
                //if (item.DisplayType == "MenuItem")
                //{
                //}
                if (item.DisplayType == "MenuSplitLine")
                {
                    outString.Append("<li class=\"line\">-*- �����ķָ��� -*-</li>");
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

            outString.Append("<!-- ��ʼ�˵���������-->");

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:GetStartDropdownMenu(string menuId)
        private string GetStartDropdownMenu(string menuId)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##StartMenu## AND Status = 1 ORDER BY OrderId ", menuId);

            AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
            {
                if (item.DisplayType == "MenuSplitLine")
                {
                    outString.Append("<li class=\"line\">-*- �����ķָ��� -*-</li>");
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

        #region 属性:GetTopMenu(IAccountInfo account)
        /// <summary>��ȡ�����˵���Ϣ</summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetTopMenu(IAccountInfo account)
        {
            VelocityContext context = new VelocityContext();

            IMemberInfo member = MembershipManagement.Instance.MemberService[account.Id];

            context.Put("hostName", HostName);

            context.Put("account", account);

            context.Put("member", member);

            context.Put("navigationPortalTopMenu", GetNavigationPortalTopMenu(account));

            context.Put("navigationPortalShortcut", GetNavigationPortalShortcut(account));

            context.Put("navigationPortalMenu", GetNavigationPortalMenu());

            return VelocityManager.Instance.Merge(context, "web/builder/themes/enterprise-portal/NavigationBuilder.GetTopMenu.vm");
        }
        #endregion

        #region ˽�к���:TryGetNavigationPortalIdentity(IAccountInfo account)
        /// <summary></summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private string TryGetNavigationPortalIdentity(IAccountInfo account)
        {
            string portalIdentity = HttpContext.Current.Request.QueryString["portalIdentity"];

            // ����û��ָ���Ż���ʶ������ȡ��ǰ�û���Ĭ�Ϲ�˾ΪĬ���Ż�
            if (string.IsNullOrEmpty(portalIdentity))
            {
                if (HttpContext.Current.Request.Cookies["portalIdentity"] == null)
                {
                    MemberInfo member = (MemberInfo)MembershipManagement.Instance.MemberService[account.Id];

                    // Ĭ���Ż���ʶ
                    string defaultPortIdentity = "00000000-0000-0000-0000-000000000001";

                    if (member != null && member.Corporation != null)
                    {
                        string whereClause = string.Format(@" OrganizationId = ##{0}## AND Status = 1 ORDER BY OrderId ", member.CorporationId);

                        IList<NavigationPortalInfo> list = NavigationContext.Instance.NavigationPortalService.FindAll(whereClause);

                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("portalIdentity"));
                        HttpContext.Current.Response.Cookies["portalIdentity"].Value = list.Count > 0 ? list[0].Id : defaultPortIdentity;
                        HttpContext.Current.Response.Cookies["portalIdentity"].Domain = HttpAuthenticationCookieSetter.ParseDomain();
                    }
                    else
                    {
                        // Ĭ���Ż�
                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("portalIdentity"));
                        HttpContext.Current.Response.Cookies["portalIdentity"].Value = defaultPortIdentity;
                        HttpContext.Current.Response.Cookies["portalIdentity"].Domain = HttpAuthenticationCookieSetter.ParseDomain();
                    }
                }

                portalIdentity = HttpContext.Current.Request.Cookies["portalIdentity"].Value;
            }
            else
            {
                // �����ֹ�ָ���Ż���ʶ�������õ�ǰ�û����Ż���
                if (HttpContext.Current.Request.Cookies["portalIdentity"] == null)
                {
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("portalIdentity"));
                }

                HttpContext.Current.Response.Cookies["portalIdentity"].Value = portalIdentity;
                HttpContext.Current.Response.Cookies["portalIdentity"].Domain = HttpAuthenticationCookieSetter.ParseDomain();
            }

            return portalIdentity;
        }
        #endregion

        #region ˽�к���:GetNavigationPortalTopMenu(IAccountInfo account)
        /// <summary></summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private string GetNavigationPortalTopMenu(IAccountInfo account)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(@" PortalId = ##{0}## AND Status = 1 ORDER BY OrderId ", this.TryGetNavigationPortalIdentity(account));

            IList<NavigationPortalTopMenuInfo> list = NavigationContext.Instance.NavigationPortalTopMenuService.FindAll(whereClause);

            foreach (NavigationPortalTopMenuInfo item in list)
            {
                outString.AppendFormat("<li><a href=\"{0}\" target=\"{1}\" title=\"{2}\" >{3}</a></li> ", item.Url, item.Target, item.Description, item.Text);
            }

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:GetNavigationPortalShortcut(IAccountInfo account)
        private string GetNavigationPortalShortcut(IAccountInfo account)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(@" PortalId = ##{0}## AND Status = 1 ORDER BY OrderId ", this.TryGetNavigationPortalIdentity(account));

            IList<NavigationPortalShortcutInfo> list = NavigationContext.Instance.NavigationPortalShortcutService.FindAll(whereClause);

            outString.Append("<table class=\"table-style\" summary=\"\" ><tr>");

            if (list.Count == 0)
            {
                outString.Append("<td>&nbsp;</td>");
            }
            else
            {
                foreach (NavigationPortalShortcutInfo item in list)
                {
                    outString.AppendFormat("<td><a href=\"{0}\" target=\"{1}\" ><img border=\"0\" src=\"{2}\" alt=\"{4}\" /></a><div><a href=\"{0}\" target=\"{1}\" >{3}</a></div></td>",
                                    item.Url,
                                    item.Target,
                                    item.BigIconPath,
                                    item.Text,
                                    item.Description);
                }
            }

            outString.Append("</tr></table>");

            return outString.ToString();
        }
        #endregion

        #region ˽�к���:GetNavigationPortalMenu()
        private string GetNavigationPortalMenu()
        {
            IList<NavigationPortalInfo> list = NavigationContext.Instance.NavigationPortalService.FindAll(" Status = 1 ORDER BY  OrderId , GroupId");

            StringBuilder outString = new StringBuilder();

            outString.Append("<div class=\"header-account-menu setting-menu-show\" style=\"display:none;\" onmouseout=\"masterpage.closeSettingMenu(event);\"> ");

            string currentGroupId = string.Empty;

            foreach (NavigationPortalInfo item in list)
            {
                if (string.IsNullOrEmpty(currentGroupId))
                {
                    currentGroupId = item.GroupId;
                }
                else if (item.GroupId != currentGroupId)
                {
                    outString.Append("<div class=\"header-account-menu-line setting-menu-show\"></div>");

                    currentGroupId = item.GroupId;
                }

                outString.Append("<div class=\"header-account-menu-item setting-menu-show\" >");
                outString.Append("<a class=\"setting-menu-show\" href=\"javascript:" + string.Format("x.cookies.add('portalIdentity','{0}',false,'/','{1}');location.href='{1}'", item.Id, item.Url, HttpAuthenticationCookieSetter.ParseDomain()) + "\" >");
                outString.Append("<span class=\"menu-text setting-menu-show\">" + item.Text + "</span>");
                outString.Append("<span class=\"menu-discption setting-menu-show\" >" + item.Description + "</span>");
                outString.Append("</a>");
                outString.Append("</div>");
            }

            outString.Append("</div>");


            return outString.ToString();
        }
        #endregion

        #region ˽�к���:GetTopDropdownMenu(string menuId)
        private string GetTopDropdownMenu(string menuId)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##TopMenu## AND Status = 1 ORDER BY OrderId ", menuId);

            AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
            {
                if (item.DisplayType == "MenuSplitLine")
                {
                    outString.Append("<li class=\"line\">-*- �����ķָ��� -*-</li>");
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

        #region 属性:GetBottomMenu(IAccountInfo account)
        public string GetBottomMenu(IAccountInfo account)
        {
            VelocityContext context = new VelocityContext();

            context.Put("hostName", HostName);

            context.Put("year", DateTime.Now.Year);

            context.Put("account", account);

            return VelocityManager.Instance.Merge(context, "web/builder/themes/enterprise-portal/NavigationBuilder.GetBottomMenu.vm");
        }
        #endregion

        public string GetShortcutMenu(IAccountInfo account)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" ParentId=##{0}## AND MenuType = ##ShortcutMenu## AND Status = 1 ORDER BY OrderId ", Guid.Empty);

            AppsContext.Instance.ApplicationMenuService.FindAll(whereClause).ToList().ForEach(item =>
            {
                //
                // �˵� DisplayType = "MenuItem" , �˵���. 
                // �˵� Status = 0 , ����.
                //
                //if (item.DisplayType == "MenuItem")
                //{
                //}
                if (item.DisplayType == "MenuSplitLine")
                {
                    outString.Append("<li class=\"line\">-*- �����ķָ��� -*-</li>");
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

            outString.Append("<!-- Ӧ�ò˵�����ʼ��-->");
            outString.Append("<div id=\"windowApplicationMenuContainer\" class=\"ajax-menu-slide-menu-container\">");
            outString.Append("<div id=\"windowApplicationMenuWrapper\" class=\"ajax-menu-slide-menu-wrapper\">");
            outString.Append("<div class=\"ajax-menu-slide-menu-submenu first-child\" >");
            outString.Append("<span>" + application.ApplicationDisplayName + "</span>");

            foreach (ApplicationMenuInfo item in list)
            {
                if (item.DisplayType == "MenuGroup")
                {
                    outString.Append("</div>");
                    outString.Append("<div class=\"ajax-menu-slide-menu-submenu\" >");
                    outString.Append("<span>" + item.Name + "</span>");
                }
                else if (item.DisplayType == "MenuSplitLine")
                {
                    // Ĭ�������²���ʾ
                }
                else
                {
                    if (currentMenuId == item.Id || currentMenuFullPath == item.FullPath)
                    {
                        outString.AppendFormat("<a id=\"{0}\" href=\"{1}\" target=\"{2}\" class=\"current\" >{3}</a>", item.Id, ReplaceOptionValue(options, item.Url), item.Target, item.Name);
                    }
                    else
                    {
                        outString.AppendFormat("<a id=\"{0}\" href=\"{1}\" target=\"{2}\" >{3}</a>", item.Id, ReplaceOptionValue(options, item.Url), item.Target, item.Name);
                    }
                }
            }

            outString.Append("</div>");
            outString.Append("</div>");
            outString.Append("</div>");

            outString.Append("<!-- Ӧ�ò˵���������-->");

            return outString.ToString();
        }

        public string GetApplicationFunctionMenu(IAccountInfo account, string applicationFunctionName)
        {
            return string.Empty;
        }
    }
}