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

namespace X3Platform.Tasks.DAL.IBatis
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
    public class TaskCategoryProvider : ITaskCategoryProvider
    {
        /// <summary>����</summary>
        private TasksConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Task_Category";

        #region ���캯��:TaskCategoryProvider()
        /// <summary>���캯��</summary>
        public TaskCategoryProvider()
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

        #region ����:Save(TaskCategoryInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ����ϸ��Ϣ</param>
        /// <returns></returns>
        public TaskCategoryInfo Save(TaskCategoryInfo param)
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

        #region ����:Insert(TaskCategoryInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        public void Insert(TaskCategoryInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region ����:Update(TaskCategoryInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        public void Update(TaskCategoryInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region ����:CanDelete(string id)
        /// <summary>�ж����������Ƿ��ܹ���ɾ��</summary>
        /// <param name="id">����������ʶ</param>
        /// <returns>true����ɾ����false������ɾ����</returns>
        public bool CanDelete(string id)
        {
            bool canDelete = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CategoryId", StringHelper.ToSafeSQL(id));

            canDelete = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_CanDelete", this.tableName)), args) == 0) ? true : false;

            return canDelete;
        }
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">����������ʶ</param>
        public void Delete(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ������ϸ��Ϣ</returns>
        public TaskCategoryInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            TaskCategoryInfo param = this.ibatisMapper.QueryForObject<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);

            return param;
        }
        #endregion

        #region ����:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="categoryIndex">��������</param>
        /// <returns>����ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        public TaskCategoryInfo FindOneByCategoryIndex(string categoryIndex)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CategoryIndex", categoryIndex);

            TaskCategoryInfo param = this.ibatisMapper.QueryForObject<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByCategoryIndex", this.tableName)), args);

            return param;
        }
        #endregion

        #region ����:FindAll(string whereClause,int length)
        /// <summary>
        /// ��ѯ�������ؼ�¼
        /// </summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns></returns>
        public IList<TaskCategoryInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<TaskCategoryInfo> list = this.ibatisMapper.QueryForList<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);

            return list;
        }
        #endregion

        #region ����:FindAllQueryObject(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        public IList<TaskCategoryInfo> FindAllQueryObject(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<TaskCategoryInfo> list = this.ibatisMapper.QueryForList<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllQueryObject", this.tableName)), args);

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��.</param>
        /// <param name="pageSize">ÿҳ��ʾ����������</param>
        /// <param name="whereClause">WHERE ��ѯ����.</param>
        /// <param name="orderBy">ORDER BY ��������.</param>
        /// <param name="rowCount">��������������������</param>
        /// <returns></returns>
        public IList<TaskCategoryInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<TaskCategoryInfo> list = this.ibatisMapper.QueryForList<TaskCategoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", this.tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args);

            return list;
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>
        /// ��ѯ�Ƿ��������صļ�¼
        /// </summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id='{0}' ", StringHelper.ToSafeSQL(id)));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region ����:SetStatus(string id, int status)
        /// <summary>��������״̬(ͣ��/����)</summary>
        /// <param name="id">����������ʶ</param>
        /// <param name="status">1��ͣ�õ��������ã�0�����õ�����ͣ��</param>
        /// <returns></returns>
        public bool SetStatus(string id, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);
            args.Add("Status", status);

            return (this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetStatus", this.tableName)), args) == 1 ? true : false);
        }
        #endregion
    }
}
