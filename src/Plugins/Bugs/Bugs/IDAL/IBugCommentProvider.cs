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

namespace X3Platform.Plugins.Bugs.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Bugs.IDAL.IBugCommentProvider")]
    public interface IBugCommentProvider
	{
		// -------------------------------------------------------
		// ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">BugCommentInfo Id��</param>
		/// <returns>����һ�� BugCommentInfo ʵ������ϸ��Ϣ</returns>
        BugCommentInfo FindOne(string id);
		#endregion

		#region ����:FindAll(string whereClause,int length)
		/// <summary>��ѯ������ؼ�¼</summary>
		/// <param name="whereClause">SQL ��ѯ����</param>
		/// <param name="length">����</param>
		/// <returns>�������� BugCommentInfo ʵ������ϸ��Ϣ</returns>
        IList<BugCommentInfo> FindAll(string whereClause, int length);
		#endregion

		// -------------------------------------------------------
        // ���� ��� �޸� ɾ��
		// -------------------------------------------------------

        #region ����:Save(BugCommentInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">BugCommentInfo ʵ����ϸ��Ϣ</param>
        /// <returns>BugCommentInfo ʵ����ϸ��Ϣ</returns>
        BugCommentInfo Save(BugCommentInfo param);
        #endregion

		#region ����:Insert(BugCommentInfo param)
		/// <summary>��Ӽ�¼</summary>
		/// <param name="param">BugCommentInfo ʵ������ϸ��Ϣ</param>
		void Insert(BugCommentInfo param);
		#endregion

		#region ����:Update(BugCommentInfo param)
		/// <summary>�޸ļ�¼</summary>
		/// <param name="param">BugCommentInfo ʵ������ϸ��Ϣ</param>
		void Update(BugCommentInfo param);
		#endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,����Զ��Ÿ���</param>
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
        IList<BugCommentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
		/// <param name="param">BugCommentInfo ʵ����ϸ��Ϣ</param>
		/// <returns>����ֵ</returns>
		bool IsExist(string id);
		#endregion

	}
}
