#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityDraftWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

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
    using X3Platform.Globalization; using X3Platform.Messages;
  #endregion

  /// <summary></summary>
  public class EntityDraftWrapper : ContextWrapper
  {
    /// <summary>数据服务</summary>
    private IEntityDraftService service = EntitiesManagement.Instance.EntityDraftService;

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(XmlDocument doc)
    /// <summary>保存记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("save")]
    public string Save(XmlDocument doc)
    {
      EntityDraftInfo param = new EntityDraftInfo();

      param = (EntityDraftInfo)AjaxUtil.Deserialize(param, doc);

      this.service.Save(param);

      return MessageObject.Stringify("0", I18n.Strings["msg_save_success"]);
    }
    #endregion

    #region 函数:Delete(XmlDocument doc)
    /// <summary>删除记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("delete")]
    public string Delete(XmlDocument doc)
    {
      string ids = XmlHelper.Fetch("ids", doc);

      this.service.Delete(ids);

      return MessageObject.Stringify("0", I18n.Strings["msg_delete_success"]);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(XmlDocument doc)
    /// <summary>获取详细信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("findOne")]
    public string FindOne(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string id = XmlHelper.Fetch("id", doc);

      EntityDraftInfo param = this.service.FindOne(id);

      outString.Append("{\"data\":" + AjaxUtil.Parse<EntityDraftInfo>(param) + ",");

      outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion

    #region 函数:FindAll(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("findAll")]
    public string FindAll(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string whereClause = XmlHelper.Fetch("whereClause", doc);

      int length = Convert.ToInt32(XmlHelper.Fetch("length", doc));

      IList<EntityDraftInfo> list = this.service.FindAll(whereClause, length);

      outString.Append("{\"data\":" + AjaxUtil.Parse<EntityDraftInfo>(list) + ",");

      outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

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
    [AjaxMethod("getPages")]
    public string GetPaging(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      PagingHelper pages = PagingHelper.Create(XmlHelper.Fetch("pages", doc, "xml"));

      int rowCount = -1;

      IList<EntityDraftInfo> list = this.service.GetPaging(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

      pages.RowCount = rowCount;

      outString.Append("{\"data\":" + AjaxUtil.Parse<EntityDraftInfo>(list) + ",");

      outString.Append("\"pages\":" + pages + ",");

      outString.Append(MessageObject.Stringify("0", I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion

    #region 函数:IsExist(XmlDocument doc)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    [AjaxMethod("isExist")]
    public string IsExist(XmlDocument doc)
    {
      string id = XmlHelper.Fetch("id", doc);

      bool result = this.service.IsExist(id);

      return "{\"message\":{\"returnCode\":0,\"value\":\"" + result.ToString().ToLower() + "\"}}";
    }
    #endregion
  }
}
