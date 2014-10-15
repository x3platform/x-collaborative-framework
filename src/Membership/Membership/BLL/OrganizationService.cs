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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using X3Platform.ActiveDirectory;
    using X3Platform.ActiveDirectory.Configuration;
    using X3Platform.CacheBuffer;
    using X3Platform.Configuration;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Spring;

    /// <summary></summary>
    public class OrganizationService : IOrganizationService
    {
        private MembershipConfiguration configuration = null;

        private IOrganizationProvider provider = null;

        private Dictionary<string, IOrganizationInfo> dictionary = new Dictionary<string, IOrganizationInfo>();

        #region ���캯��:OrganizationService()
        /// <summary>���캯��</summary>
        public OrganizationService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IOrganizationProvider>(typeof(IOrganizationProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id">��֯��ʶ</param>
        /// <returns></returns>
        public IOrganizationInfo this[string id]
        {
            get { return this.provider.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ�� �޸�
        // -------------------------------------------------------

        #region 属性:Save(AccountInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">IOrganizationInfo ʵ����ϸ��Ϣ</param>
        /// <returns>IOrganizationInfo ʵ����ϸ��Ϣ</returns>
        public IOrganizationInfo Save(IOrganizationInfo param)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IOrganizationInfo originalObject = FindOne(param.Id);

                if (originalObject == null)
                {
                    originalObject = param;
                }

                SyncToActiveDirectory(param, originalObject.Name, originalObject.GlobalName, originalObject.ParentId);
            }

            // ������֯ȫ·��
            param.FullPath = this.CombineFullPath(param.Name, param.ParentId);

            // ����Ψһʶ������
            param.DistinguishedName = this.CombineDistinguishedName(param.GlobalName, param.Id);

            return this.provider.Save(param);
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

        #region 属性:FindOne(int id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">AccountInfo Id��</param>
        /// <returns>����һ�� IOrganizationInfo ʵ������ϸ��Ϣ</returns>
        public IOrganizationInfo FindOne(string id)
        {
            IOrganizationInfo param = null;

            if (this.dictionary.ContainsKey(id))
            {
                param = dictionary[id];
            }

            if (param == null)
            {
                param = this.provider.FindOne(id);

                if (param != null)
                {
                    dictionary.Add(id, param);
                }
            }

            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindOneByGlobalName(string globalName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="globalName">��֯��ȫ������</param>
        /// <returns>����һ��<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationInfo FindOneByGlobalName(string globalName)
        {
            return this.provider.FindOneByGlobalName(globalName);
        }
        #endregion

        #region 属性:FindOneByRoleId(string roleId)
        /// <summary>��ѯĳ����ɫ��������֯��Ϣ</summary>
        /// <param name="roleId">��ɫ��ʶ</param>
        /// <returns>��������<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationInfo FindOneByRoleId(string roleId)
        {
            return this.provider.FindOneByRoleId(roleId);
        }
        #endregion

        #region 属性:FindOneByRoleId(string roleId, int level)
        /// <summary>��ѯĳ����ɫ������ĳһ���ε���֯��Ϣ</summary>
        /// <param name="roleId">��ɫ��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationInfo FindOneByRoleId(string roleId, int level)
        {
            return this.provider.FindOneByRoleId(roleId, level);
        }
        #endregion

        #region 属性:FindCorporationByOrganizationId(string id)
        /// <summary>��ѯĳ����֯�����Ĺ�˾��Ϣ</summary>
        /// <param name="id">��֯��ʶ</param>
        /// <returns>��������<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationInfo FindCorporationByOrganizationId(string id)
        {
            return this.provider.FindCorporationByOrganizationId(id);
        }
        #endregion

        #region 属性:FindDepartmentByOrganizationId(string organizationId, int level)
        /// <summary>��ѯĳ����֯������ĳ���ϼ�������Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationInfo FindDepartmentByOrganizationId(string organizationId, int level)
        {
            return this.provider.FindDepartmentByOrganizationId(organizationId, level);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� IOrganizationInfo ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� IOrganizationInfo ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� IOrganizationInfo ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByParentId(string parentId)
        /// <summary>��ѯĳ�򸸽ڵ��µ�������֯��λ</summary>
        /// <param name="parentId">���ڱ�ʶ</param>
        /// <returns>��������ʵ��<see cref="IOrganizationInfo"/>����ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAllByParentId(string parentId)
        {
            return FindAllByParentId(parentId, 0);
        }
        #endregion

        #region 属性:FindAllByParentId(string parentId, int depth)
        /// <summary>��ѯĳ�򸸽ڵ��µ�������֯��λ</summary>
        /// <param name="parentId">���ڱ�ʶ</param>
        /// <param name="depth">������ȡ�Ĳ��Σ�0��ʾֻ��ȡ�����Σ�-1��ʾȫ����ȡ</param>
        /// <returns>��������ʵ��<see cref="IOrganizationInfo"/>����ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAllByParentId(string parentId, int depth)
        {
            // �����б�
            List<IOrganizationInfo> list = new List<IOrganizationInfo>();

            //
            // ������֯�Ӳ�����Ϣ
            //

            IList<IOrganizationInfo> organizations = this.provider.FindAllByParentId(parentId);

            list.AddRange(organizations);

            if (depth == -1)
            {
                depth = int.MaxValue;
            }

            if (organizations.Count > 0 && depth > 0)
            {
                foreach (IOrganizationInfo organization in organizations)
                {
                    list.AddRange(FindAllByParentId(organization.Id, (depth - 1)));
                }
            }

            return list;
        }
        #endregion

        #region 属性:FindAllByAccountId(string accountId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����һ�� IOrganizationInfo ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAllByAccountId(string accountId)
        {
            // ���� �Ƿ���������Ϣ
            if (string.IsNullOrEmpty(accountId) || accountId == "0")
            {
                return new List<IOrganizationInfo>();
            }

            return this.provider.FindAllByAccountId(accountId);
        }
        #endregion

        #region 属性:FindAllByRoleIds(string roleIds)
        /// <summary>��ѯĳ����ɫ������������֯</summary>
        /// <param name="roleIds">��ɫ��ʶ</param>
        /// <returns>��������<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAllByRoleIds(string roleIds)
        {
            IList<IOrganizationInfo> list = new List<IOrganizationInfo>();

            string[] ids = roleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string id in ids)
            {
                IOrganizationInfo organization = FindOneByRoleId(id);

                if (organization != null)
                    list.Add(organization);
            }

            return list;
        }
        #endregion

        #region 属性:FindAllByRoleIds(string roleIds)
        /// <summary>��ѯĳ����ɫ������������֯</summary>
        /// <param name="roleIds">��ɫ��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAllByRoleIds(string roleIds, int level)
        {
            IList<IOrganizationInfo> list = new List<IOrganizationInfo>();

            string[] ids = roleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string id in ids)
            {
                IOrganizationInfo organization = FindOneByRoleId(id, level);

                if (organization != null)
                    list.Add(organization);
            }

            return list;
        }
        #endregion

        #region 属性:FindAllByCorporationId(string corporationId)
        /// <summary>�ݹ���ѯĳ����˾�������еĽ�ɫ</summary>
        /// <param name="corporationId">��֯��ʶ</param>
        /// <returns>��������<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAllByCorporationId(string corporationId)
        {
            // �����б�
            List<IOrganizationInfo> list = new List<IOrganizationInfo>();

            //
            // ���Ҳ���(��˾��һ����֯�ܹ�)
            //
            IList<IOrganizationInfo> organizations = MembershipManagement.Instance.OrganizationService.FindAllByParentId(corporationId);

            foreach (IOrganizationInfo organization in organizations)
            {
                list.Add(organization);

                // ��ȡ��Ŀ�Ŷ�������ֻ�ܲ���
                if (organization.Name.IndexOf("��Ŀ�Ŷ�") == -1)
                {
                    list.AddRange(FindAllByParentId(organization.Id, -1));
                }
            }

            list.Add(FindOne(corporationId));

            return list;
        }
        #endregion

        #region 属性:FindAllByProjectId(string projectId)
        /// <summary>�ݹ���ѯĳ����Ŀ�������еĽ�ɫ</summary>
        /// <param name="projectId">��֯��ʶ</param>
        /// <returns>����һ��<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindAllByProjectId(string projectId)
        {
            // 
            // ��Ŀ�Ŷӵı�ʶ �� ��Ŀ��ʶ ����һ��
            //

            string organizationId = projectId;

            IList<IOrganizationInfo> list = FindAllByParentId(organizationId, 1);

            list.Add(FindOne(organizationId));

            return list;
        }
        #endregion

        #region 属性:FindCorporationsByAccountId(string accountId)
        /// <summary>��ѯĳ���ʻ����������й�˾��Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>��������<see cref="IOrganizationInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationInfo> FindCorporationsByAccountId(string accountId)
        {
            IList<IOrganizationInfo> corporations = new List<IOrganizationInfo>();

            IMemberInfo member = MembershipManagement.Instance.MemberService[accountId];

            if (member != null && member.Corporation != null)
            {
                corporations.Add((OrganizationInfo)member.Corporation);

                IList<IOrganizationInfo> list = this.provider.FindCorporationsByAccountId(accountId);

                foreach (OrganizationInfo item in list)
                {
                    if (item.Id != member.Corporation.Id)
                    {
                        corporations.Add(item);
                    }
                }
            }

            return corporations;
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
        /// <returns>����һ���б�ʵ��<see cref="IOrganizationInfo"/></returns>
        public IList<IOrganizationInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="name">��֯��λ����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 属性:IsExistGlobalName(string globalName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="globalName">��֯��λȫ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGlobalName(string globalName)
        {
            return this.provider.IsExistGlobalName(globalName);
        }
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��֯��ʶ</param>
        /// <param name="name">��֯����</param>
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

        #region 属性:CombineFullPath(string name, string parentId)
        /// <summary>����ȫ·��</summary>
        /// <param name="name">��֯����</param>
        /// <param name="parentId">����������ʶ</param>
        /// <returns></returns>
        public string CombineFullPath(string name, string parentId)
        {
            string path = GetOrganizationPathByOrganizationId(parentId);

            return string.Format(@"{0}{1}", path, name);
        }
        #endregion

        #region 属性:GetOrganizationPathByOrganizationId(string organizationId)
        /// <summary>������֯��ʶ������֯��ȫ·��</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns></returns>
        public string GetOrganizationPathByOrganizationId(string organizationId)
        {
            string path = FormatOrganizationPath(organizationId);

            return string.Format(@"{0}\", path);
        }
        #endregion

        #region ˽�к���:FormatOrganizationPath(string id)
        /// <summary>��ʽ����֯·��</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string FormatOrganizationPath(string id)
        {
            string path = string.Empty;

            string parentId = string.Empty;

            string name = null;

            IOrganizationInfo param = FindOne(id);

            if (param == null)
            {
                return string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(param.ParentId))
                {
                    parentId = param.ParentId;
                }

                name = param.Name;

                if (!string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(parentId) && parentId != Guid.Empty.ToString())
                    {
                        path = FormatOrganizationPath(parentId);
                    }

                    path = string.IsNullOrEmpty(path) ? name : string.Format(@"{0}\{1}", path, name);

                    return path;
                }
            }

            return string.Empty;
        }
        #endregion

        #region 属性:CombineDistinguishedName(string globalName, string id)
        /// <summary>����Ψһ����</summary>
        /// <param name="globalName">��֯ȫ������</param>
        /// <param name="id">������ʶ</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string globalName, string id)
        {
            string path = this.GetActiveDirectoryOUPathByOrganizationId(id);

            return string.Format("CN={0},{1}{2}", globalName, path, ActiveDirectoryConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        #region 属性:GetActiveDirectoryOUPathByOrganizationId(string organizationId)
        /// <summary>������֯��ʶ���� Active Directory OU ·��</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns></returns>
        public string GetActiveDirectoryOUPathByOrganizationId(string organizationId)
        {
            return FormatActiveDirectoryPath(organizationId);
        }
        #endregion

        #region ˽�к���:FormatActiveDirectoryPath(string id)
        /// <summary>��ʽ�� Active Directory ·��</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string FormatActiveDirectoryPath(string id)
        {
            string path = string.Empty;

            string parentId = string.Empty;

            // OU������
            string name = null;

            IOrganizationInfo param = FindOne(id);

            if (param == null)
            {
                return string.Empty;
            }
            else
            {
                name = param.Name;

                // ��֯�ṹ�ĸ��ڵ�OU���⴦�� Ĭ��Ϊ��֯�ṹ
                if (id == "00000000-0000-0000-0000-000000000001")
                {
                    name = ActiveDirectoryConfigurationView.Instance.CorporationOrganizationFolderRoot;
                }

                // 1.���Ʋ���Ϊ�� 2.����������ʶ����Ϊ�� 
                if (!string.IsNullOrEmpty(name)
                    && !string.IsNullOrEmpty(param.ParentId) && param.ParentId != Guid.Empty.ToString())
                {
                    parentId = param.ParentId;

                    path = FormatActiveDirectoryPath(parentId);

                    path = string.IsNullOrEmpty(path) ? string.Format("OU={0}", name) : string.Format("OU={0}", name) + "," + path;

                    return path;
                }

                return string.Format("OU={0}", name);
            }
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
                IOrganizationInfo originalObject = FindOne(id);

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
                        ActiveDirectoryManagement.Instance.Organization.Add(originalObject.Name, MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(originalObject.ParentId));

                        ActiveDirectoryManagement.Instance.Group.Add(globalName, MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(originalObject.Id));
                    }
                }
            }

            return this.provider.SetGlobalName(id, globalName);
        }
        #endregion

        #region 属性:SetParentId(string id, string parentId)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��֯��ʶ</param>
        /// <param name="parentId">������֯��ʶ</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetParentId(string id, string parentId)
        {
            return this.provider.SetGlobalName(id, parentId);
        }
        #endregion

        #region 属性:SetExchangeStatus(string id, int status)
        /// <summary>������ҵ����״̬</summary>
        /// <param name="id">��֯��ʶ</param>
        /// <param name="status">״̬��ʶ, 1:����, 0:����</param>
        /// <returns>0 ���óɹ�, 1 ����ʧ��.</returns>
        public int SetExchangeStatus(string id, int status)
        {
            return this.provider.SetExchangeStatus(id, status);
        }
        #endregion

        #region 属性:GetChildNodes(string organizationId)
        /// <summary>��ȡ��֯���ӳ�Ա</summary>
        /// <param name="organizationId">��֯��λ��ʶ</param>
        public IList<IAuthorizationObject> GetChildNodes(string organizationId)
        {
            IList<IAuthorizationObject> list = new List<IAuthorizationObject>();

            IList<IOrganizationInfo> listA = this.FindAllByParentId(organizationId);

            IList<IAccountInfo> listB = MembershipManagement.Instance.AccountService.FindAllByOrganizationId(organizationId);

            foreach (IAuthorizationObject item in listA)
            {
                list.Add(item);
            }

            foreach (IAuthorizationObject item in listB)
            {
                list.Add(item);
            }

            return list;
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

            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"organization\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<organizations>");

            foreach (IOrganizationInfo item in list)
            {
                outString.Append(((OrganizationInfo)item).Serializable());
            }

            outString.Append("</organizations>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IOrganizationInfo param)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">��֯��Ϣ</param>
        public int SyncToActiveDirectory(IOrganizationInfo param)
        {
            return SyncToActiveDirectory(param, param.Name, param.GlobalName, param.ParentId);
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IOrganizationInfo param, string originalName, string originalGlobalName, string originalParentId)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">��֯��Ϣ</param>
        /// <param name="originalName">ԭʼ����</param>
        /// <param name="originalGlobalName">ԭʼȫ������</param>
        /// <param name="originalParentId">ԭʼ������ʶ</param>
        public int SyncToActiveDirectory(IOrganizationInfo param, string originalName, string originalGlobalName, string originalParentId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.GlobalName))
                {
                    // ��֯��${FullPath}��ȫ������Ϊ�գ�������������Ϣ��
                    return 1;
                }
                else if (string.IsNullOrEmpty(param.Name))
                {
                    // ��֯��${FullPath}������Ϊ�գ�������������Ϣ��
                    return 2;
                }
                else
                {
                    // 1.ԭʼ��ȫ�����Ʋ�Ϊ�ա�
                    // 2.Active Directory �������ض�����
                    if (!string.IsNullOrEmpty(originalGlobalName)
                        && ActiveDirectoryManagement.Instance.Group.IsExistName(originalGlobalName))
                    {
                        if (param.Name != originalName)
                        {
                            // ��֯��${GlobalName}�������Ʒ����ı䡣
                            ActiveDirectoryManagement.Instance.Organization.Rename(
                                    originalName,
                                    MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(originalParentId),
                                    param.Name);
                        }

                        if (param.GlobalName != originalGlobalName)
                        {
                            // ��֯��${GlobalName}����ȫ�����Ʒ����ı䡣
                            ActiveDirectoryManagement.Instance.Group.Rename(originalGlobalName, param.GlobalName);
                        }

                        if (param.ParentId != originalParentId)
                        {
                            // ��֯��${GlobalName}���ĸ����ڵ㷢���ı䡣
                            ActiveDirectoryManagement.Instance.Organization.MoveTo(
                                this.GetActiveDirectoryOUPathByOrganizationId(param.Id),
                                this.GetActiveDirectoryOUPathByOrganizationId(param.ParentId));
                        }

                        return 0;
                    }
                    else
                    {
                        ActiveDirectoryManagement.Instance.Organization.Add(param.Name, this.GetActiveDirectoryOUPathByOrganizationId(param.ParentId));

                        ActiveDirectoryManagement.Instance.Group.Add(param.GlobalName, this.GetActiveDirectoryOUPathByOrganizationId(param.Id));

                        // ��֯��${GlobalName}�������ɹ���
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

        #region 属性:SyncFromPackPage(IOrganizationInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">��֯��Ϣ</param>
        public int SyncFromPackPage(IOrganizationInfo param)
        {
            return this.provider.SyncFromPackPage(param);
        }
        #endregion

        // -------------------------------------------------------
        // �����ʺź���֯��ϵ
        // -------------------------------------------------------

        #region 属性:FindAllRelationByAccountId(string accountId)
        /// <summary>�����ʺŲ�ѯ������֯�Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>Table Columns��AccountId, OrganizationId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountOrganizationRelationInfo> FindAllRelationByAccountId(string accountId)
        {
            return this.provider.FindAllRelationByAccountId(accountId);
        }
        #endregion

        #region 属性:FindAllRelationByRoleId(string organizationId)
        /// <summary>������֯��ѯ�����ʺŵĹ�ϵ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns>Table Columns��AccountId, OrganizationId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountOrganizationRelationInfo> FindAllRelationByRoleId(string organizationId)
        {
            return this.provider.FindAllRelationByRoleId(organizationId);
        }
        #endregion

        #region 属性:AddRelation(string accountId, string organizationId)
        /// <summary>�����ʺ���������֯�Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public int AddRelation(string accountId, string organizationId)
        {
            return AddRelation(accountId, organizationId, false, DateTime.Now, DateTime.MaxValue);
        }
        #endregion

        #region 属性:AddRelation(string accountId, string organizationId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>�����ʺ���������֯�Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="isDefault">�Ƿ���Ĭ����֯</param>
        /// <param name="beginDate">����ʱ��</param>
        /// <param name="endDate">ͣ��ʱ��</param>
        public int AddRelation(string accountId, string organizationId, bool isDefault, DateTime beginDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                // �ʺű�ʶ����Ϊ��
                return 1;
            }

            if (string.IsNullOrEmpty(organizationId))
            {
                // ��֯��ʶ����Ϊ��
                return 2;
            }

            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IOrganizationInfo organization = MembershipManagement.Instance.OrganizationService[organizationId];

                // �ʺŶ������ʺŵ�ȫ�����ơ��ʺŵĵ�¼������֯��������֯��ȫ�����ƵȲ���Ϊ�ա�
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && organization != null && !string.IsNullOrEmpty(organization.GlobalName))
                {
                    ActiveDirectoryManagement.Instance.Group.AddRelation(account.LoginName, ActiveDirectorySchemaClassType.User, organization.GlobalName);
                }
            }

            return this.provider.AddRelation(accountId, organizationId, isDefault, beginDate, endDate);
        }
        #endregion

        #region 属性:AddRelationRange(string accountIds, string organizationId)
        /// <summary>�����ʺ���������֯�Ĺ�ϵ</summary>
        /// <param name="accountIds">�ʺű�ʶ�������Զ��Ÿ���</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public int AddRelationRange(string accountIds, string organizationId)
        {
            string[] list = accountIds.Split(new char[] { ',' });

            foreach (string accountId in list)
            {
                AddRelation(accountId, organizationId);
            }

            return 0;
        }
        #endregion

        #region 属性:AddParentRelations(string accountId, string organizationId)
        /// <summary>�����ʺ���������֯�ĸ�����֯��ϵ(�ݹ�)</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public int AddParentRelations(string accountId, string organizationId)
        {
            IOrganizationInfo organization = MembershipManagement.Instance.OrganizationService[organizationId];

            // [�ݴ�]������ɫ��ϢΪ�գ���ֹ������֯����
            if (organization != null && !string.IsNullOrEmpty(organization.ParentId) && organization.Parent != null)
            {
                // ���Ӹ���������ϵ
                AddRelation(accountId, organization.ParentId);

                // �ݹ����Ҹ���������ϵ
                AddParentRelations(accountId, organization.ParentId);
            }

            return 0;
        }
        #endregion

        #region 属性:ExtendRelation(string accountId, string organizationId, DateTime endDate)
        /// <summary>��Լ�ʺ���������֯�Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="endDate">�µĽ�ֹʱ��</param>
        public int ExtendRelation(string accountId, string organizationId, DateTime endDate)
        {
            return this.provider.ExtendRelation(accountId, organizationId, endDate);
        }
        #endregion

        #region 属性:RemoveRelation(string accountId, string organizationId)
        /// <summary>�Ƴ��ʺ���������֯�Ĺ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public int RemoveRelation(string accountId, string organizationId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IOrganizationInfo organization = MembershipManagement.Instance.OrganizationService[organizationId];

                // �ʺŶ������ʺŵ�ȫ�����ơ��ʺŵĵ�¼������֯��������֯��ȫ�����ƵȲ���Ϊ�ա�
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && organization != null && !string.IsNullOrEmpty(organization.GlobalName))
                {
                    ActiveDirectoryManagement.Instance.Group.RemoveRelation(account.LoginName, ActiveDirectorySchemaClassType.User, organization.GlobalName);
                }
            }

            return this.provider.RemoveRelation(accountId, organizationId);
        }
        #endregion

        #region 属性:RemoveDefaultRelation(string accountId)
        /// <summary>�Ƴ��ʺ�������֯��Ĭ�Ϲ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        public int RemoveDefaultRelation(string accountId)
        {
            return this.provider.RemoveDefaultRelation(accountId);
        }
        #endregion

        #region 属性:RemoveNondefaultRelation(string accountId)
        /// <summary>�Ƴ��ʺ�������֯�ķ�Ĭ�Ϲ�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        public int RemoveNondefaultRelation(string accountId)
        {
            return this.provider.RemoveNondefaultRelation(accountId);
        }
        #endregion

        #region 属性:RemoveExpiredRelation(string accountId)
        /// <summary>�Ƴ��ʺ��ѹ��ڵ���֯��ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        public int RemoveExpiredRelation(string accountId)
        {
            return this.provider.RemoveExpiredRelation(accountId);
        }
        #endregion

        #region 属性:RemoveAllRelation(string accountId)
        /// <summary>�Ƴ��ʺ�������֯�����й�ϵ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        public int RemoveAllRelation(string accountId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountOrganizationRelationInfo> list = FindAllRelationByAccountId(accountId);

                foreach (IAccountOrganizationRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.OrganizationId);
                }

                return 0;
            }
            else
            {
                return this.provider.RemoveAllRelation(accountId);
            }
        }
        #endregion

        #region 属性:HasDefaultRelation(string accountId)
        /// <summary>�����ʺŵ�Ĭ����֯</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        public bool HasDefaultRelation(string accountId)
        {
            return this.provider.HasDefaultRelation(accountId);
        }
        #endregion

        #region 属性:SetDefaultRelation(string accountId, string organizationId)
        /// <summary>�����ʺŵ�Ĭ����֯</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public int SetDefaultRelation(string accountId, string organizationId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IOrganizationInfo organization = MembershipManagement.Instance.OrganizationService[organizationId];

                if (account != null && organization != null)
                {
                    ActiveDirectoryManagement.Instance.Group.AddRelation(account.GlobalName, ActiveDirectorySchemaClassType.User, organization.Name);
                }
            }

            return this.provider.SetDefaultRelation(accountId, organizationId);
        }
        #endregion

        #region 属性:ClearupRelation(string organizationId)
        /// <summary>������֯���ʺŵĹ�ϵ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        public int ClearupRelation(string organizationId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountOrganizationRelationInfo> list = FindAllRelationByRoleId(organizationId);

                foreach (IAccountOrganizationRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.OrganizationId);
                }

                return 0;
            }
            else
            {
                return this.provider.ClearupRelation(organizationId);
            }
        }
        #endregion
    }
}