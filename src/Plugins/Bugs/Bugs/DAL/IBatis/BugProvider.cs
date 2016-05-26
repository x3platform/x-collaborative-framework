namespace X3Platform.Plugins.Bugs.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership.Scope;
    using X3Platform.Util;

    using X3Platform.Plugins.Bugs.Configuration;
    using X3Platform.Plugins.Bugs.IDAL;
    using X3Platform.Plugins.Bugs.Model;

    [DataObject]
    public class BugProvider : IBugProvider
    {
        /// <summary>����</summary>
        private BugConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ�����</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Bug";

        public BugProvider()
        {
            this.configuration = BugConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region ����:CreateGenericSqlCommand()
        /// <summary>����ͨ��SQL�������</summary>
        public GenericSqlCommand CreateGenericSqlCommand()
        {
            return this.ibatisMapper.CreateGenericSqlCommand();
        }
        #endregion

        #region ����:BeginTransaction()
        /// <summary>��������</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region ����:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">������뼶��</param>
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
        // ���� ��� �޸� ɾ��
        // -------------------------------------------------------

        #region ����:Save(BugInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">BugInfo ʵ����ϸ��Ϣ</param>
        /// <returns>BugInfo ʵ����ϸ��Ϣ</returns>
        public BugInfo Save(BugInfo param)
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

        #region ����:Insert(BugInfo param)
        /// <summary>��Ӽ�¼</summary>
        /// <param name="param">BugInfo ʵ������ϸ��Ϣ</param>
        public void Insert(BugInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", this.tableName)), param);
        }
        #endregion

        #region ����:Update(BugInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">BugInfo ʵ������ϸ��Ϣ</param>
        public void Update(BugInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", this.tableName)), param);
        }
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        public int Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return 1; }

            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                //  ɾ������ĸ�����Ϣ
                args.Add("BugId", StringHelper.ToSafeSQL(id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_History_DeleteByBugId", this.tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Comment_DeleteByBugId", this.tableName)), args);

                //  ɾ�������ʵ����Ϣ
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

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="param">BugInfo Id��</param>
        /// <returns>����һ�� BugInfo ʵ������ϸ��Ϣ</returns>
        public BugInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return this.ibatisMapper.QueryForObject<BugInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region ����:FindOneByCode(string code)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="code">������</param>
        /// <returns>����һ�� BugInfo ʵ������ϸ��Ϣ</returns>
        public BugInfo FindOneByCode(string code)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Code", StringHelper.ToSafeSQL(code));

            return this.ibatisMapper.QueryForObject<BugInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByCode", this.tableName)), args);
        }
        #endregion

        #region ����:FindAll(string whereClause,int length)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� BugInfo ʵ������ϸ��Ϣ</returns>
        public IList<BugInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            whereClause = (string.IsNullOrEmpty(whereClause)) ? " 1=1 ORDER BY CreatedDate " : whereClause;

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<BugInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
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
        public IList<BugInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = null;

            if (query.Variables["scence"] == "Query")
            {
                string docStatus = StringHelper.ToSafeSQL(query.Where["DocStatus"].ToString(), true);
                string code = StringHelper.ToSafeSQL(query.Where["Code"].ToString(), true);
                string subject = StringHelper.ToSafeSQL(query.Where["Subject"].ToString());
                string categoryId = StringHelper.ToSafeSQL(query.Where["CategoryId"].ToString(), true);

                whereClause = " T.Status=1 AND T.DocStatus!='Draft' AND T.DocStatus!='Abandon' ";

                if (!string.IsNullOrEmpty(docStatus))
                {
                    whereClause += " AND T.DocStatus = '" + docStatus + "' ";
                }

                if (!string.IsNullOrEmpty(code))
                {
                    whereClause += " AND T.Code LIKE '%" + code + "%' ";
                }

                if (!string.IsNullOrEmpty(subject))
                {
                    whereClause += " AND T.Subject LIKE '%" + subject + "%' ";
                }

                if (!string.IsNullOrEmpty(categoryId))
                {
                    whereClause += " AND T.CategoryId = '" + categoryId + "' ";
                }

                args.Add("WhereClause", whereClause);
            }
            else if (query.Variables["scence"] == "QueryMyList")
            {
                string accountId = query.Variables["accountId"];

                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());
                string status = StringHelper.ToSafeSQL(query.Where["Status"].ToString(), true);

                whereClause += " ( T.AccountId = '" + accountId + "' OR T.AssignToAccountId = '" + accountId + "' ) ";

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause += " AND T.Title LIKE '%" + searchText + "%'  OR T.Code LIKE '%" + searchText + "%' ";
                }

                if (!string.IsNullOrEmpty(status))
                {
                    whereClause += " AND T.Status IN (" + status + ") ";
                }

                args.Add("WhereClause", whereClause);
            }
            else
            {
                args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Title", "LIKE" } }));
            }

            args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            IList<BugInfo> list = this.ibatisMapper.QueryForList<BugInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

            return list;
        }
        #endregion

        #region ����:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        public IList<BugQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = null;

            if (query.Variables["scence"] == "Query")
            {
                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString(), true);
                string categoryIndex = StringHelper.ToSafeSQL(query.Where["CategoryIndex"].ToString(), true);
                string status = StringHelper.ToSafeSQL(query.Where["Status"].ToString(), true);

                whereClause = " 1 = 1 ";

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause += " AND ( T.Code LIKE '%" + searchText + "%' OR T.Title LIKE '%" + searchText + "%' OR T.Content LIKE '%" + searchText + "%' OR T.AccountId IN (SELECT AuthorizationObjectId FROM view_AuthObject_Account WHERE AccountGlobalName LIKE '%" + searchText + "%' OR AccountLoginName LIKE '%" + searchText + "%' ) OR T.AssignToAccountId IN (SELECT AuthorizationObjectId FROM view_AuthObject_Account WHERE AccountGlobalName LIKE '%" + searchText + "%' OR AccountLoginName LIKE '%" + searchText + "%' ) ) ";
                }

                if (!string.IsNullOrEmpty(categoryIndex))
                {
                    whereClause += " AND CategoryId IN ( SELECT Id FROM tb_Bug_Category WHERE CategoryIndex LIKE '" + categoryIndex.Replace("\\", "\\\\") + "%' ) ";
                }

                if (!string.IsNullOrEmpty(status))
                {
                    whereClause += " AND T.Status = " + status + " ";
                }

                // �Ƴ� 1 = 1
                if (whereClause.IndexOf(" 1 = 1  AND ") > -1)
                {
                    whereClause = whereClause.Replace(" 1 = 1  AND ", string.Empty);
                }

                args.Add("WhereClause", whereClause);
            }
            else if (query.Variables["scence"] == "QueryMyList")
            {
                string accountId = query.Variables["accountId"];

                string searchText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());
                string status = StringHelper.ToSafeSQL(query.Where["Status"].ToString(), true);

                whereClause += " ( T.AccountId = '" + accountId + "' OR T.AssignToAccountId = '" + accountId + "' ) ";

                if (!string.IsNullOrEmpty(searchText))
                {
                    whereClause += " AND T.Title LIKE '%" + searchText + "%'  OR T.Code LIKE '%" + searchText + "%' ";
                }

                if (!string.IsNullOrEmpty(status))
                {
                    whereClause += " AND T.Status IN (" + status + ") ";
                }

                args.Add("WhereClause", whereClause);
            }
            else
            {
                args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Title", "LIKE" } }));
            }

            args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            IList<BugQueryInfo> list = this.ibatisMapper.QueryForList<BugQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

            return list;
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="id">BugInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        // -------------------------------------------------------
        // Ȩ������
        // -------------------------------------------------------

        #region ����:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>��ѯʵ������Ȩ����Ϣ</summary> 
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            string scopeText = null;

            IList<IAuthorizationScope> result = new List<IAuthorizationScope>();

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(entityId));

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetAuthorizationScope", this.tableName)), args);

            foreach (DataRow row in table.Rows)
            {
                if (!string.IsNullOrEmpty(row["AuthorizationObjectType"].ToString()) && !string.IsNullOrEmpty(row["AuthorizationObjectId"].ToString()))
                {
                    scopeText += row["AuthorizationObjectType"] + "#" + row["AuthorizationObjectId"] + ";";
                }
            }

            return MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjects(scopeText);
        }
        #endregion
    }
}
