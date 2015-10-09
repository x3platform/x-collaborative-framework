namespace X3Platform.Apps.BLL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Text;

  using X3Platform.CacheBuffer;
  using X3Platform.Membership;
  using X3Platform.Spring;

  using X3Platform.Apps.Configuration;
  using X3Platform.Apps.IBLL;
  using X3Platform.Apps.IDAL;
  using X3Platform.Apps.Model;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  public class ApplicationSettingService : IApplicationSettingService
  {
    /// <summary>配置</summary>
    private AppsConfiguration configuration = null;

    /// <summary>数据提供器</summary>
    private IApplicationSettingProvider provider = null;

    /// <summary>参数文本信息缓存存储</summary>
    private Dictionary<string, ApplicationSettingInfo> settingTextDictionary = new Dictionary<string, ApplicationSettingInfo>();

    /// <summary>参数值信息缓存存储</summary>
    private Dictionary<string, ApplicationSettingInfo> settingValueDictionary = new Dictionary<string, ApplicationSettingInfo>();

    #region 构造函数:ApplicationSettingService()
    /// <summary>构造函数</summary>
    public ApplicationSettingService()
    {
      this.configuration = AppsConfigurationView.Instance.Configuration;

      // 创建对象构建器(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

      // 创建数据提供器
      this.provider = objectBuilder.GetObject<IApplicationSettingProvider>(typeof(IApplicationSettingProvider));
    }
    #endregion

    #region 索引:this[string id]
    /// <summary>索引</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ApplicationSettingInfo this[string id]
    {
      get { return this.FindOne(id); }
    }
    #endregion

    // -------------------------------------------------------
    // 保存 删除
    // -------------------------------------------------------

    #region 函数:Save(ApplicationSettingInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">实例<see cref="ApplicationSettingInfo"/>详细信息</param>
    /// <returns>实例<see cref="ApplicationSettingInfo"/>详细信息</returns>
    public ApplicationSettingInfo Save(ApplicationSettingInfo param)
    {
      return this.provider.Save(param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="ids">实例的标识,多条记录以逗号分开</param>
    public void Delete(string ids)
    {
      this.provider.Delete(ids);
    }
    #endregion

    // -------------------------------------------------------
    // 查询
    // -------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">标识</param>
    /// <returns>返回实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
    public ApplicationSettingInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region 函数:FindAll()
    /// <summary>查询所有相关记录</summary>
    /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
    public IList<ApplicationSettingInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region 函数:FindAll(string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
    public IList<ApplicationSettingInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region 函数:FindAll(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
    public IList<ApplicationSettingInfo> FindAll(string whereClause, int length)
    {
      return this.provider.FindAll(whereClause, length);
    }
    #endregion

    #region 函数:FindAllQueryObject(string whereClause, int length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <param name="length">条数</param>
    /// <returns>返回所有实例<see cref="ApplicationSettingQueryInfo"/>的详细信息</returns>
    public IList<ApplicationSettingQueryInfo> FindAllQueryObject(string whereClause, int length)
    {
      return this.provider.FindAllQueryObject(whereClause, length);
    }
    #endregion

    #region 函数:FindAllByApplicationSettingGroupId(string applicationSettingGroupId)
    /// <summary>根据参数分组信息查询所有相关记录</summary>
    /// <param name="applicationSettingGroupId">参数分组标识</param>
    /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
    public IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupId(string applicationSettingGroupId)
    {
      // return this.provider.FindAllByApplicationSettingGroupId(applicationSettingGroupId, null, this.BindAuthorizationScopeSQL(string.Empty));
      return this.provider.FindAllByApplicationSettingGroupId(applicationSettingGroupId, null, string.Empty);
    }
    #endregion

    #region 函数:FindAllByApplicationSettingGroupId(string applicationSettingGroupId, string keyword)
    /// <summary>根据参数分组信息查询所有相关记录</summary>
    /// <param name="applicationSettingGroupId">参数分组标识</param>
    /// <param name="keyword">文本信息关键字匹配</param>
    /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
    public IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupId(string applicationSettingGroupId, string keyword)
    {
      // return this.provider.FindAllByApplicationSettingGroupId(applicationSettingGroupId, keyword, this.BindAuthorizationScopeSQL(string.Empty));
      return this.provider.FindAllByApplicationSettingGroupId(applicationSettingGroupId, keyword, string.Empty);
    }
    #endregion

    #region 函数:FindAllByApplicationSettingGroupName(string applicationSettingGroupName)
    /// <summary>根据参数分组信息查询所有相关记录</summary>
    /// <param name="applicationSettingGroupName">参数分组名称</param>
    /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
    public IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupName(string applicationSettingGroupName)
    {
      return this.provider.FindAllByApplicationSettingGroupName(applicationSettingGroupName, null, string.Empty);
    }
    #endregion

    #region 函数:FindAllByApplicationSettingGroupName(string applicationSettingGroupName, string keyword)
    /// <summary>根据参数分组信息查询所有相关记录</summary>
    /// <param name="applicationSettingGroupName">参数分组名称</param>
    /// <param name="keyword">文本信息关键字匹配</param>
    /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
    public IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupName(string applicationSettingGroupName, string keyword)
    {
      return this.provider.FindAllByApplicationSettingGroupName(applicationSettingGroupName, keyword, string.Empty);
    }
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
    /// <returns>返回一个列表实例<see cref="ApplicationSettingInfo"/></returns>
    public IList<ApplicationSettingInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
    }
    #endregion

    #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="orderBy">ORDER BY 排序条件</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表实例<see cref="ApplicationSettingQueryInfo"/></returns>
    public IList<ApplicationSettingQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
    {
      return this.provider.GetQueryObjectPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      return this.provider.IsExist(id);
    }
    #endregion

    #region 函数:GetText(string applicationId, string applicationSettingGroupName, string value)
    /// <summary>根据配置的文本获取值信息</summary>
    /// <param name="applicationId"></param>
    /// <param name="applicationSettingGroupName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public string GetText(string applicationId, string applicationSettingGroupName, string value)
    {
      ApplicationSettingInfo param = FetchDictionaryItem(applicationId, applicationSettingGroupName, "value", value);

      // 如果缓存中未找到相关数据，则查找数据库内容
      return param == null ? this.provider.GetText(applicationId, applicationSettingGroupName, value) : param.Text;
    }
    #endregion

    #region 函数:GetValue(string applicationId, string applicationSettingGroupName, string text)
    /// <summary>根据配置的文本获取值信息</summary>
    /// <param name="applicationId"></param>
    /// <param name="applicationSettingGroupName"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public string GetValue(string applicationId, string applicationSettingGroupName, string text)
    {
      ApplicationSettingInfo param = FetchDictionaryItem(applicationId, applicationSettingGroupName, "value", text);

      // 如果缓存中未找到相关数据，则查找数据库内容
      return param == null ? this.provider.GetValue(applicationId, applicationSettingGroupName, text) : param.Value;
    }
    #endregion

    #region 函数:FetchDictionaryItem(string applicationId, string applicationSettingGroupName)
    ///<summary>获取需要同步的数据</summary>
    ///<param name="param">应用参数信息</param>
    private ApplicationSettingInfo FetchDictionaryItem(string applicationId, string applicationSettingGroupName, string fetchItemType, string fetchItemValue)
    {
      ApplicationSettingInfo param = null;

      string cacheKey = string.Format("{0}${1}${2}${3}", applicationId, applicationSettingGroupName, fetchItemType, fetchItemValue);

      // 初始化缓存
      if (settingTextDictionary.Count == 0)
      {
        IList<ApplicationSettingInfo> list = AppsContext.Instance.ApplicationSettingService.FindAll();

        foreach (ApplicationSettingInfo item in list)
        {
          string prefix = string.Format("{0}${1}", item.ApplicationId, item.ApplicationSettingGroupName);

          if (this.settingValueDictionary.ContainsKey(string.Format("{0}$text${1}", prefix, item.Text)))
          {
            AppsContext.Log.Warn(string.Format("{0}$text${1}", prefix, item.Text)+ " is exists.");
          }
          else
          {
            this.settingTextDictionary.Add(string.Format("{0}$text${1}", prefix, item.Text), item);
          }

          if (this.settingValueDictionary.ContainsKey(string.Format("{0}$value${1}", prefix, item.Value)))
          {
            AppsContext.Log.Warn(string.Format("{0}$value${1}", prefix, item.Value) + " is exists.");
          }
          else
          {
            this.settingValueDictionary.Add(string.Format("{0}$value${1}", prefix, item.Value), item);
          }
        }
      }

      // 查找缓存数据
      if (fetchItemType == "text")
      {
        if (this.settingTextDictionary.ContainsKey(cacheKey))
        {
          param = this.settingTextDictionary[cacheKey];
        }
      }
      else if (fetchItemType == "value")
      {
        if (this.settingValueDictionary.ContainsKey(cacheKey))
        {
          param = this.settingValueDictionary[cacheKey];
        }
      }

      return param;
    }
    #endregion

    // -------------------------------------------------------
    // 同步管理
    // -------------------------------------------------------

    #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
    ///<summary>获取需要同步的数据</summary>
    ///<param name="param">应用参数信息</param>
    public IList<ApplicationSettingInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate)
    {
      string whereClause = string.Format(" ModifiedDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

      return this.provider.FindAll(whereClause, 0);
    }
    #endregion

    #region 函数:SyncFromPackPage(ApplicationSettingInfo param)
    ///<summary>同步信息</summary>
    ///<param name="param">应用参数信息</param>
    public void SyncFromPackPage(ApplicationSettingInfo param)
    {
      this.provider.SyncFromPackPage(param);

      // this.BindAuthorizationScopeObjects(param.Id, "应用_通用_查看权限", param.AuthorizationReadScopeObjectText);
    }
    #endregion
  }
}
