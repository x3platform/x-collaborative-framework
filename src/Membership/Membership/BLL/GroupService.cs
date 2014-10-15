// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :GroupService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.ActiveDirectory;
    using X3Platform.ActiveDirectory.Configuration;
    using X3Platform.Configuration;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Spring;
    using System.Text;
    using X3Platform.Membership.Model;

    /// <summary></summary>
    public class GroupService : IGroupService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IGroupProvider provider = null;

        #region ���캯��:GroupService()
        /// <summary>���캯��</summary>
        public GroupService()
              {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IGroupProvider>(typeof(IGroupProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IGroupInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IGroupInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IGroupInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IGroupInfo"/>��ϸ��Ϣ</returns>
        public IGroupInfo Save(IGroupInfo param)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IGroupInfo originalObject = FindOne(param.Id);

                if (originalObject == null)
                {
                    originalObject = param;
                }

                this.SyncToActiveDirectory(param, originalObject.GlobalName, originalObject.GroupTreeNodeId);
            }

            // ������֯ȫ·��
            param.FullPath = this.CombineFullPath(param.Name, param.GroupTreeNodeId);

            // ����Ψһʶ������
            param.DistinguishedName = this.CombineDistinguishedName(param.Name, param.GroupTreeNodeId);

            this.provider.Save(param);

            if (param != null)
            {
                string groupId = param.Id;

                // �����µĹ�ϵ
                if (!string.IsNullOrEmpty(groupId))
                {
                    // 1.�Ƴ���Ĭ�ϳ�Ա��ϵ
                    MembershipManagement.Instance.GroupService.ClearupRelation(groupId);

                    // 2.�����µĹ�ϵ
                    foreach (IAccountInfo item in param.Members)
                    {
                        MembershipManagement.Instance.GroupService.AddRelation(item.Id, groupId);
                    }
                }
            }

            return param;
        }
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IGroupInfo"/>����ϸ��Ϣ</returns>
        public IGroupInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IGroupInfo"/>����ϸ��Ϣ</returns>
        public IList<IGroupInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IGroupInfo"/>����ϸ��Ϣ</returns>
        public IList<IGroupInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IGroupInfo"/>����ϸ��Ϣ</returns>
        public IList<IGroupInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByAccountId(string accountId)
        /// <summary>��ѯĳ���û����ڵ�����Ⱥ����Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>��������<see cref="IGroupInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IGroupInfo> FindAllByAccountId(string accountId)
        {
            return this.provider.FindAllByAccountId(accountId);
        }
        #endregion

        #region 属性:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="groupTreeNodeId">�����ڵ���ʶ</param>
        /// <returns>��������ʵ��<see cref="IGroupInfo"/>����ϸ��Ϣ</returns>
        public IList<IGroupInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId)
        {
            return this.provider.FindAllByGroupTreeNodeId(groupTreeNodeId);
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
        /// <returns>����һ���б�ʵ��<see cref="IGroupInfo"/></returns>
        public IList<IGroupInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="name">Ⱥ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 属性:IsExistGlobalName(string globalName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="globalName">Ⱥ��ȫ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGlobalName(string globalName)
        {
            return this.provider.IsExistGlobalName(globalName);
        }
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">Ⱥ����ʶ</param>
        /// <param name="name">Ⱥ������</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        public int Rename(string id, string name)
        {
            return this.provider.Rename(id, name);
        }
        #endregion

        #region 属性:CombineFullPath(string name, string groupTreeNodeId)
        /// <summary>��ɫȫ·��</summary>
        /// <param name="name">ͨ�ý�ɫ����</param>
        /// <param name="groupTreeNodeId">����������ʶ</param>
        /// <returns></returns>
        public string CombineFullPath(string name, string groupTreeNodeId)
        {
            string path = MembershipManagement.Instance.GroupTreeNodeService.GetGroupTreeNodePathByGroupTreeNodeId(groupTreeNodeId);

            return string.Format(@"{0}{1}", path, name);
        }
        #endregion

        #region 属性:CombineDistinguishedName(string name, string groupTreeNodeId)
        /// <summary>ͨ�ý�ɫΨһ����</summary>
        /// <param name="name">ͨ�ý�ɫ����</param>
        /// <param name="groupTreeNodeId">����������ʶ</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string name, string groupTreeNodeId)
        {
            string path = MembershipManagement.Instance.GroupTreeNodeService.GetActiveDirectoryOUPathByGroupTreeNodeId(groupTreeNodeId);

            return string.Format("CN={0},{1}{2}", name, path, ActiveDirectoryConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        #region 属性:SetGlobalName(string id, string globalName)
        /// <summary>����ȫ������</summary>
        /// <param name="id">�ʻ���ʶ</param>
        /// <param name="globalName">ȫ������</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetGlobalName(string id, string globalName)
        {
            if (string.IsNullOrEmpty(globalName))
            {
                // ������${Id}��ȫ�����Ʋ���Ϊ�ա�
                return 1;
            }

            if (IsExistGlobalName(globalName))
            {
                return 2;
            }

            // �����Ƿ����ڶ���
            if (!IsExist(id))
            {
                // ������${Id}�������ڡ�
                return 3;
            }

            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                /*
                IGroupInfo originalObject = FindOne(id);

                if (originalObject != null)
                {
                    // �����ⲿϵͳֱ��ͬ������Ա��Ȩ�޹��������ݿ��У�
                    // ���� Active Directory �ϲ���ֱ�Ӵ������ض�������Ҫ�ֹ�����ȫ�����Ʋ��������ض�����
                    if (!string.IsNullOrEmpty(originalObject.GlobalName) 
                        && ActiveDirectoryManagement.Instance.Group.IsExistName(originalObject.GlobalName))
                    {
                        ActiveDirectoryManagement.Instance.Group.Rename(originalObject.GlobalName, globalName);
                    }
                    else
                    {
                        ActiveDirectoryManagement.Instance.Group.Add(globalName, MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(originalObject.Id));
                    }
                }
                 */
            }

            return this.provider.SetGlobalName(id, globalName);
        }
        #endregion

        #region 属性:SetExchangeStatus(string id, int status)
        /// <summary>������ҵ����״̬</summary>
        /// <param name="id">Ⱥ����ʶ</param>
        /// <param name="status">״̬��ʶ, 1:����, 0:����</param>
        /// <returns>0 ���óɹ�, 1 ����ʧ��.</returns>
        public int SetExchangeStatus(string id, int status)
        {
            return this.provider.SetExchangeStatus(id, status);
        }
        #endregion

        #region 属性:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>�������ݰ�</summary>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        public string CreatePackage(DateTime beginDate, DateTime endDate)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" UpdateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"group\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<groups>");

            foreach (IGroupInfo item in list)
            {
                outString.Append(((GroupInfo)item).Serializable());
            }

            outString.Append("</groups>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IGroupInfo param)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">��ɫ��Ϣ</param>
        public int SyncToActiveDirectory(IGroupInfo param)
        {
            return SyncToActiveDirectory(param, param.GlobalName, param.GroupTreeNodeId);
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IGroupInfo param, string originalGlobalName, string originalGroupTreeNodeId)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">��ɫ��Ϣ</param>
        /// <param name="originalGlobalName">ԭʼ��ȫ������</param>
        /// <param name="originalGroupTreeNodeId">ԭʼ�ķ�����ʶ</param>
        public int SyncToActiveDirectory(IGroupInfo param, string originalGlobalName, string originalGroupTreeNodeId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.Name))
                {
                    // ��ɫ��${Name}������Ϊ�գ�������������Ϣ��
                    return 1;
                }
                else if (string.IsNullOrEmpty(param.GlobalName))
                {
                    // ��ɫ��${GlobalName}������Ϊ�գ�������������Ϣ��
                    return 1;
                }
                else
                {
                    // 1.ԭʼ��ȫ�����Ʋ�Ϊ�ա�
                    // 2.Active Directory �������ض�����
                    if (!string.IsNullOrEmpty(originalGlobalName)
                        && ActiveDirectoryManagement.Instance.Group.IsExistName(originalGlobalName))
                    {
                        if (param.GlobalName != originalGlobalName)
                        {
                            // ��ɫ��${Name}�������Ʒ����ı䡣
                            ActiveDirectoryManagement.Instance.Group.Rename(originalGlobalName, param.GlobalName);
                        }

                        if (param.GroupTreeNodeId != originalGroupTreeNodeId)
                        {
                            // ��ɫ��${Name}����������֯�����仯��
                            ActiveDirectoryManagement.Instance.Group.MoveTo(param.GlobalName,
                                MembershipManagement.Instance.GroupTreeNodeService.GetActiveDirectoryOUPathByGroupTreeNodeId(param.GroupTreeNodeId));
                        }

                        return 0;
                    }
                    else
                    {
                        string parentPath = MembershipManagement.Instance.GroupTreeNodeService.GetActiveDirectoryOUPathByGroupTreeNodeId(param.GroupTreeNodeId);

                        ActiveDirectoryManagement.Instance.Group.Add(param.GlobalName, parentPath);

                        // ��ɫ��${Name}�������ɹ���
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

        #region 属性:SyncFromPackPage(IGroupInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">Ⱥ����Ϣ</param>
        public int SyncFromPackPage(IGroupInfo param)
        {
            return this.provider.SyncFromPackPage(param);
        }
        #endregion

        // -------------------------------------------------------
        // �����ʺź�Ⱥ����ϵ
        // -------------------------------------------------------

        #region 属性:FindAllRelationByAccountId(string accountId)
        /// <summary>�����ʺŲ�ѯ����Ⱥ���Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>Table Columns��AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountGroupRelationInfo> FindAllRelationByAccountId(string accountId)
        {
            return this.provider.FindAllRelationByAccountId(accountId);
        }
        #endregion

        #region 属性:FindAllRelationByGroupId(string groupId)
        /// <summary>����Ⱥ����ѯ�����ʺŵĹ�ϵ</summary>
        /// <param name="groupId">Ⱥ����ʶ</param>
        /// <returns>Table Columns��AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountGroupRelationInfo> FindAllRelationByGroupId(string groupId)
        {
            return this.provider.FindAllRelationByGroupId(groupId);
        }
        #endregion

        #region 属性:AddRelation(string accountId, string groupId)
        /// <summary>�����ʺ�������Ⱥ���Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="groupId">Ⱥ����ʶ</param>
        public int AddRelation(string accountId, string groupId)
        {
            return AddRelation(accountId, groupId, DateTime.Now, DateTime.MaxValue);
        }
        #endregion

        #region 属性:AddRelation(string accountId, string groupId, DateTime beginDate, DateTime endDate)
        /// <summary>�����ʺ�������Ⱥ���Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="groupId">Ⱥ����ʶ</param>
        /// <param name="beginDate">����ʱ��</param>
        /// <param name="endDate">ͣ��ʱ��</param>
        public int AddRelation(string accountId, string groupId, DateTime beginDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                // �ʺű�ʶ����Ϊ��
                return 1;
            }

            if (string.IsNullOrEmpty(groupId))
            {
                // Ⱥ����ʶ����Ϊ��
                return 2;
            }

            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IGroupInfo group = MembershipManagement.Instance.GroupService[groupId];

                // �ʺŶ������ʺŵ�ȫ�����ơ��ʺŵĵ�¼����Ⱥ��������Ⱥ����ȫ�����ƵȲ���Ϊ�ա�
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && group != null && !string.IsNullOrEmpty(group.GlobalName))
                {
                    ActiveDirectoryManagement.Instance.Group.AddRelation(account.LoginName, ActiveDirectorySchemaClassType.User, group.Name);
                }
            }

            return this.provider.AddRelation(accountId, groupId, beginDate, endDate);
        }
        #endregion

        #region 属性:ExtendRelation(string accountId, string groupId, DateTime endDate)
        /// <summary>��Լ�ʺ������ؽ�ɫ�Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="groupId">Ⱥ����ʶ</param>
        /// <param name="endDate">�µĽ�ֹʱ��</param>
        public int ExtendRelation(string accountId, string groupId, DateTime endDate)
        {
            return this.provider.ExtendRelation(accountId, groupId, endDate);
        }
        #endregion

        #region 属性:RemoveRelation(string accountId, string groupId)
        /// <summary>�Ƴ��ʺ�������Ⱥ���Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="groupId">Ⱥ����ʶ</param>
        public int RemoveRelation(string accountId, string groupId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IGroupInfo group = MembershipManagement.Instance.GroupService[groupId];

                // �ʺŶ������ʺŵ�ȫ�����ơ��ʺŵĵ�¼����Ⱥ��������Ⱥ����ȫ�����ƵȲ���Ϊ�ա�
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && group != null && !string.IsNullOrEmpty(group.GlobalName))
                {
                    ActiveDirectoryManagement.Instance.Group.RemoveRelation(account.LoginName, ActiveDirectorySchemaClassType.User, group.GlobalName);
                }
            }

            return this.provider.RemoveRelation(accountId, groupId);
        }
        #endregion

        #region 属性:RemoveExpiredRelation(string accountId)
        /// <summary>�Ƴ��ʺ��ѹ��ڵ�Ⱥ����ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        public int RemoveExpiredRelation(string accountId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountGroupRelationInfo> list = FindAllRelationByAccountId(accountId);

                foreach (IAccountGroupRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.GroupId);
                }

                return 0;
            }
            else
            {
                return this.provider.RemoveExpiredRelation(accountId);
            }
        }
        #endregion

        #region 属性:RemoveAllRelation(string accountId)
        /// <summary>�Ƴ��ʺ���������Ⱥ���Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        public int RemoveAllRelation(string accountId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountGroupRelationInfo> list = FindAllRelationByAccountId(accountId);

                foreach (IAccountGroupRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.GroupId);
                }

                return 0;
            }
            else
            {
                return this.provider.RemoveAllRelation(accountId);
            }
        }
        #endregion

        #region 属性:ClearupRelation(string groupId)
        /// <summary>����Ⱥ�����ʺŵĹ�ϵ</summary>
        /// <param name="groupId">Ⱥ����ʶ</param>
        public int ClearupRelation(string groupId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountGroupRelationInfo> list = this.FindAllRelationByGroupId(groupId);

                foreach (IAccountGroupRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.GroupId);
                }

                return 0;
            }
            else
            {
                return this.provider.ClearupRelation(groupId);
            }
        }
        #endregion
    }
}
