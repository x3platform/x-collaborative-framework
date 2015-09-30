namespace X3Platform.Tasks.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;

  using X3Platform.Util;
  using X3Platform.IBatis.DataMapper;

  using X3Platform.Tasks.IDAL;
  using X3Platform.Tasks.Configuration;
  using X3Platform.Tasks.Model;
  using System.Data;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class TaskWorkReceiverProvider : ITaskWorkReceiverProvider
  {
    /// <summary>配置</summary>
    private TasksConfiguration configuration = null;

    /// <summary>IBatis映射文件</summary>
    private string ibatisMapping = null;

    /// <summary>IBatis映射对象</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>数据表名</summary>
    private string tableName = "tb_Task_Receiver";

    #region 构造函数:TaskWorkReceiverProvider()
    /// <summary>构造函数</summary>
    public TaskWorkReceiverProvider()
    {
      this.configuration = TasksConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    #region 函数:FindOne(string taskId, string receiverId)
    /// <summary>查询某条记录</summary>
    /// <param name="taskId">任务标识</param>
    /// <param name="receiverId">接收人标识</param>
    /// <returns>返回一个 TaskWorkReceiverInfo 实例的详细信息</returns>
    public TaskWorkItemInfo FindOne(string taskId, string receiverId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("TaskId", taskId);
      args.Add("ReceiverId", receiverId);

      return this.ibatisMapper.QueryForObject<TaskWorkItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
    }
    #endregion

    #region 函数:FindOneByTaskCode(string applicationId, string taskCode, string receiverId)
    /// <summary>查询某条记录</summary>
    /// <param name="applicationId">应用系统的标识</param>
    /// <param name="taskCode">任务编码</param>
    /// <param name="receiverId">接收人标识</param>
    /// <returns>返回一个 TaskWorkReceiverInfo 实例的详细信息</returns>
    public TaskWorkItemInfo FindOneByTaskCode(string applicationId, string taskCode, string receiverId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("ApplicationId", applicationId);
      args.Add("TaskCode", taskCode);
      args.Add("ReceiverId", receiverId);

      return this.ibatisMapper.QueryForObject<TaskWorkItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByTaskCode", this.tableName)), args);
    }
    #endregion

    #region 函数:FindAllByReceiverId(string receiverId, string whereClause)
    /// <summary>查询所有相关记录</summary>
    /// <param name="receiverId">接收者帐号标识</param>
    /// <param name="whereClause">SQL 查询条件</param>
    /// <returns>返回所有 TaskWorkItemInfo 实例的详细信息</returns>
    public IList<TaskWorkItemInfo> FindAllByReceiverId(string receiverId, string whereClause)
    {
      return this.FindAllByReceiverId(receiverId, whereClause, 0);
    }
    #endregion

    #region 函数:FindAllByReceiverId(string receiverId, string whereClause, string length)
    /// <summary>查询所有相关记录</summary>
    /// <param name="receiverId">接收者帐号标识</param>
    /// <param name="whereClause">SQL 查询条件</param>  
    /// <param name="length">条数</param>
    /// <returns>返回所有 TaskWorkItemInfo 实例的详细信息</returns>
    public IList<TaskWorkItemInfo> FindAllByReceiverId(string receiverId, string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Length", length);
      args.Add("ReceiverId", StringHelper.ToSafeSQL(receiverId, true));
      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

      return this.ibatisMapper.QueryForList<TaskWorkItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllByReceiverId", this.tableName)), args);
    }
    #endregion

    // -------------------------------------------------------
    // 自定义功能
    // -------------------------------------------------------

    #region 函数:GetPaging(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
    /// <summary>分页函数</summary>
    /// <param name="receiverId">接收人标识</param>
    /// <param name="startIndex">开始行索引数,由0开始统计</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="whereClause">WHERE 查询条件</param>
    /// <param name="orderBy">ORDER BY 排序条件.</param>
    /// <param name="rowCount">记录行数</param>
    /// <returns>返回一个列表</returns> 
    public IList<TaskWorkItemInfo> GetPaging(string receiverId, int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      string whereClause = string.Empty;

      if (query.Variables["scence"] == "Query")
      {
        string type = StringHelper.ToSafeSQL(query.Where["Type"].ToString());

        whereClause += " ReceiverId = '" + receiverId + "' AND Type IN (" + type + ") ";

        if (query.Where.ContainsKey("Status") && !string.IsNullOrEmpty(query.Where["Status"].ToString()))
        {
          whereClause += " AND Status IN (" + StringHelper.ToSafeSQL(query.Where["Status"].ToString()) + ") ";
        }

        if (query.Where.ContainsKey("SearchText") && !string.IsNullOrEmpty(query.Where["SearchText"].ToString()))
        {
          whereClause += " AND Title LIKE '%" + StringHelper.ToSafeSQL(query.Where["SearchText"].ToString()) + "%' ";
        }

        if (query.Where.ContainsKey("DateBegin") && !string.IsNullOrEmpty(query.Where["DateBegin"].ToString()))
        {
          whereClause += " AND CreateDate > '" + StringHelper.ToSafeSQL(query.Where["DateBegin"].ToString()) + "' ";
        }

        if (query.Where.ContainsKey("DateEnd") && !string.IsNullOrEmpty(query.Where["DateEnd"].ToString()))
        {
          whereClause += " AND CreateDate < '" + StringHelper.ToSafeSQL(query.Where["DateEnd"].ToString()) + "' ";
        }

        args.Add("WhereClause", whereClause);
      }
      else
      {
        args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "LIKE", "LIKE" } }));
      }

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);
      args.Add("OrderBy", query.GetOrderBySql(" CreateDate DESC "));

      args.Add("RowCount", 0);

      IList<TaskWorkItemInfo> list = this.ibatisMapper.QueryForList<TaskWorkItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

      return list;
    }
    #endregion

    #region 函数:IsExist(string taskId, string receiverId)
    /// <summary>查询是否存在相关的记录</summary>
    /// <param name="taskId">任务标识</param>
    /// <param name="receiverId">接收者标识</param>
    /// <returns>布尔值</returns>
    public bool IsExist(string taskId, string receiverId)
    {
      if (string.IsNullOrEmpty(taskId) || string.IsNullOrEmpty(receiverId)) { throw new Exception("实例标识不能为空。"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" TaskId = '{0}' AND ReceiverId = '{1}' ", StringHelper.ToSafeSQL(taskId), StringHelper.ToSafeSQL(receiverId)));

      return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;
    }
    #endregion

    #region 函数:Copy(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
    /// <summary>复制待办信息</summary>
    /// <param name="fromReceiverId">待办来源接收者标识</param>
    /// <param name="toReceiverId">待办目标接收者标识</param>
    /// <param name="beginDate">复制待办的开始时间</param>
    /// <param name="endDate">复制待办结束时间</param>
    /// <returns></returns>
    public int Copy(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
    {
      return this.Copy(string.Empty, fromReceiverId, toReceiverId, beginDate, endDate);
    }
    #endregion

    #region 函数:Copy(string applicationId, string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
    /// <summary>复制待办信息</summary>
    /// <param name="applicationId">所属应用标识</param>
    /// <param name="fromReceiverId">待办来源接收者标识</param>
    /// <param name="toReceiverId">待办目标接收者标识</param>
    /// <param name="beginDate">复制待办的开始时间</param>
    /// <param name="endDate">复制待办结束时间</param>
    /// <returns></returns>
    public int Copy(string applicationId, string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
    {
      string whereClause = string.Format(" ApplicationId = ##{0}## AND Status = 0 AND CreateDate BETWEEN ##{0}## AND ##{1}## ", applicationId, beginDate, endDate);

      if (string.IsNullOrEmpty(applicationId))
      {
        whereClause = string.Format(" Status = 0 AND CreateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);
      }

      IList<TaskWorkItemInfo> list = this.FindAllByReceiverId(fromReceiverId, whereClause);

      foreach (TaskWorkItemInfo item in list)
      {
        Dictionary<string, object> args = new Dictionary<string, object>();

        args.Add("TaskId", item.Id);
        args.Add("TaskCode", item.TaskCode);
        args.Add("Type", item.Type);
        args.Add("Title", item.Title);
        args.Add("Tags", item.Tags);
        args.Add("SenderId", item.SenderId);
        args.Add("FromReceiverId", fromReceiverId);
        args.Add("ToReceiverId", toReceiverId);
        args.Add("Status", item.Status);
        args.Add("IsRead", 0);
        args.Add("FinishTime", new DateTime(2000, 1, 1));

        this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Copy", this.tableName)), args);
      }

      return 0;
    }
    #endregion

    #region 函数:Cut(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
    /// <summary>剪切待办信息</summary>
    /// <param name="fromReceiverId">待办来源接收者标识</param>
    /// <param name="toReceiverId">待办目标接收者标识</param>
    /// <param name="beginDate">复制待办的开始时间</param>
    /// <param name="endDate">复制待办结束时间</param>
    /// <returns></returns>
    public int Cut(string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
    {
      return this.Cut(string.Empty, fromReceiverId, toReceiverId, beginDate, endDate);
    }
    #endregion

    #region 函数:Cut(string applicationId, string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
    /// <summary>剪切待办信息</summary>
    /// <param name="applicationId">所属应用标识</param>
    /// <param name="fromReceiverId">待办来源接收者标识</param>
    /// <param name="toReceiverId">待办目标接收者标识</param>
    /// <param name="beginDate">复制待办的开始时间</param>
    /// <param name="endDate">复制待办结束时间</param>
    /// <returns></returns>
    public int Cut(string applicationId, string fromReceiverId, string toReceiverId, DateTime beginDate, DateTime endDate)
    {
      string whereClause = string.Format(" ApplicationId = ##{0}## AND Status = 0 AND CreateDate BETWEEN ##{0}## AND ##{1}## ", applicationId, beginDate, endDate);

      if (string.IsNullOrEmpty(applicationId))
      {
        whereClause = string.Format(" Status = 0 AND CreateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);
      }

      IList<TaskWorkItemInfo> list = this.FindAllByReceiverId(fromReceiverId, whereClause);

      foreach (TaskWorkItemInfo item in list)
      {
        Dictionary<string, object> args = new Dictionary<string, object>();

        args.Add("TaskId", item.Id);
        args.Add("TaskCode", item.TaskCode);
        args.Add("Type", item.Type);
        args.Add("Title", item.Title);
        args.Add("Tags", item.Tags);
        args.Add("SenderId", item.SenderId);
        args.Add("FromReceiverId", fromReceiverId);
        args.Add("ToReceiverId", toReceiverId);
        args.Add("Status", item.Status);
        args.Add("IsRead", 0);
        args.Add("FinishTime", new DateTime(2000, 1, 1));

        this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Cut", this.tableName)), args);
      }

      return 0;
    }
    #endregion

    #region 函数:SetRead(string taskId, string receiverId, bool isRead)
    /// <summary>设置任务阅读状态</summary>
    /// <param name="taskId">任务标识</param>
    /// <param name="receiverId">接收者的用户名</param>
    /// <param name="isRead">状态: true 已读 | false 未读 </param>
    public void SetRead(string taskId, string receiverId, bool isRead)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", taskId);
      args.Add("ReceiverId", receiverId);
      args.Add("IsRead", (isRead ? 1 : 0));

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetRead", this.tableName)), args);
    }
    #endregion

    #region 函数:SetStatus(string taskId, string receiverId, int status)
    /// <summary>强制设置任务状态</summary>
    /// <param name="taskId">任务标识</param>
    /// <param name="receiverId">接收者的用户名</param>
    /// <param name="status">状态</param>
    public void SetStatus(string taskId, string receiverId, int status)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", taskId);
      args.Add("ReceiverId", receiverId);
      args.Add("Status", status);

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetStatus", this.tableName)), args);
    }
    #endregion

    #region 函数:SetFinished(string receiverId)
    /// <summary>设置任务完成</summary>
    /// <param name="receiverId">接收者</param>
    public void SetFinished(string receiverId)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      // 因为修改状态的同时需要设置待办完成时间, 所以只修改状态等于零的待办信息
      args.Add("WhereClause", string.Format(" Status = 0 AND ReceiverId = '{0}' ", receiverId));

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetFinished", this.tableName)), args);
    }
    #endregion

    #region 函数:SetFinished(string receiverId, string taskIds)
    /// <summary>设置任务完成</summary>
    /// <param name="receiverId">接收者</param>
    /// <param name="taskIds">任务编号,多个以逗号分开</param>
    public void SetFinished(string receiverId, string taskIds)
    {
      if (string.IsNullOrEmpty(taskIds)) { return; }

      Dictionary<string, object> args = new Dictionary<string, object>();

      taskIds = "'" + StringHelper.ToSafeSQL(taskIds).Replace(",", "','") + "'";

      // 因为修改状态的同时需要设置待办完成时间, 所以只修改状态等于零的待办信息
      args.Add("WhereClause", string.Format(" Status = 0 AND ReceiverId = '{0}' AND Id IN ({1}) ", receiverId, taskIds));

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetFinished", this.tableName)), args);
    }
    #endregion

    #region 函数:SetUnfinished(string receiverId, string taskIds)
    /// <summary>设置任务未完成</summary>
    /// <param name="receiverId">接收者</param>
    /// <param name="taskIds">任务编号,多个以逗号分开</param>
    public void SetUnfinished(string receiverId, string taskIds)
    {
      if (string.IsNullOrEmpty(taskIds)) { return; }

      Dictionary<string, object> args = new Dictionary<string, object>();

      taskIds = "'" + StringHelper.ToSafeSQL(taskIds).Replace(",", "','") + "'";

      args.Add("WhereClause", string.Format(" ReceiverId = '{0}' AND Id IN ({1}) ", receiverId, taskIds));

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetUnfinished", this.tableName)), args);

      // 刷新任务的创建时间
      args["WhereClause"] = string.Format(" Id IN ({1}) ", receiverId, taskIds);

      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetUnfinished_RefreshCreateDate", this.tableName)), args);
    }
    #endregion

    #region 函数:SetFinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
    /// <summary>设置任务完成</summary>
    /// <param name="applicationId">应用系统的标识</param>
    /// <param name="taskCodes">任务编号,多个以逗号分开</param>
    /// <param name="receiverId">接收者</param>
    public void SetFinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
    {
      if (string.IsNullOrEmpty(taskCodes)) { return; }

      string taskIds = TasksContext.Instance.TaskWorkService.GetIdsByTaskCodes(applicationId, taskCodes);

      this.SetFinished(receiverId, taskIds);
    }
    #endregion

    #region 函数:SetUnfinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
    /// <summary>设置任务未完成</summary>
    /// <param name="applicationId">应用系统的标识</param>
    /// <param name="taskCodes">任务编号,多个以逗号分开</param>
    /// <param name="receiverId">接收者</param>
    public void SetUnfinishedByTaskCode(string applicationId, string taskCodes, string receiverId)
    {
      if (string.IsNullOrEmpty(taskCodes)) { return; }

      string taskIds = TasksContext.Instance.TaskWorkService.GetIdsByTaskCodes(applicationId, taskCodes);

      this.SetUnfinished(receiverId, taskIds);
    }
    #endregion

    #region 函数:GetUnfinishedQuantities(string receiverId)
    /// <summary>获取未完成任务的数量</summary>
    /// <param name="receiverId">接收人标识</param>
    /// <returns>返回一个包含每个类型的统计数的 DataTable </returns>
    public Dictionary<int, int> GetUnfinishedQuantities(string receiverId)
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("ReceiverId", StringHelper.ToSafeSQL(receiverId, true));

      DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetUnfinishedQuantities", this.tableName)), args);

      // 任务的类型
      int[] types = new int[] { 1, 2, 4, 8, 258, 260, 264, 266 };

      for (int i = 0; i < types.Length; i++)
      {
        dictionary.Add(types[i], 0);
      }

      for (int i = 0; i < table.Rows.Count; i++)
      {
        if (dictionary.ContainsKey(Convert.ToInt32(table.Rows[i][0])))
        {
          dictionary[Convert.ToInt32(table.Rows[i][0])] = Convert.ToInt32(table.Rows[i][1]);
        }
      }

      // 待办数量 = (1)审批数量 + (8)催办数量 + (266)自动催办数量
      // 通知数量 = (2)消息数量 + (4)通知数量 + (258)自动消息数量 + (260)自动通知数量

      dictionary.Add(275, dictionary[1] + dictionary[8] + dictionary[266]);
      dictionary.Add(524, dictionary[2] + dictionary[4] + dictionary[258] + dictionary[260]);

      return dictionary;
    }
    #endregion
  }
}
