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

        #region 属性:BeginTransaction()
        /// <summary>��������</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 属性:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 属性:CommitTransaction()
        /// <summary>�ύ����</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 属性:RollBackTransaction()
        /// <summary>�ع�����</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(TaskCategoryInfo param)
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

        #region 属性:Insert(TaskCategoryInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        public void Insert(TaskCategoryInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 属性:Update(TaskCategoryInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        public void Update(TaskCategoryInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region 属性:CanDelete(string id)
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

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">����������ʶ</param>
        public void Delete(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            string scopeTableName = string.Format("{0}_Scope", this.tableName);

            string entityId = StringHelper.ToSafeSQL(id);
            string entityClassName = KernelContext.ParseObjectType(typeof(TaskCategoryInfo));

            try
            {
                this.BeginTransaction();

                MembershipManagement.Instance.AuthorizationObjectService.RemoveAuthorizationScopeObjects(
                    this.ibatisMapper.CreateGenericSqlCommand(),
                    scopeTableName,
                    entityId,
                    entityClassName,
                    "Ӧ��_ͨ��_����Ȩ��");

                MembershipManagement.Instance.AuthorizationObjectService.RemoveAuthorizationScopeObjects(
                    this.ibatisMapper.CreateGenericSqlCommand(),
                    scopeTableName,
                    entityId,
                    entityClassName,
                    "Ӧ��_ͨ��_�鿴Ȩ��");

                MembershipManagement.Instance.AuthorizationObjectService.RemoveAuthorizationScopeObjects(
                    this.ibatisMapper.CreateGenericSqlCommand(),
                    scopeTableName,
                    entityId,
                    entityClassName,
                    "Ӧ��_ͨ��_�޸�Ȩ��");

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

                this.CommitTransaction();
            }
            catch
            {
                this.RollBackTransaction();

                throw;
            }
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
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

        #region 属性:FindOneByCategoryIndex(string categoryIndex)
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

        #region 属性:FindAll(string whereClause,int length)
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

        #region 属性:FindAllQueryObject(string whereClause,int length)
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

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
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

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
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

        #region 属性:SetStatus(string id, int status)
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
