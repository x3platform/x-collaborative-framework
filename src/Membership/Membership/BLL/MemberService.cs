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

    using X3Platform.Configuration;
    using X3Platform.DigitalNumber;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;

    /// <summary></summary>
    public class MemberService : IMemberService
    {
        private MembershipConfiguration configuration = null;

        private IMemberProvider provider = null;

        /// <summary></summary>
        public MemberService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IMemberProvider>(typeof(IMemberProvider));
        }

        #region 属性:this[string index]
        /// <summary>����</summary>
        /// <param name="id">��Ա��ʶ</param>
        /// <returns></returns>
        public IMemberInfo this[string id]
        {
            get { return provider.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IMemberInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">IMemberInfo ʵ����ϸ��Ϣ</param>
        /// <returns>IMemberInfo ʵ����ϸ��Ϣ</returns>
        public IMemberInfo Save(IMemberInfo param)
        {
            IMemberInfo originalObject = this.FindOne(param.Id);

            if (originalObject == null) { originalObject = param; }

            IRoleInfo defaultRole = null;

            IAssignedJobInfo defaultAssignedJob = null;

            // �����ֶ����� �Զ�������֯��Ϣ
            // AutoBindingOrganizationByPrimaryKey : None | RoleId | AssignedJobId
            if (MembershipConfigurationView.Instance.AutoBindingOrganizationByPrimaryKey == "RoleId" && !string.IsNullOrEmpty(param.RoleId))
            {
                defaultRole = MembershipManagement.Instance.RoleService[param.RoleId];

                param.OrganizationId = defaultRole.OrganizationId;

                this.SetDefaultOrganization(param.Account.Id, param.OrganizationId);
                this.SetDefaultCorporationAndDepartmentsByOrganizationId(param.Account.Id, param.OrganizationId);
            }
            else if (MembershipConfigurationView.Instance.AutoBindingOrganizationByPrimaryKey == "AssignedJobId" && !string.IsNullOrEmpty(param.AssignedJobId))
            {
                defaultAssignedJob = MembershipManagement.Instance.AssignedJobService[param.AssignedJobId];

                if (defaultAssignedJob != null)
                {
                    param.OrganizationId = defaultAssignedJob.OrganizationId;

                    this.SetDefaultOrganization(param.Account.Id, param.OrganizationId);
                    this.SetDefaultCorporationAndDepartmentsByOrganizationId(param.Account.Id, param.OrganizationId);
                }
            }
            else if (MembershipConfigurationView.Instance.AutoBindingOrganizationByPrimaryKey == "Self" && !string.IsNullOrEmpty(param.OrganizationId))
            {
                // �����ֶ��󶨹���������֯��Ϣ
                this.SetDefaultOrganization(param.Account.Id, param.OrganizationId);
                this.SetDefaultCorporationAndDepartmentsByOrganizationId(param.Account.Id, param.OrganizationId);
            }

            // ���ݸ�λ���� �Զ�����ְλ��Ϣ
            if (MembershipConfigurationView.Instance.AutoBindingJobByPrimaryKey == "AssignedJobId" && !string.IsNullOrEmpty(param.AssignedJobId))
            {
                if (defaultAssignedJob == null)
                {
                    defaultAssignedJob = MembershipManagement.Instance.AssignedJobService[param.AssignedJobId];
                }

                if (defaultAssignedJob != null)
                {
                    this.SetDefaultJob(param.Account.Id, defaultAssignedJob.JobId);
                }
            }
            else if (MembershipConfigurationView.Instance.AutoBindingJobByPrimaryKey == "Self" && !string.IsNullOrEmpty(param.JobId))
            {
                // ����������������ְλ��Ϣ
                this.SetDefaultJob(param.Account.Id, param.JobId);
            }

            // ���ݸ�λ���� �Զ�����ְ����Ϣ
            if (MembershipConfigurationView.Instance.AutoBindingJobGradeByPrimaryKey == "AssignedJobId" && !string.IsNullOrEmpty(param.AssignedJobId))
            {
                if (defaultAssignedJob == null)
                {
                    defaultAssignedJob = MembershipManagement.Instance.AssignedJobService[param.AssignedJobId];
                }

                if (defaultAssignedJob != null)
                {
                    this.SetDefaultJobGrade(param.Account.Id, defaultAssignedJob.JobGradeId);
                }
            }
            else if (MembershipConfigurationView.Instance.AutoBindingJobGradeByPrimaryKey == "Self" && !string.IsNullOrEmpty(param.JobGradeId))
            {
                // ����Ĭ����������ְ����Ϣ
                this.SetDefaultJobGrade(param.Account.Id, param.JobGradeId);
            }

            // ������֯ȫ·��
            param.FullPath = CombineFullPath(param.Account.Name, param.OrganizationId);

            param = provider.Save(param);

            // 1.������չ��Ϣ
            if (param.ExtensionInformation != null)
            {
                param.ExtensionInformation.Save();
            }

            // 2.����Ĭ�Ͻ�ɫ��Ϣ
            if (param.Role != null)
            {
                MembershipManagement.Instance.RoleService.SetDefaultRelation(param.Account.Id, param.Role.Id);
            }

            // 3.����Ĭ�ϸ�λ��Ϣ
            if (param.AssignedJob != null)
            {
                MembershipManagement.Instance.AssignedJobService.RemoveDefaultRelation(param.Account.Id);
                MembershipManagement.Instance.AssignedJobService.SetDefaultRelation(param.Account.Id, param.AssignedJob.Id);
            }

            // 4.���ü�ְ��Ϣ
            if (param.PartTimeJobs.Count > 0)
            {
                this.BindPartTimeJobs(param);
            }
            else if (param.PartTimeJobs.Count == 0 && originalObject.PartTimeJobs.Count > 0)
            {
                // �Ƴ���Ĭ�ϸ�λ��ϵ
                MembershipManagement.Instance.AssignedJobService.RemoveNondefaultRelation(param.Account.Id);
            }

            return param;
        }
        #endregion

        #region ˽�к���:BindPartTimeJobs(IMemberInfo param)
        /// <summary>�󶨼�ְ��Ϣ</summary>
        /// <param name="param"></param>
        private void BindPartTimeJobs(IMemberInfo param)
        {
            if (param != null && param.Account != null)
            {
                string accountId = param.Account.Id;

                // �����µĹ�ϵ
                if (!string.IsNullOrEmpty(accountId))
                {
                    // 1.�Ƴ���Ĭ�Ͻ�ɫ��ϵ
                    MembershipManagement.Instance.AssignedJobService.RemoveNondefaultRelation(accountId);

                    // 2.�����µĹ�ϵ
                    foreach (IAssignedJobInfo item in param.PartTimeJobs)
                    {
                        MembershipManagement.Instance.AssignedJobService.AddRelation(accountId, item.Id);
                    }
                }
            }
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(int id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">AccountInfo Id��</param>
        /// <returns>����һ�� IMemberInfo ʵ������ϸ��Ϣ</returns>
        public IMemberInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindOneByAccountId(string accountId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����һ�� MemberInfo ʵ������ϸ��Ϣ</returns>
        public IMemberInfo FindOneByAccountId(string accountId)
        {
            return provider.FindOneByAccountId(accountId);
        }
        #endregion

        #region 属性:FindOneByLoginName(string loginName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="loginName">��½��</param>
        /// <returns>����һ�� IMemberInfo ʵ������ϸ��Ϣ</returns>
        public IMemberInfo FindOneByLoginName(string loginName)
        {
            return provider.FindOneByLoginName(loginName);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� IMemberInfo ʵ������ϸ��Ϣ</returns>
        public IList<IMemberInfo> FindAll()
        {
            return provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� IMemberInfo ʵ������ϸ��Ϣ</returns>
        public IList<IMemberInfo> FindAll(string whereClause)
        {
            return provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� IMemberInfo ʵ������ϸ��Ϣ</returns>
        public IList<IMemberInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllWithoutDefaultOrganization(int length)
        /// <summary>��������û��Ĭ����֯�ĳ�Ա��Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IMemberInfo" />ʵ������ϸ��Ϣ</returns>
        public IList<IMemberInfo> FindAllWithoutDefaultOrganization(int length)
        {
            return provider.FindAllWithoutDefaultOrganization(length);
        }
        #endregion

        #region 属性:FindAllWithoutDefaultJob(int length)
        /// <summary>��������û��Ĭ��ְλ�ĳ�Ա��Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IMemberInfo" />ʵ������ϸ��Ϣ</returns>
        public IList<IMemberInfo> FindAllWithoutDefaultJob(int length)
        {
            return provider.FindAllWithoutDefaultJob(length);
        }
        #endregion

        #region 属性:FindAllWithoutDefaultAssignedJob(int length)
        /// <summary>��������û��Ĭ�ϸ�λ�ĳ�Ա��Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IMemberInfo" />ʵ������ϸ��Ϣ</returns>
        public IList<IMemberInfo> FindAllWithoutDefaultAssignedJob(int length)
        {
            return provider.FindAllWithoutDefaultAssignedJob(length);
        }
        #endregion

        #region 属性:FindAllWithoutDefaultRole(int length)
        /// <summary>��������û��Ĭ�Ͻ�ɫ�ĳ�Ա��Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IMemberInfo" />ʵ������ϸ��Ϣ</returns>
        public IList<IMemberInfo> FindAllWithoutDefaultRole(int length)
        {
            return provider.FindAllWithoutDefaultRole(length);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="IMemberInfo"/></returns> 
        public IList<IMemberInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��Ա��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 属性:CreateEmptyMember(string accountId)
        /// <summary>�����յ���Ա��Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns></returns>
        public IMemberInfo CreateEmptyMember(string accountId)
        {
            MemberInfo param = new MemberInfo();

            param.Id = param.AccountId = accountId;

            param.Account = MembershipManagement.Instance.AccountService.CreateEmptyAccount(accountId);

            param.UpdateDate = param.CreateDate = DateTime.Now;

            return param;
        }
        #endregion

        #region 属性:CombineFullPath(string name, string organizationId)
        /// <summary>����ȫ·��</summary>
        /// <param name="name">�û�����</param>
        /// <param name="organizationId">������֯��ʶ</param>
        /// <returns></returns>
        public string CombineFullPath(string name, string organizationId)
        {
            // ��֯�ṹ\�����ܲ�\������\��Ϣ����\����
            string path = MembershipManagement.Instance.OrganizationService.GetOrganizationPathByOrganizationId(organizationId);

            return string.Format(@"{0}{1}", path, name);
        }
        #endregion

        #region 属性:SetContactCard(string accountId, Dictionary<string,string> contactItems);
        /// <summary>������ϵ����Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="contactItems">��ϵ���ֵ�</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        public int SetContactCard(string accountId, Dictionary<string, string> contactItems)
        {
            return this.provider.SetContactCard(accountId, contactItems);
        }
        #endregion

        #region 属性:SetDefaultOrganization(string accountId, string organizationId)
        /// <summary>����Ĭ����֯��λ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">��֯��λ��ʶ</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        public int SetDefaultOrganization(string accountId, string organizationId)
        {
            return provider.SetDefaultOrganization(accountId, organizationId);
        }
        #endregion

        #region 属性:SetDefaultCorporationAndDepartments(string accountId, string organizationIds)
        /// <summary>����Ĭ����֯��λ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationIds">��֯��λ��ʶ��[0]��˾��ʶ��[1]һ�����ű�ʶ��[2]�������ű�ʶ��[3]�������ű�ʶ��</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        public int SetDefaultCorporationAndDepartments(string accountId, string organizationIds)
        {
            return provider.SetDefaultCorporationAndDepartments(accountId, organizationIds);
        }
        #endregion

        #region 属性:SetDefaultCorporationAndDepartmentsByOrganizationId(string accountId, string organizationId)
        /// <summary>����Ĭ����֯��λ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">Ĭ��������ĩ����֯��λ��ʶ</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        public int SetDefaultCorporationAndDepartmentsByOrganizationId(string accountId, string organizationId)
        {
            string organizationIds = null;

            IOrganizationInfo corporation = MembershipManagement.Instance.OrganizationService.FindCorporationByOrganizationId(organizationId);

            if (corporation == null)
            {
                return 0;
            }
            else
            {
                organizationIds = corporation.Id;
            }

            IOrganizationInfo department1 = MembershipManagement.Instance.OrganizationService.FindDepartmentByOrganizationId(organizationId, 1);

            if (department1 != null)
            {
                organizationIds = organizationIds + "," + department1.Id;

                IOrganizationInfo department2 = MembershipManagement.Instance.OrganizationService.FindDepartmentByOrganizationId(organizationId, 2);

                if (department2 != null)
                {
                    organizationIds = organizationIds + "," + department2.Id;

                    IOrganizationInfo department3 = MembershipManagement.Instance.OrganizationService.FindDepartmentByOrganizationId(organizationId, 3);

                    if (department3 != null)
                    {
                        organizationIds = organizationIds + "," + department3.Id;
                    }
                }
            }

            this.SetDefaultCorporationAndDepartments(accountId, organizationIds);

            return 0;
        }
        #endregion

        #region 属性:SetDefaultRole(string accountId, string roleId)
        /// <summary>����Ĭ�Ͻ�ɫ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="roleId">��ɫ��ʶ</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public virtual int SetDefaultRole(string accountId, string roleId)
        {
            IRoleInfo role = MembershipManagement.Instance.RoleService[roleId];

            if (role == null)
            {
                // δ�ҵ���ɫ��Ϣ��
                return 1;
            }

            // ����Ĭ�Ϲ�˾��������Ϣ��

            string organizationIds = string.Empty;

            IOrganizationInfo organization = MembershipManagement.Instance.OrganizationService.FindCorporationByOrganizationId(role.OrganizationId);

            if (organization == null)
            {
                // δ�ҵ���ɫ���صĹ�˾��Ϣ��
                return 2;
            }

            organizationIds += organization.Id + ",";

            int depth = 1;

            organization = MembershipManagement.Instance.OrganizationService.FindOneByRoleId(roleId, depth);

            while (organization != null)
            {
                organizationIds += organization.Id + ",";

                depth++;

                organization = MembershipManagement.Instance.OrganizationService.FindOneByRoleId(roleId, depth);
            }

            MembershipManagement.Instance.MemberService.SetDefaultCorporationAndDepartments(accountId, organizationIds);

            // ͳһ������ɫ���ݷ������á�
            return MembershipManagement.Instance.RoleService.SetDefaultRelation(accountId, roleId);
        }
        #endregion

        #region 属性:SetDefaultJob(string accountId, string jobId)
        /// <summary>����Ĭ��ְλ��Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="jobId">ְλ��Ϣ</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        public int SetDefaultJob(string accountId, string jobId)
        {
            return provider.SetDefaultJob(accountId, jobId);
        }
        #endregion

        #region 属性:SetDefaultAssignedJob(string accountId, string assignedJobId)
        /// <summary>����Ĭ�Ͻ�ɫ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="assignedJobId">��λ��ʶ</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetDefaultAssignedJob(string accountId, string assignedJobId)
        {
            // ͳһ������λ���ݷ������á�
            return MembershipManagement.Instance.AssignedJobService.SetDefaultRelation(accountId, assignedJobId);
        }
        #endregion

        #region 属性:SetDefaultJobGrade(string accountId, string jobGradeId)
        /// <summary>����Ĭ��ְ����Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="jobGradeId">ְ����ʶ</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        public int SetDefaultJobGrade(string accountId, string jobGradeId)
        {
            return provider.SetDefaultJobGrade(accountId, jobGradeId);
        }
        #endregion

        #region 属性:SyncFromPackPage(IMemberInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">��Ա��Ϣ</param>
        public int SyncFromPackPage(IMemberInfo param)
        {
            // 1.���ó�Ա��Ϣ
            this.provider.SyncFromPackPage(param);

            // 2.�����ʺ���Ϣ
            MembershipManagement.Instance.AccountService.SyncFromPackPage(param.Account);

            // 3.����Ĭ�ϸ�λ��Ϣ
            if (param.AssignedJob != null)
            {
                MembershipManagement.Instance.AssignedJobService.SetDefaultRelation(param.Account.Id, param.AssignedJob.Id);
            }

            // 4.���ü�ְ��Ϣ
            if (param.PartTimeJobs.Count > 0)
            {
                this.BindPartTimeJobs(param);
            }

            return 0;
        }
        #endregion
    }
}