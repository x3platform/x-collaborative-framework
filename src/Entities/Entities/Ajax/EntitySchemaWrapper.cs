namespace X3Platform.Entities.Ajax
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Xml;
  using System.Text;

  using X3Platform.Ajax;
  using X3Platform.Util;

  using X3Platform.Entities.IBLL;
  using X3Platform.Entities.Model;
  using System.Collections;
  #endregion

  /// <summary></summary>
  public class EntitySchemaWrapper : ContextWrapper
  {
    /// <summary>数据服务</summary>
    private IEntitySchemaService service = EntitiesManagement.Instance.EntitySchemaService;

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Save(XmlDocument doc)
    {
      EntitySchemaInfo param = new EntitySchemaInfo();

      param = (EntitySchemaInfo)AjaxUtil.Deserialize(param, doc);

      this.service.Save(param);

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

      this.service.Delete(id);

      return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
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

      EntitySchemaInfo param = this.service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<EntitySchemaInfo>(param) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:FindOneByName(XmlDocument doc)
    /// <summary>获取详细信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string FindOneByName(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string name = XmlHelper.Fetch("name", doc);

      EntitySchemaInfo param = this.service.FindOneByName(name);

      outString.Append("{\"data\":" + AjaxUtil.Parse<EntitySchemaInfo>(param) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:FindOneByEntityClassName(XmlDocument doc)
    /// <summary>获取详细信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string FindOneByEntityClassName(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string entityClassName = XmlHelper.Fetch("entityClassName", doc);

      EntitySchemaInfo param = this.service.FindOneByEntityClassName(entityClassName);

      outString.Append("{\"data\":" + AjaxUtil.Parse<EntitySchemaInfo>(param) + ",");

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
      StringBuilder outString = new StringBuilder();

      string whereClause = XmlHelper.Fetch("whereClause", doc);

      int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

      IList<EntitySchemaInfo> list = this.service.FindAll(whereClause, length);

      outString.Append("{\"data\":" + AjaxUtil.Parse<EntitySchemaInfo>(list) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

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

      PagingHelper paging = PagingHelper.Create(XmlHelper.Fetch("paging", doc, "xml"));

      int rowCount = -1;

      IList<EntitySchemaInfo> list = this.service.GetPaging(paging.RowIndex, paging.PageSize, paging.WhereClause, paging.OrderBy, out rowCount);

      paging.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<EntitySchemaInfo>(list) + ",");

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

    #region 函数:GetCombobox(XmlDocument doc)
    /// <summary>获取权重值列表</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetCombobox(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string combobox = XmlHelper.Fetch("combobox", doc);

      string selectedValue = XmlHelper.Fetch("selectedValue", doc);

      // 显示全部流程实例
      // string whereClause = XmlHelper.Fetch("whereClause", doc);
      string whereClause = " 1 = 1 ";

      // 容错处理
      if (string.IsNullOrEmpty(selectedValue))
      {
        selectedValue = "-1";
      }

      if (whereClause.ToUpper().IndexOf("ORDER") == -1)
      {
        whereClause = whereClause + " ORDER BY OrderId ";
      }

      IList<EntitySchemaInfo> list = this.service.FindAll(whereClause, 0);

      outString.Append("{\"data\":[");

      foreach (EntitySchemaInfo item in list)
      {
        outString.Append("{\"text\":\"" + item.Name + "\",\"value\":\"" + item.EntityClassName + "\",\"selected\":\"" + ((selectedValue == item.EntityClassName) ? true : false) + "\"}" + ",");
      }

      if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
      {
        outString = outString.Remove(outString.Length - 1, 1);
      }

      outString.Append("],");

      outString.Append("\"combobox\":\"" + combobox + "\",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion

    #region 函数:GetDynamicTreeView(XmlDocument doc)
    /// <summary></summary>
    /// <param name="doc"></param>
    /// <returns></returns>
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

      // 树形控件默认根节点标识为0, 需要特殊处理.
      parentId = (string.IsNullOrEmpty(parentId) || parentId == "0") ? treeViewRootTreeNodeId : parentId;

      IList<EntitySchemaInfo> list = this.service.FindAll();

      StringBuilder outString = new StringBuilder();

      outString.Append("{\"data\":");
      outString.Append("{\"tree\":\"" + tree + "\",");
      outString.Append("\"parentId\":\"" + parentId + "\",");
      outString.Append("childNodes:[");

      foreach (EntitySchemaInfo item in list)
      {
        outString.Append("{");
        outString.Append("\"id\":\"" + item.Id + "\",");
        outString.Append("\"parentId\":\"0\",");
        outString.Append("\"name\":\"" + StringHelper.ToSafeJson(item.Name) + "\",");
        outString.Append("\"url\":\"" + StringHelper.ToSafeJson(url.Replace("{treeNodeId}", item.Id).Replace("{treeNodeName}", item.Name)) + "\",");
        outString.Append("\"target\":\"_self\",");
        outString.Append("\"hasChildren\":\"false\",");
        outString.Append("\"ajaxLoading\":\"false\",");
        outString.Append("childNodes:[]");

        outString.Append("},");
      }

      if (outString.Length > 1 && outString.ToString().Substring(outString.Length - 1, 1) == ",")
      {
        outString = outString.Remove(outString.Length - 1, 1);
      }

      // outString.Append(GetDynamicTreeChildNodesView(treeViewRootTreeNodeId, url, index));

      outString.Append("]},\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

      return outString.ToString();
    }
    #endregion
  }
}
