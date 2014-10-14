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

namespace X3Platform.Tasks.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IBLL.ITaskHistoryService")]
    public interface ITaskHistoryService
    {
        #region ����:this[string id, string receiverId]
        /// <summary>����</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns></returns>
        TaskHistoryItemInfo this[string id, string receiverId] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(TaskHistoryItemInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ����ϸ��Ϣ</param>
        /// <returns>����ʵ��<see cref="TaskHistoryItemInfo"/></returns>
        TaskHistoryItemInfo Save(TaskHistoryItemInfo param);
        #endregion

        #region ����:Delete(string id, string receiverId)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        void Delete(string id, string receiverId);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id, string receiverId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns>����ʵ��<see cref="TaskHistoryItemInfo"/></returns>
        TaskHistoryItemInfo FindOne(string id, string receiverId);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(string receiverId, int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
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

        #region ����:IsExist(string id, string receiverId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <param name="receiverId">�����˱�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id, string receiverId);
        #endregion
    }
}
