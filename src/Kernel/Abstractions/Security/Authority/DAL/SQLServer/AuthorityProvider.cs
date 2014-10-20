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

        #region 构造函数:AuthorityProvider()
        /// <summary>构造函数</summary>
        public AuthorityProvider()
        {
            this.configuration = AuthorityConfigurationView.Instance.Configuration;

            this.dataSourceName = this.configuration.Keys["DataSourceName"].Value;

            this.command = new GenericSqlCommand(ConfigurationManager.ConnectionStrings[dataSourceName].ConnectionString, ConfigurationManager.ConnectionStrings[dataSourceName].ProviderName);
        }
        #endregion

        // -------------------------------------------------------
        // 保存 添加 修改 删除 
        // -------------------------------------------------------

        #region 函数:Save(AuthorityInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">AuthorityInfo 实例详细信息</param>
        /// <returns>AuthorityInfo 实例详细信息</returns>
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

        #region 函数:Insert(AuthorityInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">AuthorityInfo 实例的详细信息</param>
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
	CURRENT_TIMESTAMP,
	CURRENT_TIMESTAMP	
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

        #region 函数:Update(AuthorityInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">AuthorityInfo 实例的详细信息</param>
        public void Update(AuthorityInfo param)
        {
            string commandText = @"
UPDATE [tb_Authority] SET
	[Name] = @Name,
	[Description] = @Description,
	[Lock] = @Lock,
	[Tags] = @Tags,
	[OrderId] = @OrderId,
	[UpdateDate] = CURRENT_TIMESTAMP
		
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

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开</param>
        public void Delete(string ids)
        {
            string commandText = string.Format(@" DELETE FROM [tb_Authority] WHERE [Id] IN ('{0}') ", StringHelper.ToSafeSQL(ids.Replace(",", "','")));

            this.command.ExecuteNonQuery(commandText);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AuthorityInfo Id号</param>
        /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
        public AuthorityInfo FindOne(string id)
        {
            string commandText = string.Format(" SELECT * FROM [tb_Authority] WHERE Id = '{0}' ", StringHelper.ToSafeSQL(id));

            return this.command.ExecuteQueryForObject<AuthorityInfo>(commandText);
        }
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">权限名称</param>
        /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
        public AuthorityInfo FindOneByName(string name)
        {
            string commandText = string.Format(" SELECT * FROM [tb_Authority] WHERE Name = '{0}' ", StringHelper.ToSafeSQL(name));

            return this.command.ExecuteQueryForObject<AuthorityInfo>(commandText);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 AuthorityInfo 实例的详细信息</returns>
        public IList<AuthorityInfo> FindAll(DataQuery query)
        {
            string commandText = @" SELECT {Top} * FROM tb_Authority {WhereClause} ";

            commandText = this.command.FillPlaceholder(commandText, "{WhereClause}", query.GetWhereSql(), string.Format(" WHERE {0} ", query.GetWhereSql()));

            commandText = this.command.FillPlaceholder(commandText, "{Top}", query.Length, string.Format(" TOP {0} ", query.Length));

            return this.command.ExecuteQueryForList<AuthorityInfo>(commandText);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件.</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
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

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (id == "0") { return false; }

            string commandText = string.Format(" SELECT COUNT(*) FROM [tb_Authority] WHERE Id = '{0}' ", StringHelper.ToSafeSQL(id));

            return ((int)command.ExecuteScalar(commandText) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return false; }

            string commandText = string.Format(" SELECT COUNT(*) FROM [tb_Authority] WHERE Name = '{0}' ", StringHelper.ToSafeSQL(name));

            return ((int)command.ExecuteScalar(commandText) == 0) ? false : true;
        }
        #endregion

        #region 函数:HasAuthorizationObject(string accountId, string authorizationObjectType, string authorizationObjectNames)
        /// <summary>查询是否存在相关的授权记录.</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectNames">授权对象标识</param>
        /// <returns>布尔值</returns>
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
        public IList<AuthorityInfo> Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
