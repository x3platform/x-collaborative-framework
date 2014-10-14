#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

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

    /// <summary>ͨ�õ�SQL����</summary>
    public class GenericSqlCommand
    {
        /// <summary>�����ṩ���򹤳�</summary>
        private DbProviderFactory providerFactory;

        /// <summary>���ݿ������ַ���</summary>
        private string connectionString;

        /// <summary>���ݿ�������</summary>
        private DbConnection connection;

        /// <summary>���ݿ�������</summary>
        public DbCommand command;

        /// <summary>���ݿ�����������</summary>
        private DbParameter parameter;

        /// <summary>���ݿ�����</summary>
        private DbTransaction transaction;

        /// <summary>�Ƿ��������ݿ�����</summary>
        private bool enableTransaction;

        #region ���캯��:GenericSqlCommand(string datasourceName)
        /// <summary>���캯��</summary>
        /// <param name="datasourceName">����Դ����</param>
        public GenericSqlCommand(string datasourceName)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[datasourceName];

            if (settings == null)
            {
                throw new NullReferenceException("����ϵͳ�����ļ��Ƿ������á�" + datasourceName + "�������ݿ����ӡ�");
            }

            this.connectionString = settings.ConnectionString;

            this.providerFactory = DbProviderFactories.GetFactory(settings.ProviderName);
        }
        #endregion

        #region ���캯��:GenericSqlCommand(string connectionString, string providerName)
        /// <summary>���캯��</summary>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <param name="providerName">�����ṩ������</param>
        public GenericSqlCommand(string connectionString, string providerName)
        {
            this.connectionString = connectionString;

            this.providerFactory = DbProviderFactories.GetFactory(GetProviderName(providerName));
        }
        #endregion

        #region ˽�к���:GetProviderName(string providerName)
        /// <summary>��ȡ�����������ṩ������</summary>
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
                case "ORACLECLIENT1.0":
                    return "System.Data.OracleClient";
                default:
                    return providerName;
            }
        }
        #endregion

        #region �򹹺���:~GenericSqlCommand()
        /// <summary></summary>
        ~GenericSqlCommand()
        {
            this.providerFactory = null;
        }
        #endregion

        #region ˽�к���:CreateConnection()
        /// <summary>��������</summary>
        private DbConnection CreateConnection()
        {
            this.connection = this.providerFactory.CreateConnection();

            this.connection.ConnectionString = this.connectionString;

            return this.connection;
        }
        #endregion

        #region ����:OpenConnection()
        /// <summary>��������</summary>
        public void OpenConnection()
        {
            if (this.connection == null)
            {
                this.connection = this.CreateConnection();
            }

            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.ConnectionString = this.connectionString;

                this.connection.Open();
            }
        }
        #endregion

        #region ����:CloseConnection()
        /// <summary>�ر�����</summary>
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
                    this.connection.Dispose();
                }
            }
        }
        #endregion

        #region ˽�к���:PrepareCommand(bool enableTransaction, CommandType commandType, string commandText)
        /// <summary>����ִ��ǰ��׼������</summary>
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
            this.command.CommandText = commandText;
            this.command.CommandType = commandType;

            if (enableTransaction)
            {
                this.command.Transaction = this.transaction;
            }
        }
        #endregion

        #region ˽�к���:PrepareCommand(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>����ִ��ǰ��׼������</summary>
        private void PrepareCommand(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
        {
            this.PrepareCommand(enableTransaction, commandType, commandText);

            if (commandParameters != null)
            {
                this.CreateCommandParameters(commandParameters);
            }
        }
        #endregion

        #region ˽�к���:PrepareCommand(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>����ִ��ǰ��׼������</summary>
        private void PrepareCommand(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        {
            this.PrepareCommand(enableTransaction, commandType, commandText);

            if (commandParameters != null)
            {
                this.CreateCommandParameters(commandParameters);
            }
        }
        #endregion

        #region ˽�к���:CreateCommandParameters(object[,] parameters)
        /// <summary>������������</summary>
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

        #region ˽�к���:CreateCommandParameters(GenericSqlCommandParameter[] parameters)
        /// <summary>��������</summary>
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
        // ����֧��
        // -------------------------------------------------------

        #region ����:BeginTransaction()
        /// <summary>��������</summary>
        public void BeginTransaction()
        {
            this.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        #endregion

        #region ����:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.transaction = this.connection.BeginTransaction(isolationLevel);

            this.enableTransaction = true;
        }
        #endregion

        #region ����:CommitTransaction()
        /// <summary>�ύ����</summary>
        public void CommitTransaction()
        {
            if (this.transaction.Connection != null)
            {
                this.transaction.Commit();

                this.enableTransaction = false;
            }
        }
        #endregion

        #region ����:RollBackTransaction()
        /// <summary>�ع�����</summary>
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
        // ִ�������������Ӱ����������
        // -------------------------------------------------------

        #region ����:ExecuteNonQuery(string commandText)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(string commandText)
        {
            return this.ExecuteNonQuery(CommandType.Text, commandText);
        }
        #endregion

        #region ����:ExecuteNonQuery(CommandType commandType, string commandText)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            this.PrepareCommand(false, commandType, commandText);

            return this.ExecuteNonQuery(false, true);
        }
        #endregion

        #region ����:ExecuteNonQuery(CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(CommandType commandType, string commandText, object[,] commandParameters)
        {
            return this.ExecuteNonQuery(commandType, commandText, commandParameters, true);
        }
        #endregion

        #region ����:ExecuteNonQuery(CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteNonQuery(false, disposeCommand);
        }
        #endregion

        #region ����:ExecuteNonQuery(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandType, commandText, commandParameters, true);
        }
        #endregion

        #region ����:ExecuteNonQuery(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteNonQuery(false, disposeCommand);
        }
        #endregion

        #region ����:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText)
        {
            this.PrepareCommand(enableTransaction, commandType, commandText);

            return this.ExecuteNonQuery(enableTransaction, true);
        }
        #endregion

        #region ����:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
        {
            return this.ExecuteNonQuery(enableTransaction, commandType, commandText, commandParameters, true);
        }
        #endregion

        #region ����:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
        {
            this.PrepareCommand(enableTransaction, commandType, commandText, commandParameters);

            return this.ExecuteNonQuery(enableTransaction, disposeCommand);
        }
        #endregion

        #region ����:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(enableTransaction, commandType, commandText, commandParameters, true);
        }
        #endregion

        #region ����:ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�������������Ӱ����������</summary>
        public int ExecuteNonQuery(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
        {
            this.PrepareCommand(enableTransaction, commandType, commandText, commandParameters);

            return this.ExecuteNonQuery(enableTransaction, disposeCommand);
        }
        #endregion

        #region ˽�к���:ExecuteNonQuery(bool enableTransaction, bool disposeCommand)
        /// <summary>ִ�������������Ӱ����������</summary>
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
        // ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�
        // -------------------------------------------------------

        #region ����:ExecuteScalar(string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(CommandType.Text, commandText);
        }
        #endregion

        #region ����:ExecuteScalar(CommandType commandType, string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            this.PrepareCommand(false, commandType, commandText);

            return this.ExecuteScalar(false, true);
        }
        #endregion

        #region ����:ExecuteScalar(CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(CommandType commandType, string commandText, object[,] commandParameters)
        {
            return this.ExecuteScalar(commandType, commandText, commandParameters, true);
        }
        #endregion

        #region ����:ExecuteScalar(CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
        {
            return this.ExecuteScalar(false, commandType, commandText, commandParameters, disposeCommand);
        }
        #endregion

        #region ����:ExecuteScalar(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandType, commandText, commandParameters, true);
        }
        #endregion

        #region ����:ExecuteScalar(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
        {
            return this.ExecuteScalar(false, commandType, commandText, commandParameters, disposeCommand);
        }
        #endregion

        #region ����:ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters)
        {
            return this.ExecuteScalar(enableTransaction, commandType, commandText, commandParameters, true);
        }
        #endregion

        #region ����:ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, object[,] commandParameters, bool disposeCommand)
        {
            this.PrepareCommand(enableTransaction, commandType, commandText, commandParameters);

            return this.ExecuteScalar(enableTransaction, disposeCommand);
        }
        #endregion

        #region ����:ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        {
            return this.ExecuteScalar(enableTransaction, commandType, commandText, commandParameters, true);
        }
        #endregion

        #region ����:ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
        public object ExecuteScalar(bool enableTransaction, CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters, bool disposeCommand)
        {
            this.PrepareCommand(enableTransaction, commandType, commandText, commandParameters);

            return this.ExecuteScalar(enableTransaction, disposeCommand);
        }
        #endregion

        #region ˽�к���:ExecuteScalar(bool enableTransaction, bool disposeCommand)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�</summary>
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
        // ִ�в�ѯ��������ز�ѯ�����ص� DataTable ��������
        // -------------------------------------------------------

        #region ����:ExecuteQueryForDataTable(string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� ExecuteQueryForDataTable ��������</summary>
        public DataTable ExecuteQueryForDataTable(string commandText)
        {
            return this.ExecuteQueryForDataTable(CommandType.Text, commandText);
        }
        #endregion

        #region ����:ExecuteQueryForDataSet(CommandType commandType, string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� ExecuteQueryForDataTable ��������</summary>
        public DataTable ExecuteQueryForDataTable(CommandType commandType, string commandText)
        {
            this.PrepareCommand(false, commandType, commandText);

            return this.ExecuteQueryForDataTable();
        }
        #endregion

        #region ����:ExecuteQueryForDataTable(CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public DataTable ExecuteQueryForDataTable(CommandType commandType, string commandText, object[,] commandParameters)
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteQueryForDataTable();
        }
        #endregion

        #region ����:ExecuteQueryForDataTable(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public DataTable ExecuteQueryForDataTable(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteQueryForDataTable();
        }
        #endregion

        #region ˽�к���:ExecuteQueryForDataTable()
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataReader ʵ����</summary>
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

                    // ������ͷ
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
        // ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������
        // -------------------------------------------------------

        #region ����:ExecuteQueryForDataSet(string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public DataSet ExecuteQueryForDataSet(string commandText)
        {
            return this.ExecuteQueryForDataSet(CommandType.Text, commandText);
        }
        #endregion

        #region ����:ExecuteQueryForDataSet(CommandType commandType, string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public DataSet ExecuteQueryForDataSet(CommandType commandType, string commandText)
        {
            this.PrepareCommand(false, commandType, commandText);

            return this.ExecuteQueryForDataSet();
        }
        #endregion

        #region ����:ExecuteQueryForDataSet(CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public DataSet ExecuteQueryForDataSet(CommandType commandType, string commandText, object[,] commandParameters)
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteQueryForDataSet();
        }
        #endregion

        #region ����:ExecuteQueryForDataSet(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public DataSet ExecuteQueryForDataSet(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteQueryForDataSet();
        }
        #endregion

        #region ˽�к���:ExecuteQueryForDataSet()
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
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
        // ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�
        // -------------------------------------------------------

        #region ����:ExecuteQueryForList<T>(string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public IList<T> ExecuteQueryForList<T>(string commandText)
           where T : new()
        {
            return this.ExecuteQueryForList<T>(CommandType.Text, commandText);
        }
        #endregion

        #region ����:ExecuteQueryForList<T>(CommandType commandType, string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public IList<T> ExecuteQueryForList<T>(CommandType commandType, string commandText)
           where T : new()
        {
            this.PrepareCommand(false, commandType, commandText);

            return this.ExecuteQueryForList<T>();
        }
        #endregion

        #region ����:ExecuteQueryForList(CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public IList<T> ExecuteQueryForList<T>(CommandType commandType, string commandText, object[,] commandParameters)
           where T : new()
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteQueryForList<T>();
        }
        #endregion

        #region ����:ExecuteQueryForList(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public IList<T> ExecuteQueryForList<T>(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
           where T : new()
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteQueryForList<T>();
        }
        #endregion

        #region ˽�к���:ExecuteQueryForList()
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
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
        // ִ�в�ѯ��������ز�ѯ�����صĽ������ĵ�һ�еĵ�һ�С�
        // -------------------------------------------------------

        #region ����:ExecuteQueryForObject(string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public T ExecuteQueryForObject<T>(string commandText)
           where T : new()
        {
            return this.ExecuteQueryForObject<T>(CommandType.Text, commandText);
        }
        #endregion

        #region ����:ExecuteQueryForObject(CommandType commandType, string commandText)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public T ExecuteQueryForObject<T>(CommandType commandType, string commandText)
           where T : new()
        {
            this.PrepareCommand(false, commandType, commandText);

            return this.ExecuteQueryForObject<T>();
        }
        #endregion

        #region ����:ExecuteQueryForObject(CommandType commandType, string commandText, object[,] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public T ExecuteQueryForObject<T>(CommandType commandType, string commandText, object[,] commandParameters)
           where T : new()
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteQueryForObject<T>();
        }
        #endregion

        #region ����:ExecuteQueryForObject(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
        public T ExecuteQueryForObject<T>(CommandType commandType, string commandText, GenericSqlCommandParameter[] commandParameters)
           where T : new()
        {
            this.PrepareCommand(false, commandType, commandText, commandParameters);

            return this.ExecuteQueryForObject<T>();
        }
        #endregion

        #region ˽�к���:ExecuteQueryForObject()
        /// <summary>ִ�в�ѯ��������ز�ѯ�����ص� DataSet ��������</summary>
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

                        // ���������в����ں�������Ϊ����Ϣ
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

        /// <summary>�������������ƻ�ȡ����������, IDataReader ���õ�GetOrdinal������ѯ�����ڵ���ʱ��ֱ�����쳣</summary>
        /// <param name="reader"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private int GetDataReaderColumnOrdinal(DbDataReader reader, string name)
        {
            // �������������ƻ�ȡ����������
            int ordinal = -1;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                // �жϺ��Դ�Сд
                if (reader.GetName(i).ToLower() == name.ToLower())
                {
                    ordinal = i;
                    break;
                }
            }

            return ordinal;
        }

        /// <summary>����ռλ��������</summary>
        /// <param name="commandText">SQL����</param>
        /// <param name="placeholder">ռλ������</param>
        /// <param name="fillText">�������ı�</param>
        /// <returns></returns>
        public string FillPlaceholder(string commandText, string placeholder, string fillText)
        {
            return FillPlaceholder(commandText, placeholder, fillText, fillText);
        }

        /// <summary>����ռλ��������</summary>
        /// <param name="commandText">SQL����</param>
        /// <param name="placeholder">ռλ������</param>
        /// <param name="placeholderValue">ռλ��ʵ��������ֵ</param>
        /// <param name="fillText">�������ı�</param>
        /// <returns></returns>
        public string FillPlaceholder(string commandText, string placeholder, int placeholderValue, string fillText)
        {
            return FillPlaceholder(commandText, placeholder, (placeholderValue == 0 ? string.Empty : placeholderValue.ToString()), fillText);
        }

        /// <summary>����ռλ��������</summary>
        /// <param name="commandText">SQL����</param>
        /// <param name="placeholder">ռλ������</param>
        /// <param name="placeholderValue">ռλ��ʵ��������ֵ</param>
        /// <param name="fillText">�������ı�</param>
        /// <returns></returns>
        public string FillPlaceholder(string commandText, string placeholder, string placeholderValue, string fillText)
        {
            return commandText = commandText.Replace(placeholder, (string.IsNullOrEmpty(placeholderValue) ? string.Empty : fillText));
        }
    }
}