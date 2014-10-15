#region Copyright & Author
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
#endregion

namespace X3Platform.Membership.DAL.MySQL
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Membership.Scope;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class AccountProvider : IAccountProvider
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Account";

        /// <summary></summary>
        public AccountProvider()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

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
        // ���� ɾ�� �޸�
        // -------------------------------------------------------

        #region 属性:Save(AccountInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">IAccountInfo ʵ����ϸ��Ϣ</param>
        /// <returns>IAccountInfo ʵ����ϸ��Ϣ</returns>
        public IAccountInfo Save(IAccountInfo param)
        {
            if (string.IsNullOrEmpty(param.Id) || !this.IsExist(param.Id))
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

        #region 属性:Insert(IAccountInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">IAccountInfo ʵ������ϸ��Ϣ</param>
        public void Insert(IAccountInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_Account_Key_Code");
            }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);

            MembershipManagement.Instance.MemberService.Save(new MemberInfo(param.Id, param.Id));
        }
        #endregion

        #region 属性:Update(AccountInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">IAccountInfo ʵ������ϸ��Ϣ</param>
        public void Update(IAccountInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);

            // ˢ�����ض�������ʱ��
            this.RefreshUpdateDate(param.Id);
        }
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">�ʺű�ʶ</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            id = StringHelper.ToSafeSQL(id, true);

            try
            {
                this.ibatisMapper.BeginTransaction();

                Dictionary<string, object> args = new Dictionary<string, object>();

                // ɾ���ʺŹ�ϵ��Ϣ
                args.Add("AccountId", id);

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountGroup", tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountRole", tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountAssignedJob", tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountOrganization", tableName)), args);

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountGrant", tableName)), args);
                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete_AccountLog", tableName)), args);

                // ɾ���ʺ���Ϣ
                args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

                this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

                this.ibatisMapper.CommitTransaction();
            }
            catch (DataException ex)
            {
                this.ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">AccountInfo Id��</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IAccountInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            IAccountInfo param = this.ibatisMapper.QueryForObject<IAccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 属性:FindOneByGlobalName(string globalName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="globalName">�ʺŵ�ȫ������</param>
        /// <returns>����һ��<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IAccountInfo FindOneByGlobalName(string globalName)
        {
            string whereClause = string.Format(" GlobalName = ##{0}## ", StringHelper.ToSafeSQL(globalName));

            IList<IAccountInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? null : list[0];
        }
        #endregion

        #region 属性:FindOneByLoginName(string loginName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="loginName">��½��</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IAccountInfo FindOneByLoginName(string loginName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", loginName);

            return this.ibatisMapper.QueryForObject<AccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByLoginName", tableName)), args);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAll(string whereClause, int length)
        {
            IList<IAccountInfo> results = new List<IAccountInfo>();

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<AccountInfo> list = this.ibatisMapper.QueryForList<AccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            foreach (IAccountInfo item in list)
            {
                results.Add(item);
            }

            return results;
        }
        #endregion

        #region 属性:FindAllByOrganizationId(string organizationId)
        /// <summary>��ѯĳ���û����ڵ�������֯��λ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByOrganizationId(string organizationId)
        {
            string whereClause = string.Format(" Id IN ( SELECT AccountId FROM tb_Account_Organization WHERE OrganizationId =  ##{0}## ) ", organizationId);

            return this.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAllByOrganizationId(string organizationId, bool defaultOrganizationRelation)
        /// <summary>��ѯĳ����֯�µ����������ʺ�</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="defaultOrganizationRelation">Ĭ����֯��ϵ</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByOrganizationId(string organizationId, bool defaultOrganizationRelation)
        {
            if (defaultOrganizationRelation)
            {
                string whereClause = string.Format(" Id IN ( SELECT AccountId FROM tb_Member WHERE OrganizationId = ##{0}## ) ", organizationId);

                return this.FindAll(whereClause, 0);
            }
            else
            {
                return this.FindAllByOrganizationId(organizationId);
            }
        }
        #endregion

        #region 属性:FindAllByRoleId(string roleId)
        /// <summary>��ѯĳ����ɫ�µ����������ʺ�</summary>
        /// <param name="roleId">��ɫ��ʶ</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByRoleId(string roleId)
        {
            string whereClause = string.Format(" Id IN ( SELECT AccountId FROM [tb_Account_Role] WHERE RoleId = ##{0}## ) ", roleId);

            return FindAll(whereClause, 0);
        }

        #endregion

        #region 属性:FindAllByGroupId(string groupId)
        /// <summary>��ѯĳ��Ⱥ���µ����������ʺ�</summary>
        /// <param name="groupId">Ⱥ����ʶ</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByGroupId(string groupId)
        {
            string whereClause = string.Format(" Id IN ( SELECT AccountId FROM [tb_Account_Group] WHERE GroupId = ##{0}##) ", groupId);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAllWithoutMemberInfo(int length)
        /// <summary>��������û�г�Ա��Ϣ���ʺ���Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllWithoutMemberInfo(int length)
        {
            string whereClause = " Id NOT IN ( SELECT AccountId FROM [tb_Member] ) ";

            return FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindForwardLeaderAccountsByOrganizationId(string organizationId, int level)
        /// <summary>�������������쵼���ʺ���Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationId(string organizationId, int level)
        {
            string whereClause = string.Format(@" 
Id IN ( SELECT AccountId FROM tb_Account_Role WHERE RoleId IN ( 
            SELECT Id FROM tb_Role WHERE
                OrganizationId = dbo.func_GetDepartmentIdByOrganizationId( ##{0}## , {1} ) 
                AND StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE Priority >= 40 )
)) ", organizationId, level);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindBackwardLeaderAccountsByOrganizationId(string organizationId, int level)
        /// <summary>�������з����쵼���ʺ���Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationId(string organizationId, int level)
        {
            string whereClause = string.Format(@" 
Id IN ( SELECT AccountId FROM tb_Account_Role WHERE Role IN ( 
            SELECT Id FROM tb_Role WHERE
                OrganizationId IN ( dbo.func_GetDepartmentIdByOrganizationId( ##{0}## , ( dbo.func_GetDepartmentLevelByOrganizationId( ##{0}## ) - {1} ) ) )
                AND StandardRoleId IN ( SELECT Id FROM tb_StandardRole WHERE Priority >= 40 )
)) ", organizationId, level);

            return FindAll(whereClause, 0);
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
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns>
        public IList<IAccountInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " OrderId DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<IAccountInfo> list = this.ibatisMapper.QueryForList<IAccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��¼��</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 属性:IsExistLoginNameAndGlobalName(string loginName, string nickName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="loginName">��¼��</param>
        /// <param name="name">����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistLoginNameAndGlobalName(string loginName, string name)
        {
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(name)) { throw new Exception("ʵ����¼������������Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" LoginName = '{0}' AND Name = '{0}' ", StringHelper.ToSafeSQL(loginName), StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 属性:IsExistLoginName(string loginName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="loginName">��¼��</param>
        /// <returns>����ֵ</returns>
        public bool IsExistLoginName(string loginName)
        {
            if (string.IsNullOrEmpty(loginName)) { throw new Exception("ʵ����¼������Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" LoginName = '{0}' ", StringHelper.ToSafeSQL(loginName)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="name">����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("ʵ����������Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 属性:IsExistGlobalName(string globalName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="globalName">��Աȫ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGlobalName(string globalName)
        {
            if (string.IsNullOrEmpty(globalName)) { throw new Exception("ʵ��ȫ�����Ʋ���Ϊ�ա�"); }

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" GlobalName = '{0}' ", StringHelper.ToSafeSQL(globalName)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">�ʺű�ʶ</param>
        /// <param name="name">�ʺ�����</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        public int Rename(string id, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("Name", StringHelper.ToSafeSQL(name));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Rename", tableName)), args);

            // ˢ�����ض�������ʱ��
            this.RefreshUpdateDate(id);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // ����Ա����
        // -------------------------------------------------------

        #region 属性:SetGlobalName(string accountId, string globalName)
        /// <summary>����ȫ������</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="globalName">ȫ������</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetGlobalName(string accountId, string globalName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("GlobalName", StringHelper.ToSafeSQL(globalName));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetGlobalName", tableName)), args);

            // ˢ�����ض�������ʱ��
            RefreshUpdateDate(accountId);

            return 0;
        }
        #endregion

        #region 属性:GetPassword(string loginName)
        /// <summary>��ȡ����(����Ա)</summary>
        /// <param name="loginName">�˺�</param>
        /// <returns>����</returns>
        public string GetPassword(string loginName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", StringHelper.ToSafeSQL(loginName));

            return this.ibatisMapper.QueryForText(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPassword", tableName)), args);
        }
        #endregion

        #region 属性:SetPassword(string accountId, string password)
        /// <summary>�����ʺ�����(����Ա)</summary>
        /// <param name="accountId">����</param>
        /// <param name="password">����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �����벻ƥ��, ���� 1.</returns>
        public int SetPassword(string accountId, string password)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("Password", StringHelper.ToSafeSQL(password));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetPassword", tableName)), args);

            return 0;
        }
        #endregion

        #region 属性:SetLoginName(string accountId, string loginName)
        /// <summary>���õ�¼��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="loginName">��¼��</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetLoginName(string accountId, string loginName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("LoginName", StringHelper.ToSafeSQL(loginName));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetLoginName", tableName)), args);

            // ˢ�����ض�������ʱ��
            this.RefreshUpdateDate(accountId);

            return 0;
        }
        #endregion

        #region 属性:SetCertifiedTelephone(string accountId, string telephone)
        /// <summary>��������֤����ϵ�绰</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="telephone">��ϵ�绰</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetCertifiedTelephone(string accountId, string telephone)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("CertifiedTelephone", StringHelper.ToSafeSQL(telephone));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetCertifiedTelephone", tableName)), args);

            // ˢ�����ض�������ʱ��
            this.RefreshUpdateDate(accountId);

            return 0;
        }
        #endregion

        #region 属性:SetCertifiedEmail(string accountId, string email)
        /// <summary>��������֤������</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="email">����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetCertifiedEmail(string accountId, string email)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("CertifiedEmail", StringHelper.ToSafeSQL(email));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetCertifiedEmail", tableName)), args);

            // ˢ�����ض�������ʱ��
            this.RefreshUpdateDate(accountId);

            return 0;
        }
        #endregion

        #region 属性:SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        /// <summary>��������֤��ͷ��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="avatarVirtualPath">ͷ��������·��</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("CertifiedAvatar", StringHelper.ToSafeSQL(avatarVirtualPath));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetCertifiedAvatar", tableName)), args);

            // ˢ�����ض�������ʱ��
            this.RefreshUpdateDate(accountId);

            return 0;
        }
        #endregion

        #region 属性:SetExchangeStatus(string accountId, int status)
        /// <summary>������ҵ����״̬</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="status">״̬��ʶ, 1:����, 0:����</param>
        /// <returns>0 ���óɹ�, 1 ����ʧ��.</returns>
        public int SetExchangeStatus(string accountId, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("EnableExchangeEmail", status);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetExchangeStatus", tableName)), args);

            // ˢ�����ض�������ʱ��
            RefreshUpdateDate(accountId);

            return 0;
        }
        #endregion

        #region 属性:SetStatus(string accountId, int status)
        /// <summary>����״̬</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="status">״̬��ʶ, 1:����, 0:����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetStatus(string accountId, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("Status", status);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetStatus", tableName)), args);

            // ˢ�����ض�������ʱ��
            RefreshUpdateDate(accountId);

            return 0;
        }
        #endregion

        #region 属性:SetIPAndLoginDate(string accountId, string ip, string loginDate)
        /// <summary>���õ�¼��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="ip">��¼IP</param>
        /// <param name="loginDate">��¼ʱ��</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetIPAndLoginDate(string accountId, string ip, string loginDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("IP", ip);
            args.Add("LoginDate", loginDate);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetIPAndLoginDate", tableName)), args);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // ��ͨ�û�����
        // -------------------------------------------------------

        #region 属性:ConfirmPassword(string accountId, string passwordType, string password)
        /// <summary>ȷ������</summary>
        /// <param name="accountId">�ʺ�Ψһ��ʶ</param>
        /// <param name="passwordType">����属性: default Ĭ��, query ��ѯ����, trader ��������</param>
        /// <param name="password">����</param>
        /// <returns>����ֵ: 0 �ɹ� | 1 ʧ��</returns>
        public int ConfirmPassword(string accountId, string passwordType, string password)
        {
            if (string.IsNullOrEmpty(password)) { throw new Exception("���벻��Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("PasswordType", StringHelper.ToSafeSQL(passwordType));
            args.Add("Password", StringHelper.ToSafeSQL(password));

            if (passwordType == "trader")
            {
                return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_ConfirmTraderPassword", tableName)), args)) == 0) ? 1 : 0;
            }
            else if (passwordType == "query")
            {
                return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_ConfirmQueryPassword", tableName)), args)) == 0) ? 1 : 0;
            }
            else
            {
                return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_ConfirmPassword", tableName)), args)) == 0) ? 1 : 0;
            }
        }
        #endregion

        #region 属性:LoginCheck(string loginName, string password)
        /// <summary>��½����</summary>
        /// <param name="loginName">�ʺ�</param>
        /// <param name="password">����</param>
        /// <returns>IAccountInfo ʵ��</returns>
        public IAccountInfo LoginCheck(string loginName, string password)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", loginName);
            args.Add("Password", password);

            IAccountInfo param = this.ibatisMapper.QueryForObject<IAccountInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_LoginCheck", tableName)), args);

            return param;
        }
        #endregion

        #region 属性:ChangeBasicInfo(IAccount param)
        /// <summary>�޸Ļ�����Ϣ</summary>
        /// <param name="param">IAccount ʵ������ϸ��Ϣ</param>
        public void ChangeBasicInfo(IAccountInfo param)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        #region 属性:ChangePassword(string loginName, string password, string originalPassword)
        /// <summary>�޸�����</summary>
        /// <param name="loginName">����</param>
        /// <param name="password">������</param>
        /// <param name="originalPassword">ԭʼ����</param>
        /// <returns>�����벻ƥ��,����1.</returns>
        public int ChangePassword(string loginName, string password, string originalPassword)
        {
            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("LoginName", loginName);
            args.Add("Password", originalPassword);

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_CheckPassword", tableName)), args) == 0) ? false : true;

            if (isExist)
            {
                args.Clear();

                args.Add("LoginName", loginName);
                args.Add("Password", password);

                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_ChangePassword", tableName)), args);

                return 0;
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region 属性:RefreshUpdateDate(string accountId)
        /// <summary>ˢ���ʺŵĸ���ʱ��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <returns>0 ���óɹ�, 1 ����ʧ��.</returns>
        public int RefreshUpdateDate(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Refresh_Table_Account", tableName)), args);

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Refresh_Table_Member", tableName)), args);

            return 0;
        }
        #endregion

        #region 属性:GetAuthorizationScopeObjects(IAccountInfo account)
        /// <summary>��ȡ�ʺ����ص�Ȩ�޶���</summary>
        /// <param name="account">IAccount ʵ������ϸ��Ϣ</param>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(IAccountInfo account)
        {
            string scopeText = null;

            IList<IAuthorizationScope> result = new List<IAuthorizationScope>();

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountId = '{0}' ", StringHelper.ToSafeSQL(account.Id)));

            DataTable table = this.ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_GetAuthorizationScopesByAccount", tableName)), args);

            foreach (DataRow row in table.Rows)
            {
                scopeText += row["AuthorizationObjectType"] + "#" + row["AuthorizationObjectId"] + "#" + row["AuthorizationObjectName"] + ";";
            }

            return MembershipAuthorizationScopeManagement.GetAuthorizationScopeObjects(scopeText);
        }
        #endregion

        #region 属性:SyncFromPackPage(MemberInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">�ʺ���Ϣ</param>
        public int SyncFromPackPage(IAccountInfo param)
        {
            // �˰汾ֻͬ���������ʺ�״̬����ͬ����¼����

            string accountId = param.Id;

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            if (param.RoleRelations.Count > 0)
            {
                // 1.����Ĭ�Ͻ�ɫ��Ϣ
                IMemberInfo member = MembershipManagement.Instance.MemberService.FindOne(param.Id);

                if (member.Role != null)
                {
                    IRoleInfo defaultRole = MembershipManagement.Instance.RoleService[member.Role.Id];

                    if (defaultRole != null)
                    {
                        MembershipManagement.Instance.RoleService.SetDefaultRelation(accountId, member.Role.Id);

                        MembershipManagement.Instance.OrganizationService.SetDefaultRelation(accountId, member.Role.OrganizationId);
                    }
                }

                // 2.�Ƴ���Ĭ�Ͻ�ɫ��ϵ
                MembershipManagement.Instance.RoleService.RemoveNondefaultRelation(accountId);

                // 3.�Ƴ���Ĭ����֯��ϵ
                MembershipManagement.Instance.OrganizationService.RemoveNondefaultRelation(accountId);

                // 4.�����µĹ�ϵ
                foreach (IAccountRoleRelationInfo item in param.RoleRelations)
                {
                    MembershipManagement.Instance.RoleService.AddRelation(accountId, item.RoleId);

                    // ���ݽ�ɫ������֯��ϵ

                    IRoleInfo role = MembershipManagement.Instance.RoleService.FindOne(item.RoleId);

                    // [�ݴ�]������ɫ��ϢΪ�գ���ֹ������֯����
                    if (role == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(role.OrganizationId))
                    {
                        MembershipManagement.Instance.OrganizationService.AddRelation(accountId, role.OrganizationId);

                        MembershipManagement.Instance.OrganizationService.AddParentRelations(accountId, role.OrganizationId);
                    }
                }

                // 5.�ٴ�����Ĭ�Ͻ�ɫ��Ϣ
                if (member.Role != null)
                {
                    MembershipManagement.Instance.RoleService.SetDefaultRelation(accountId, member.Role.Id);

                    MembershipManagement.Instance.OrganizationService.SetDefaultRelation(accountId, member.Role.OrganizationId);
                }

                //
                // ����Ⱥ����ϵ
                //

                // 1.�Ƴ�Ⱥ����ϵ
                MembershipManagement.Instance.GroupService.RemoveAllRelation(accountId);

                // 2.�����µĹ�ϵ
                foreach (IAccountGroupRelationInfo item in param.GroupRelations)
                {
                    MembershipManagement.Instance.GroupService.AddRelation(accountId, item.GroupId);
                }

            }

            return 0;
        }
        #endregion
    }
}