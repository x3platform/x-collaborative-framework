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

namespace X3Platform.Membership.DAL.MySQL
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

    [DataObject]
    public class AuthorizationObjectProvider : IAuthorizationObjectProvider
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "view_AuthorizationObject";

        #region ���캯��:AuthorizationObjectProvider()
        /// <summary>���캯��</summary>
        public AuthorizationObjectProvider()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

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
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:Filter(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ѯ��Ȩ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns>
        public DataTable Filter(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " AccountLoginName, AuthorizationObjectType " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_Filter", tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_FilterRowCount", tableName)), args);

            return table;
        }
        #endregion

        #region 属性:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>�ж���Ȩ�����Ƿ�ӵ��ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="account">��Ȩ��������</param>
        /// <returns>����ֵ</returns>
        public bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        {
            return this.HasAuthority(scopeTableName, entityId, entityClassName, authorityName, "Account", account.Id);
        }
        #endregion

        #region 属性:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>�ж���Ȩ�����Ƿ�ӵ��ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <returns>����ֵ</returns>
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

        #region 属性:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>�ж���Ȩ�����Ƿ�ӵ��ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="account">��Ȩ��������</param>
        /// <returns>����ֵ</returns>
        public bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        {
            return this.HasAuthority(command, scopeTableName, entityId, entityClassName, authorityName, "Account", account.Id);
        }
        #endregion

        #region 属性:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>�ж���Ȩ�����Ƿ�ӵ��ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <returns>����ֵ</returns>
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

        #region 属性:AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>�Ƴ�ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
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

        #region 属性:AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>�Ƴ�ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
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

        #region 属性:RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>�Ƴ�ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
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

        #region 属性:RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>�Ƴ�ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
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

        #region 属性:BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>����ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
        public void BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            if (!string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(authorityName))
            {
                // �Ƴ�Ȩ����Ϣ
                this.RemoveAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);

                // ����Ȩ����Ϣ
                this.AddAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName, scopeText);
            }
        }
        #endregion

        #region 属性:BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>����ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
        public void BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            if (!string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(authorityName))
            {
                // �Ƴ�Ȩ����Ϣ
                this.RemoveAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName);

                // ����Ȩ����Ϣ
                this.AddAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName, scopeText);
            }
        }
        #endregion

        #region 属性:GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>��ѯӦ�õ�Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            if (string.IsNullOrEmpty(entityId))
            {
                new Exception("ʵ�������ı�ʶ������Ϊ�ա�");
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

        #region 属性:GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>��ѯӦ�õ�Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            if (string.IsNullOrEmpty(entityId))
            {
                new Exception("ʵ�������ı�ʶ������Ϊ�ա�");
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

        #region 属性:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        /// <summary>��ȡʵ��������ʶSQL����</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="accountId">�û���ʶ</param>
        /// <param name="contactType">��ϵ�˶���</param>
        /// <param name="authorityIds">Ȩ�ޱ�ʶ</param>
        /// <param name="entityIdDataColumnName">ʵ������ʶ����������</param>
        /// <param name="entityClassNameDataColumnName">ʵ�������Ƶ���������</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns></returns>
        public string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        {
            StringBuilder outString = new StringBuilder();

            string[] authorities = authorityIds.Split(new char[] { ',' });

            string prefixWhereText = string.Empty;

            if (string.IsNullOrEmpty(entityIdDataColumnName))
            {
                outString.AppendFormat("SELECT DISTINCT {0}.`EntityId` FROM {0} INNER JOIN (\r\n", scopeTableName);
            }
            else
            {
                outString.AppendFormat("SELECT {0}.`{1}` AS `EntityId` FROM {0} INNER JOIN (\r\n", scopeTableName, entityIdDataColumnName);
            }

            string PrefixAuthorizationScopeEntitySQLTable = this.configuration.Keys["PrefixAuthorizationScopeEntitySQLTable"].Value;

            // �ʺš���ǰ�ʺš�
            outString.AppendFormat("SELECT ##Account## AS AuthorizationObjectType, ##{0}## AS AuthorizationObjectId\r\n", accountId);

            // �ʺ�ί��
            outString.AppendFormat("UNION SELECT ##Account## AS AuthorizationObjectType, GranteeId AS AuthorizationObjectId FROM {1}`tb_Account_Grant` WHERE GrantedTimeFrom < Now() AND GrantedTimeTo > Now() AND GrantorId = ##{0}##\r\n", accountId, PrefixAuthorizationScopeEntitySQLTable);

            // ��֯
            if ((contactType & ContactType.Organization) == ContactType.Organization)
            {
                outString.AppendFormat("UNION SELECT ##Organization## AS AuthorizationObjectType, OrganizationId AS AuthorizationObjectId FROM {1}`tb_Account_Organization` WHERE AccountId = ##{0}##\r\n", accountId, PrefixAuthorizationScopeEntitySQLTable);
            }

            // ��ɫ
            if ((contactType & ContactType.Role) == ContactType.Role)
            {
                outString.AppendFormat("UNION SELECT ##Role## AS AuthorizationObjectType, RoleId AS AuthorizationObjectId FROM {1}`tb_Account_Role` WHERE AccountId = ##{0}##\r\n", accountId, PrefixAuthorizationScopeEntitySQLTable);
            }

            // Ⱥ��
            if ((contactType & ContactType.Group) == ContactType.Group)
            {
                outString.AppendFormat("UNION SELECT ##Group## AS AuthorizationObjectType, GroupId AS AuthorizationObjectId FROM {1}`tb_Account_Group` WHERE AccountId = ##{0}##\r\n", accountId, PrefixAuthorizationScopeEntitySQLTable);
            }

            //  ͨ�ý�ɫ
            if ((contactType & ContactType.StandardRole) == ContactType.StandardRole)
            {
                outString.AppendFormat("UNION SELECT ##GeneralRole## AS AuthorizationObjectType, GeneralRoleId AS AuthorizationObjectId FROM {1}`tb_Account_Role` INNER JOIN {1}`tb_Role` ON `tb_Role`.`Id` = `tb_Account_Role`.RoleId AND AccountId = ##{0}##\r\n", accountId, PrefixAuthorizationScopeEntitySQLTable);
            }

            //  ��׼��֯
            if ((contactType & ContactType.StandardOrganization) == ContactType.StandardOrganization)
            {
                outString.AppendFormat("UNION SELECT ##StandardOrganization## AS AuthorizationObjectType, StandardOrganizationId AS AuthorizationObjectId FROM {1}`tb_StandardRole` INNER JOIN {1}`tb_Role` ON `tb_StandardRole`.Id = `tb_Role`.StandardRoleId INNER JOIN {1}`tb_Account_Role` ON `tb_Role`.Id = `tb_Account_Role`.RoleId AND AccountId = ##{0}##\r\n", accountId, PrefixAuthorizationScopeEntitySQLTable);
            }

            //  ��׼��ɫ
            if ((contactType & ContactType.StandardRole) == ContactType.StandardRole)
            {
                outString.AppendFormat("UNION SELECT ##StandardRole## AS AuthorizationObjectType, StandardRoleId AS AuthorizationObjectId FROM {1}`tb_Role` INNER JOIN {1}`tb_Account_Role` ON `tb_Role`.`Id` = `tb_Account_Role`.`RoleId` AND AccountId = ##{0}##\r\n", accountId, PrefixAuthorizationScopeEntitySQLTable);
            }

            // `��ʱ����`����׼��ɫ��
            // �����ٳ�������ɺ�ȡ���˴���
            outString.AppendFormat("UNION SELECT ##StandardRole## AS AuthorizationObjectType, StandardRoleId AS AuthorizationObjectId FROM {1}`tb_Role` INNER JOIN {1}`tb_Account_Role` ON `tb_Role`.`Id` = `tb_Account_Role`.`RoleId` AND AccountId = ##{0}##\r\n", accountId, PrefixAuthorizationScopeEntitySQLTable);

            // �����ˡ�������ɫ��
            outString.Append("UNION SELECT ##Role## AS AuthorizationObjectType, ##00000000-0000-0000-0000-000000000000##\r\n");

            outString.AppendFormat(") AS AuthorizationObject ON {0}.AuthorityId IN (##{1}##) ", scopeTableName, authorityIds.Replace(",", "##,##"));
            outString.AppendFormat("AND {0}.AuthorizationObjectId = AuthorizationObject.AuthorizationObjectId AND {0}.AuthorizationObjectType = AuthorizationObject.AuthorizationObjectType", scopeTableName);

            return outString.ToString();
        }
        #endregion
    }
}