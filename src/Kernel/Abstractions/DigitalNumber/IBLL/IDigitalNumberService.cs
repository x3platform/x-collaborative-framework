// =============================================================================
//
// Copyright (c) ruany@live.com
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

namespace X3Platform.DigitalNumber.IBLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.Spring;
    using X3Platform.Security.Authority;

    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.DigitalNumber.IBLL.IDigitalNumberService")]
    public interface IDigitalNumberService
    {
        #region ����:this[string name]
        /// <summary>����</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DigitalNumberInfo this[string name] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(DigitalNumberInfo param)
        /// <summary>������¼</summary>
        /// <param name="param"> ʵ��<see cref="DigitalNumberInfo"/>��ϸ��Ϣ</param>
        /// <returns>DigitalNumberInfo ʵ����ϸ��Ϣ</returns>
        DigitalNumberInfo Save(DigitalNumberInfo param);
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">IAccount ʵ���� Id ��Ϣ</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">DigitalNumberInfo Id��</param>
        /// <returns>����һ�� DigitalNumberInfo ʵ������ϸ��Ϣ</returns>
        DigitalNumberInfo FindOne(string name);
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� IAccount ʵ������ϸ��Ϣ</returns>
        IList<DigitalNumberInfo> FindAll();
        #endregion

        #region ����:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� IAccount ʵ������ϸ��Ϣ</returns>
        IList<DigitalNumberInfo> FindAll(string whereClause);
        #endregion

        #region ����:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� IAccount ʵ������ϸ��Ϣ</returns>
        IList<DigitalNumberInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        IList<DigitalNumberInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region ����:IsExistName(string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="name">����</param>
        /// <returns>����ֵ</returns>
        bool IsExistName(string name);
        #endregion

        #region ����:Generate(string name)
        /// <summary>�������ֱ���</summary>
        /// <param name="name">��������</param>
        /// <returns>���ֱ���</returns>
        string Generate(string name);
        #endregion

        #region ����:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        /// <summary>����ǰ׺�������ֱ���</summary>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="prefixCode">ǰ׺����</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression);
        #endregion

        #region ����:GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        /// <summary>����ǰ׺�������ֱ���</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="prefixCode">ǰ׺����</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        string GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression);
        #endregion

        #region ����:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>����������ʶ�����ֱ���</summary>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="entityCategoryTableName">ʵ���������ݱ�</param>
        /// <param name="entityCategoryId">ʵ��������ʶ</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression);
        #endregion

        #region ����:GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>����������ʶ�����ֱ���</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="entityCategoryTableName">ʵ���������ݱ�</param>
        /// <param name="entityCategoryId">ʵ��������ʶ</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        string GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression);
        #endregion
    }
}
