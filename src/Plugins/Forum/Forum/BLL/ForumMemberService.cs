// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ForumThreadService.cs
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
using X3Platform.DigitalNumber;
using X3Platform.Data;

namespace X3Platform.Plugins.Forum.BLL
{
  public class ForumMemberService : IForumMemberService
  {
    /// <summary>配置</summary>
    private ForumConfiguration configuration = null;

    /// <summary>数据提供器</summary>
    private IForumMemberProvider provider = null;

    #region 构造函数:ForumMemberService()
    /// <summary>构造函数</summary>
    public ForumMemberService()
    {
      this.configuration = ForumConfigurationView.Instance.Configuration;

      // 创建对象构建器(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ForumConfiguration.ApplicationName, springObjectFile);

      // 创建数据提供器
      this.provider = objectBuilder.GetObject<IForumMemberProvider>(typeof(IForumMemberProvider));
    }
    #endregion

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(ForumMemberInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ForumMemberInfo"/>详细信息</param>
    /// <returns>实例<see cref="ForumMemberInfo"/>详细信息</returns>
    public ForumMemberInfo Save(ForumMemberInfo param)
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
    /// <returns>返回实例<see cref="ForumMemberInfo"/>的详细信息</returns>
    public ForumMemberInfo FindOne(string id)
    {
      return provider.FindOne(id);
    }
    #endregion

    #region 函数:FindOneByAccountId(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ForumMemberInfo"/>的详细信息</returns>
    public ForumMemberInfo FindOneByAccountId(string id)
    {
      return this.provider.FindOneByAccountId(id);
    }
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有实例<see cref="ForumMemberInfo"/>的详细信息</returns>
    public IList<ForumMemberInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有实例<see cref="ForumMemberInfo"/>的详细信息</returns>
    public IList<ForumMemberInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ForumMemberInfo"/>的详细信息</returns>
    public IList<ForumMemberInfo> FindAll(string whereClause, int length)
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
    /// <returns>返回一个列表实例<see cref="ForumMemberInfo"/></returns>
    public IList<ForumMemberInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return provider.GetPaging(startIndex, pageSize, query, out rowCount);
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

    #region 函数:SetIconPath(string id)
    /// <summary>
    /// 设置头像
    /// </summary>
    /// <param name="id">用户编号</param>
    /// <returns>布尔值</returns>
    public void SetIconPath(string id)
    {
      // 根据用户编号查询是否存在个人基本信息
      ForumMemberInfo info = this.provider.FindOneByAccountId(id);

      if (info == null)
      {
        info = new ForumMemberInfo();
        info.Id = DigitalNumberContext.Generate("Key_Guid");
        info.AccountId = KernelContext.Current.User.Id;
        info.Signature = "";
        info.Point = 0;
        info.IconPath = KernelContext.Current.User.Id + "_120x120.png";
        info.ModifiedDate = DateTime.Now;
        info.CreatedDate = DateTime.Now;

        this.provider.Insert(info);
      }
      else
      {
        this.provider.SetIconPath(id);
      }
    }
    #endregion

    #region 函数:SetPoint(string id,int score)
    /// <summary>
    /// 增加积分
    /// </summary>
    /// <param name="id">用户编号</param>
    /// <param name="score">积分</param>
    public void SetPoint(string id, int score)
    {
      this.provider.SetPoint(id, score);
    }
    #endregion

    #region 函数:SetThreadCount(string id)
    /// <summary>
    /// 增加帖子数
    /// </summary>
    /// <param name="id">用户编号</param>
    public void SetThreadCount(string id)
    {
      this.provider.SetThreadCount(id);
    }
    #endregion

    #region 函数:SetFollowCount(string id, int value)
    /// <summary>
    /// 增加关注数
    /// </summary>
    /// <param name="id">用户编号</param>
    /// <param name="value">关注数值</param>
    public void SetFollowCount(string id, int value)
    {
      this.provider.SetFollowCount(id, value);
    }
    #endregion

    #region 函数:UpdateMemberInfo(string applicationTag)
    /// <summary>
    /// 同步论坛member信息
    /// </summary>
    /// <param name="applicationTag">论坛模块标识</param>
    /// <returns></returns>
    public bool UpdateMemberInfo(string applicationTag)
    {
      return this.provider.UpdateMemberInfo(applicationTag);
    }
    #endregion
  }
}
