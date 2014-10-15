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

namespace X3Platform.Tasks.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IDAL.ITaskHistoryProvider")]
    public interface ITaskHistoryProvider
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
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(TaskHistoryItemInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ����ϸ��Ϣ</param>
        /// <returns>����ʵ��<see cref="TaskHistoryItemInfo"/></returns>
        TaskHistoryItemInfo Save(TaskHistoryItemInfo param);
        #endregion

        #region 属性:Delete(string id, string receiverId)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        void Delete(string id, string receiverId);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id, string receiverId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns>����ʵ��<see cref="TaskHistoryItemInfo"/></returns>
        TaskHistoryItemInfo FindOne(string id, string receiverId);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns> 
        IList<TaskHistoryItemInfo> GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id, string receiverId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id, string receiverId);
        #endregion
    }
}
