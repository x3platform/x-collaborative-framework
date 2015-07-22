namespace X3Platform.Plugins.Forum.BLL
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  using X3Platform.Data;
  using X3Platform.DigitalNumber;
  using X3Platform.Spring;

  using X3Platform.Plugins.Forum.Configuration;
  using X3Platform.Plugins.Forum.IBLL;
  using X3Platform.Plugins.Forum.IDAL;
  using X3Platform.Plugins.Forum.Model;
  
  public class ForumFollowService : IForumFollowService
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>数据提供器</summary>
    private IForumFollowProvider provider = null;

    #region 构造函数:ForumFollowService()
    /// <summary>构造函数</summary>
    public ForumFollowService()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      // 创建对象构建器(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ForumConfiguration.ApplicationName, springObjectFile);

      // 创建数据提供器
      this.provider = objectBuilder.GetObject<IForumFollowProvider>(typeof(IForumFollowProvider));
    }
    #endregion

    // -------------------------------------------------------
    // 添加 删除
    // -------------------------------------------------------

    #region 函数:Insert(string,accountId,string followAccountId)
    /// <summary>
    /// 添加关注用户
    /// </summary>
    /// <param name="accountId">主标识</param>
    /// <param name="followAccountId">添加标识</param>
    public void Insert(string accountId, string followAccountId)
    {
      this.provider.Insert(accountId, followAccountId);
    }
    #endregion

    #region 函数:Delete(string,accountId,string followAccountId)
    /// <summary>
    /// 删除关注用户
    /// </summary>
    /// <param name="accountId">主标识</param>
    /// <param name="followAccountId">添加标识</param>
    public void Delete(string accountId, string followAccountId)
    {
      this.provider.Delete(accountId, followAccountId);
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
    /// <returns>返回一个列表实例<see cref="ForumMemberInfo"/></returns>
    public IList<ForumFollowInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="orderBy">ORDER BY 排序条件</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表实例<see cref="ForumFollowQueryInfo"/></returns>
    public IList<ForumFollowQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetQueryObjectPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region 函数:IsExist(string accountId,string followAccountId)
    /// <summary>
    /// 判断是否已经关注用户
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="followAccountId"></param>
    /// <returns></returns>
    public bool IsExist(string accountId, string followAccountId)
    {
      return this.provider.IsExist(accountId, followAccountId);
    }
    #endregion

    #region 函数:IsMutual(string FollowAccountId)
    /// <summary>
    /// 查看是否相互关注
    /// </summary>
    /// <param name="FollowAccountId">被关注人标识</param>
    /// <returns></returns>
    public bool IsMutual(string FollowAccountId)
    {
      return this.provider.IsMutual(FollowAccountId);
    }
    #endregion

    #region 函数:GetFollowCount(string accountId)
    /// <summary>
    /// 根据用户标识查询用户人气
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public int GetFollowCount(string accountId)
    {
      return this.provider.GetFollowCount(accountId);
    }
    #endregion
  }
}
