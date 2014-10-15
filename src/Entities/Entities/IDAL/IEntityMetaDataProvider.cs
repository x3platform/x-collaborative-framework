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

namespace X3Platform.Entities.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IDAL.IEntityMetaDataProvider")]
    public interface IEntityMetaDataProvider
    {
        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region 属性:BeginTransaction()
        /// <summary>��������</summary>
        void BeginTransaction();
        #endregion

        #region 属性:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region 属性:CommitTransaction()
        /// <summary>�ύ����</summary>
        void CommitTransaction();
        #endregion

        #region 属性:RollBackTransaction()
        /// <summary>�ع�����</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(EntityMetaDataInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</returns>
        EntityMetaDataInfo Save(EntityMetaDataInfo param);
        #endregion

        #region 属性:Insert(EntityMetaDataInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</param>
        void Insert(EntityMetaDataInfo param);
        #endregion

        #region 属性:Update(EntityMetaDataInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="EntityMetaDataInfo"/>��ϸ��Ϣ</param>
        void Update(EntityMetaDataInfo param);
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        EntityMetaDataInfo FindOne(string id);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        IList<EntityMetaDataInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAllByEntitySchemaId(string entitySchemaId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entitySchemaId">ʵ���ṹ��ʶ</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        IList<EntityMetaDataInfo> FindAllByEntitySchemaId(string entitySchemaId);
        #endregion

        #region 属性:FindAllByEntitySchemaName(string entitySchemaName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entitySchemaName">ʵ���ṹ����</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        IList<EntityMetaDataInfo> FindAllByEntitySchemaName(string entitySchemaName);
        #endregion

        #region 属性:FindAllByEntityClassName(string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        IList<EntityMetaDataInfo> FindAllByEntityClassName(string entityClassName);
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
        IList<EntityMetaDataInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        // -------------------------------------------------------
        // ҵ�����ݹ���
        // -------------------------------------------------------

        #region 属性:GenerateSQL(string entitySchemaId, string sqlType, int effectScope)
        /// <summary>����ʵ��ҵ�����ݵ�����SQL����</summary>
        /// <param name="entitySchemaId">ʵ���ṹ��ʶ</param>
        /// <param name="sqlType">SQL���� create | read | update | delete </param>
        /// <param name="effectScope">���÷�Χ 0 ��ͨ�ֶ� | 1 �������ֶ�</param>
        /// <returns>��������ʵ��<see cref="EntityMetaDataInfo"/>����ϸ��Ϣ</returns>
        string GenerateSQL(string entitySchemaId, string sqlType, int effectScope);
        #endregion

        #region 属性:ExecuteNonQuerySQL(string sql, Dictionary<string, string> args)
        /// <summary>ִ��SQL����</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>0 �ɹ� 1 ʧ��</returns>
        int ExecuteNonQuerySQL(string sql, Dictionary<string, string> args);
        #endregion

        #region 属性:ExecuteSQL(string sql, Dictionary<string, string> args)
        /// <summary>ִ��SQL����</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>����ҵ��������Ϣ</returns>
        DataTable ExecuteReaderSQL(string sql, Dictionary<string, string> args);
        #endregion

        #region 属性:AnalyzeConditionSQL(string sql, Dictionary<string, string> args)
        /// <summary>�����ж�����SQL</summary>
        /// <param name="sql">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>�ж������Ƿ�����</returns>
        bool AnalyzeConditionSQL(string sql, Dictionary<string, string> args);
        #endregion
    }
}
