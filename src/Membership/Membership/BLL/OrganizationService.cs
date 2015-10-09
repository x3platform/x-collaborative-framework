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
    using X3Platform.LDAP;
    using X3Platform.LDAP.Configuration;
    using X3Platform.CacheBuffer;
    using X3Platform.Configuration;
    using X3Platform.Data;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Spring;

    /// <summary></summary>
    public class OrganizationUnitService : IOrganizationUnitService
    {
        private MembershipConfiguration configuration = null;

        private IOrganizationUnitProvider provider = null;

        private Dictionary<string, IOrganizationUnitInfo> dictionary = new Dictionary<string, IOrganizationUnitInfo>();

        #region 构造函数:OrganizationUnitService()
        /// <summary>构造函数</summary>
        public OrganizationUnitService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IOrganizationUnitProvider>(typeof(IOrganizationUnitProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id">��֯��ʶ</param>
        /// <returns></returns>
        public IOrganizationUnitInfo this[string id]
        {
            get { return this.provider.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ�� �޸�
        // -------------------------------------------------------

        #region 属性:Save(AccountInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">IOrganizationUnitInfo ʵ����ϸ��Ϣ</param>
        /// <returns>IOrganizationUnitInfo ʵ����ϸ��Ϣ</returns>
        public IOrganizationUnitInfo Save(IOrganizationUnitInfo param)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IOrganizationUnitInfo originalObject = FindOne(param.Id);

                if (originalObject == null)
                {
                    originalObject = param;
                }

                SyncToLDAP(param, originalObject.Name, originalObject.GlobalName, originalObject.ParentId);
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
        /// <returns>����һ�� IOrganizationUnitInfo ʵ������ϸ��Ϣ</returns>
        public IOrganizationUnitInfo FindOne(string id)
        {
            IOrganizationUnitInfo param = null;

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
        /// <returns>����һ��<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationUnitInfo FindOneByGlobalName(string globalName)
        {
            return this.provider.FindOneByGlobalName(globalName);
        }
        #endregion

        #region 属性:FindOneByRoleId(string roleId)
        /// <summary>��ѯĳ����ɫ��������֯��Ϣ</summary>
        /// <param name="roleId">��ɫ��ʶ</param>
        /// <returns>��������<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationUnitInfo FindOneByRoleId(string roleId)
        {
            return this.provider.FindOneByRoleId(roleId);
        }
        #endregion

        #region 属性:FindOneByRoleId(string roleId, int level)
        /// <summary>��ѯĳ����ɫ������ĳһ���ε���֯��Ϣ</summary>
        /// <param name="roleId">��ɫ��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationUnitInfo FindOneByRoleId(string roleId, int level)
        {
            return this.provider.FindOneByRoleId(roleId, level);
        }
        #endregion

        #region 属性:FindCorporationByOrganizationUnitId(string id)
        /// <summary>��ѯĳ����֯�����Ĺ�˾��Ϣ</summary>
        /// <param name="id">��֯��ʶ</param>
        /// <returns>��������<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationUnitInfo FindCorporationByOrganizationUnitId(string id)
        {
            return this.provider.FindCorporationByOrganizationUnitId(id);
        }
        #endregion

        #region 属性:FindDepartmentByOrganizationUnitId(string organizationId, int level)
        /// <summary>��ѯĳ����֯������ĳ���ϼ�������Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IOrganizationUnitInfo FindDepartmentByOrganizationUnitId(string organizationId, int level)
        {
            return this.provider.FindDepartmentByOrganizationUnitId(organizationId, level);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� IOrganizationUnitInfo ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� IOrganizationUnitInfo ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� IOrganizationUnitInfo ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByParentId(string parentId)
        /// <summary>��ѯĳ�򸸽ڵ��µ�������֯��λ</summary>
        /// <param name="parentId">���ڱ�ʶ</param>
        /// <returns>��������ʵ��<see cref="IOrganizationUnitInfo"/>����ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAllByParentId(string parentId)
        {
            return FindAllByParentId(parentId, 0);
        }
        #endregion

        #region 属性:FindAllByParentId(string parentId, int depth)
        /// <summary>��ѯĳ�򸸽ڵ��µ�������֯��λ</summary>
        /// <param name="parentId">���ڱ�ʶ</param>
        /// <param name="depth">������ȡ�Ĳ��Σ�0��ʾֻ��ȡ�����Σ�-1��ʾȫ����ȡ</param>
        /// <returns>��������ʵ��<see cref="IOrganizationUnitInfo"/>����ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAllByParentId(string parentId, int depth)
        {
            // �����б�
            List<IOrganizationUnitInfo> list = new List<IOrganizationUnitInfo>();

            //
            // ������֯�Ӳ�����Ϣ
            //

            IList<IOrganizationUnitInfo> organizations = this.provider.FindAllByParentId(parentId);

            list.AddRange(organizations);

            if (depth == -1)
            {
                depth = int.MaxValue;
            }

            if (organizations.Count > 0 && depth > 0)
            {
                foreach (IOrganizationUnitInfo organization in organizations)
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
        /// <returns>����һ�� IOrganizationUnitInfo ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAllByAccountId(string accountId)
        {
            // ���� �Ƿ���������Ϣ
            if (string.IsNullOrEmpty(accountId) || accountId == "0")
            {
                return new List<IOrganizationUnitInfo>();
            }

            return this.provider.FindAllByAccountId(accountId);
        }
        #endregion

        #region 属性:FindAllByRoleIds(string roleIds)
        /// <summary>��ѯĳ����ɫ������������֯</summary>
        /// <param name="roleIds">��ɫ��ʶ</param>
        /// <returns>��������<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAllByRoleIds(string roleIds)
        {
            IList<IOrganizationUnitInfo> list = new List<IOrganizationUnitInfo>();

            string[] ids = roleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string id in ids)
            {
                IOrganizationUnitInfo organization = FindOneByRoleId(id);

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
        /// <returns>��������<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAllByRoleIds(string roleIds, int level)
        {
            IList<IOrganizationUnitInfo> list = new List<IOrganizationUnitInfo>();

            string[] ids = roleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string id in ids)
            {
                IOrganizationUnitInfo organization = FindOneByRoleId(id, level);

                if (organization != null)
                    list.Add(organization);
            }

            return list;
        }
        #endregion

        #region 属性:FindAllByCorporationId(string corporationId)
        /// <summary>�ݹ���ѯĳ����˾�������еĽ�ɫ</summary>
        /// <param name="corporationId">��֯��ʶ</param>
        /// <returns>��������<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAllByCorporationId(string corporationId)
        {
            // �����б�
            List<IOrganizationUnitInfo> list = new List<IOrganizationUnitInfo>();

            //
            // ���Ҳ���(��˾��һ����֯�ܹ�)
            //
            IList<IOrganizationUnitInfo> organizations = MembershipManagement.Instance.OrganizationUnitService.FindAllByParentId(corporationId);

            foreach (IOrganizationUnitInfo organization in organizations)
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
        /// <returns>����һ��<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindAllByProjectId(string projectId)
        {
            // 
            // ��Ŀ�Ŷӵı�ʶ �� ��Ŀ��ʶ ����һ��
            //

            string organizationId = projectId;

            IList<IOrganizationUnitInfo> list = FindAllByParentId(organizationId, 1);

            list.Add(FindOne(organizationId));

            return list;
        }
        #endregion

        #region 属性:FindCorporationsByAccountId(string accountId)
        /// <summary>��ѯĳ���ʻ����������й�˾��Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>��������<see cref="IOrganizationUnitInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IOrganizationUnitInfo> FindCorporationsByAccountId(string accountId)
        {
            IList<IOrganizationUnitInfo> corporations = new List<IOrganizationUnitInfo>();

            IMemberInfo member = MembershipManagement.Instance.MemberService[accountId];

            if (member != null && member.Corporation != null)
            {
                corporations.Add((OrganizationUnitInfo)member.Corporation);

                IList<IOrganizationUnitInfo> list = this.provider.FindCorporationsByAccountId(accountId);

                foreach (OrganizationUnitInfo item in list)
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

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        public IList<IOrganizationUnitInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
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
            string path = GetOrganizationPathByOrganizationUnitId(parentId);

            return string.Format(@"{0}{1}", path, name);
        }
        #endregion

        #region 属性:GetOrganizationPathByOrganizationUnitId(string organizationId)
        /// <summary>������֯��ʶ������֯��ȫ·��</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns></returns>
        public string GetOrganizationPathByOrganizationUnitId(string organizationId)
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

            IOrganizationUnitInfo param = FindOne(id);

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
            string path = this.GetLDAPOUPathByOrganizationUnitId(id);

            return string.Format("CN={0},{1}{2}", globalName, path, LDAPConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        #region 属性:GetLDAPOUPathByOrganizationUnitId(string organizationId)
        /// <summary>������֯��ʶ���� Active Directory OU ·��</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns></returns>
        public string GetLDAPOUPathByOrganizationUnitId(string organizationId)
        {
            return FormatLDAPPath(organizationId);
        }
        #endregion

        #region ˽�к���:FormatLDAPPath(string id)
        /// <summary>��ʽ�� Active Directory ·��</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string FormatLDAPPath(string id)
        {
            string path = string.Empty;

            string parentId = string.Empty;

            // OU������
            string name = null;

            IOrganizationUnitInfo param = FindOne(id);

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
                    name = LDAPConfigurationView.Instance.CorporationOrganizationUnitFolderRoot;
                }

                // 1.���Ʋ���Ϊ�� 2.����������ʶ����Ϊ�� 
                if (!string.IsNullOrEmpty(name)
                    && !string.IsNullOrEmpty(param.ParentId) && param.ParentId != Guid.Empty.ToString())
                {
                    parentId = param.ParentId;

                    path = FormatLDAPPath(parentId);

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

            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IOrganizationUnitInfo originalObject = FindOne(id);

                if (originalObject != null)
                {
                    // �����ⲿϵͳֱ��ͬ������Ա��Ȩ�޹��������ݿ��У�
                    // ���� Active Directory �ϲ���ֱ�Ӵ������ض�������Ҫ�ֹ�����ȫ�����Ʋ��������ض�����
                    if (!string.IsNullOrEmpty(originalObject.GlobalName)
                        && LDAPManagement.Instance.Group.IsExistName(originalObject.GlobalName))
                    {
                        LDAPManagement.Instance.Group.Rename(originalObject.GlobalName, globalName);
                    }
                    else
                    {
                        LDAPManagement.Instance.OrganizationUnit.Add(originalObject.Name, MembershipManagement.Instance.OrganizationUnitService.GetLDAPOUPathByOrganizationUnitId(originalObject.ParentId));

                        LDAPManagement.Instance.Group.Add(globalName, MembershipManagement.Instance.OrganizationUnitService.GetLDAPOUPathByOrganizationUnitId(originalObject.Id));
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

            IList<IOrganizationUnitInfo> listA = this.FindAllByParentId(organizationId);

            IList<IAccountInfo> listB = MembershipManagement.Instance.AccountService.FindAllByOrganizationUnitId(organizationId);

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

            string whereClause = string.Format(" ModifiedDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            IList<IOrganizationUnitInfo> list = MembershipManagement.Instance.OrganizationUnitService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"organization\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<organizations>");

            foreach (IOrganizationUnitInfo item in list)
            {
                outString.Append(((OrganizationUnitInfo)item).Serializable());
            }

            outString.Append("</organizations>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        #region 属性:SyncToLDAP(IOrganizationUnitInfo param)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">��֯��Ϣ</param>
        public int SyncToLDAP(IOrganizationUnitInfo param)
        {
            return SyncToLDAP(param, param.Name, param.GlobalName, param.ParentId);
        }
        #endregion

        #region 属性:SyncToLDAP(IOrganizationUnitInfo param, string originalName, string originalGlobalName, string originalParentId)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">��֯��Ϣ</param>
        /// <param name="originalName">ԭʼ����</param>
        /// <param name="originalGlobalName">ԭʼȫ������</param>
        /// <param name="originalParentId">ԭʼ������ʶ</param>
        public int SyncToLDAP(IOrganizationUnitInfo param, string originalName, string originalGlobalName, string originalParentId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
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
                        && LDAPManagement.Instance.Group.IsExistName(originalGlobalName))
                    {
                        if (param.Name != originalName)
                        {
                            // ��֯��${GlobalName}�������Ʒ����ı䡣
                            LDAPManagement.Instance.OrganizationUnit.Rename(
                                    originalName,
                                    MembershipManagement.Instance.OrganizationUnitService.GetLDAPOUPathByOrganizationUnitId(originalParentId),
                                    param.Name);
                        }

                        if (param.GlobalName != originalGlobalName)
                        {
                            // ��֯��${GlobalName}����ȫ�����Ʒ����ı䡣
                            LDAPManagement.Instance.Group.Rename(originalGlobalName, param.GlobalName);
                        }

                        if (param.ParentId != originalParentId)
                        {
                            // ��֯��${GlobalName}���ĸ����ڵ㷢���ı䡣
                            LDAPManagement.Instance.OrganizationUnit.MoveTo(
                                this.GetLDAPOUPathByOrganizationUnitId(param.Id),
                                this.GetLDAPOUPathByOrganizationUnitId(param.ParentId));
                        }

                        return 0;
                    }
                    else
                    {
                        LDAPManagement.Instance.OrganizationUnit.Add(param.Name, this.GetLDAPOUPathByOrganizationUnitId(param.ParentId));

                        LDAPManagement.Instance.Group.Add(param.GlobalName, this.GetLDAPOUPathByOrganizationUnitId(param.Id));

                        // ��֯��${GlobalName}�������ɹ���
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

        #region 属性:SyncFromPackPage(IOrganizationUnitInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">��֯��Ϣ</param>
        public int SyncFromPackPage(IOrganizationUnitInfo param)
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
        /// <returns>Table Columns��AccountId, OrganizationUnitId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountOrganizationUnitRelationInfo> FindAllRelationByAccountId(string accountId)
        {
            return this.provider.FindAllRelationByAccountId(accountId);
        }
        #endregion

        #region 属性:FindAllRelationByRoleId(string organizationId)
        /// <summary>������֯��ѯ�����ʺŵĹ�ϵ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns>Table Columns��AccountId, OrganizationUnitId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountOrganizationUnitRelationInfo> FindAllRelationByRoleId(string organizationId)
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

            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService[organizationId];

                // �ʺŶ������ʺŵ�ȫ�����ơ��ʺŵĵ�¼������֯��������֯��ȫ�����ƵȲ���Ϊ�ա�
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && organization != null && !string.IsNullOrEmpty(organization.GlobalName))
                {
                    LDAPManagement.Instance.Group.AddRelation(account.LoginName, LDAPSchemaClassType.User, organization.GlobalName);
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
            IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService[organizationId];

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
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService[organizationId];

                // �ʺŶ������ʺŵ�ȫ�����ơ��ʺŵĵ�¼������֯��������֯��ȫ�����ƵȲ���Ϊ�ա�
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && organization != null && !string.IsNullOrEmpty(organization.GlobalName))
                {
                    LDAPManagement.Instance.Group.RemoveRelation(account.LoginName, LDAPSchemaClassType.User, organization.GlobalName);
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
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountOrganizationUnitRelationInfo> list = FindAllRelationByAccountId(accountId);

                foreach (IAccountOrganizationUnitRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.OrganizationUnitId);
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
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService[organizationId];

                if (account != null && organization != null)
                {
                    LDAPManagement.Instance.Group.AddRelation(account.GlobalName, LDAPSchemaClassType.User, organization.Name);
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
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IList<IAccountOrganizationUnitRelationInfo> list = FindAllRelationByRoleId(organizationId);

                foreach (IAccountOrganizationUnitRelationInfo item in list)
                {
                    RemoveRelation(item.AccountId, item.OrganizationUnitId);
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