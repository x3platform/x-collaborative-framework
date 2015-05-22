namespace X3Platform.Apps.IBLL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Text;

  using X3Platform.Spring;
  using X3Platform.Membership.Scope;

  using X3Platform.Apps.Model;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [SpringObject("X3Platform.Apps.IBLL.IApplicationFeatureService")]
  public interface IApplicationFeatureService
  {
    #region 索引:this[string id]
    /// <summary>索引</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ApplicationFeatureInfo this[string id] { get; }
    #endregion

    #region 函数:RefreshCache()
    /// <summary>刷新缓存信息</summary>
    int RefreshCache();
    #endregion

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(ApplicationFeatureInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ApplicationFeatureInfo"/>详细信息</param>
    /// <returns>实例<see cref="ApplicationFeatureInfo"/>详细信息</returns>
    ApplicationFeatureInfo Save(ApplicationFeatureInfo param);
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">实例的标识,多条记录以逗号分开</param>
    void Delete(string ids);
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
    ApplicationFeatureInfo FindOne(string id);
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
    IList<ApplicationFeatureInfo> FindAll();
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
    IList<ApplicationFeatureInfo> FindAll(string whereClause);
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
    IList<ApplicationFeatureInfo> FindAll(string whereClause, int length);
    #endregion

    #region 函数:FindAllByParentId(string parentId)
    /// <summary>查询所有相关记录</summary>
    /// <param name="parentId">父级对象的标识</param>
    /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
    IList<ApplicationFeatureInfo> FindAllByParentId(string parentId);
    #endregion

    #region 函数:FindAllByApplicationId(string applicationId)
    /// <summary>查询所有相关记录</summary>
    /// <param name="applicationId">应用系统的标识</param>
    /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
    IList<ApplicationFeatureInfo> FindAllByApplicationId(string applicationId);
    #endregion

    #region 函数:FindAllBetweenDateTime(DateTime beginDate, DateTime endDate)
    /// <summary>查询所有相关记录</summary>
    /// <param name="beginDate">开始时间</param>
    /// <param name="endDate">开始时间</param>
    /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
    IList<ApplicationFeatureInfo> FindAllBetweenDateTime(DateTime beginDate, DateTime endDate);
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
    /// <returns>返回一个列表实例<see cref="ApplicationMethodInfo"/></returns>
    IList<ApplicationFeatureInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    bool IsExist(string id);
    #endregion

    #region 函数:IsExistName(string name)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="name">应用名称</param>
    /// <returns>布尔值</returns>
    bool IsExistName(string name);
    #endregion

    #region 函数:Authorize(string name, string accountId)
    /// <summary>验证用户是否拥有某一功能权限</summary>
    /// <param name="name">应用功能名称</param>
    /// <param name="accountId">用户帐号标识</param>
    /// <returns>布尔值</returns>
    bool Authorize(string name, string accountId);
    #endregion

    // -------------------------------------------------------
    // 授权范围管理
    // -------------------------------------------------------

    #region 函数:GetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId)
    /// <summary>获取某一个应用系统功能的授权范围的记录.</summary>
    /// <param name="applicationId">所属应用标识</param>
    /// <param name="authorizationObjectType">授权对象类型</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <returns>布尔值</returns>
    DataTable GetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId);
    #endregion

    #region 函数:SetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId, string applicationFeatureIds)
    /// <summary>设置某一个应用系统功能的授权范围的记录.</summary>
    /// <param name="applicationId">所属应用标识</param>
    /// <param name="authorizationObjectType">授权对象类型</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <param name="applicationFeatureIds">应用功能标识, 多个以逗号隔开.</param>
    /// <returns>布尔值</returns>
    void SetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId, string applicationFeatureIds);
    #endregion

    #region 函数:HasAuthority(string entityId, string authorityName, string authorizationObjectId, string entityId)
    /// <summary>判断授权对象是否拥有实体权限的权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorizationObjectType">授权对象类型</param>
    /// <param name="authorizationObjectId">授权对象标识</param>
    /// <returns>布尔值</returns>
    bool HasAuthority(string entityId, string authorityName, string authorizationObjectType, string authorizationObjectId);
    #endregion

    #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
    /// <summary>配置实体对象的权限信息</summary>
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <param name="scopeText">权限范围的文本</param>
    void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText);
    #endregion

    #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
    /// <summary>查询实体对象的权限信息</summary> 
    /// <param name="entityId">实体标识</param>
    /// <param name="authorityName">权限名称</param>
    /// <returns></returns>
    IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName);
    #endregion

    // -------------------------------------------------------
    // 同步管理
    // -------------------------------------------------------

    #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
    ///<summary>获取需要同步的数据</summary>
    /// <param name="beginDate">开始时间</param>
    /// <param name="endDate">结束时间</param>
    IList<ApplicationFeatureInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate);
    #endregion

    #region 函数:SyncFromPackPage(ApplicationFeatureInfo param)
    ///<summary>同步信息</summary>
    ///<param name="param">应用功能信息</param>
    void SyncFromPackPage(ApplicationFeatureInfo param);
    #endregion
  }
}
