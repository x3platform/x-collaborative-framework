#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Tasks.DAL.MySQL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership;
    using X3Platform.Util;

    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.Configuration;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class TaskHistoryProvider : ITaskHistoryProvider
    {
        /// <summary>����</summary>
        private TasksConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Task_HistoryItem";

        #region ���캯��:TaskHistoryProvider()
        /// <summary>���캯��</summary>
        public TaskHistoryProvider()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region ����:BeginTransaction()
        /// <summary>��������</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region ����:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region ����:CommitTransaction()
        /// <summary>�ύ����</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region ����:RollBackTransaction()
        /// <summary>�ع�����</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region ����:Save(TaskHistoryItemInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ����ϸ��Ϣ</param>
        /// <returns></returns>
        public TaskHistoryItemInfo Save(TaskHistoryItemInfo param)
        {
            if (!this.IsExist(param.Id, param.ReceiverId))
            {
                this.Insert(param);
            }
            else
            {
                this.Update(param);
            }

            return param;
        }
        #endregion

        #region ����:Insert(TaskHistoryItemInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        public void Insert(TaskHistoryItemInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region ����:Update(TaskHistoryItemInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        public void Update(TaskHistoryItemInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region ����:Delete(string id, string receiverId)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        public void Delete(string id, string receiverId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(receiverId)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' AND ReceiverId = '{1}' ", StringHelper.ToSafeSQL(id), StringHelper.ToSafeSQL(receiverId)));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id, string receiverId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns>����ʵ��<see cref="TaskHistoryItemInfo"/></returns>
        public TaskHistoryItemInfo FindOne(string id, string receiverId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("ReceiverId", StringHelper.ToSafeSQL(receiverId));

            return this.ibatisMapper.QueryForObject<TaskHistoryItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns> 
        public IList<TaskHistoryItemInfo> GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(whereClause))
            {
                whereClause = string.Format(" ReceiverId = ##{0}## ", receiverId);
            }
            else
            {
                whereClause = string.Format(" ReceiverId = ##{0}## AND {1}", receiverId, whereClause);
            }

            orderBy = string.IsNullOrEmpty(orderBy) ? " CreateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<TaskHistoryItemInfo> list = this.ibatisMapper.QueryForList<TaskHistoryItemInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region ����:IsExist(string id, string receiverId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id, string receiverId)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' AND ReceiverId = '{1}' ", StringHelper.ToSafeSQL(id), StringHelper.ToSafeSQL(receiverId)));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;
        }
        #endregion
    }
}
