// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :GroupTreeNodeService.cs
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
    using System.Text;

    using X3Platform.ActiveDirectory;
    using X3Platform.ActiveDirectory.Configuration;
    using X3Platform.Configuration;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;

    /// <summary></summary>
    public class GroupTreeNodeService : IGroupTreeNodeService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IGroupTreeNodeProvider provider = null;

        #region ���캯��:GroupTreeNodeService()
        /// <summary>���캯��</summary>
        public GroupTreeNodeService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IGroupTreeNodeProvider>(typeof(IGroupTreeNodeProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GroupTreeNodeInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(GroupTreeNodeInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="GroupTreeNodeInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="GroupTreeNodeInfo"/>��ϸ��Ϣ</returns>
        public GroupTreeNodeInfo Save(GroupTreeNodeInfo param)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                GroupTreeNodeInfo originalObject = FindOne(param.Id);

                if (originalObject == null)
                    originalObject = param;

                SyncToActiveDirectory(param, originalObject.Name, originalObject.ParentId);
            }

            // ����Ψһʶ������
            param.DistinguishedName = this.CombineDistinguishedName(param.Name, param.ParentId);

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
        /// <returns>����ʵ��<see cref="GroupTreeNodeInfo"/>����ϸ��Ϣ</returns>
        public GroupTreeNodeInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="GroupTreeNodeInfo"/>����ϸ��Ϣ</returns>
        public IList<GroupTreeNodeInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="GroupTreeNodeInfo"/>����ϸ��Ϣ</returns>
        public IList<GroupTreeNodeInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="GroupTreeNodeInfo"/>����ϸ��Ϣ</returns>
        public IList<GroupTreeNodeInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByParentId(string parentId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="parentId">���ڵ���ʶ</param>
        /// <returns>��������ʵ��<see cref="GroupTreeNodeInfo"/>����ϸ��Ϣ</returns>
        public IList<GroupTreeNodeInfo> FindAllByParentId(string parentId)
        {
            return provider.FindAllByParentId(parentId);
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
        /// <returns>����һ���б�ʵ��<see cref="GroupTreeNodeInfo"/></returns>
        public IList<GroupTreeNodeInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
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

        #region 属性:GetGroupTreeNodePathByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>���ݷ��������ڵ���ʶ����������ȫ·��</summary>
        /// <param name="groupTreeNodeId">���������ڵ���ʶ</param>
        /// <returns></returns>
        public string GetGroupTreeNodePathByGroupTreeNodeId(string groupTreeNodeId)
        {
            string path = FormatGroupTreeNodePath(groupTreeNodeId);

            return string.Format(@"{0}\", path);
        }
        #endregion

        #region ˽�к���:FormatTreeNodePath(string id)
        /// <summary>��ʽ����֯·��</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string FormatGroupTreeNodePath(string id)
        {
            string path = string.Empty;

            string parentId = string.Empty;

            string name = null;

            GroupTreeNodeInfo param = FindOne(id);

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
                        path = FormatGroupTreeNodePath(parentId);
                    }

                    path = string.IsNullOrEmpty(path) ? name : string.Format(@"{0}\{1}", path, name);

                    return path;
                }
            }

            return string.Empty;
        }
        #endregion

        #region 属性:CombineDistinguishedName(string name, string parentId)
        /// <summary>����Ψһ����</summary>
        /// <param name="name">��֯ȫ������</param>
        /// <param name="parentId">����������ʶ</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string name, string parentId)
        {
            string path = GetActiveDirectoryOUPathByGroupTreeNodeId(parentId);

            return string.Format("OU={0},{1}{2}", name, path, ActiveDirectoryConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        #region 属性:GetActiveDirectoryOUPathByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>���ݷ��������ڵ���ʶ���� Active Directory OU ·��</summary>
        /// <param name="groupTreeNodeId">���������ڵ���ʶ</param>
        /// <returns></returns>
        public string GetActiveDirectoryOUPathByGroupTreeNodeId(string groupTreeNodeId)
        {
            return FormatActiveDirectoryPath(groupTreeNodeId);
        }
        #endregion

        #region ˽�к���:FormatActiveDirectoryPath(string id)
        /// <summary>��ʽ�� Active Directory ·��</summary>
        /// <param name="groupTreeNodeId"></param>
        /// <returns></returns>
        private string FormatActiveDirectoryPath(string groupTreeNodeId)
        {
            string path = string.Empty;

            string parentId = string.Empty;

            // OU������
            string name = null;

            GroupTreeNodeInfo param = FindOne(groupTreeNodeId);

            if (param == null)
            {
                return string.Empty;
            }
            else
            {
                name = param.Name;

                // ��֯�ṹ�ĸ��ڵ�OU���⴦�� Ĭ��Ϊ��֯�ṹ
                //if (groupTreeNodeId == tree.RootTreeNodeId)
                //{
                //    name = tree.Name;
                //}

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

        #region 属性:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>�������ݰ�</summary>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        public string CreatePackage(DateTime beginDate, DateTime endDate)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" UpdateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            IList<GroupTreeNodeInfo> list = MembershipManagement.Instance.GroupTreeNodeService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"groupTree\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<groupTree>");

            foreach (GroupTreeNodeInfo item in list)
            {
                outString.Append(item.Serializable());
            }

            outString.Append("</groupTree>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IRoleInfo param)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">��ɫ��Ϣ</param>
        public int SyncToActiveDirectory(GroupTreeNodeInfo param)
        {
            return SyncToActiveDirectory(param, param.Name, param.ParentId);
        }
        #endregion

        #region 属性:SyncToActiveDirectory(IRoleInfo param, string originalName, string originalParentId)
        /// <summary>ͬ����Ϣ�� Active Directory</summary>
        /// <param name="param">����������Ϣ</param>
        /// <param name="originalName">ԭʼ������</param>
        /// <param name="originalParentId">ԭʼ�ĸ�����ʶ</param>
        public int SyncToActiveDirectory(GroupTreeNodeInfo param, string originalName, string originalParentId)
        {
            if (ActiveDirectoryConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.Name))
                {
                    // ��ɫ��${Name}������Ϊ�գ�������������Ϣ��
                    return 1;
                }
                else
                {
                    string parentPath = this.GetActiveDirectoryOUPathByGroupTreeNodeId(originalParentId);

                    // 1.ԭʼ��ȫ�����Ʋ�Ϊ�ա�
                    // 2.Active Directory �������ض�����
                    if (!string.IsNullOrEmpty(originalName)
                        && ActiveDirectoryManagement.Instance.Organization.IsExistName(originalName, parentPath))
                    {
                        if (param.Name != originalName)
                        {
                            // ����������${Name}�������Ʒ����ı䡣
                            ActiveDirectoryManagement.Instance.Organization.Rename(
                                    originalName,
                                    this.GetActiveDirectoryOUPathByGroupTreeNodeId(originalParentId),
                                    param.Name);
                        }

                        if (param.ParentId != originalParentId)
                        {
                            // ����������${Name}���ĸ����ڵ㷢���ı䡣
                            ActiveDirectoryManagement.Instance.Organization.MoveTo(
                                this.GetActiveDirectoryOUPathByGroupTreeNodeId(param.Id),
                                this.GetActiveDirectoryOUPathByGroupTreeNodeId(param.ParentId));
                        }

                        return 0;
                    }
                    else
                    {
                        ActiveDirectoryManagement.Instance.Organization.Add(param.Name, this.GetActiveDirectoryOUPathByGroupTreeNodeId(param.ParentId));

                        // ����������${Name}�������ɹ���
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

    }
}
