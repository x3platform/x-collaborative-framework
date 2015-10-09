#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AuthorityProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Security.Authority.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Security.Authority.Configuration;
  using X3Platform.Security.Authority.IDAL;
  using X3Platform.Data;
  using Common.Logging;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class AuthorityProvider : IAuthorityProvider
  {
    /// <summary>日志记录器</summary>
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>配置</summary>
    private AuthorityConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Authority";

    #region 构造函数:AuthorityProvider()
    /// <summary>构造函数</summary>
    public AuthorityProvider()
    {
      this.configuration = AuthorityConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    //-------------------------------------------------------
    // 保存 添加 修改 删除 
    //-------------------------------------------------------

    #region 函数:Save(AuthorityInfo param)
    /// <summary>保存记录</summary>
    /// <param name="param">AuthorityInfo 实例详细信息</param>
    /// <returns>AuthorityInfo 实例详细信息</returns>
    public AuthorityInfo Save(AuthorityInfo param)
    {
      if (!IsExist(param.Id))
      {
        Insert(param);
      }
      else
      {
        Update(param);
      }

      return param;
    }
    #endregion

    #region 属性:Insert(AuthorityInfo param)
    /// <summary>���Ӽ�¼</summary>
    /// <param name="param">ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</param>
    public void Insert(AuthorityInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
    }
    #endregion

    #region 函数:Update(AuthorityInfo param)
    /// <summary>修改记录</summary>
    /// <param name="param">AuthorityInfo 实例的详细信息</param>
    public void Update(AuthorityInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
    }
    #endregion

    #region 函数:Delete(string ids)
    /// <summary>删除记录</summary>
    /// <param name="param">AuthorityInfo 实例的标识信息,多个以逗号隔开</param>
    public void Delete(string ids)
    {
      if (string.IsNullOrEmpty(ids))
        return;

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
    }
    #endregion

    //-------------------------------------------------------
    // 查询
    //-------------------------------------------------------

    #region 函数:FindOne(string id)
    /// <summary>查询某条记录</summary>
    /// <param name="id">AuthorityInfo Id号</param>
    /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
    public AuthorityInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", StringHelper.ToSafeSQL(id));

      return this.ibatisMapper.QueryForObject<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
    }
    #endregion

    #region 函数:FindOneByName(string name)
    /// <summary>查询某条记录</summary>
    /// <param name="name">权限名称</param>
    /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
    public AuthorityInfo FindOneByName(string name)
    {
      try
      {
        Dictionary<string, object> args = new Dictionary<string, object>();

        args.Add("Name", StringHelper.ToSafeSQL(name));

        AuthorityInfo param = this.ibatisMapper.QueryForObject<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);

        if (param == null)
        {
          logger.Debug(string.Format("[{0}] is null", name));
          logger.Error(this.ibatisMapper.DataSource.ConnectionString);
        }

        return param;
      }
      catch (Exception ex)
      {
        logger.Error(this.ibatisMapper.DataSource.ConnectionString);
        logger.Error(ex);
        throw ex;
      }
    }
    #endregion

    #region 函数:FindAll(DataQuery query)
    /// <summary>查询所有相关记录</summary>
    /// <param name="query">查询参数</param>
    /// <returns>返回所有<see cref="AuthorityInfo"/>实例的详细信息</returns>
    public IList<AuthorityInfo> FindAll(DataQuery query)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql());
      args.Add("Length", query.Length);

      return this.ibatisMapper.QueryForList<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
    }
    #endregion

    //-------------------------------------------------------
    // 自定义功能
    //-------------------------------------------------------

    #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="query">数据查询参数</param>
    /// <param name="rowCount">行数</param>
    /// <returns>返回一个列表<see cref="AuthorityInfo"/>实例</returns> 
    public IList<AuthorityInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();
      
      args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" }, { "Value", "LIKE" } }));
      args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<AuthorityInfo> list = this.ibatisMapper.QueryForList<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string id)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="id">标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new Exception("标识必须填写。");

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion
  }
}
