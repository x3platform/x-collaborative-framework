// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
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

namespace X3Platform.Connect.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Connect.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Connect.IDAL.IConnectCallProvider")]
    public interface IConnectCallProvider
	{
		// -------------------------------------------------------
		// ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">ConnectCallInfo Id��</param>
		/// <returns>����һ�� ConnectCallInfo ʵ������ϸ��Ϣ</returns>
        ConnectCallInfo FindOne(string id);
		#endregion

		#region 属性:FindAll(string whereClause,int length)
		/// <summary>��ѯ�������ؼ�¼</summary>
		/// <param name="whereClause">SQL ��ѯ����</param>
		/// <param name="length">����</param>
		/// <returns>�������� ConnectCallInfo ʵ������ϸ��Ϣ</returns>
        IList<ConnectCallInfo> FindAll(string whereClause, int length);
		#endregion

		// -------------------------------------------------------
        // ���� ���� �޸� ɾ��
		// -------------------------------------------------------

        #region 属性:Save(ConnectCallInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ConnectCallInfo ʵ����ϸ��Ϣ</param>
        /// <returns>ConnectCallInfo ʵ����ϸ��Ϣ</returns>
        ConnectCallInfo Save(ConnectCallInfo param);
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���</param>
		void Delete(string ids);
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
        /// <returns>����һ���б�ʵ��</returns> 
        IList<ConnectCallInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
		/// <param name="param">ConnectCallInfo ʵ����ϸ��Ϣ</param>
		/// <returns>����ֵ</returns>
		bool IsExist(string id);
		#endregion

	}
}
