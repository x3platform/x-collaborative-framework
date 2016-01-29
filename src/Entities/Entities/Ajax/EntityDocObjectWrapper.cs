namespace X3Platform.Entities.Ajax
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Xml;
  using System.Text;

  using X3Platform.Ajax;
  using X3Platform.Data;
  using X3Platform.Util;

  using X3Platform.Entities.IBLL;
  using X3Platform.Entities.Model;
  using X3Platform.Membership.Scope;
  using X3Platform.Membership;
  using X3Platform.Membership.IBLL;
    using X3Platform.Globalization;
  #endregion

  /// <summary></summary>
  public class EntityDocObjectWrapper : ContextWrapper
  {
    /// <summary>数据服务</summary>
    private IEntityDocObjectService service = EntitiesManagement.Instance.EntityDocObjectService;

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindAllByDocToken(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string FindAllByDocToken(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string customTableName = XmlHelper.Fetch("customTableName", doc);

      string docToken = XmlHelper.Fetch("docToken", doc);

      DataResultMapper mapper = DataResultMapper.CreateMapper(doc.DocumentElement);

      IList<IEntityDocObjectInfo> list = null;

      if (mapper.Count == 0)
      {
        list = this.service.FindAllByDocToken(customTableName, docToken);
      }
      else
      {
        list = this.service.FindAllByDocToken(customTableName, docToken, mapper);
      }

      outString.Append("{\"data\":" + AjaxUtil.Parse<IEntityDocObjectInfo>(list) + ",");

      outString.Append(GenericException.Serialize(0, I18n.Strings["msg_query_success"], true) + "}");

      return outString.ToString();
    }
    #endregion
  }
}
