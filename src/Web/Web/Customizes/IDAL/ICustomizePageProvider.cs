namespace X3Platform.Web.Customizes.IDAL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;

  using X3Platform.Spring;
  using X3Platform.Web.Configuration;
  using X3Platform.Web.Customizes.Model;
  using X3Platform.Messages;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [SpringObject("X3Platform.Web.Customizes.IDAL.ICustomizePageProvider")]
  public interface ICustomizePageProvider
  {
    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">CustomizePageInfo Id号</param>
    /// <returns>返回一个实例<see cref="CustomizePageInfo"/>的详细信息</returns>
    CustomizePageInfo FindOne(string id);
    #endregion

    #region 函数:FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
    /// <summary>查询某条记录</summary>
    /// <param name="authorizationObjectType">授权对象类别</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <param name="name">页面名称</param>
    /// <returns>返回一个实例<see cref="CustomizePageInfo"/>的详细信息</returns>
    CustomizePageInfo FindOneByName(string authorizationObjectType, string authorizationObjectId, string name);
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="CustomizePageInfo"/>的详细信息</returns>
    IList<CustomizePageInfo> FindAll(string whereClause, int length);
    #endregion

    // -------------------------------------------------------
    // 保存 添加 修改 删除
    // -------------------------------------------------------

    #region 函数:Save(CustomizePageInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">CustomizePageInfo 实例详细信息</param>
    /// <returns>CustomizePageInfo 实例详细信息</returns>
    CustomizePageInfo Save(CustomizePageInfo param);
    #endregion

    #region 函数:Insert(CustomizePageInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">CustomizePageInfo 实例的详细信息</param>
    void Insert(CustomizePageInfo param);
    #endregion

    #region 函数:Update(CustomizePageInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">CustomizePageInfo 实例的详细信息</param>
    void Update(CustomizePageInfo param);
    #endregion

    #region 函数:Delete(string id)
    /// <summary>删除记录</summary>
    /// <param name="id">标识</param>
    void Delete(string id);
    #endregion

    // -------------------------------------------------------
    // 自定义功能
    // -------------------------------------------------------

    #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="query">数据查询参数</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表实例</returns> 
    IList<CustomizePageInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="param">CustomizePageInfo 实例详细信息</param>
    /// <returns>布尔值</returns>
    bool IsExist(string id);
    #endregion

    #region 函数:IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="authorizationObjectType">授权对象类别</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <param name="name">页面名称</param>
    /// <returns>布尔值</returns>
    bool IsExistName(string authorizationObjectType, string authorizationObjectId, string name);
    #endregion

    #region 函数:TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name)
    /// <summary>查询某条记录</summary>
    /// <param name="authorizationObjectType">授权对象类别</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <param name="name">页面名称</param>
    /// <returns>返回一个实例<see cref="CustomizePageInfo"/>的详细信息</returns>
    string TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name);
    #endregion
  }
}
