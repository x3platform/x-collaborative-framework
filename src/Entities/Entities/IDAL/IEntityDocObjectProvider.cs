#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityDocObjectProvider.cs
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

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    using X3Platform.Entities;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IDAL.IEntityDocObjectProvider")]
    public interface IEntityDocObjectProvider
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
        // ����
        // -------------------------------------------------------

        #region 属性:Save(string customTableName, IEntityDocObjectInfo param)
        /// <summary>������¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="param">ʵ��<see cref="IEntityDocObjectInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IEntityDocObjectInfo"/>��ϸ��Ϣ</returns>
        IEntityDocObjectInfo Save(string customTableName, IEntityDocObjectInfo param);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindAll(string customTableName, string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IEntityDocObjectInfo"/>����ϸ��Ϣ</returns>
        IList<IEntityDocObjectInfo> FindAll(string customTableName, string whereClause, int length);
        #endregion

        #region 属性:FindAllByDocToken(string customTableName, string docToken)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="docToken">�ĵ�ȫ�ֱ�ʶ</param>
        /// <returns>��������ʵ��<see cref="IEntityDocObjectInfo"/>����ϸ��Ϣ</returns>
        IList<IEntityDocObjectInfo> FindAllByDocToken(string customTableName, string docToken);
        #endregion

        #region 属性:FindAllByDocToken(string customTableName, string docToken, DataResultMapper mapper)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="docToken">�ĵ�ȫ�ֱ�ʶ</param>
        /// <param name="mapper">���ݽ���ӳ����</param>
        /// <returns>��������ʵ��<see cref="IEntityDocObjectInfo"/>����ϸ��Ϣ</returns>
        IList<IEntityDocObjectInfo> FindAllByDocToken(string customTableName, string docToken, DataResultMapper mapper);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:IsExist(string customTableName, string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string customTableName, string id);
        #endregion
    }
}
