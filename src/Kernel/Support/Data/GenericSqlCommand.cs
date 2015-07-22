namespace X3Platform.Data
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Data;
  using System.Data.Common;
  using System.Collections;
  using System.Configuration;
  using System.Reflection;
  #endregion

  /// <summary>通用的SQL命令</summary>
  public class GenericSqlCommand
  {
    /// <summary>数据提供程序工厂</summary>
    private DbProviderFactory providerFactory;

    /// <summary>数据库链接字符串</summary>
    private string connectionString;

    /// <summary>数据库的链接</summary>
    private DbConnection connection;

    /// <summary>数据库的命令</summary>
    public DbCommand command;

    /// <summary>数据库的命令参数</summary>
    private DbParameter parameter;

    /// <summary>数据库事务</summary>
    private DbTransaction transaction;

    /// <summary>是否启用数据库事务</summary>
    private bool enableTransaction;

    #region 构造函数:GenericSqlCommand(string datasourceName)
    /// <summary>构造函数</summary>
    /// <param name="datasourceName">数据源名称</param>
    public GenericSqlCommand(string datasourceName)
    {
      ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[datasourceName];

      if (settings == null)
      {
        throw new NullReferenceException("检查系统配置文件是否已配置【" + datasourceName + "】的数据库连接。");
      }

      this.connectionString = settings.ConnectionString;

      this.providerFactory = DbProviderFactories.GetFactory(settings.ProviderName);
    }
    #endregion

    #region 构造函数:GenericSqlCommand(string connectionString, string providerName)
    /// <summary>构造函数</summary>
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="providerName">数据提供器名称</param>
    public GenericSqlCommand(string connectionString, string providerName)
    {
      this.connectionString = connectionString;

      this.providerFactory = DbProviderFactories.GetFactory(GetProviderName(providerName));
    }
    #endregion

    #region 私有函数:GetProviderName(string providerName)
    /// <summary>获取规则的数据提供器名称</summary>
    /// <param name="providerName"></param>
    /// <returns></returns>
    private string GetProviderName(string providerName)
    {
      switch (providerName.ToUpper())
      {
        case "SQLSERVER":
        case "SQLSERVER1.0":
        case "SQLSERVER1.1":
        case "SQLSERVER2.0":
        case "SQLSERVER2005":
        case "System.Data.SqlClient":
          return "System.Data.SqlClient";
        case "ORACLE":
        case "ORACLE9.2":
        case "ORACLE10.1":
          return "Oracle.DataAccess.Client";
        case "MYSQL":
          return "MySql.Data.MySqlClient";
        case "ORACLECLIENT":
        case "ORACLECLIENT1.0":
          return "System.Data.OracleClient";
        default:
          return providerName;
      }
    }
    #endregion

    #region 析构函数:~GenericSqlCommand()
    /// <summary></summary>
    ~GenericSqlCommand()
    {
      this.providerFactory = null;
    }
    #endregion

    #region 私有函数:CreateConnection()
    /// <summary>创建连接</summary>
    private DbConnection CreateConnection()
    {
      this.connection = this.providerFactory.CreateConnection();

      this.connection.ConnectionString = this.connectionString;

      return this.connection;
    }
    #endregion

    #region 函数:OpenConnection()
    /// <summary>打开连接</summary>
    public void OpenConnection()
    {
      if (this.connection == null)
      {
        this.connection = this.CreateConnection();
      }

      if (this.connection.State != ConnectionState.Open)
      {
        this.connection.ConnectionString = this.connectionString;

        try
        {
          this.connection.Open();
        }
        catch
        {
          throw new GenericSqlConnectionException("连接数据库失败，请检查数据库配置信息.");
        }
      }
    }
    #endregion

    #region 函数:CloseConnection()
    /// <summary>关闭连接</summary>
    public void CloseConnection()
    {
      if (this.connection == null) { return; }

      try
      {
        if (this.connection.State != ConnectionState.Closed)
        {
          this.connection.Close();
        }
      }
      catch (DbException dbException)
      {
        //catch any SQL server data provider generated error messag
        throw new Exception(dbException.Message);
      }
      catch (NullReferenceException nullReferenceException)
      {
        throw new Exception(nullReferenceException.Message);
      }
      finally
      {
        if (this.connection != null)
        {
          // this.connection.Dispose();
        }
      }
    }
    #endregion

    #region 私有函数:PrepareCommand(bool enableTransaction, CommandType commandType, string commandText)
    /// <summary>命令执行前的准备操作</summary>
    private void PrepareCommand(bool enableTransaction, CommandType commandType, string commandText)
    {
      if (this.connection == null)
      {
        this.connection = this.CreateConnection();
      }

      if (this.command == null)
      {
        this.command = this.providerFactory.CreateCommand();
      }

      this.command.Connection = this.connection;
      this.command.CommandTimeout = this.connection.ConnectionTimeout;

      this.command.CommandText = commandText;
      this.command.CommandType = commandType;

      if (enableTransaction)
      {
        this.command.Transaction = this.transaction;
      }
    }
    #endregion

    #region 私有函数:PrepareCommand(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>命令执行前的准备操作</summary>
    private void PrepareCommand(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
    {
      this.PrepareCommand(enableTransaction, commandType, commandText);

      if (commandParameters != null)
      {
        this.CreateCommandParameters(commandParameters);
      }
    }
    #endregion

    #region 私有函数:PrepareCommand(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>命令执行前的准备操作</summary>
    private void PrepareCommand(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    {
      this.PrepareCommand(enableTransaction, commandType, commandText);

      if (commandParameters != null)
      {
        this.CreateCommandParameters(commandParameters);
      }
    }
    #endregion

    #region 私有函数:CreateCommandParameters(object[,] parameters)
    /// <summary>创建命令参数</summary>
    private void CreateCommandParameters(object[,] parameters)
    {
      for (int i = 0; i < parameters.Length / 2; i++)
      {
        this.parameter = this.command.CreateParameter();
        this.parameter.ParameterName = parameters[i, 0].ToString();
        this.parameter.Value = parameters[i, 1];
        this.command.Parameters.Add(this.parameter);
      }
    }
    #endregion

    #region 私有函数:CreateCommandParameters(GenericSqlCommandParameter[] parameters)
    /// <summary>创建参数</summary>
    private void CreateCommandParameters(GenericSqlCommandParameter[] parameters)
    {
      for (int i = 0; i < parameters.Length; i++)
      {
        GenericSqlCommandParameter parameter = (GenericSqlCommandParameter)parameters[i];

        this.parameter = this.command.CreateParameter();
        this.parameter.ParameterName = parameter.Name;
        this.parameter.Value = parameter.Value;
        this.parameter.Direction = parameter.Direction;
        this.command.Parameters.Add(this.parameter);
      }
    }
    #endregion

    // -------------------------------------------------------
    // 事务支持
    // -------------------------------------------------------

    #region 函数:BeginTransaction()
    /// <summary>启动事务</summary>
    public void BeginTransaction()
    {
      this.BeginTransaction(IsolationLevel.ReadCommitted);
    }
    #endregion

    #region 函数:BeginTransaction(IsolationLevel isolationLevel)
    /// <summary>启动事务</summary>
    /// <param name="isolationLevel">事务隔离级别</param>
    public void BeginTransaction(IsolationLevel isolationLevel)
    {
      this.transaction = this.connection.BeginTransaction(isolationLevel);

      this.enableTransaction = true;
    }
    #endregion

    #region 函数:CommitTransaction()
    /// <summary>提交事务</summary>
    public void CommitTransaction()
    {
      if (this.transaction.Connection != null)
      {
        this.transaction.Commit();

        this.enableTransaction = false;
      }
    }
    #endregion

    #region 函数:RollBackTransaction()
    /// <summary>回滚事务</summary>
    public void RollBackTransaction()
    {
      if (this.enableTransaction)
      {
        this.transaction.Rollback();
      }

      this.enableTransaction = false;
    }
    #endregion

    // -------------------------------------------------------
    // ExecuteNonQuery
    // 执行命令，并返回受影响的行数。
    // -------------------------------------------------------

    #region 函数:ExecuteNonQuery(string commandText)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(string commandText)
    {
      return this.ExecuteNonQuery(CommandType.Text, commandText);
    }
    #endregion

    #region 函数:ExecuteNonQuery(CommandType commandType, string commandText)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(CommandType commandType, string commandText)
    {
      this.PrepareCommand(false, commandType, commandText);

      return this.ExecuteNonQuery(false, true);
    }
    #endregion

    #region 函数:ExecuteNonQuery(CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(CommandType commandType, string commandText, object[,] commandParameters)
    {
      return this.ExecuteNonQuery(commandType, commandText, commandParameters, true);
    }
    #endregion

    #region 函数:ExecuteNonQuery(CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteNonQuery(false, disposeCommand);
    }
    #endregion

    #region 函数:ExecuteNonQuery(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    {
      return this.ExecuteNonQuery(commandType, commandText, commandParameters, true);
    }
    #endregion

    #region 函数:ExecuteNonQuery(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteNonQuery(false, disposeCommand);
    }
    #endregion

    #region 函数:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText)
    {
      this.PrepareCommand(enableTransaction, commandType, commandText);

      return this.ExecuteNonQuery(enableTransaction, true);
    }
    #endregion

    #region 函数:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
    {
      return this.ExecuteNonQuery(enableTransaction, commandType, commandText, commandParameters, true);
    }
    #endregion

    #region 函数:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
    {
      this.PrepareCommand(enableTransaction, commandType, commandText, commandParameters);

      return this.ExecuteNonQuery(enableTransaction, disposeCommand);
    }
    #endregion

    #region 函数:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    {
      return this.ExecuteNonQuery(enableTransaction, commandType, commandText, commandParameters, true);
    }
    #endregion

    #region 函数:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
    {
      this.PrepareCommand(enableTransaction, commandType, commandText, commandParameters);

      return this.ExecuteNonQuery(enableTransaction, disposeCommand);
    }
    #endregion

    #region 私有函数:ExecuteNonQuery(bool enableTransaction, bool disposeCommand)
    /// <summary>执行命令，并返回受影响的行数。</summary>
    private int ExecuteNonQuery(bool enableTransaction, bool disposeCommand)
    {
      try
      {
        this.OpenConnection();

        return this.command.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        if (disposeCommand && this.command != null)
        {
          this.command.Dispose();
        }

        if (!enableTransaction)
        {
          this.CloseConnection();
        }
      }
    }
    #endregion

    // -------------------------------------------------------
    // ExecuteScalar 
    // 执行查询命令，并返回查询所返回的结果集的第一行的第一列。
    // -------------------------------------------------------

    #region 函数:ExecuteScalar(string commandText)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(string commandText)
    {
      return this.ExecuteScalar(CommandType.Text, commandText);
    }
    #endregion

    #region 函数:ExecuteScalar(CommandType commandType, string commandText)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(CommandType commandType, string commandText)
    {
      this.PrepareCommand(false, commandType, commandText);

      return this.ExecuteScalar(false, true);
    }
    #endregion

    #region 函数:ExecuteScalar(CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(CommandType commandType, string commandText, object[,] commandParameters)
    {
      return this.ExecuteScalar(commandType, commandText, commandParameters, true);
    }
    #endregion

    #region 函数:ExecuteScalar(CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
    {
      return this.ExecuteScalar(false, commandType, commandText, commandParameters, disposeCommand);
    }
    #endregion

    #region 函数:ExecuteScalar(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    {
      return this.ExecuteScalar(commandType, commandText, commandParameters, true);
    }
    #endregion

    #region 函数:ExecuteScalar(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
    {
      return this.ExecuteScalar(false, commandType, commandText, commandParameters, disposeCommand);
    }
    #endregion

    #region 函数:ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
    {
      return this.ExecuteScalar(enableTransaction, commandType, commandText, commandParameters, true);
    }
    #endregion

    #region 函数:ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
    {
      this.PrepareCommand(enableTransaction, commandType, commandText, commandParameters);

      return this.ExecuteScalar(enableTransaction, disposeCommand);
    }
    #endregion

    #region 函数:ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    {
      return this.ExecuteScalar(enableTransaction, commandType, commandText, commandParameters, true);
    }
    #endregion

    #region 函数:ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    public object ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
    {
      this.PrepareCommand(enableTransaction, commandType, commandText, commandParameters);

      return this.ExecuteScalar(enableTransaction, disposeCommand);
    }
    #endregion

    #region 私有函数:ExecuteScalar(bool enableTransaction, bool disposeCommand)
    /// <summary>执行查询命令，并返回查询所返回的结果集的第一行的第一列。</summary>
    private object ExecuteScalar(bool enableTransaction, bool disposeCommand)
    {
      try
      {
        this.OpenConnection();

        return this.command.ExecuteScalar();
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        if (disposeCommand && this.command != null)
        {
          this.command.Dispose();
        }

        if (!enableTransaction)
        {
          this.CloseConnection();
        }
      }
    }
    #endregion

    // -------------------------------------------------------
    // ExecuteQueryForDataTable
    // 执行查询命令，并返回查询所返回的 DataTable 结果集。
    // -------------------------------------------------------

    #region 函数:ExecuteQueryForDataTable(string commandText)
    /// <summary>执行查询命令，并返回查询所返回的 ExecuteQueryForDataTable 结果集。</summary>
    public DataTable ExecuteQueryForDataTable(string commandText)
    {
      return this.ExecuteQueryForDataTable(CommandType.Text, commandText);
    }
    #endregion

    #region 函数:ExecuteQueryForDataSet(CommandType commandType, string commandText)
    /// <summary>执行查询命令，并返回查询所返回的 ExecuteQueryForDataTable 结果集。</summary>
    public DataTable ExecuteQueryForDataTable(CommandType commandType, string commandText)
    {
      this.PrepareCommand(false, commandType, commandText);

      return this.ExecuteQueryForDataTable();
    }
    #endregion

    #region 函数:ExecuteQueryForDataTable(CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public DataTable ExecuteQueryForDataTable(CommandType commandType, string commandText, object[,] commandParameters)
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteQueryForDataTable();
    }
    #endregion

    #region 函数:ExecuteQueryForDataTable(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public DataTable ExecuteQueryForDataTable(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteQueryForDataTable();
    }
    #endregion

    #region 私有函数:ExecuteQueryForDataTable()
    /// <summary>执行查询命令，并返回查询所返回的 DataReader 实例。</summary>
    private DataTable ExecuteQueryForDataTable()
    {
      try
      {
        this.OpenConnection();

        DataTable table = new DataTable();

        DbDataReader reader = this.command.ExecuteReader();

        if (reader != null)
        {
          table.BeginLoadData();

          // 创建表头
          for (int i = 0; i < reader.FieldCount; ++i)
          {
            table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
          }

          object[] values = new object[reader.FieldCount];

          while (reader.Read())
          {
            reader.GetValues(values);

            table.LoadDataRow(values, true);
          }

          reader.Close();

          table.EndLoadData();
        }

        return table;
      }
      catch (Exception ex)
      {
        this.CloseConnection();
        throw ex;
      }
      finally
      {
        if (this.command != null)
        {
          this.command.Dispose();
        }

        this.CloseConnection();
      }
    }
    #endregion

    // -------------------------------------------------------
    // ExecuteQueryForDataSet 
    // 执行查询命令，并返回查询所返回的 DataSet 结果集。
    // -------------------------------------------------------

    #region 函数:ExecuteQueryForDataSet(string commandText)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public DataSet ExecuteQueryForDataSet(string commandText)
    {
      return this.ExecuteQueryForDataSet(CommandType.Text, commandText);
    }
    #endregion

    #region 函数:ExecuteQueryForDataSet(CommandType commandType, string commandText)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public DataSet ExecuteQueryForDataSet(CommandType commandType, string commandText)
    {
      this.PrepareCommand(false, commandType, commandText);

      return this.ExecuteQueryForDataSet();
    }
    #endregion

    #region 函数:ExecuteQueryForDataSet(CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public DataSet ExecuteQueryForDataSet(CommandType commandType, string commandText, object[,] commandParameters)
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteQueryForDataSet();
    }
    #endregion

    #region 函数:ExecuteQueryForDataSet(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public DataSet ExecuteQueryForDataSet(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteQueryForDataSet();
    }
    #endregion

    #region 私有函数:ExecuteQueryForDataSet()
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    private DataSet ExecuteQueryForDataSet()
    {
      try
      {
        this.OpenConnection();

        DbDataAdapter adapter = this.providerFactory.CreateDataAdapter();

        adapter.SelectCommand = this.command;

        DataSet dataSet = new DataSet();

        adapter.Fill(dataSet);

        return dataSet;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        if (this.command != null)
        {
          this.command.Dispose();
        }

        this.CloseConnection();
      }
    }
    #endregion

    // -------------------------------------------------------
    // ExecuteQueryForList<T>
    // 执行查询命令，并返回查询所返回的结果集的第一行的第一列。
    // -------------------------------------------------------

    #region 函数:ExecuteQueryForList<T>(string commandText)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public IList<T> ExecuteQueryForList<T>(string commandText)
       where T : new()
    {
      return this.ExecuteQueryForList<T>(CommandType.Text, commandText);
    }
    #endregion

    #region 函数:ExecuteQueryForList<T>(CommandType commandType, string commandText)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public IList<T> ExecuteQueryForList<T>(CommandType commandType, string commandText)
       where T : new()
    {
      this.PrepareCommand(false, commandType, commandText);

      return this.ExecuteQueryForList<T>();
    }
    #endregion

    #region 函数:ExecuteQueryForList(CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public IList<T> ExecuteQueryForList<T>(CommandType commandType, string commandText, object[,] commandParameters)
       where T : new()
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteQueryForList<T>();
    }
    #endregion

    #region 函数:ExecuteQueryForList(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public IList<T> ExecuteQueryForList<T>(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
       where T : new()
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteQueryForList<T>();
    }
    #endregion

    #region 私有函数:ExecuteQueryForList()
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    private IList<T> ExecuteQueryForList<T>()
        where T : new()
    {
      try
      {
        List<T> list = new List<T>();

        this.OpenConnection();

        using (DbDataReader reader = this.command.ExecuteReader())
        {
          while (reader.Read())
          {
            list.Add(CreateObjectByDataReader<T>(reader));
          }

          reader.Close();
        }

        return list;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        if (this.command != null)
        {
          this.command.Dispose();
          this.command = null;
        }

        this.CloseConnection();
      }
    }
    #endregion

    // -------------------------------------------------------
    // ExecuteQueryForObject<T>
    // 执行查询命令，并返回查询所返回的结果集的第一行的第一列。
    // -------------------------------------------------------

    #region 函数:ExecuteQueryForObject(string commandText)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public T ExecuteQueryForObject<T>(string commandText)
       where T : new()
    {
      return this.ExecuteQueryForObject<T>(CommandType.Text, commandText);
    }
    #endregion

    #region 函数:ExecuteQueryForObject(CommandType commandType, string commandText)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public T ExecuteQueryForObject<T>(CommandType commandType, string commandText)
       where T : new()
    {
      this.PrepareCommand(false, commandType, commandText);

      return this.ExecuteQueryForObject<T>();
    }
    #endregion

    #region 函数:ExecuteQueryForObject(CommandType commandType, string commandText, object[,] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public T ExecuteQueryForObject<T>(CommandType commandType, string commandText, object[,] commandParameters)
       where T : new()
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteQueryForObject<T>();
    }
    #endregion

    #region 函数:ExecuteQueryForObject(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    public T ExecuteQueryForObject<T>(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
       where T : new()
    {
      this.PrepareCommand(false, commandType, commandText, commandParameters);

      return this.ExecuteQueryForObject<T>();
    }
    #endregion

    #region 私有函数:ExecuteQueryForObject()
    /// <summary>执行查询命令，并返回查询所返回的 DataSet 结果集。</summary>
    private T ExecuteQueryForObject<T>()
        where T : new()
    {
      try
      {
        this.OpenConnection();

        using (DbDataReader reader = this.command.ExecuteReader())
        {
          if (reader.Read())
          {
            return CreateObjectByDataReader<T>(reader);
          }
        }

        return default(T);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        if (this.command != null)
        {
          this.command.Dispose();
          this.command = null;
        }

        this.CloseConnection();
      }
    }
    #endregion

    private T CreateObjectByDataReader<T>(DbDataReader reader)
        where T : new()
    {
      T targetObject = new T();

      Type type = targetObject.GetType();

      MethodInfo[] methods = type.GetMethods();

      string name;

      foreach (MethodInfo method in methods)
      {
        if (method.Name.Contains("set_"))
        {
          name = method.Name.Substring(4, method.Name.Length - 4);

          if (!string.IsNullOrEmpty(name))
          {
            int ordinal = GetDataReaderColumnOrdinal(reader, name);

            // 过滤数据列不存在和列内容为空信息
            if (ordinal == -1 || reader.GetValue(ordinal) == DBNull.Value) { continue; }

            switch (reader.GetFieldType(ordinal).FullName)
            {
              case "System.Int16":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetInt16(ordinal) });
                break;
              case "System.Int32":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetInt32(ordinal) });
                break;
              case "System.Int64":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetInt64(ordinal) });
                break;
              case "System.Double":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetDouble(ordinal) });
                break;
              case "System.Decimal":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetDecimal(ordinal) });
                break;
              case "System.Boolean":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetBoolean(ordinal) });
                break;
              case "System.Guid":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetGuid(ordinal) });
                break;
              case "System.DateTime":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetDateTime(ordinal) });
                break;
              case "System.String":
                type.InvokeMember(method.Name, BindingFlags.InvokeMethod, null, targetObject, new object[] { reader.GetString(ordinal) });
                break;
              default:
                break;
            }
          }
        }
      }

      return targetObject;
    }

    /// <summary>根据数据列名称获取数据列序号, IDataReader 内置的GetOrdinal方法查询不存在的列时会直接抛异常</summary>
    /// <param name="reader"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private int GetDataReaderColumnOrdinal(DbDataReader reader, string name)
    {
      // 根据数据列名称获取数据列序号
      int ordinal = -1;

      for (int i = 0; i < reader.FieldCount; i++)
      {
        // 判断忽略大小写
        if (reader.GetName(i).ToLower() == name.ToLower())
        {
          ordinal = i;
          break;
        }
      }

      return ordinal;
    }

    /// <summary>填充占位符的内容</summary>
    /// <param name="commandText">SQL命令</param>
    /// <param name="placeholder">占位符名称</param>
    /// <param name="fillText">填充的文本</param>
    /// <returns></returns>
    public string FillPlaceholder(string commandText, string placeholder, string fillText)
    {
      return FillPlaceholder(commandText, placeholder, fillText, fillText);
    }

    /// <summary>填充占位符的内容</summary>
    /// <param name="commandText">SQL命令</param>
    /// <param name="placeholder">占位符名称</param>
    /// <param name="placeholderValue">占位符实际填充的值</param>
    /// <param name="fillText">填充的文本</param>
    /// <returns></returns>
    public string FillPlaceholder(string commandText, string placeholder, int placeholderValue, string fillText)
    {
      return FillPlaceholder(commandText, placeholder, (placeholderValue == 0 ? string.Empty : placeholderValue.ToString()), fillText);
    }

    /// <summary>填充占位符的内容</summary>
    /// <param name="commandText">SQL命令</param>
    /// <param name="placeholder">占位符名称</param>
    /// <param name="placeholderValue">占位符实际填充的值</param>
    /// <param name="fillText">填充的文本</param>
    /// <returns></returns>
    public string FillPlaceholder(string commandText, string placeholder, string placeholderValue, string fillText)
    {
      return commandText = commandText.Replace(placeholder, (string.IsNullOrEmpty(placeholderValue) ? string.Empty : fillText));
    }
  }
}