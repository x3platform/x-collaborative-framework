namespace X3Platform.Plugins.Bugs.Ajax
{
  #region Using Libraries
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Xml;
  using System.Text;

  using X3Platform.Ajax;
  using X3Platform.Apps;
  using X3Platform.CategoryIndexes;
  using X3Platform.Web.Component.Combobox;
  using X3Platform.Util;

  using X3Platform.Plugins.Bugs.IBLL;
  using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Plugins.Bugs.Configuration;
  #endregion

  /// <summary></summary>
  public class BugCategoryWrapper : ContextWrapper
  {
    private IBugCategoryService service = BugContext.Instance.BugCategoryService;

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Save(XmlDocument doc)
    {
      BugCategoryInfo param = new BugCategoryInfo();

      string authorizationAddScopeObjectText = XmlHelper.Fetch("authorizationAddScopeObjectText", doc);
      string authorizationReadScopeObjectText = XmlHelper.Fetch("authorizationReadScopeObjectText", doc);
      string authorizationEditScopeObjectText = XmlHelper.Fetch("authorizationEditScopeObjectText", doc);

      param = (BugCategoryInfo)AjaxUtil.Deserialize(param, doc);

      param.BindAuthorizationAddScope(authorizationAddScopeObjectText);
      param.BindAuthorizationReadScope(authorizationReadScopeObjectText);
      param.BindAuthorizationEditScope(authorizationEditScopeObjectText);

      this.service.Save(param);

      this.service.BindAuthorizationScopeObjects(param.Id, "应用_通用_添加权限", authorizationAddScopeObjectText);
      this.service.BindAuthorizationScopeObjects(param.Id, "应用_通用_查看权限", authorizationReadScopeObjectText);
      this.service.BindAuthorizationScopeObjects(param.Id, "应用_通用_修改权限", authorizationEditScopeObjectText);

      return "{\"message\":{\"returnCode\":0,\"value\":\"保存成功。\"}}";
    }
    #endregion

    #region 函数:Delete(XmlDocument doc)
    /// <summary>删除记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Delete(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      if (this.service.CanDelete(id))
      {
        this.service.Delete(id);

        return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
      }
      else
      {
        return "{message:{\"returnCode\":1,\"value\":\"此类别正在被其他数据使用，请移除此类别下的数据后再删除。\"}}";
      }
    }
    #endregion

    #region 函数:Remove(XmlDocument doc)
    /// <summary>删除记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Remove(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      if (this.service.CanDelete(id))
      {
        this.service.Remove(id);

        return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
      }
      else
      {
        return "{message:{\"returnCode\":1,\"value\":\"此类别正在被业务数据使用，请移除此类别下的数据后再删除。\"}}";
      }
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(XmlDocument doc)
    /// <summary>获取详细信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string FindOne(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string id = XmlHelper.Fetch("id", doc);

      BugCategoryInfo param = this.service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<BugCategoryInfo>(param) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:FindAll(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string FindAll(XmlDocument doc)
    {
      IList<BugCategoryInfo> list = this.service.FindAll();

      StringBuilder outString = new StringBuilder();

      outString.Append(AjaxUtil.Parse<BugCategoryInfo>(list));

      return outString.ToString();
    }
    #endregion

    // -------------------------------------------------------
    // 自定义功能
    // -------------------------------------------------------

    #region 函数:GetPaging(XmlDocument doc)
    /// <summary>获取分页内容</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetPaging(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"), XmlHelper.Fetch("query", doc, "xml"));

      // 设置当前用户权限
      if (XmlHelper.Fetch("su", doc) == "1" && AppsSecurity.IsAdministrator(KernelContext.Current.User, BugConfiguration.ApplicationName))
      {
        paging.Query.Variables["elevatedPrivileges"] = "1";
      }

      paging.Query.Variables["accountId"] = KernelContext.Current.User.Id;

      int rowCount = -1;

      IList<BugCategoryQueryInfo> list = this.service.GetQueryObjectPaging(paging.RowIndex, paging.PageSize, paging.Query, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<BugCategoryQueryInfo>(list) + ",");

      outString.Append("\"paging\":" + paging + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:IsExist(XmlDocument doc)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string IsExist(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      bool result = this.service.IsExist(id);

      return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
    }
    #endregion

    #region 函数:SetStatus(XmlDocument doc)
    /// <summary>设置类别状态(停用/启用)</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string SetStatus(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      int status = Convert.ToInt32(XmlHelper.Fetch("status", doc));

      if (this.service.SetStatus(id, status))
      {
        return "{message:{\"returnCode\":0,\"value\":\"操作成功。\"}}";
      }
      else
      {
        return "{message:{\"returnCode\":1,\"value\":\"操作失败。\"}}";
      }
    }
    #endregion

    #region 函数:GetCombobox(XmlDocument doc)
    /// <summary>查询类别数据以供形成类别下拉框数据源（含停用的）</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetCombobox(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string combobox = XmlHelper.Fetch("combobox", doc);

      string selectedValue = XmlHelper.Fetch("selectedValue", doc);

      string emptyItemText = XmlHelper.Fetch("emptyItemText", doc);

      string whereClause = " Status = 1 ORDER BY OrderId ";

      IList<ComboboxItem> list = this.service.GetComboboxByWhereClause(whereClause, selectedValue);

      if (!string.IsNullOrEmpty(emptyItemText))
      {
        list.Insert(0, new ComboboxItem("全部", string.Empty));
      }

      outString.Append("{\"data\":" + FormatCombobox(list) + ",");

      outString.Append("\"combobox\":\"" + combobox + "\",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:GetComboboxWithDrafter(XmlDocument doc)
    /// <summary>查询类别数据以供形成类别下拉框数据源</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetComboboxWithDrafter(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string combobox = XmlHelper.Fetch("combobox", doc);

      string selectedValue = XmlHelper.Fetch("selectedValue", doc);

      string emptyItemText = XmlHelper.Fetch("emptyItemText", doc);

      string whereClause = string.Empty;

      if (AppsSecurity.IsAdministrator(KernelContext.Current.User, BugConfiguration.ApplicationName))
      {
        // 管理员可以编辑所有新闻类别
        whereClause = " Status = 1 ORDER BY OrderId ";
      }
      else
      {
        whereClause = string.Format(@" (
(   Id IN ( 
        SELECT DISTINCT EntityId FROM view_AuthorizationObject_Account View1, tb_Bug_Category_Scope Scope
        WHERE 
            View1.AccountId = ##{0}##
            AND View1.AuthorizationObjectId = Scope.AuthorizationObjectId
            AND View1.AuthorizationObjectType = Scope.AuthorizationObjectType
            AND AuthorityId = ##00000000-0000-0000-0000-000000000002##)) 
) AND Status = 1 ORDER BY OrderId ", KernelContext.Current.User.Id);
      }
      IList<ComboboxItem> list = this.service.GetComboboxByWhereClause(whereClause, selectedValue);

      if (!string.IsNullOrEmpty(emptyItemText))
      {
        list.Insert(0, new ComboboxItem("全部", string.Empty));
      }

      outString.Append("{\"data\":" + FormatCombobox(list) + ",");

      outString.Append("\"combobox\":\"" + combobox + "\",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:GetCategories(XmlDocument doc)
    /// <summary>查询类别数据以供形成其他类别选择时的数据列表</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns></returns>
    public string GetCategories(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = " Status = 1 ORDER BY OrderId ";

      IList<ComboboxItem> list = this.service.GetComboboxByWhereClause(whereClause, string.Empty);

      outString.Append("{\"data\":" + FormatCombobox(list) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:FormatCombobox(IList<ComboboxItem> list)
    /// <summary>格式化下拉框数据为JSON格式</summary>
    private string FormatCombobox(IList<ComboboxItem> list)
    {
      StringBuilder outString = new StringBuilder();

      outString.Append("[");

      for (int i = 0; i < list.Count; i++)
      {
        outString.Append(list[i].ToString());

        if (i < list.Count - 1)
        {
          outString.Append(",");
        }
      }

      outString.Append("]");

      return outString.ToString();
    }
    #endregion

    #region 函数:GetDynamicTreeView(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetDynamicTreeView(XmlDocument doc)
    {
      // 必填字段
      string tree = XmlHelper.Fetch("tree", doc);
      string parentId = XmlHelper.Fetch("parentId", doc);

      // 附加属性
      string treeViewId = XmlHelper.Fetch("treeViewId", doc);
      string treeViewName = XmlHelper.Fetch("treeViewName", doc);
      string treeViewRootTreeNodeId = XmlHelper.Fetch("treeViewRootTreeNodeId", doc);

      string url = XmlHelper.Fetch("url", doc);

      // 是否关闭非叶子节点的js事件
      bool enabledLeafClick = Convert.ToBoolean(XmlHelper.Fetch("enabledLeafClick", doc));

      // 是否提升权限显示所有数据
      bool elevatedPrivileges = Convert.ToBoolean(XmlHelper.Fetch("elevatedPrivileges", doc));

      DynamicTreeView treeView = this.service.GetDynamicTreeView(tree, parentId, url, enabledLeafClick, elevatedPrivileges);

      return "{\"data\":" + treeView.ToString() + ",\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}";
    }
    #endregion
  }
}
