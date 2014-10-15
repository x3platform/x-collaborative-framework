// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
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

namespace X3Platform.Security.Authority.DAL.SQLServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;

    using X3Platform.Data;
    using X3Platform.Util;

    using X3Platform.Security.Authority.Configuration;
    using X3Platform.Security.Authority.IDAL;

    /// <summary></summary>
    [DataObject]
    public class AuthorityProvider : IAuthorityProvider
    {
        private AuthorityConfiguration configuration = null;

        private GenericSqlCommand command = null;

        private string dataSourceName = null;

        private string tableName = "tb_Authority";

        #region ���캯��:AuthorityProvider()
        /// <summary>���캯��</summary>
        public AuthorityProvider()
        {
            this.configuration = AuthorityConfigurationView.Instance.Configuration;

            this.dataSourceName = this.configuration.Keys["DataSourceName"].Value;

            this.command = new GenericSqlCommand(ConfigurationManager.ConnectionStrings[dataSourceName].ConnectionString, ConfigurationManager.ConnectionStrings[dataSourceName].ProviderName);
        }
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ�� 
        // -------------------------------------------------------

        #region 属性:Save(AuthorityInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">AuthorityInfo ʵ����ϸ��Ϣ</param>
        ///<returns>AuthorityInfo ʵ����ϸ��Ϣ</returns>
        public AuthorityInfo Save(AuthorityInfo param)
        {
            if (!this.IsExist(param.Id))
            {
                this.Insert(param);
            }
            else
            {
                this.Update(param);
            }

            return param;
        }
        #endregion

        #region 属性:Insert(AuthorityInfo param)
        ///<summary>���Ӽ�¼</summary>
        ///<param name="param">AuthorityInfo ʵ������ϸ��Ϣ</param>
        public void Insert(AuthorityInfo param)
        {
            if (this.IsExistName(param.Name))
            {
                new Exception("The same authority's name already exists. |-_-||");
            }

            string commandText = @"
INSERT INTO [tb_Authority] 
(
	[Id],
	[Name],
	[Description],
	[Lock],
	[Tags],
	[OrderId],
	[UpdateDate],
	[CreateDate]
)
VALUES
(
	@Id,
	@Name,
	@Description,
	@Lock,
	@Tags,
	@OrderId,
	GETDATE(),
	GETDATE()	
)";

            GenericSqlCommandParameter[] commandParameters = new GenericSqlCommandParameter[6];

            commandParameters[0] = new GenericSqlCommandParameter("Id", param.Id);
            commandParameters[1] = new GenericSqlCommandParameter("Name", param.Name);
            commandParameters[2] = new GenericSqlCommandParameter("Description", param.Description);
            commandParameters[3] = new GenericSqlCommandParameter("Lock", param.Lock);
            commandParameters[4] = new GenericSqlCommandParameter("Tags", param.Tags);
            commandParameters[5] = new GenericSqlCommandParameter("OrderId", param.OrderId);

            this.command.ExecuteNonQuery(CommandType.Text, commandText, commandParameters);
        }
        #endregion

        #region 属性:Update(AuthorityInfo param)
        ///<summary>�޸ļ�¼</summary>
        ///<param name="param">AuthorityInfo ʵ������ϸ��Ϣ</param>
        public void Update(AuthorityInfo param)
        {
            string commandText = @"
UPDATE [tb_Authority] SET
	[Name] = @Name,
	[Description] = @Description,
	[Lock] = @Lock,
	[Tags] = @Tags,
	[OrderId] = @OrderId,
	[UpdateDate] = GETDATE()
		
WHERE
	[Id] = @Id";

            GenericSqlCommandParameter[] commandParameters = new GenericSqlCommandParameter[6];

            commandParameters[0] = new GenericSqlCommandParameter("Id", param.Id);
            commandParameters[1] = new GenericSqlCommandParameter("Name", param.Name);
            commandParameters[2] = new GenericSqlCommandParameter("Description", param.Description);
            commandParameters[3] = new GenericSqlCommandParameter("Lock", param.Lock);
            commandParameters[4] = new GenericSqlCommandParameter("Tags", param.Tags);
            commandParameters[5] = new GenericSqlCommandParameter("OrderId", param.OrderId);

            this.command.ExecuteNonQuery(CommandType.Text, commandText, commandParameters);
        }
        #endregion

        #region 属性:Delete(string ids)
        ///<summary>ɾ����¼</summary>
        ///<param name="ids">��ʶ,�����Զ��Ÿ���</param>
        public void Delete(string ids)
        {
            string commandText = string.Format(@" DELETE FROM [tb_Authority] WHERE [Id] IN ('{0}') ", StringHelper.ToSafeSQL(ids.Replace(",", "','")));

            this.command.ExecuteNonQuery(commandText);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">AuthorityInfo Id��</param>
        ///<returns>����һ�� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        public AuthorityInfo FindOne(string id)
        {
            string commandText = string.Format(" SELECT * FROM [tb_Authority] WHERE Id = '{0}' ", StringHelper.ToSafeSQL(id));

            return this.command.ExecuteQueryForObject<AuthorityInfo>(commandText);
        }
        #endregion

        #region 属性:FindOneByName(string name)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="name">Ȩ������</param>
        ///<returns>����һ�� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        public AuthorityInfo FindOneByName(string name)
        {
            string commandText = string.Format(" SELECT * FROM [tb_Authority] WHERE Name = '{0}' ", StringHelper.ToSafeSQL(name));

            return this.command.ExecuteQueryForObject<AuthorityInfo>(commandText);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>�������� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        public IList<AuthorityInfo> FindAll(string whereClause, int length)
        {
            string commandText = @" SELECT {Top} * FROM tb_Authority {WhereClause} ";

            commandText = this.command.FillPlaceholder(commandText, "{WhereClause}", whereClause, string.Format(" WHERE {0} ", StringHelper.ToSafeSQL(whereClause)));

            commandText = this.command.FillPlaceholder(commandText, "{Top}", length, string.Format(" TOP {0} ", length));

            return this.command.ExecuteQueryForList<AuthorityInfo>(commandText);
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
        /// <param name="orderBy">ORDER BY ��������.</param>
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns> 
        public IList<AuthorityInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            string commandText = @" 
SELECT *

FROM ( SELECT *, ROW_NUMBER() OVER(ORDER BY CreateDate DESC ) AS RowIndex 

FROM {TableName} T {WhereClause}) TableIndex

WHERE TableIndex.RowIndex BETWEEN @StartIndex + 1 AND @StartIndex + @PageSize 

{OrderBy};";

            commandText = this.command.FillPlaceholder(commandText, "{TableName}", tableName);

            commandText = this.command.FillPlaceholder(commandText, "{WhereClause}", whereClause, string.Format(" WHERE {0} ", StringHelper.ToSafeSQL(whereClause)));

            commandText = this.command.FillPlaceholder(commandText, "{OrderBy}", orderBy, string.Format(" ORDER BY {0} ", StringHelper.ToSafeSQL(orderBy)));

            GenericSqlCommandParameter[] commandParameters = new GenericSqlCommandParameter[2];

            commandParameters[0] = new GenericSqlCommandParameter("@StartIndex", startIndex);
            commandParameters[1] = new GenericSqlCommandParameter("@PageSize", pageSize);

            IList<AuthorityInfo> list = this.command.ExecuteQueryForList<AuthorityInfo>(CommandType.Text, commandText, commandParameters);

            commandText = string.Format(@" SELECT COUNT(*) FROM {0} T {1}; ",
                this.tableName,
                (string.IsNullOrEmpty(whereClause) ? string.Empty : string.Format(" WHERE {0} ", StringHelper.ToSafeSQL(whereClause))));

            rowCount = (int)this.command.ExecuteScalar(commandText);

            command.CloseConnection();

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (id == "0") { return false; }

            string commandText = string.Format(" SELECT COUNT(*) FROM [tb_Authority] WHERE Id = '{0}' ", StringHelper.ToSafeSQL(id));

            return ((int)command.ExecuteScalar(commandText) == 0) ? false : true;
        }
        #endregion

        #region 属性:IsExistName(string name)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
        ///<param name="name">����</param>
        ///<returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return false; }

            string commandText = string.Format(" SELECT COUNT(*) FROM [tb_Authority] WHERE Name = '{0}' ", StringHelper.ToSafeSQL(name));

            return ((int)command.ExecuteScalar(commandText) == 0) ? false : true;
        }
        #endregion

        #region 属性:HasAuthorizationObject(string accountId, string authorizationObjectType, string authorizationObjectNames)
        ///<summary>��ѯ�Ƿ��������ص���Ȩ��¼.</summary>
        ///<param name="accountId">�ʺű�ʶ</param>
        ///<param name="authorizationObjectType">��Ȩ��������</param>
        ///<param name="authorizationObjectNames">��Ȩ������ʶ</param>
        ///<returns>����ֵ</returns>
        public bool HasAuthorizationObject(string accountId, string authorizationObjectType, string authorizationObjectNames)
        {
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(authorizationObjectType) || string.IsNullOrEmpty(authorizationObjectNames)) { return false; }

            string commandText = string.Format(" SELECT COUNT(*) FROM [view_AuthorizationObject_Account] WHERE AccountId = '{0}' AND AuthorizationObjectType = '{1}' AND AuthorizationObjectName IN ('{2}')",
                StringHelper.ToSafeSQL(accountId),
                StringHelper.ToSafeSQL(authorizationObjectType),
                StringHelper.ToSafeSQL(authorizationObjectNames).Replace(",", "','"));

            return ((int)command.ExecuteScalar(commandText) == 0) ? false : true;

            //string whereClause = string.Format(" AccountId = '{0}' AND AuthorizationObjectType = '{1}' AND AuthorizationObjectName IN ('{2}')",
            //    StringHelper.ToSafeSQL(accountId),
            //    StringHelper.ToSafeSQL(authorizationObjectType),
            //    StringHelper.ToSafeSQL(authorizationObjectNames).Replace(",", "','"));

            //DbCommand dbCommand = db.GetStoredProcCommand(StringHelper.ToProcedurePrefix(string.Format("{0}_HasAuthorizationObject", tableName)));

            //db.AddInParameter(dbCommand, "@WhereClause", DbType.String, whereClause);

            //db.AddOutParameter(dbCommand, "@Count", DbType.Int32, 0);

            //db.ExecuteNonQuery(dbCommand);

            // return ((int)db.GetParameterValue(dbCommand, "@Count") == 0) ? false : true;
        }
        #endregion

        #region IAuthorityProvider ��Ա


        public IList<AuthorityInfo> FindAll(DataQuery query)
        {
            throw new NotImplementedException();
        }

        public IList<AuthorityInfo> Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
