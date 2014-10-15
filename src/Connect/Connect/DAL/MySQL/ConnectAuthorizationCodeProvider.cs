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

namespace X3Platform.Connect.DAL.MySQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership.Scope;
    using X3Platform.Util;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.IDAL;
    using X3Platform.Connect.Model;

    [DataObject]
    public class ConnectAuthorizationCodeProvider : IConnectAuthorizationCodeProvider
    {
        /// <summary>����</summary>
        private ConnectConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Connect_AuthorizationCode";

        public ConnectAuthorizationCodeProvider()
        {
            this.configuration = ConnectConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region 属性:CreateGenericSqlCommand()
        /// <summary>����ͨ��SQL��������</summary>
        public GenericSqlCommand CreateGenericSqlCommand()
        {
            return this.ibatisMapper.CreateGenericSqlCommand();
        }
        #endregion

        #region 属性:BeginTransaction()
        /// <summary>��������</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 属性:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 属性:CommitTransaction()
        /// <summary>�ύ����</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 属性:RollBackTransaction()
        /// <summary>�ع�����</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(ConnectAuthorizationCodeInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ConnectAuthorizationCodeInfo ʵ����ϸ��Ϣ</param>
        /// <returns>ConnectAuthorizationCodeInfo ʵ����ϸ��Ϣ</returns>
        public ConnectAuthorizationCodeInfo Save(ConnectAuthorizationCodeInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region 属性:Insert(ConnectAuthorizationCodeInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="ConnectAuthorizationCodeInfo"/>��ϸ��Ϣ</param>
        public void Insert(ConnectAuthorizationCodeInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region 属性:Update(ConnectAuthorizationCodeInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="ConnectAuthorizationCodeInfo"/>��ϸ��Ϣ</param>
        public void Update(ConnectAuthorizationCodeInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        public int Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return 1; }

            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                //  ɾ���������ĸ�����Ϣ
                args.Add("ConnectAuthorizationCodeId", StringHelper.ToSafeSQL(id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_History_DeleteByConnectAuthorizationCodeId", this.tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Comment_DeleteByConnectAuthorizationCodeId", this.tableName)), args);

                //  ɾ����������ʵ����Ϣ
                args.Add("Id", StringHelper.ToSafeSQL(id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

                this.ibatisMapper.CommitTransaction();
            }
            catch
            {
                this.ibatisMapper.RollBackTransaction();
                throw;
            }

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="param">ConnectAuthorizationCodeInfo Id��</param>
        /// <returns>����һ��ʵ��<see cref="ConnectAuthorizationCodeInfo"/>����ϸ��Ϣ</returns>
        public ConnectAuthorizationCodeInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<ConnectAuthorizationCodeInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region 属性:FindOneByAccountId(string appKey, string accountId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����һ��ʵ��<see cref="ConnectAuthorizationCodeInfo"/>����ϸ��Ϣ</returns>
        public ConnectAuthorizationCodeInfo FindOneByAccountId(string appKey, string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AppKey", StringHelper.ToSafeSQL(appKey, true));
            args.Add("AccountId", StringHelper.ToSafeSQL(accountId, true));

            return this.ibatisMapper.QueryForObject<ConnectAuthorizationCodeInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByAccountId", this.tableName)), args);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="ConnectAuthorizationCodeInfo"/>����ϸ��Ϣ</returns>
        public IList<ConnectAuthorizationCodeInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            whereClause = (string.IsNullOrEmpty(whereClause)) ? " 1=1 ORDER BY CreateDate " : whereClause;

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<ConnectAuthorizationCodeInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
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
        public IList<ConnectAuthorizationCodeInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ConnectAuthorizationCodeInfo> list = this.ibatisMapper.QueryForList<ConnectAuthorizationCodeInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", this.tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args);

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">ConnectAuthorizationCodeInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id, true)));

            return Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0 ? false : true;
        }
        #endregion

        #region 属性:IsExist(string appKey, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string appKey, string accountId)
        {
            if (string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(accountId)) { throw new Exception("Ӧ�ñ�ʶ���ʺű�ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AppKey = '{0}' AND AccountId = '{1}' ", StringHelper.ToSafeSQL(appKey, true), StringHelper.ToSafeSQL(accountId, true)));

            return Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0 ? false : true;
        }
        #endregion
    }
}
