#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityClickProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Entities.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    using X3Platform.Entities;
    #endregion

    [DataObject]
    public class EntityClickProvider : IEntityClickProvider
    {
        /// <summary>����</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Entity_Click";

        #region ���캯��:EntityClickProvider()
        /// <summary>���캯��</summary>
        public EntityClickProvider()
        {
            this.configuration = EntitiesConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
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
        // ���� ɾ�� �޸�
        // -------------------------------------------------------

        #region 属性:Save(IEntityClickInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</returns>
        public IEntityClickInfo Save(IEntityClickInfo param)
        {
            return this.Save(this.tableName, param);
        }
        #endregion

        #region 属性:Save(string customTableName, IEntityClickInfo param)
        /// <summary>������¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</returns>
        public IEntityClickInfo Save(string customTableName, IEntityClickInfo param)
        {
            if (!this.IsExist(customTableName, param.EntityId, param.EntityClassName, param.AccountId))
            {
                this.Insert(customTableName, param);
            }
            else
            {
                this.Update(customTableName, param);
            }

            return param;
        }
        #endregion

        #region 属性:Insert(IEntityClickInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        public void Insert(IEntityClickInfo param)
        {
            this.Insert(this.tableName, param);
        }
        #endregion

        #region 属性:Insert(string customTableName, IEntityClickInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        public void Insert(string customTableName, IEntityClickInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));

            args.Add("EntityId", StringHelper.ToSafeSQL(param.EntityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(param.EntityClassName));
            args.Add("AccountId", StringHelper.ToSafeSQL(param.AccountId));
            args.Add("Click", param.Click);

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), args);
        }
        #endregion

        #region 属性:Update(IEntityClickInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        public void Update(IEntityClickInfo param)
        {
            this.Update(this.tableName, param);
        }
        #endregion

        #region 属性:Update(string customTableName, IEntityClickInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        public void Update(string customTableName, IEntityClickInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));

            args.Add("EntityId", StringHelper.ToSafeSQL(param.EntityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(param.EntityClassName));
            args.Add("AccountId", StringHelper.ToSafeSQL(param.AccountId));
            args.Add("Click", param.Click);

            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntityDraftInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAll(string whereClause, int length)
        {
            return this.FindAll(this.tableName, whereClause, length);
        }
        #endregion

        #region 属性:FindAll(string customTableName, string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntityDraftInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAll(string customTableName, string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<IEntityClickInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        #region 属性:FindAllByEntityId(string entityId, string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string entityId, string entityClassName)
        {
            return this.FindAllByEntityId(this.tableName, entityId, entityClassName);
        }
        #endregion

        #region 属性:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        {
            string whereClause = string.Format(" EntityId = ##{0}## AND EntityClassName = ##{1}## ", StringHelper.ToSafeSQL(entityId), StringHelper.ToSafeSQL(entityClassName));

            return this.FindAll(customTableName, whereClause, 0);
        }
        #endregion

        #region 属性:FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
        {
            string whereClause = string.Format(" {0} = ##{1}## AND {2} = ##{3}## ORDER BY {4} DESC ", mapper["EntityId"].DataColumnName, entityId, mapper["EntityClassName"].DataColumnName, entityClassName, mapper["UpdateDate"].DataColumnName);
            
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("DataColumnSql", StringHelper.ToSafeSQL(mapper.GetDataColumnSql()));
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", 0);

            return ibatisMapper.QueryForList<IEntityClickInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:IsExist(string entityId, string entityClassName, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string entityId, string entityClassName, string accountId)
        {
            return this.IsExist(this.tableName, entityId, entityClassName, accountId);
        }
        #endregion

        #region 属性:IsExist(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string customTableName, string entityId, string entityClassName, string accountId)
        {
            if (string.IsNullOrEmpty(entityId))
            {
                throw new Exception("ʵ����ʶ����Ϊ��.");
            }

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("WhereClause", string.Format(" EntityId = '{0}' AND EntityClassName = '{1}' AND AccountId = '{2}' ", StringHelper.ToSafeSQL(entityId), StringHelper.ToSafeSQL(entityClassName), StringHelper.ToSafeSQL(accountId)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 属性:Increment(string entityId, string entityClassName, string accountId)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns></returns>
        public int Increment(string entityId, string entityClassName, string accountId)
        {
            return this.Increment(this.tableName, entityId, entityClassName, accountId);
        }
        #endregion

        #region 属性:Increment(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public int Increment(string customTableName, string entityId, string entityClassName, string accountId)
        {
            if (string.IsNullOrEmpty(customTableName))
            {
                throw new Exception("���ݱ����˲���Ϊ��.");
            }

            if (string.IsNullOrEmpty(entityId))
            {
                throw new Exception("ʵ����ʶ����Ϊ��.");
            }

            if (string.IsNullOrEmpty(entityClassName))
            {
                throw new Exception("ʵ���������Ʋ���Ϊ��.");
            }

            if (string.IsNullOrEmpty(accountId))
            {
                throw new Exception("�ʺű�ʶ����Ϊ��.");
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("EntityId", StringHelper.ToSafeSQL(entityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(entityClassName));
            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Increment", tableName)), args);

            return 0;
        }
        #endregion

        #region 属性:CalculateClickCount(string entityId, string entityClassName)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>������</returns>
        public int CalculateClickCount(string entityId, string entityClassName)
        {
            return this.CalculateClickCount(this.tableName, entityId, entityClassName);
        }
        #endregion

        #region 属性:CalculateClickCount(string tableName, string entityId, string entityClassName)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>������</returns>
        public int CalculateClickCount(string customTableName, string entityId, string entityClassName)
        {
            if (string.IsNullOrEmpty(customTableName))
            {
                throw new Exception("���ݱ����˲���Ϊ��.");
            }

            if (string.IsNullOrEmpty(entityId))
            {
                throw new Exception("ʵ����ʶ����Ϊ��.");
            }

            if (string.IsNullOrEmpty(entityClassName))
            {
                throw new Exception("ʵ���������Ʋ���Ϊ��.");
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("CustomTableName", StringHelper.ToSafeSQL(customTableName));
            args.Add("EntityId", StringHelper.ToSafeSQL(entityId));
            args.Add("EntityClassName", StringHelper.ToSafeSQL(entityClassName));

            return (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_CalculateClickCount", tableName)), args);
        }
        #endregion

    }
}
