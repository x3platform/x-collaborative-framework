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

namespace X3Platform.Connect.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership.Scope;
    using X3Platform.Spring;

    using X3Platform.Connect.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Connect.IBLL.IConnectService")]
    public interface IConnectService
    {
        #region 属性:this[string index]
        /// <summary>����</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ConnectInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">ConnectInfo Id��</param>
        /// <returns>����һ�� ConnectInfo ʵ������ϸ��Ϣ</returns>
        ConnectInfo FindOne(string id);
        #endregion

        #region 属性:FindOneByAppKey(string appKey)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>����һ��ʵ��<see cref="ConnectInfo"/>����ϸ��Ϣ</returns>
        ConnectInfo FindOneByAppKey(string appKey);
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� ConnectInfo ʵ������ϸ��Ϣ</returns>
        IList<ConnectInfo> FindAll();
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� ConnectInfo ʵ������ϸ��Ϣ</returns>
        IList<ConnectInfo> FindAll(string whereClause);
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� ConnectInfo ʵ������ϸ��Ϣ</returns>
        IList<ConnectInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(ConnectInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ConnectInfo ʵ����ϸ��Ϣ</param>
        /// <returns>ConnectInfo ʵ����ϸ��Ϣ</returns>
        ConnectInfo Save(ConnectInfo param);
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
        IList<ConnectInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="CostInfo"/></returns>
        IList<ConnectQueryInfo> GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:IsExistAppKey(string appKey)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>����ֵ</returns>
        bool IsExistAppKey(string appKey);
        #endregion

        #region 属性:ResetAppSecret(string appKey)
        /// <summary>����Ӧ������������Կ</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns></returns>
        int ResetAppSecret(string appKey);
        #endregion

        #region 属性:ResetAppSecret(string appKey, string appSecret)
        /// <summary>����Ӧ������������Կ</summary>
        /// <param name="appKey">App Key</param>
        /// <param name="appSecret">App Secret</param>
        /// <returns></returns>
        int ResetAppSecret(string appKey, string appSecret);
        #endregion
    }
}
