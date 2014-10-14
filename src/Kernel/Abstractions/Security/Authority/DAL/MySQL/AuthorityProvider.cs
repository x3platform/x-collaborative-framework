#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AuthorityProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Security.Authority.DAL.MySQL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Security.Authority.Configuration;
    using X3Platform.Security.Authority.IDAL;
    using X3Platform.Data;
    using Common.Logging;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class AuthorityProvider : IAuthorityProvider
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����</summary>
        private AuthorityConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Authority";

        #region ���캯��:AuthorityProvider()
        /// <summary>���캯��</summary>
        public AuthorityProvider()
        {
            this.configuration = AuthorityConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region ����:BeginTransaction()
        /// <summary>��������</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region ����:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region ����:CommitTransaction()
        /// <summary>�ύ����</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region ����:RollBackTransaction()
        /// <summary>�ع�����</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ�� �޸�
        // -------------------------------------------------------

        #region ����:Save(AuthorityInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</returns>
        public AuthorityInfo Save(AuthorityInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (AuthorityInfo)param;
        }
        #endregion

        #region ����:Insert(AuthorityInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</param>
        public void Insert(AuthorityInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region ����:Update(AuthorityInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</param>
        public void Update(AuthorityInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="AuthorityInfo"/>����ϸ��Ϣ</returns>
        public AuthorityInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region ����:FindOneByName(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">Ȩ������</param>
        /// <returns>����һ�� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        public AuthorityInfo FindOneByName(string name)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object>();

                args.Add("WhereClause", " Name = '" + name + "' ");
                args.Add("Length", 0);

                IList<AuthorityInfo> list = this.ibatisMapper.QueryForList<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

                logger.Debug(string.Format("list 1 count = [{0}]", list.Count));

                args["WhereClause"] = string.Empty;

                list = this.ibatisMapper.QueryForList<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

                logger.Debug(string.Format("list 2 count = [{0}]", list.Count));

                args["WhereClause"] = " Name = BINARY'" + name + "' ";

                list = this.ibatisMapper.QueryForList<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

                logger.Debug(string.Format("list 3 count = [{0}]", list.Count));

                args["WhereClause"] = " STRCMP(Name,'" + name + "') = 0 ";

                list = this.ibatisMapper.QueryForList<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

                logger.Debug(string.Format("list 3 count = [{0}]", list.Count));

                args.Add("Name", StringHelper.ToSafeSQL(name));

                AuthorityInfo param = this.ibatisMapper.QueryForObject<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);

                if (param == null)
                {
                    logger.Debug(string.Format("[{0}] is null", name));
                    logger.Error(this.ibatisMapper.DataSource.ConnectionString);
                }

                return param;

                // return this.ibatisMapper.QueryForObject<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);
            }
            catch (Exception ex)
            {
                logger.Error(this.ibatisMapper.DataSource.ConnectionString);
                logger.Error(ex);
                throw ex;
            }
        }
        #endregion

        #region ����:FindAll(DataQuery query)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <returns>��������ʵ��<see cref="AuthorityInfo"/>����ϸ��Ϣ</returns>
        public IList<AuthorityInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", query.GetWhereSql());
            args.Add("Length", query.Limit);

            return this.ibatisMapper.QueryForList<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="AuthorityInfo"/></returns> 
        public IList<AuthorityInfo> Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { 
                { "Name", "LIKE" }, { "Value", "LIKE" } 
            }));
            args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

            args.Add("RowCount", 0);

            IList<AuthorityInfo> list = this.ibatisMapper.QueryForList<AuthorityInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_Query", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ��."); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion
    }
}
