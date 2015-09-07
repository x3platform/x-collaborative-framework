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
  #endregion

  /// <summary></summary>
  public class EntityClickWrapper : ContextWrapper
  {
    /// <summary>数据服务</summary>
    private IEntityClickService service = EntitiesManagement.Instance.EntityClickService;

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindAllByEntityId(XmlDocument doc)
    /// <summary>获取列表信息</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string FindAllByEntityId(XmlDocument doc)
    {
      StringBuilder outString = new StringBuilder();

      string customTableName = XmlHelper.Fetch("customTableName", doc);
      string entityId = XmlHelper.Fetch("entityId", doc);
      string entityClassName = XmlHelper.Fetch("entityClassName", doc);

      DataResultMapper mapper = DataResultMapper.CreateMapper(doc.DocumentElement);

      IList<IEntityClickInfo> list = null;

      if (mapper.Count == 0)
      {
        list = this.service.FindAllByEntityId(customTableName, entityId, entityClassName);
      }
      else
      {
        list = this.service.FindAllByEntityId(customTableName, entityId, entityClassName, mapper);
      }

      outString.Append("{\"data\":" + AjaxUtil.Parse<IEntityClickInfo>(list) + ",");

      outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功.\"}}");

      return outString.ToString();
    }
    #endregion

    // -------------------------------------------------------
    // 自定义功能
    // -------------------------------------------------------

    #region 函数:Increment(XmlDocument doc)
    /// <summary>点击量自增</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns>
    public string Increment(XmlDocument doc)
    {
      string customTableName = XmlHelper.Fetch("customTableName", doc);
      string entityId = XmlHelper.Fetch("entityId", doc);
      string entityClassName = XmlHelper.Fetch("entityClassName", doc);
      string accountId = KernelContext.Current.User.Id;

      this.service.Increment(customTableName, entityId, entityClassName, accountId);

      return "{\"message\":{\"returnCode\":0,\"value\":\"点击成功。\"}}";
    }
    #endregion
  }
}
