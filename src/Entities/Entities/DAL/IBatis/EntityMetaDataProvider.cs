#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityMetaDataProvider.cs
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
    using System.Text;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class EntityMetaDataProvider : IEntityMetaDataProvider
    {
        /// <summary>配置</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Entity_MetaData";

        #region ���캯��:EntityMetaDataProvider()
        /// <summary>���캯��</summary>
        public EntityMetaDataProvider()
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

        #region 属性:Save(EntityMetaDataInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</returns>
        public EntityMetaDataInfo Save(EntityMetaDataInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (EntityMetaDataInfo)param;
        }
        #endregion

        #region 属性:Insert(EntityMetaDataInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</param>
        public void Insert(EntityMetaDataInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 属性:Update(EntityMetaDataInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</param>
        public void Update(EntityMetaDataInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return;
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public EntityMetaDataInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            EntityMetaDataInfo param = this.ibatisMapper.QueryForObject<EntityMetaDataInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<EntityMetaDataInfo> list = this.ibatisMapper.QueryForList<EntityMetaDataInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 属性:FindAllByEntitySchemaId(string entitySchemaId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAllByEntitySchemaId(string entitySchemaId)
        {
            string whereClause = string.Format(" EntitySchemaId = ##{0}## ", entitySchemaId);

            return this.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAllByEntitySchemaName(string entitySchemaName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entitySchemaName">ʵ���ṹ����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAllByEntitySchemaName(string entitySchemaName)
        {
            string whereClause = string.Format(" EntitySchemaId IN ( SELECT Id FROM tb_Entity_Schema WHERE EntitySchemaName = ##{0}## ) ", entitySchemaName);

            return this.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAllByEntityClassName(string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAllByEntityClassName(string entityClassName)
        {
            string whereClause = string.Format(" EntitySchemaId IN ( SELECT Id FROM tb_Entity_Schema WHERE EntityClassName = ##{0}## ) ", entityClassName);

            return this.FindAll(whereClause, 0);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="EntityMetaDataInfo"/></returns>
        public IList<EntityMetaDataInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<EntityMetaDataInfo> list = this.ibatisMapper.QueryForList<EntityMetaDataInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("ʵ����ʶ����Ϊ��.");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 属性:GenerateSQL(string entitySchemaId, string sqlType, int effectScope)
        /// <summary>����ʵ��ҵ�����ݵ�����SQL����</summary>
        /// <param name="entitySchemaId">ʵ���ṹ��ʶ</param>
        /// <param name="sqlType">SQL���� create | read | update | delete </param>
        /// <param name="effectScope">���÷�Χ 0 ��ͨ�ֶ� | 1 �������ֶ�</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public string GenerateSQL(string entitySchemaId, string sqlType, int effectScope)
        {
            StringBuilder outString = new StringBuilder();

            return outString.ToString();
        }
        #endregion

        #region 属性:ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        /// <summary>ִ��SQL����</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>0 �ɹ� 1 ʧ��</returns>
        public int ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 属性:ExecuteSQL(string sql, Dictionary<string, string> args)
        /// <summary>ִ��SQL����</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>����ҵ��������Ϣ</returns>
        public DataTable ExecuteReaderSQL(string sql, Dictionary<string, string> args)
        {
            //this.ibatisMapper.DataSource.DbProvider
            //using(SqlConnection conn = new SqlConnection()){
            //}
            //return db;
            return null;
        }
        #endregion

        #region 属性:AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        /// <summary>�����ж�����SQL</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>�ж������Ƿ�����</returns>
        public bool AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        {
            GenericSqlCommand command = new GenericSqlCommand(this.ibatisMapper.DataSource.ConnectionString, this.ibatisMapper.DataSource.DbProvider.Name);

            return (command.ExecuteScalar(CommandType.Text, StringHelper.ToSafeSQL(sql)).ToString() == "0") ? true : false;
        }
        #endregion
    }
}
