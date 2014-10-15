#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityMetaDataService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Entities.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    using System.Data;
    #endregion

    /// <summary></summary>
    public class EntityMetaDataService : IEntityMetaDataService
    {
        /// <summary>����</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IEntityMetaDataProvider provider = null;

        #region ���캯��:EntityMetaDataService()
        /// <summary>���캯��</summary>
        public EntityMetaDataService()
        {
            configuration = EntitiesConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IEntityMetaDataProvider>(typeof(IEntityMetaDataProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityMetaDataInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(EntityMetaDataInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</returns>
        public EntityMetaDataInfo Save(EntityMetaDataInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
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
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByEntitySchemaId(string entitySchemaId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAllByEntitySchemaId(string entitySchemaId)
        {
            return this.provider.FindAllByEntitySchemaId(entitySchemaId);
        }
        #endregion

        #region 属性:FindAllByEntitySchemaName(string entitySchemaName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entitySchemaName">ʵ���ṹ����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAllByEntitySchemaName(string entitySchemaName)
        {
            return this.provider.FindAllByEntitySchemaName(entitySchemaName);
        }
        #endregion

        #region 属性:FindAllByEntityClassName(string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityMetaDataInfo> FindAllByEntityClassName(string entityClassName)
        {
            return this.provider.FindAllByEntityClassName(entityClassName);
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
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        // -------------------------------------------------------
        // ҵ�����ݹ���
        // -------------------------------------------------------

        #region 属性:GenerateSQL(string entitySchemaId, string sqlType)
        /// <summary>����ʵ��ҵ�����ݵ�����SQL����</summary>
        /// <param name="entitySchemaId">ʵ���ṹ��ʶ</param>
        /// <param name="sqlType">SQL���� create | read | update | delete </param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public string GenerateSQL(string entitySchemaId, string sqlType)
        {
            return this.GenerateSQL(entitySchemaId, sqlType, 0);
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
            return this.provider.GenerateSQL(entitySchemaId, sqlType, effectScope);
        }
        #endregion

        #region 属性:ExecuteNonQuerySQL(string entitySchemaId, string sqlType, Dictionary<string, string> args)
        /// <summary>ִ��SQL����</summary>
        /// <param name="entitySchemaId">ʵ���ṹ��ʶ</param>
        /// <param name="sqlType">SQL���� create | update | delete </param>
        /// <param name="args">����</param>
        /// <returns>0 �ɹ� 1 ʧ��</returns>
        public int ExecuteNonQuerySQL(string entitySchemaId, string sqlType)
        {
            string SQL = this.GenerateSQL(entitySchemaId, "read");

            return this.ExecuteNonQuerySQL(SQL);
        }
        #endregion

        #region 属性:GenerateSQL(string entitySchemaId, string sqlType)
        /// <summary>����SQL����</summary>
        /// <param name="entitySchemaId">ʵ���ṹ��ʶ</param>
        /// <param name="sqlType">SQL���� create | read | update | delete </param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        public int ExecuteNonQuerySQL(string entitySchemaId, string sqlType, Dictionary<string, string> args)
        {
            return this.ExecuteNonQuerySQL(entitySchemaId, sqlType, new Dictionary<string, string>());
        }
        #endregion

        #region 属性:ExecuteNonQuerySQL(string sql)
        /// <summary>ִ��SQL����</summary>
        /// <param name="sql">SQL����</param>
        /// <returns>0 �ɹ� 1 ʧ��</returns>
        public int ExecuteNonQuerySQL(string sql)
        {
            return this.ExecuteNonQuerySQL(sql, new Dictionary<string, string>());
        }
        #endregion

        #region 属性:ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        /// <summary>ִ��SQL����</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>0 �ɹ� 1 ʧ��</returns>
        public int ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        {
            return this.provider.ExecuteNonQuerySQL(sql, args);
        }
        #endregion

        #region 属性:ExecuteReaderSQL(string entitySchemaId, int effectScope)
        /// <summary>ִ��SQL����</summary>
        /// <param name="entitySchemaId">ʵ���ṹ��ʶ</param>
        /// <param name="effectScope">���÷�Χ 1 ��ͨ�ֶ� | 2 �������ֶ�</param>
        /// <returns>����ҵ��������Ϣ</returns>
        public DataTable ExecuteReaderSQL(string entitySchemaId, int effectScope)
        {
            return this.ExecuteReaderSQL(entitySchemaId, effectScope, new Dictionary<string, string>());
        }
        #endregion

        #region 属性:ExecuteReaderSQL(string entitySchemaId, int effectScope, Dictionary<string, string> args)
        /// <summary>ִ��SQL����</summary>
        /// <param name="entitySchemaId">ʵ���ṹ��ʶ</param>
        /// <param name="effectScope">���÷�Χ 0 ��ͨ�ֶ� | 1 �������ֶ�</param>
        /// <param name="args">����</param>
        /// <returns>����ҵ��������Ϣ</returns>
        public DataTable ExecuteReaderSQL(string entitySchemaId, int effectScope, Dictionary<string, string> args)
        {
            string SQL = this.GenerateSQL(entitySchemaId, "read", effectScope);

            return this.ExecuteReaderSQL(SQL, args);
        }
        #endregion

        #region 属性:ExecuteReaderSQL(string sql)
        /// <summary>ִ��SQL����</summary>
        /// <param name="sql">SQL����</param>
        /// <returns>����ҵ��������Ϣ</returns>
        public DataTable ExecuteReaderSQL(string sql)
        {
            return this.ExecuteReaderSQL(sql, new Dictionary<string, string>());
        }
        #endregion

        #region 属性:ExecuteReaderSQL(string sql, Dictionary<string, string> args)
        /// <summary>ִ��SQL����</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>����ҵ��������Ϣ</returns>
        public DataTable ExecuteReaderSQL(string sql, Dictionary<string, string> args)
        {
            return this.provider.ExecuteReaderSQL(sql, args);
        }
        #endregion

        #region 属性:AnalyzeConditionSQL(string sql)
        /// <summary>�����ж�����SQL</summary>
        /// <param name="sql">SQL����</param>
        /// <returns>�ж������Ƿ�����</returns>
        public bool AnalyzeConditionSQL(string sql)
        {
            return this.AnalyzeConditionSQL(sql, new Dictionary<string, string>());
        }
        #endregion

        #region 属性:AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        /// <summary>�����ж�����SQL</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>�ж������Ƿ�����</returns>
        public bool AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        {
            return this.provider.AnalyzeConditionSQL(sql, args);
        }
        #endregion
    }
}
