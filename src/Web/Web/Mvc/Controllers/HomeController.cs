namespace X3Platform.Web.Mvc.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Web.Mvc;
  using System.Reflection;
  using System.Linq;
  using System.Text;

  using Common.Logging;

  using X3Platform.Json;
  using X3Platform.Configuration;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Web.Customizes;
  using X3Platform.Navigation.Model;
  using X3Platform.Membership;
  using X3Platform.Docs;
  using X3Platform.Navigation;
  using X3Platform.Web.Builder;


  [LoginFilter]
  public sealed class HomeController : CustomController
  {
    public ActionResult Index()
    {
      if (KernelConfigurationView.Instance.ApplicationHomePage != "/")
      {
        return Redirect(KernelConfigurationView.Instance.ApplicationHomePage);
      }
      /*
      
      // 当前帐户信息
      IAccountInfo account = KernelContext.Current.User;
      // 当前帐户的哟用户详细信息
      IMemberInfo member = MembershipManagement.Instance.MemberService[account.Id];

      // 当前公司标识，如果没有指定则为当前用户默认所属公司
      string portalIdentity = Request.QueryString["portalIdentity"] == null ? string.Empty : Request.QueryString["portalIdentity"];
      // 当前显示的公司信息
      NavigationPortalInfo navigationPortal = null;
      
      if (member.Corporation == null)
      {
        throw new ApplicationException("当前用户没有默认所属公司信息，请联系系统管理员。");
      }

      if (string.IsNullOrEmpty(portalIdentity))
      {
        // 根据所属公司自上而下查找组织相关的门户信息。

        // 首先查找所属公司相关的门户，
        navigationPortal = NavigationContext.Instance.NavigationPortalService.FindOneByOrganizationId(member.CorporationId);

        if (navigationPortal == null)
        {
          // 如果公司没有设置门户，则查找一级部门信息。
          navigationPortal = NavigationContext.Instance.NavigationPortalService.FindOneByOrganizationId(member.DepartmentId);
        }

        if (navigationPortal == null)
        {
          // 如果一级部门没有设置门户，则查找二级部门信息。
          navigationPortal = NavigationContext.Instance.NavigationPortalService.FindOneByOrganizationId(member.Department2Id);
        }

        if (navigationPortal == null)
        {
          // 如果二级部门没有设置门户，则查找三级部门信息。
          navigationPortal = NavigationContext.Instance.NavigationPortalService.FindOneByOrganizationId(member.Department3Id);
        }
      }
      else
      {
        navigationPortal = NavigationContext.Instance.NavigationPortalService.FindOne(portalIdentity);
      }
      
      if (navigationPortal == null)
      {
        throw new ApplicationException("未找到相关门户信息配置，请联系系统管理员。");
      }
      
      ViewBag.customizePageHtml = GetContainerHtml(navigationPortal, KernelContext.Current.User);

      */

      ViewBag.customizePageHtml = GetContainerHtml();

      return View("~/views/main/default.cshtml");
    }

    private string GetContainerHtml()
    {
      StringBuilder outString = new StringBuilder();

      outString.Append("<table style=\"width:100%;\" >");
      outString.Append("<tr>");
      outString.Append("<td>");

      outString.Append("<div class=\"x-ui-pkg-customize-wrapper\">");

      outString.Append(BuilderContext.Instance.CustomizeManagement.ParseHomePage("Schema", "00000000-0000-0000-0000-000000000001"));

      outString.Append("</div>");
      outString.Append("</td>");
      outString.Append("</tr>");
      outString.Append("</table>");

      return outString.ToString();
    }

    private string GetContainerHtml(NavigationPortalInfo navigationPortal, IAccountInfo account)
    {
      StringBuilder outString = new StringBuilder();

      outString.Append("<table style=\"width:100%;\" >");
      outString.Append("<tr>");
      outString.Append("<td style=\"width:137px; padding-right:10px; vertical-align:top;\" >");

      IList<NavigationPortalSidebarItemGroupInfo> sidebarItemGroups = NavigationContext.Instance.NavigationPortalSidebarItemGroupService.FindAllByPortalId(navigationPortal.Id);

      IList<NavigationPortalSidebarItemInfo> sidebarItems = NavigationContext.Instance.NavigationPortalSidebarItemService.FindAllByPortalId(navigationPortal.Id);

      int sidebarItemGroupIndex = 0;

      outString.Append("<div class=\"ajax-accordion-container\" >");
      outString.Append("<div id=\"portal$sidebarItemGroup$accordion\" class=\"ajax-accordion-wrapper\" >");
      outString.Append("<script type=\"text/javascript\">");
      outString.Append("var treeViewNodeStorages=[];");
      outString.Append("</script>");

      foreach (NavigationPortalSidebarItemGroupInfo sidebarItemGroup in sidebarItemGroups)
      {
        if (sidebarItemGroup.Status == 1)
        {
          outString.AppendFormat("<div id=\"portal$sidebarItemGroup$accordion${0}\" class=\"ajax-accordion\" >", sidebarItemGroup.Id);

          if (sidebarItemGroupIndex == 0)
          {
            outString.AppendFormat("<a class=\"ajax-accordion-header begin\" >{0}</a>", sidebarItemGroup.Text);
          }
          else if (sidebarItemGroupIndex + 1 == sidebarItemGroups.Count)
          {
            outString.AppendFormat("<a class=\"ajax-accordion-header end\" >{0}</a>", sidebarItemGroup.Text);
          }
          else
          {
            outString.AppendFormat("<a class=\"ajax-accordion-header\" >{0}</a>", sidebarItemGroup.Text);
          }

          outString.Append("<div class=\"ajax-accordion-content\" style=\"display:none;\" >");
          outString.AppendFormat("<div id=\"portal$sidebarItemGroup$accordion$tree${0}\" class=\"ajax-accordion-tree-view\" ></div>", sidebarItemGroup.Id);
          outString.Append("<script type=\"text/javascript\">");
          outString.AppendFormat("treeViewNodeStorages[{0}]=[", sidebarItemGroupIndex);

          // {"id":"无合同无工作联系单类的报销",
          // "parentId":"0",
          // "name":"无合同无工作联系单类的报销",
          // "url":"javascript:xTreeExtend.selected('无合同无工作联系单类的报销','0','无合同无工作联系单类的报销')",
          // "title":"无合同无工作联系单类的报销",
          // "target":"",
          // "hasChildren":"true"},
          sidebarItems.Where<NavigationPortalSidebarItemInfo>((item, index) => (item.SidebarItemGroupId == sidebarItemGroup.Id && item.Status == 1)).ToList().ForEach(item =>
          {
            if (item.Status == 1)
            {
              outString.Append("{");
              outString.Append("id:\"" + item.Id + "\",");
              outString.Append("parentId:\"" + (item.ParentId == "00000000-0000-0000-0000-000000000000" ? "0" : item.ParentId) + "\",");
              outString.Append("name:\"" + item.Text + "\",");
              outString.Append("url:\"" + item.Url + "\",");
              outString.Append("title:\"" + item.Description + "\",");
              outString.Append("target:\"" + item.Target + "\",");
              outString.Append("hasChildren:\"true\"");
              outString.Append("},");
            }
          });

          if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
          {
            outString = outString.Remove(outString.Length - 1, 1);
          }

          outString.AppendLine("];");
          outString.Append("</script>");
          outString.Append("</div>");
          outString.Append("</div>");

          sidebarItemGroupIndex++;
        }
      }
      outString.Append("</div>");
      outString.Append("</div>");
      outString.Append("<div class=\"ajax-accordion-placeholder\"></div>");

      outString.Append("</td>");
      outString.Append("<td>");
      // outString.Append("<td>");

      outString.Append("<div class=\"x-ui-pkg-customize-menu\" style=\"padding:0;\" ></div>");
      outString.Append("<div class=\"x-ui-pkg-customize-dialog-wrapper\" style=\"display:none;\" >");
      outString.Append("<div class=\"x-ui-pkg-customize-dialog\" ></div>");
      outString.Append("</div>");
      outString.Append("<div class=\"x-ui-pkg-customize-wrapper\">");

      //if (docEditMode == DocEditMode.Edit && !string.IsNullOrEmpty(resetWidgetZone))
      //{
      //  outString.Append(CustomizeContext.Instance.WidgetZoneService.GetHtml(resetWidgetZone));
      //}
      //else
      //{
      //  outString.Append(BuilderContext.Instance.CustomizeManagement.ParseHomePage("Organization", navigationPortal.OrganizationId));
      //}

      outString.Append(BuilderContext.Instance.CustomizeManagement.ParseHomePage("Organization", navigationPortal.OrganizationId));

      outString.Append("</div>");
      outString.Append("</td>");
      outString.Append("</tr>");
      outString.Append("</table>");

      return outString.ToString();
    }
  }
}
