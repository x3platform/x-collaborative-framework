namespace X3Platform.Web.Customizes.IBLL
{
  #region Using Libraries
  using System.Collections.Generic;
  using X3Platform.Data;
  using X3Platform.Spring;
  using X3Platform.Web.Customizes.Model;
  #endregion

  /// <summary></summary>
  [SpringObject("X3Platform.Web.Customizes.IBLL.ICustomizeContentService")]
  public interface ICustomizeContentService
  {
    #region 索引:this[string index]
    /// <summary>索引</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    CustomizeContentInfo this[string index] { get; }
    #endregion

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(CustomizeContentInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">CustomizeContentInfo 实例详细信息</param>
    /// <returns>CustomizeContentInfo 实例详细信息</returns>
    CustomizeContentInfo Save(CustomizeContentInfo param);
    #endregion

    #region 函数:Delete(string id)
    /// <summary>删除记录</summary>
    /// <param name="keys">标识,多个以逗号隔开</param>
    void Delete(string id);
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">CustomizeContentInfo Id号</param>
    /// <returns>返回一个 CustomizeContentInfo 实例的详细信息</returns>
    CustomizeContentInfo FindOne(string id);
    #endregion

    #region 函数:FindOneByName(string name)
    /// <summary>查询某条记录</summary>
    /// <param name="name">页面名称</param>
    /// <returns>返回一个 CustomizeContentInfo 实例的详细信息</returns>
    CustomizeContentInfo FindOneByName(string name);
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有 CustomizeContentInfo 实例的详细信息</returns>
    IList<CustomizeContentInfo> FindAll();
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有 CustomizeContentInfo 实例的详细信息</returns>
    IList<CustomizeContentInfo> FindAll(string whereClause);
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有 CustomizeContentInfo 实例的详细信息</returns>
    IList<CustomizeContentInfo> FindAll(string whereClause, int length);
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
    IList<CustomizeContentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">CustomizeContentInfo 实例详细信息</param>
    /// <returns>布尔值</returns>
    bool IsExist(string id);
    #endregion

    #region 函数:IsExistName(string name)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="name">页面名称</param>
    /// <returns>布尔值</returns>
    bool IsExistName(string name);
    #endregion

    #region 函数:GetHtml(string name)
    /// <summary>获取Html文本</summary>
    /// <param name="name">区域划分模板名称</param>
    /// <returns>Html文本</returns>
    string GetHtml(string name);
    #endregion
  }
}