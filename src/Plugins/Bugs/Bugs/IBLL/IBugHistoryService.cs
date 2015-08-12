#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Bugs.IBLL.IBugHistoryService")]
    public interface IBugHistoryService
    {
        #region ����:this[string index]
        /// <summary>����</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        BugHistoryInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">BugHistoryInfo Id��</param>
        /// <returns>����һ�� BugHistoryInfo ʵ������ϸ��Ϣ</returns>
        BugHistoryInfo FindOne(string id);
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <returns>�������� BugHistoryInfo ʵ������ϸ��Ϣ</returns>
        IList<BugHistoryInfo> FindAll();
        #endregion

        #region ����:FindAll(string whereClause)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� BugHistoryInfo ʵ������ϸ��Ϣ</returns>
        IList<BugHistoryInfo> FindAll(string whereClause);
        #endregion

        #region ����:FindAll(string whereClause,int length)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� BugHistoryInfo ʵ������ϸ��Ϣ</returns>
        IList<BugHistoryInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(BugHistoryInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">BugHistoryInfo ʵ����ϸ��Ϣ</param>
        /// <returns>BugHistoryInfo ʵ����ϸ��Ϣ</returns>
        BugHistoryInfo Save(BugHistoryInfo param);
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="keys">��ʶ,����Զ��Ÿ���</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        IList<BugHistoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="id">BugHistoryInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
