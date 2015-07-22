// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ForumCommentService.cs
//
// Description  :
//
// Author       :RuanYu
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Plugins.Forum.Configuration;
using X3Platform.Plugins.Forum.IBLL;
using X3Platform.Plugins.Forum.IDAL;
using X3Platform.Plugins.Forum.Model;
using X3Platform.Data;

namespace X3Platform.Plugins.Forum.BLL
{
  /// <summary></summary>
  public class ForumCommentService : IForumCommentService
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>数据提供器</summary>
    private IForumCommentProvider provider = null;

    #region 构造函数:ForumCommentService()
    /// <summary>构造函数</summary>
    public ForumCommentService()
    {
      configuration = ForumConfigurationView.Instance.Configuration;

      // 创建对象构建器(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ForumConfiguration.ApplicationName, springObjectFile);

      // 创建数据提供器
      provider = objectBuilder.GetObject<IForumCommentProvider>(typeof(IForumCommentProvider));
    }
    #endregion

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(ForumCommentInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ForumCommentInfo"/>详细信息</param>
    /// <returns>实例<see cref="ForumCommentInfo"/>详细信息</returns>
    public ForumCommentInfo Save(ForumCommentInfo param)
    {
      return provider.Save(param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">实例的标识,多条记录以逗号分开</param>
    public void Delete(string ids)
    {
      provider.Delete(ids);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ForumCommentInfo"/>的详细信息</returns>
    public ForumCommentInfo FindOne(string id)
    {
      return provider.FindOne(id);
    }
    #endregion

    #region 函数:FindOne(string id,string theadId)
    /// <summary>
    /// 查询回帖信息
    /// </summary>
    /// <param name="id">标识</param>
    /// <param name="threaId">主题编号</param>
    /// <returns>返回信息</returns>
    public ForumCommentInfo FindOne(string id, string threaId)
    {
      return provider.FindOne(id, threaId);
    }
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有实例<see cref="ForumCommentInfo"/>的详细信息</returns>
    public IList<ForumCommentInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有实例<see cref="ForumCommentInfo"/>的详细信息</returns>
    public IList<ForumCommentInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumCommentInfo"/>的详细信息</returns>
    public IList<ForumCommentInfo> FindAll(string whereClause, int length)
    {
      return provider.FindAll(whereClause, length);
    }
    #endregion

    // -------------------------------------------------------
    // 自定义功能
    // -------------------------------------------------------

    #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="orderBy">ORDER BY 排序条件</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表实例<see cref="ForumCommentInfo"/></returns>
    public IList<ForumCommentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return provider.GetPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="orderBy">ORDER BY 排序条件</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表实例<see cref="ForumCommentQueryInfo"/></returns>
    public IList<ForumCommentQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录.</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      return provider.IsExist(id);
    }
    #endregion

    #region 函数:GetCommentCount(string id);
    /// <summary>
    /// 根据主题查询回帖数
    /// </summary>
    /// <param name="id">标识</param>
    /// <returns>回帖数</returns>
    public int GetCommentCount(string id)
    {
      return provider.GetCommentCount(id);
    }
    #endregion

    #region 函数:DeleteByTheadId(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">实例的标识,多条记录以逗号分开</param>
    public void DeleteByTheadId(string ids)
    {
      this.provider.DeleteByTheadId(ids);
    }
    #endregion

    #region 函数:GetLastComment(string id)
    /// <summary>
    /// 根据帖子编号查询最后回帖信息
    /// </summary>
    /// <param name="id">帖子编号</param>
    /// <returns>最后回帖信息</returns>
    public string GetLastComment(string id)
    {
      return this.provider.GetLastComment(id);
    }
    #endregion
  }
}
