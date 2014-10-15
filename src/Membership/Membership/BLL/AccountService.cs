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

namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.ActiveDirectory;
    using X3Platform.ActiveDirectory.Configuration;
    using X3Platform.Collections;
    using X3Platform.Configuration;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Scope;
    using X3Platform.Membership.Model;

    /// <summary>�ʺŷ���</summary>
    public class AccountService : IAccountService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IAccountProvider provider = null;

        /// <summary>�����洢</summary>
        private IDictionary<string, IDictionary<string, IAccountInfo>> dictionary
            = new Dictionary<string, IDictionary<string, IAccountInfo>>() { 
                { "id", new SyncDictionary<string, IAccountInfo>() },
                { "loginName", new SyncDictionary<string, IAccountInfo>() }
            };

        #region ���캯��:AccountService()
        /// <summary>���캯��</summary>
        public AccountService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IAccountProvider>(typeof(IAccountProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id">�ʺű�ʶ</param>
        /// <returns></returns>
        public IAccountInfo this[string id]
        {
            get { return this.provider.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ��������
        // -------------------------------------------------------

        /// <summary></summary>
        /// <returns></returns>
        public int CreateCache()
        {
            this.dictionary = new Dictionary<string, IDictionary<string, IAccountInfo>>() { 
                { "id", new SyncDictionary<string, IAccountInfo>() },
                { "loginName", new SyncDictionary<string, IAccountInfo>() }
            };

            return 0;
        }

        /// <summary></summary>
        /// <returns></returns>
        public int ClearCache()
        {
            this.dictionary["id"].Clear();
            this.dictionary["loginName"].Clear();

            return 0;
        }

        /// <summary></summary>
        /// <returns></returns>
        public void AddCacheItem(object item)
        {
            if (item is IAccountInfo)
            {
                this.AddCacheItem((IAccountInfo)item);
            }
        }

        private void AddCacheItem(IAccountInfo item)
        {
            if (!string.IsNullOrEmpty(item.Id))
            {
                if (this.dictionary["id"].ContainsKey(item.Id))
                {
                    this.dictionary["id"].Add(item.Id, item);
                }
                else
                {
                    this.dictionary["id"][item.Id] = item;
                }
            }

            if (!string.IsNullOrEmpty(item.LoginName))
            {
                if (this.dictionary["loginName"].ContainsKey(item.LoginName))
                {
                    this.dictionary["loginName"].Add(item.LoginName, item);
                }
                else
                {
                    this.dictionary["loginName"][item.LoginName] = item;
                }
            }
        }

        /// <summary></summary>
        /// <returns></returns>
        public void RemoveCacheItem(object item)
        {
            if (item is IAccountInfo)
            {
                this.RemoveCacheItem((IAccountInfo)item);
            }
        }

        private void RemoveCacheItem(IAccountInfo item)
        {
            if (!string.IsNullOrEmpty(item.Id))
            {
                if (this.dictionary["id"].ContainsKey(item.Id))
                {
                    this.dictionary["id"].Remove(item.Id);
                }
            }

            if (!string.IsNullOrEmpty(item.LoginName))
            {
                if (this.dictionary["loginName"].ContainsKey(item.LoginName))
                {
                    this.dictionary["loginName"].Remove(item.LoginName);
                }
            }
        }

        // -------------------------------------------------------
        // ���� ɾ�� �޸�
        // -------------------------------------------------------

        #region 属性:Save(AccountInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">IAccountInfo ʵ����ϸ��Ϣ</param>
        /// <returns>IAccountInfo ʵ����ϸ��Ϣ</returns>
        public IAccountInfo Save(IAccountInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo originalObject = this.FindOne(param.Id);

                if (originalObject == null) { originalObject = param; }

                this.SyncToActiveDirectory(param, originalObject.GlobalName, originalObject.Status);
            }

            param.DistinguishedName = CombineDistinguishedName(param.Name);

            param = this.provider.Save(param);

            if (param != null)
            {
                string accountId = param.Id;

                // �����µĹ�ϵ
                if (!string.IsNullOrEmpty(accountId))
                {
                    // -------------------------------------------------------
                    // ���ý�ɫ��ϵ
                    // -------------------------------------------------------

                    // 1.�Ƴ���Ĭ�Ͻ�ɫ��ϵ
                    MembershipManagement.Instance.RoleService.RemoveNondefaultRelation(accountId);

                    // -------------------------------------------------------
                    // ���ݽ�ɫ������֯��ϵ
                    // -------------------------------------------------------

                    // 1.�Ƴ���Ĭ�Ͻ�ɫ��ϵ
                    MembershipManagement.Instance.OrganizationService.RemoveNondefaultRelation(accountId);

                    // 2.�����µĹ�ϵ
                    foreach (IAccountRoleRelationInfo item in param.RoleRelations)
                    {
                        MembershipManagement.Instance.RoleService.AddRelation(accountId, item.RoleId);

                        MembershipManagement.Instance.OrganizationService.AddRelation(accountId, item.GetRole().OrganizationId);

                        MembershipManagement.Instance.OrganizationService.AddParentRelations(accountId, item.GetRole().OrganizationId);
                    }

                    // -------------------------------------------------------
                    // ����Ⱥ����ϵ
                    // -------------------------------------------------------

                    // 1.�Ƴ�Ⱥ����ϵ
                    MembershipManagement.Instance.GroupService.RemoveAllRelation(accountId);

                    // 2.�����µĹ�ϵ
                    foreach (IAccountGroupRelationInfo item in param.GroupRelations)
                    {
                        MembershipManagement.Instance.GroupService.AddRelation(accountId, item.GroupId);
                    }
                }
            }

            return param;
        }
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">�ʺű�ʶ</param>
        public void Delete(string id)
        {
            IAccountInfo originalObject = this.FindOne(id);

            // ɾ������
            if (originalObject != null)
            {
                this.RemoveCacheItem(originalObject);
            }

            // ɾ�����ݿ���¼
            this.provider.Delete(id);
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
            IAccountInfo param = null;

            // ���һ�������
            if (this.dictionary["id"].ContainsKey(id))
            {
                param = this.dictionary["id"][id];
            }

            // ����������δ�ҵ��������ݣ����������ݿ�����
            if (param == null)
            {
                param = this.provider.FindOne(id);

                if (param != null)
                {
                    this.AddCacheItem(param);
                }
            }

            return param;
        }
        #endregion

        #region 属性:FindOneByGlobalName(string globalName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="globalName">�ʺŵ�ȫ������</param>
        /// <returns>����һ��<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IAccountInfo FindOneByGlobalName(string globalName)
        {
            return this.provider.FindOneByGlobalName(globalName);
        }
        #endregion

        #region 属性:FindOneByLoginName(string loginName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="loginName">��½��</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IAccountInfo FindOneByLoginName(string loginName)
        {
            IAccountInfo param = null;

            // ���һ�������
            if (this.dictionary["loginName"].ContainsKey(loginName))
            {
                param = this.dictionary["loginName"][loginName];
            }

            // ����������δ�ҵ��������ݣ����������ݿ�����
            if (param == null)
            {
                param = this.provider.FindOneByLoginName(loginName);

                if (param != null)
                {
                    this.AddCacheItem(param);
                }
            }

            return param;
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� IAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByOrganizationId(string organizationId)
        /// <summary>��ѯĳ���û����ڵ�������֯��λ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns>����һ�� IIAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByOrganizationId(string organizationId)
        {
            return this.provider.FindAllByOrganizationId(organizationId);
        }
        #endregion

        #region 属性:FindAllByOrganizationId(string organizationId, bool defaultOrganizationRelation)
        /// <summary>��ѯĳ����֯�µ����������ʺ�</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="defaultOrganizationRelation">Ĭ����֯��ϵ</param>
        /// <returns>����һ�� IIAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByOrganizationId(string organizationId, bool defaultOrganizationRelation)
        {
            return this.provider.FindAllByOrganizationId(organizationId, defaultOrganizationRelation);
        }
        #endregion

        #region 属性:FindAllByRoleId(string roleId)
        /// <summary>��ѯĳ����ɫ�µ����������ʺ�</summary>
        /// <param name="roleId">��ɫ��ʶ</param>
        /// <returns>����һ�� IIAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByRoleId(string roleId)
        {
            return this.provider.FindAllByRoleId(roleId);
        }
        #endregion

        #region 属性:FindAllByGroupId(string groupId)
        /// <summary>��ѯĳ��Ⱥ���µ����������ʺ�</summary>
        /// <param name="groupId">Ⱥ����ʶ</param>
        /// <returns>����һ�� IIAccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllByGroupId(string groupId)
        {
            return this.provider.FindAllByGroupId(groupId);
        }
        #endregion

        #region 属性:FindAllWithoutMemberInfo(int length)
        /// <summary>��������û�г�Ա��Ϣ���ʺ���Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindAllWithoutMemberInfo(int length)
        {
            return this.provider.FindAllWithoutMemberInfo(length);
        }
        #endregion

        #region 属性:FindForwardLeaderAccountsByOrganizationId(string organizationId)
        /// <summary>�������������쵼���ʺ���Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationId(string organizationId)
        {
            return this.provider.FindForwardLeaderAccountsByOrganizationId(organizationId, 1);
        }
        #endregion

        #region 属性:FindForwardLeaderAccountsByOrganizationId(string organizationId, int level)
        /// <summary>�������������쵼���ʺ���Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationId(string organizationId, int level)
        {
            return this.provider.FindForwardLeaderAccountsByOrganizationId(organizationId, level);
        }
        #endregion

        #region 属性:FindBackwardLeaderAccountsByOrganizationId(string organizationId)
        /// <summary>�������з����쵼���ʺ���Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationId(string organizationId)
        {
            return this.provider.FindBackwardLeaderAccountsByOrganizationId(organizationId, 1);
        }
        #endregion

        #region 属性:FindBackwardLeaderAccountsByOrganizationId(string organizationId, int level)
        /// <summary>�������з����쵼���ʺ���Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationId(string organizationId, int level)
        {
            return this.provider.FindBackwardLeaderAccountsByOrganizationId(organizationId, level);
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
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out  rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>�����Ƿ��������صļ�¼.</summary>
        /// <param name="id">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistLoginNameAndGlobalName(string loginName, string globalName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="loginName">��¼��</param>
        /// <param name="globalName">����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistLoginNameAndGlobalName(string loginName, string globalName)
        {
            bool result = this.provider.IsExistLoginNameAndGlobalName(loginName, globalName);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("LoginName", loginName));

                if (!result)
                {
                    result = Convert.ToBoolean(IsExistFieldValue("GlobalName", globalName));
                }
            }

            return result;
        }
        #endregion

        #region 属性:IsExistLoginName(string loginName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="loginName">��¼��</param>
        /// <returns>����ֵ</returns>
        public bool IsExistLoginName(string loginName)
        {
            bool result = this.provider.IsExistLoginName(loginName);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("LoginName", loginName));
            }

            return result;
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="name">����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            bool result = this.provider.IsExistName(name);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("Name", name));
            }

            return result;
        }
        #endregion

        #region 属性:IsExistGlobalName(string globalName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="globalName">��֯��λȫ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGlobalName(string globalName)
        {
            bool result = this.provider.IsExistGlobalName(globalName);

            if (!result)
            {
                result = Convert.ToBoolean(IsExistFieldValue("GlobalName", globalName));
            }

            return result;
        }
        #endregion

        #region 属性:IsExistFieldValue(string fieldName, string fieldValue)
        /// <summary>�����Ƿ��������ص��ֶε�ֵ</summary>
        /// <param name="fieldName">�ֶε�����</param>
        /// <param name="fieldValue">�ֶε�ֵ</param>
        public virtual string IsExistFieldValue(string fieldName, string fieldValue)
        {
            return "False";
        }
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">�ʺű�ʶ</param>
        /// <param name="name">�ʺ�����</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        public int Rename(string id, string name)
        {
            // �����Ƿ����ڶ���
            if (!IsExist(id))
            {
                // �����ڶ���
                return 1;
            }

            return this.provider.Rename(id, name);
        }
        #endregion

        #region 属性:CreateEmptyAccount(string accountId)
        /// <summary>�����յ��ʺ���Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns></returns>
        public IAccountInfo CreateEmptyAccount(string accountId)
        {
            AccountInfo param = new AccountInfo();

            param.Id = accountId;

            param.Lock = 0;

            param.Status = -1;

            param.UpdateDate = param.CreateDate = DateTime.Now;

            return param;
        }
        #endregion

        #region 属性:CombineDistinguishedName(string name)
        /// <summary>����Ψһ����</summary>
        /// <param name="name">�ʺű�ʶ</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string name)
        {
            //CN=${����},OU=��֯�û�,DC=lhwork,DC=net

            return string.Format("CN={0},OU={1}{2}", name,
                ActiveDirectoryConfigurationView.Instance.CorporationUserFolderRoot,
                ActiveDirectoryConfigurationView.Instance.SuffixDistinguishedName);
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
            if (IsExistGlobalName(globalName))
            {
                return 1;
            }

            // �����Ƿ����ڶ���
            if (!IsExist(accountId))
            {
                // ������${Id}�������ڡ�
                return 2;
            }

            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                // ͬ�� Active Directory �ʺ�ȫ������
                IAccountInfo account = FindOne(accountId);

                if (account != null && !string.IsNullOrEmpty(account.LoginName))
                {
                    // �����ⲿϵͳֱ��ͬ������Ա��Ȩ�޹��������ݿ��У�
                    // ���� Active Directory �ϲ���ֱ�Ӵ������ض�������Ҫ�ֹ�����ȫ�����Ʋ��������ض�����

                    if (ActiveDirectoryManagement.Instance.User.IsExistLoginName(account.LoginName))
                    {
                        ActiveDirectoryManagement.Instance.User.Rename(account.LoginName, globalName);
                    }
                    else
                    {
                        // ����δ���������ʺţ��򴴽������ʺš�
                        ActiveDirectoryManagement.Instance.User.Add(account.LoginName, globalName, string.Empty, string.Empty);

                        ActiveDirectoryManagement.Instance.User.SetStatus(account.LoginName, account.Status == 1 ? true : false);

                        // ActiveDirectory �����û����������������ڵĲ�����

                        ActiveDirectoryManagement.Instance.Group.AddRelation(account.LoginName, ActiveDirectorySchemaClassType.User, "������");
                    }
                }
            }

            return this.provider.SetGlobalName(accountId, globalName);
        }
        #endregion

        #region 属性:GetPassword(string loginName)
        /// <summary>��ȡ����</summary>
        /// <param name="loginName">�˺�</param>
        public string GetPassword(string loginName)
        {
            return MembershipManagement.Instance.PasswordEncryptionManagement.Encrypt(this.provider.GetPassword(loginName));
        }
        #endregion

        #region 属性:SetPassword(int accountId, string password)
        /// <summary>�����ʺ�����.(����Ա)</summary>
        /// <param name="accountId">����</param>
        /// <param name="password">����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �����벻ƥ��, ���� 1.</returns>
        public int SetPassword(string accountId, string password)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                // ͬ�� Active Directory �ʺ�״̬
                IAccountInfo account = FindOne(accountId);

                if (account != null && !string.IsNullOrEmpty(account.LoginName)
                    && !string.IsNullOrEmpty(password))
                {
                    ActiveDirectoryManagement.Instance.User.SetPassword(account.LoginName, password);
                }
            }

            return this.provider.SetPassword(accountId, password);
        }
        #endregion

        #region 属性:SetLoginName(string accountId, string loginName)
        /// <summary>���õ�¼��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="loginName">��¼��</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetLoginName(string accountId, string loginName)
        {
            return this.provider.SetLoginName(accountId, loginName);
        }
        #endregion

        #region 属性:SetCertifiedTelephone(string accountId, string telephone)
        /// <summary>��������֤����ϵ�绰</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="telephone">��ϵ�绰</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetCertifiedTelephone(string accountId, string telephone)
        {
            return this.provider.SetCertifiedTelephone(accountId, telephone);
        }
        #endregion

        #region 属性:SetCertifiedEmail(string accountId, string email)
        /// <summary>��������֤������</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="email">����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetCertifiedEmail(string accountId, string email)
        {
            return this.provider.SetCertifiedEmail(accountId, email);
        }
        #endregion

        #region 属性:SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        /// <summary>��������֤��ͷ��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="avatarVirtualPath">ͷ��������·��</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        {
            return this.provider.SetCertifiedAvatar(accountId, avatarVirtualPath);
        }
        #endregion

        #region 属性:SetExchangeStatus(string accountId, int status)
        /// <summary>������ҵ����״̬</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="status">״̬��ʶ, 1:����, 0:����</param>
        /// <returns>0 ���óɹ�, 1 ����ʧ��.</returns>
        public int SetExchangeStatus(string accountId, int status)
        {
            return this.provider.SetExchangeStatus(accountId, status);
        }
        #endregion

        #region 属性:SetStatus(string accountId, int status)
        /// <summary>�����ʺ�״̬</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="status">״̬��ʶ, 1:����, 0:����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetStatus(string accountId, int status)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                // ͬ�� Active Directory �ʺ�״̬
                IAccountInfo account = FindOne(accountId);

                if (account != null
                    && !string.IsNullOrEmpty(account.LoginName))
                {
                    ActiveDirectoryManagement.Instance.User.SetStatus(account.LoginName, status == 1 ? true : false);
                }
            }

            return this.provider.SetStatus(accountId, status);
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
            return this.provider.SetIPAndLoginDate(accountId, ip, loginDate);
        }
        #endregion

        // -------------------------------------------------------
        // ��ͨ�û�����
        // -------------------------------------------------------

        #region 属性:ApplyPasswordPolicy(string password)
        /// <summary>У�������Ƿ�������������</summary>
        /// <param name="password">����</param>
        /// <returns>0 ��ʾ�ɹ� 1</returns>
        public int ApplyPasswordPolicy(string password)
        {
            byte[] buffer = System.Text.Encoding.Default.GetBytes(password);

            string passwordPolicyRules = MembershipConfigurationView.Instance.PasswordPolicyRules;
            int passwordPolicyMinimumLength = MembershipConfigurationView.Instance.PasswordPolicyMinimumLength;
            int passwordPolicyCharacterRepeatedTimes = MembershipConfigurationView.Instance.PasswordPolicyCharacterRepeatedTimes;

            bool flag = false;
            int charCode = -1;

            if (passwordPolicyRules.IndexOf("[Number]") > -1)
            {
                flag = false;
                charCode = -1;

                // charCode 48 - 57
                for (var i = 0; i < buffer.Length; i++)
                {
                    charCode = buffer[i];

                    if (charCode >= 48 && charCode <= 57)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    // 2 ��������һ����0��9������
                    return 2;
                }
            }

            if (passwordPolicyRules.IndexOf("[Character]") > -1)
            {
                flag = false;
                charCode = -1;

                for (var i = 0; i < buffer.Length; i++)
                {
                    charCode = buffer[i];

                    if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    // 3 ��������һ����A��Z��a��z���ַ�);
                    return 3;
                }
            }

            if (passwordPolicyRules.IndexOf("[SpecialCharacter]") > -1)
            {
                flag = false;
                charCode = -1;

                // ! 33 # 35 $ 36 @ 64
                for (var i = 0; i < buffer.Length; i++)
                {
                    charCode = buffer[i];

                    if (charCode == 33 || charCode == 35 || charCode == 36 || charCode == 64)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    // 4 ��������һ����# $ @ !�������ַ�;
                    return 4;
                }
            }

            if (passwordPolicyMinimumLength > 0 && buffer.Length < passwordPolicyMinimumLength)
            {
                // 5 ���볤�ȱ������ڡ�' + passwordPolicyMinimumLength + '��');
                return 5;
            }

            if (passwordPolicyCharacterRepeatedTimes > 1 && buffer.Length > passwordPolicyCharacterRepeatedTimes)
            {
                // �ж��ַ��������ֵĴ���
                var repeatedTimes = 1;

                for (var i = 0; i < buffer.Length - passwordPolicyCharacterRepeatedTimes; i++)
                {
                    charCode = buffer[i];

                    repeatedTimes = 1;

                    for (var j = 1; j < passwordPolicyCharacterRepeatedTimes; j++)
                    {
                        if (charCode == buffer[i + j])
                        {
                            repeatedTimes++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (repeatedTimes >= passwordPolicyCharacterRepeatedTimes)
                    {
                        // '��' + newPassword.charAt(i) + '�����������ظ����γ��֣������ַ��ظ��������ܳ�����' + passwordPolicyCharacterRepeatedTimes + '���Ρ�;
                        return 6;
                    }
                }
            }

            return 0;
        }
        #endregion

        #region 属性:ConfirmPassword(string accountId, string passwordType, string password)
        /// <summary>ȷ������</summary>
        /// <param name="accountId">�ʺ�Ψһ��ʶ</param>
        /// <param name="passwordType">����属性: default Ĭ��, query ��ѯ����, trader ��������</param>
        /// <param name="password">����</param>
        /// <returns>����ֵ: 0 �ɹ� | 1 ʧ��</returns>
        public int ConfirmPassword(string accountId, string passwordType, string password)
        {
            return this.provider.ConfirmPassword(accountId, passwordType, password);
        }
        #endregion

        #region 属性:LoginCheck(string loginName, string password)
        /// <summary>��½����</summary>
        /// <param name="loginName">�ʺ�</param>
        /// <param name="password">����</param>
        /// <returns>IAccountInfo ʵ��</returns>
        public IAccountInfo LoginCheck(string loginName, string password)
        {
            return this.provider.LoginCheck(loginName, password);
        }
        #endregion

        #region 属性:ChangeBasicInfo(IAccount param)
        /// <summary>�޸Ļ�����Ϣ</summary>
        /// <param name="param">IAccount ʵ������ϸ��Ϣ</param>
        public void ChangeBasicInfo(IAccountInfo param)
        {
            this.provider.ChangeBasicInfo(param);
        }
        #endregion

        #region 属性:ChangePassword(string loginName, string newPassword, string originalPassword)
        /// <summary>�޸�����</summary>
        /// <param name="loginName">����</param>
        /// <param name="password">������</param>
        /// <param name="originalPassword">ԭʼ����</param>
        /// <returns>�����벻ƥ�䣬����0.</returns>
        public int ChangePassword(string loginName, string password, string originalPassword)
        {
            int result = this.provider.ChangePassword(loginName,
                            password,
                            originalPassword);

            if (result == 0 && ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                // ͬ�� Active Directory �ʺ�״̬
                IAccountInfo account = this.FindOneByLoginName(loginName);

                if (account != null && !string.IsNullOrEmpty(account.LoginName)
                    && !string.IsNullOrEmpty(password))
                {
                    ActiveDirectoryManagement.Instance.User.SetPassword(account.LoginName, password);
                }
            }

            return result;
        }
        #endregion

        #region 属性:RefreshUpdateDate(string accountId)
        /// <summary>ˢ���ʺŵĸ���ʱ��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <returns>0 ���óɹ�, 1 ����ʧ��.</returns>
        public int RefreshUpdateDate(string accountId)
        {
            return this.provider.RefreshUpdateDate(accountId);
        }
        #endregion

        #region 属性:GetAuthorizationScopeObjects(IAccount account)
        /// <summary>��ȡ�ʺ����ص�Ȩ�޶���</summary>
        /// <param name="account">IAccount ʵ������ϸ��Ϣ</param>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(IAccountInfo account)
        {
            return this.provider.GetAuthorizationScopeObjects(account);
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IAccountInfo param)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">�ʺ���Ϣ</param>
        public int SyncToActiveDirectory(IAccountInfo param)
        {
            return SyncToActiveDirectory(param, param.GlobalName, param.Status);
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IAccountInfo param, string originalGlobalName, int originalStatus)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">�ʺ���Ϣ</param>
        /// <param name="originalGlobalName">ԭʼȫ������</param>
        /// <param name="originalStatus">ԭʼ״̬</param>
        public int SyncToActiveDirectory(IAccountInfo param, string originalGlobalName, int originalStatus)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.LoginName))
                {
                    // �û���${Name}(${LoginName})����¼��Ϊ�գ�������������Ϣ��
                    return 1;
                }
                else if (string.IsNullOrEmpty(param.GlobalName))
                {
                    // �û���${Name}(${LoginName})��ȫ������Ϊ�գ�������������Ϣ��
                    return 2;
                }
                else
                {
                    // 1.ԭʼ��ȫ�����ƺ͵�¼������Ϊ�ա�
                    // 2.Active Directory �������ض�����
                    if (!(string.IsNullOrEmpty(originalGlobalName) || string.IsNullOrEmpty(param.LoginName))
                        && ActiveDirectoryManagement.Instance.User.IsExistLoginName(param.LoginName))
                    {
                        // �����Ѵ��������ʺţ�ͬ��ȫ�����ƺ��ʺ�״̬��

                        if (param.GlobalName != originalGlobalName)
                        {
                            ActiveDirectoryManagement.Instance.User.Rename(param.LoginName, param.GlobalName);
                        }

                        ActiveDirectoryManagement.Instance.User.SetStatus(param.LoginName, param.Status == 1 ? true : false);
                    }
                    else
                    {
                        if (ActiveDirectoryManagement.Instance.User.IsExist(param.LoginName, param.GlobalName))
                        {
                            // "�û���${Name}(${LoginName})����ȫ�������ѱ������˴������������������á�
                            return 3;
                        }
                        else if (param.Status == 0)
                        {
                            // "�û���${Name}(${LoginName})�����ʺ�Ϊ�����á�״̬��������Ҫ���� Active Directory �ʺţ��������������á�
                            return 4;
                        }
                        else
                        {
                            // ����δ���������ʺţ��򴴽������ʺš�
                            ActiveDirectoryManagement.Instance.User.Add(param.LoginName, param.GlobalName, string.Empty, string.Empty);

                            ActiveDirectoryManagement.Instance.User.SetStatus(param.LoginName, param.Status == 1 ? true : false);

                            // ActiveDirectory �����û����������������ڵĲ�����

                            ActiveDirectoryManagement.Instance.Group.AddRelation(param.LoginName, ActiveDirectorySchemaClassType.User, "������");

                            // "�û���${Name}(${LoginName})�������ɹ���
                            return 0;
                        }
                    }
                }
            }

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // ͬ������
        // -------------------------------------------------------

        #region 属性:SyncFromPackPage(IMemberInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">�ʺ���Ϣ</param>
        public int SyncFromPackPage(IAccountInfo param)
        {
            return this.provider.SyncFromPackPage(param);
        }
        #endregion
    }
}