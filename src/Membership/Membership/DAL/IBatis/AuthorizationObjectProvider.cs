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

namespace X3Platform.Membership.DAL.IBatis
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Text;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Security.Authority;
    using X3Platform.Util;

    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;
    using X3Platform.Membership.Scope;

    /// <summary></summary>
    [DataObject]
    public class AuthorizationObjectProvider : IAuthorizationObjectProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "view_AuthorizationObject";

        #region 构造函数:AuthorizationObjectProvider()
        /// <summary>构造函数</summary>
        public AuthorizationObjectProvider()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 事务支持
        // -------------------------------------------------------

        #region 函数:BeginTransaction()
        /// <summary>启动事务</summary>
        public void BeginTransaction()
        {
            this.ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 函数:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>启动事务</summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 函数:CommitTransaction()
        /// <summary>提交事务</summary>
        public void CommitTransaction()
        {
            this.ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 函数:RollBackTransaction()
        /// <summary>回滚事务</summary>
        public void RollBackTransaction()
        {
            this.ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:Filter(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>查询授权对象信息</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        public DataTable Filter(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" AccountLoginName, AuthorizationObjectType "));

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);

            args.Add("RowCount", 0);

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_Filter", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_FilterRowCount", tableName)), args));

            return table;
        }
        #endregion

        #region 函数:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">授权对象类型</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        {
            return this.HasAuthority(scopeTableName, entityId, entityClassName, authorityName, "Account", account.Id);
        }
        #endregion

        #region 函数:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ScopeTableName", scopeTableName);
            args.Add("EntityId", entityId);
            args.Add("EntityClassName", entityClassName);
            args.Add("AuthorityId", authority.Id);
            args.Add("AuthorizationObjectType", authorizationObjectType);
            args.Add("AuthorizationObjectId", authorizationObjectId);

            if (authorizationObjectType == "Account")
            {
                return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_HasAuthorityWithAccount", tableName)), args) == 0) ? false : true;
            }
            else
            {
                return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_HasAuthority", tableName)), args) == 0) ? false : true;
            }
        }
        #endregion

        #region 函数:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">授权对象类型</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        {
            return this.HasAuthority(command, scopeTableName, entityId, entityClassName, authorityName, "Account", account.Id);
        }
        #endregion

        #region 函数:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ScopeTableName", scopeTableName);
            args.Add("EntityId", entityId);
            args.Add("EntityClassName", entityClassName);
            args.Add("AuthorityId", authority.Id);
            args.Add("AuthorizationObjectType", authorizationObjectType);
            args.Add("AuthorizationObjectId", authorizationObjectId);

            string commandText = null;

            if (authorizationObjectType == "Account")
            {
                commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_HasAuthorityWithAccount", tableName)), args);
            }
            else
            {
                commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_HasAuthorityWithAccount", tableName)), args);
            }

            return ((int)command.ExecuteScalar(commandText) == 0) ? false : true;
        }
        #endregion

        #region 函数:AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>移除实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ScopeTableName", scopeTableName);
            args.Add("EntityId", entityId);
            args.Add("EntityClassName", entityClassName);
            args.Add("AuthorityId", authority.Id);
            args.Add("AuthorizationObjectType", string.Empty);
            args.Add("AuthorizationObjectId", string.Empty);

            string[] list = scopeText.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in list)
            {
                string[] keys = item.Split('#');

                args["AuthorizationObjectType"] = keys[0].ToString().Substring(0, 1).ToUpper() + keys[0].ToString().Substring(1);
                args["AuthorizationObjectId"] = keys[1].ToString();

                if (!string.IsNullOrEmpty(args["AuthorizationObjectType"].ToString()) && !string.IsNullOrEmpty(args["AuthorizationObjectId"].ToString()))
                {
                    this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_AddAuthorizationScopeObject", tableName)), args);
                }
            }
        }
        #endregion

        #region 函数:AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>移除实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ScopeTableName", scopeTableName);
            args.Add("EntityId", entityId);
            args.Add("EntityClassName", entityClassName);
            args.Add("AuthorityId", authority.Id);
            args.Add("AuthorizationObjectType", string.Empty);
            args.Add("AuthorizationObjectId", string.Empty);

            string[] list = scopeText.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in list)
            {
                string[] keys = item.Split('#');

                args["AuthorizationObjectType"] = keys[0].ToString().Substring(0, 1).ToUpper() + keys[0].ToString().Substring(1);
                args["AuthorizationObjectId"] = keys[1].ToString();

                if (!string.IsNullOrEmpty(args["AuthorizationObjectType"].ToString()) && !string.IsNullOrEmpty(args["AuthorizationObjectId"].ToString()))
                {
                    string commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_AddAuthorizationScopeObject", tableName)), args);

                    command.ExecuteNonQuery(commandText);
                }
            }
        }
        #endregion

        #region 函数:RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>移除实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        public void RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ScopeTableName", scopeTableName);
            args.Add("EntityId", entityId);
            args.Add("EntityClassName", entityClassName);
            args.Add("AuthorityId", authority.Id);

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveAuthorizationScopeObjects", tableName)), args);
        }
        #endregion

        #region 函数:RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>移除实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        public void RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ScopeTableName", scopeTableName);
            args.Add("EntityId", entityId);
            args.Add("EntityClassName", entityClassName);
            args.Add("AuthorityId", authority.Id);

            string commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveAuthorizationScopeObjects", tableName)), args);

            command.ExecuteNonQuery(commandText);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>配置实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            if (!string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(authorityName))
            {
                // 移除权限信息
                this.RemoveAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);

                // 添加权限信息
                this.AddAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName, scopeText);
            }
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>配置实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            if (!string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(authorityName))
            {
                // 移除权限信息
                this.RemoveAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName);

                // 添加权限信息
                this.AddAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName, scopeText);
            }
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询应用的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            if (string.IsNullOrEmpty(entityId))
            {
                new Exception("实体对象的标识不允许为空。");
            }

            string scopeText = null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ScopeTableName", scopeTableName);
            args.Add("EntityId", entityId);
            args.Add("EntityClassName", entityClassName);
            args.Add("AuthorityId", authority.Id);

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetAuthorizationScopeObjects", tableName)), args);

            foreach (DataRow row in table.Rows)
            {
                scopeText += row["AuthorizationObjectType"] + "#" + row["AuthorizationObjectId"] + "#" + row["AuthorizationObjectName"] + ";";
            }

            IList<MembershipAuthorizationScopeObject> list = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjects(scopeText);

            return list;
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询应用的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            if (string.IsNullOrEmpty(entityId))
            {
                new Exception("实体对象的标识不允许为空。");
            }

            string scopeText = null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            AuthorityInfo authority = AuthorityContext.Instance.AuthorityService[authorityName];

            args.Add("ScopeTableName", scopeTableName);
            args.Add("EntityId", entityId);
            args.Add("EntityClassName", entityClassName);
            args.Add("AuthorityId", authority.Id);

            string commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_GetAuthorizationScopeObjects", tableName)), args);

            DataTable table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                scopeText += row["AuthorizationObjectType"] + "#" + row["AuthorizationObjectId"] + "#" + row["AuthorizationObjectName"] + ";";
            }

            IList<MembershipAuthorizationScopeObject> list = MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjects(scopeText);

            return list;
        }
        #endregion

        #region 函数:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        /// <summary>获取实体对象标识SQL语句</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="accountId">用户标识</param>
        /// <param name="contactType">联系人对象</param>
        /// <param name="authorityIds">权限标识</param>
        /// <param name="entityIdDataColumnName">实体类标识的数据列名</param>
        /// <param name="entityClassNameDataColumnName">实体类名称的数据列名</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns></returns>
        public string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        {
            StringBuilder outString = new StringBuilder();

            string[] authorities = authorityIds.Split(new char[] { ',' });

            string prefixWhereText = string.Empty;

            if (string.IsNullOrEmpty(entityIdDataColumnName))
            {
                outString.AppendFormat("SELECT DISTINCT {0}.EntityId FROM {0} INNER JOIN (\r\n", scopeTableName);
            }
            else
            {
                outString.AppendFormat("SELECT {0}.{1} AS EntityId FROM {0} INNER JOIN (\r\n", scopeTableName, entityIdDataColumnName);
            }

            string dataTablePrefix = MembershipConfigurationView.Instance.DataTablePrefix;

            // 帐号【当前帐号】
            outString.AppendFormat("SELECT N##Account## AS AuthorizationObjectType, ##{0}## AS AuthorizationObjectId\r\n", accountId);

            // 帐号委托
            outString.AppendFormat("UNION SELECT N##Account## AS AuthorizationObjectType, GranteeId AS AuthorizationObjectId FROM {1}tb_Account_Grant WHERE GrantedTimeFrom < CURRENT_TIMESTAMP AND GrantedTimeTo > CURRENT_TIMESTAMP AND GrantorId = ##{0}##\r\n", accountId, dataTablePrefix);

            // 组织
            if ((contactType & ContactType.OrganizationUnit) == ContactType.OrganizationUnit)
            {
                outString.AppendFormat("UNION SELECT N##OrganizationUnit## AS AuthorizationObjectType, OrganizationUnitId AS AuthorizationObjectId FROM {1}tb_Account_OrganizationUnit WHERE AccountId = ##{0}##\r\n", accountId, dataTablePrefix);
            }

            // 角色
            if ((contactType & ContactType.Role) == ContactType.Role)
            {
                outString.AppendFormat("UNION SELECT N##Role## AS AuthorizationObjectType, RoleId AS AuthorizationObjectId FROM {1}tb_Account_Role WHERE AccountId = ##{0}##\r\n", accountId, dataTablePrefix);
            }

            // 群组
            if ((contactType & ContactType.Group) == ContactType.Group)
            {
                outString.AppendFormat("UNION SELECT N##Group## AS AuthorizationObjectType, GroupId AS AuthorizationObjectId FROM {1}tb_Account_Group WHERE AccountId = ##{0}##\r\n", accountId, dataTablePrefix);
            }

            //  通用角色
            if ((contactType & ContactType.StandardRole) == ContactType.StandardRole)
            {
                outString.AppendFormat("UNION SELECT N##GeneralRole## AS AuthorizationObjectType, GeneralRoleId AS AuthorizationObjectId FROM {1}tb_Account_Role INNER JOIN {1}[tb_Role] ON [tb_Role].[Id] = [tb_Account_Role].RoleId AND AccountId = ##{0}##\r\n", accountId, dataTablePrefix);
            }

            //  标准组织
            if ((contactType & ContactType.StandardOrganizationUnit) == ContactType.StandardOrganizationUnit)
            {
                outString.AppendFormat("UNION SELECT N##StandardOrganizationUnit## AS AuthorizationObjectType, StandardOrganizationUnitId AS AuthorizationObjectId FROM {1}tb_StandardRole INNER JOIN {1}[tb_Role] ON [tb_StandardRole].Id = [tb_Role].StandardRoleId INNER JOIN {1}[tb_Account_Role] ON [tb_Role].Id = [tb_Account_Role].RoleId AND AccountId = ##{0}##\r\n", accountId, dataTablePrefix);
            }

            //  标准角色
            if ((contactType & ContactType.StandardRole) == ContactType.StandardRole)
            {
                outString.AppendFormat("UNION SELECT N##StandardRole## AS AuthorizationObjectType, StandardRoleId AS AuthorizationObjectId FROM {1}tb_Role INNER JOIN {1}tb_Account_Role ON tb_Role.Id = tb_Account_Role.RoleId AND AccountId = ##{0}##\r\n", accountId, dataTablePrefix);
            }

            // [临时补丁]【标准角色】
            // 等请假出差开发完成后取消此代码
            // outString.AppendFormat("UNION SELECT N##StandardRole## AS AuthorizationObjectType, StandardRoleId AS AuthorizationObjectId FROM {1}[tb_Role] INNER JOIN {1}[tb_Account_Role] ON [tb_Role].[Id] = [tb_Account_Role].[RoleId] AND AccountId = ##{0}##\r\n", accountId, dataTablePrefix);

            // 所有人【特殊角色】
            outString.Append("UNION SELECT N##Role## AS AuthorizationObjectType, N##00000000-0000-0000-0000-000000000000##\r\n");

            outString.AppendFormat(") AS AuthorizationObject ON {0}.AuthorityId IN (##{1}##) ", scopeTableName, authorityIds.Replace(",", "##,##"));
            outString.AppendFormat("AND {0}.AuthorizationObjectId = AuthorizationObject.AuthorizationObjectId AND {0}.AuthorizationObjectType = AuthorizationObject.AuthorizationObjectType", scopeTableName);

            return outString.ToString();
        }
        #endregion
    }
}