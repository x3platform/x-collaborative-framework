namespace X3Platform.Web.Customizes.IDAL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Data;
  using X3Platform.Data;
  using X3Platform.Messages;
  using X3Platform.Spring;
  using X3Platform.Web.Customizes.Model;
  #endregion

  /// <summary></summary>
  [SpringObject("X3Platform.Web.Customizes.IDAL.ICustomizeLayoutProvider")]
  public interface ICustomizeLayoutProvider
  {
    // -------------------------------------------------------
    // 事务支持
    // -------------------------------------------------------

    #region 函数:BeginTransaction()
    /// <summary>启动事务</summary>
    void BeginTransaction();
    #endregion

    #region 函数:BeginTransaction(IsolationLevel isolationLevel)
    /// <summary>启动事务</summary>
    /// <param name="isolationLevel">事务隔离级别</param>
    void BeginTransaction(IsolationLevel isolationLevel);
    #endregion

    #region 函数:CommitTransaction()
    /// <summary>提交事务</summary>
    void CommitTransaction();
    #endregion

    #region 函数:RollBackTransaction()
    /// <summary>回滚事务</summary>
    void RollBackTransaction();
    #endregion

    // -------------------------------------------------------
    // 保存 添加 修改 删除
    // -------------------------------------------------------

    #region 函数:Save(CustomizeLayoutInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">CustomizeLayoutInfo 实例详细信息</param>
    /// <returns>CustomizeLayoutInfo 实例详细信息</returns>
    CustomizeLayoutInfo Save(CustomizeLayoutInfo param);
    #endregion

    #region 函数:Insert(CustomizeLayoutInfo param)
    /// <summary>添加记录</summary>
    /// <param name="param">CustomizeLayoutInfo 实例的详细信息</param>
    void Insert(CustomizeLayoutInfo param);
    #endregion

    #region 函数:Update(CustomizeLayoutInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">CustomizeLayoutInfo 实例的详细信息</param>
    void Update(CustomizeLayoutInfo param);
    #endregion

    #region 函数:Delete(string id)
    /// <summary>删除记录</summary>
    /// <param name="id">标识</param>
    void Delete(string id);
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">CustomizeLayoutInfo Id号</param>
    /// <returns>返回一个 CustomizeLayoutInfo 实例的详细信息</returns>
    CustomizeLayoutInfo FindOne(string id);
    #endregion

    #region 函数:FindOneByName(string name)
    /// <summary>查询某条记录</summary>
    /// <param name="name">页面名称</param>
    /// <returns>返回一个 CustomizeLayoutInfo 实例的详细信息</returns>
    CustomizeLayoutInfo FindOneByName(string name);
    #endregion

    #region 函数:FindAll(string whereClause,int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有 CustomizeLayoutInfo 实例的详细信息</returns>
    IList<CustomizeLayoutInfo> FindAll(string whereClause, int length);
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
    IList<CustomizeLayoutInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="param">CustomizeLayoutInfo 实例详细信息</param>
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
    string GetHtml(string id);
    #endregion
  }
}
