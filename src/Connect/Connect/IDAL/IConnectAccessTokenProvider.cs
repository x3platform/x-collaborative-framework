#region Copyright & Author
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
#endregion

namespace X3Platform.Connect.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership.Scope;
    using X3Platform.Spring;

    using X3Platform.Connect.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Connect.IDAL.IConnectAccessTokenProvider")]
    public interface IConnectAccessTokenProvider
    {
        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">ConnectAccessTokenInfo Id��</param>
        /// <returns>����һ�� ConnectAccessTokenInfo ʵ������ϸ��Ϣ</returns>
        ConnectAccessTokenInfo FindOne(string id);
        #endregion

        #region 属性:FindOneByAccountId(string appKey, string accountId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����һ��ʵ��<see cref="ConnectAccessTokenInfo"/>����ϸ��Ϣ</returns>
        ConnectAccessTokenInfo FindOneByAccountId(string appKey, string accountId);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� ConnectAccessTokenInfo ʵ������ϸ��Ϣ</returns>
        IList<ConnectAccessTokenInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(ConnectAccessTokenInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ConnectAccessTokenInfo ʵ����ϸ��Ϣ</param>
        /// <returns>ConnectAccessTokenInfo ʵ����ϸ��Ϣ</returns>
        ConnectAccessTokenInfo Save(ConnectAccessTokenInfo param);
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        int Delete(string id);
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
        IList<ConnectAccessTokenInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">ConnectAccessTokenInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:IsExist(string appKey, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string appKey, string accountId);
        #endregion

        #region 属性:Refesh(string appKey, string refreshToken, DateTime expireDate)
        /// <summary>ˢ���ʺŵķ�������</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="refreshToken">ˢ������</param>
        /// <param name="expireDate">����ʱ��</param>
        /// <returns></returns>
        int Refesh(string appKey, string refreshToken, DateTime expireDate);
        #endregion
    }
}
