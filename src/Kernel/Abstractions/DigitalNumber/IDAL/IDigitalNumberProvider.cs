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
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.DigitalNumber.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.DigitalNumber.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.DigitalNumber.IDAL.IDigitalNumberProvider")]
    public interface IDigitalNumberProvider
    {
        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region ����:Save(DigitalNumberInfo param)
        /// <summary>������¼</summary>
        /// <param name="param"> ʵ��<see cref="NotepadInfo"/>��ϸ��Ϣ</param>
        /// <returns>NotepadInfo ʵ����ϸ��Ϣ</returns>
        DigitalNumberInfo Save(DigitalNumberInfo param);
        #endregion

        #region ����:Insert(DigitalNumberInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">NotepadInfo ʵ������ϸ��Ϣ</param>
        void Insert(DigitalNumberInfo param);
        #endregion

        #region ����:Update(DigitalNumberInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">NotepadInfo ʵ������ϸ��Ϣ</param>
        void Update(DigitalNumberInfo param);
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ��Ϣ,�����Զ��ŷֿ�.</param>
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

        #region ����:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ������ϸ��Ϣ</returns>
        IList<DigitalNumberInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>

        /// <returns>����һ���б�</returns> 
        IList<DigitalNumberInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region ����:IsExistName(string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="name">����</param>
        /// <returns>����ֵ</returns>
        bool IsExistName(string name);
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
