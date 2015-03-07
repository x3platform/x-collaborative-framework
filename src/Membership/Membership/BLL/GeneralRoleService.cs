// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :GeneralRoleService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Membership.Configuration;
using X3Platform.Membership.IBLL;
using X3Platform.Membership.IDAL;
using X3Platform.Membership.Model;
using X3Platform.Configuration;
using X3Platform.ActiveDirectory.Configuration;
using X3Platform.ActiveDirectory;

namespace X3Platform.Membership.BLL
{
    /// <summary></summary>
    public class GeneralRoleService : IGeneralRoleService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IGeneralRoleProvider provider = null;

        #region ���캯��:GeneralRoleService()
        /// <summary>���캯��</summary>
        public GeneralRoleService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IGeneralRoleProvider>(typeof(IGeneralRoleProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GeneralRoleInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(GeneralRoleInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="GeneralRoleInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="GeneralRoleInfo"/>��ϸ��Ϣ</returns>
        public GeneralRoleInfo Save(GeneralRoleInfo param)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                GeneralRoleInfo originalObject = FindOne(param.Id);

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

            return provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="GeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public GeneralRoleInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="GeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<GeneralRoleInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="GeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<GeneralRoleInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="GeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<GeneralRoleInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="groupTreeNodeId">�����ڵ���ʶ</param>
        /// <returns>��������ʵ��<see cref="GeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<GeneralRoleInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId)
        {
            return provider.FindAllByGroupTreeNodeId(groupTreeNodeId);
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
        /// <returns>����һ���б�ʵ��<see cref="GeneralRoleInfo"/></returns>
        public IList<GeneralRoleInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="name">ͨ�ý�ɫ����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 属性:IsExistGlobalName(string globalName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="globalName">ͨ�ý�ɫȫ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGlobalName(string globalName)
        {
            return provider.IsExistGlobalName(globalName);
        }
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">Ⱥ����ʶ</param>
        /// <param name="name">Ⱥ������</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        public int Rename(string id, string name)
        {
            return provider.Rename(id, name);
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
                GeneralRoleInfo originalObject = FindOne(id);

                if (originalObject != null)
                {
                    // �����ⲿϵͳֱ��ͬ������Ա��Ȩ�޹��������ݿ��У�
                    // ���� Active Directory �ϲ���ֱ�Ӵ������ض�������Ҫ�ֹ�����ȫ�����Ʋ��������ض�����
                    if (!string.IsNullOrEmpty(originalObject.GlobalName) 
                        && ActiveDirectoryManagement.Instance.Group.IsExistName(globalName))
                    {
                        ActiveDirectoryManagement.Instance.Group.Rename(originalObject.GlobalName, globalName);
                    }
                    else
                    {
                        ActiveDirectoryManagement.Instance.Organization.Add(originalObject.Name, MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(originalObject.ParentId));

                        ActiveDirectoryManagement.Instance.Group.Add(globalName, MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(originalObject.Id));
                    }
                }
                 */
            }

            return provider.SetGlobalName(id, globalName);
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
        public int SyncToActiveDirectory(GeneralRoleInfo param)
        {
            return SyncToActiveDirectory(param, param.GlobalName, param.GroupTreeNodeId);
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IGroupInfo param, string originalGlobalName, string originalGroupTreeNodeId)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">��ɫ��Ϣ</param>
        /// <param name="originalGlobalName">ԭʼ��ȫ������</param>
        /// <param name="originalGroupTreeNodeId">ԭʼ�ķ�����ʶ</param>
        public int SyncToActiveDirectory(GeneralRoleInfo param, string originalGlobalName, string originalGroupTreeNodeId)
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

        #region 属性:SyncFromPackPage(GeneralRoleInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">Ⱥ����Ϣ</param>
        public int SyncFromPackPage(GeneralRoleInfo param)
        {
            return provider.SyncFromPackPage(param);
        }
        #endregion

    }
}
