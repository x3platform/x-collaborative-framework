#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityOperationLogWrapper.cs
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
  using X3Platform.Membership.Scope;
  using X3Platform.Membership;
  using X3Platform.Membership.IBLL;
    using X3Platform.Globalization;
  #endregion

  /// <summary></summary>
  public class EntityScopeWrapper : ContextWrapper
  {
    /// <summary>数据服务</summary>
    private IAuthorizationObjectService service = MembershipManagement.Instance.AuthorizationObjectService;

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:GetAuthorizationScopeObjects(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetAuthorizationScopeObjects(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string scopeTableName = XmlHelper.Fetch("scopeTableName", doc);
      string entityId = XmlHelper.Fetch("entityId", doc);
      string entityClassName = XmlHelper.Fetch("entityClassName", doc);
      string authorityName = XmlHelper.Fetch("authorityName", doc);

      IList<MembershipAuthorizationScopeObject> list = this.service.GetAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);

      outString.Append("{\"data\":\"" + AjaxUtil.Parse<MembershipAuthorizationScopeObject>(list) + "\",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion

    #region 函数:GetAuthorizationScopeObjectText(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetAuthorizationScopeObjectText(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string scopeTableName = XmlHelper.Fetch("scopeTableName", doc);
      string entityId = XmlHelper.Fetch("entityId", doc);
      string entityClassName = XmlHelper.Fetch("entityClassName", doc);
      string authorityName = XmlHelper.Fetch("authorityName", doc);

      string authorizationScopeObjectText = this.service.GetAuthorizationScopeObjectText(scopeTableName, entityId, entityClassName, authorityName);

      outString.Append("{\"data\":\"" + authorizationScopeObjectText + "\",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion

    #region 函数:GetAuthorizationScopeObjectView(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string GetAuthorizationScopeObjectView(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string scopeTableName = XmlHelper.Fetch("scopeTableName", doc);
      string entityId = XmlHelper.Fetch("entityId", doc);
      string entityClassName = XmlHelper.Fetch("entityClassName", doc);
      string authorityName = XmlHelper.Fetch("authorityName", doc);

      string authorizationScopeObjectView = this.service.GetAuthorizationScopeObjectView(scopeTableName, entityId, entityClassName, authorityName);

      outString.Append("{\"data\":\"" + authorizationScopeObjectView + "\",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion
  }
}
