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
    using System.Data;

    using X3Platform.Data;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Security.Authority;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Connect.IDAL.IConnectProvider")]
    public interface IConnectProvider
    {
        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region 属性:CreateGenericSqlCommand()
        /// <summary>����ͨ��SQL��������</summary>
        GenericSqlCommand CreateGenericSqlCommand();
        #endregion

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
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(ConnectInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ConnectInfo ʵ����ϸ��Ϣ</param>
        /// <returns>ConnectInfo ʵ����ϸ��Ϣ</returns>
        ConnectInfo Save(ConnectInfo param);
        #endregion

        #region 属性:Insert(ConnectInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ConnectInfo ʵ������ϸ��Ϣ</param>
        void Insert(ConnectInfo param);
        #endregion

        #region 属性:Update(ConnectInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ConnectInfo ʵ������ϸ��Ϣ</param>
        void Update(ConnectInfo param);
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        int Delete(string id);
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

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� ConnectInfo ʵ������ϸ��Ϣ</returns>
        IList<ConnectInfo> FindAll(string whereClause, int length);
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
        /// <param name="param">ConnectInfo ʵ����ϸ��Ϣ</param>
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
        /// <param name="appKey">App Key</param>
        /// <param name="appSecret">App Secret</param>
        /// <returns></returns>
        int ResetAppSecret(string appKey, string appSecret);
        #endregion
    }
}
