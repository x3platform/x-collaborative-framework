// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardOrganizationService.cs
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.DigitalNumber;
    using X3Platform.Data;

    /// <summary></summary>
    public class StandardGeneralRoleService : IStandardGeneralRoleService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IStandardGeneralRoleProvider provider = null;

        #region ���캯��:StandardGeneralRoleService()
        /// <summary>���캯��</summary>
        public StandardGeneralRoleService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IStandardGeneralRoleProvider>(typeof(IStandardGeneralRoleProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStandardGeneralRoleInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IStandardGeneralRoleInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</returns>
        public IStandardGeneralRoleInfo Save(IStandardGeneralRoleInfo param)
        {
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
        /// <returns>����ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IStandardGeneralRoleInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardGeneralRoleInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardGeneralRoleInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardGeneralRoleInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="groupTreeNodeId">�����ڵ���ʶ</param>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardGeneralRoleInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId)
        {
            return provider.FindAllByGroupTreeNodeId(groupTreeNodeId);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表<see cref="IStandardGeneralRoleInfo"/></returns>
        public IList<IStandardGeneralRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
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
        /// <param name="name">��׼ͨ�ý�ɫ����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 属性:GetMappingTable(string standardGeneralRoleId, string organizationId)
        /// <summary>����������֯�µĽ�ɫ�ͱ�׼ͨ�ý�ɫ��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��ʼʱ��</param>
        /// <param name="organizationId">�����ı�׼</param>
        public DataTable GetMappingTable(string standardGeneralRoleId, string organizationId)
        {
            DataTable table = new DataTable();

            IStandardGeneralRoleMappingRelationInfo relation = this.FindOneMappingRelation(standardGeneralRoleId, organizationId);

            IList<IRoleInfo> roles = MembershipManagement.Instance.RoleService.FindAllByOrganizationId(organizationId);

            table.Columns.Add("StandardGeneralRoleId");
            table.Columns.Add("OrganizationId");
            table.Columns.Add("RoleId");
            table.Columns.Add("RoleName");
            table.Columns.Add("IsMapping");

            for (int i = 0; i < roles.Count; i++)
            {
                DataRow row = table.NewRow();

                row["StandardGeneralRoleId"] = standardGeneralRoleId;
                row["OrganizationId"] = organizationId;
                row["RoleId"] = roles[i].Id;
                row["RoleName"] = roles[i].Name;
                row["IsMapping"] = "0";

                if (relation != null && roles[i].Id == relation.RoleId)
                {
                    row["IsMapping"] = "1";
                }

                table.Rows.Add(row);
            }

            return table;
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

            IList<IStandardGeneralRoleInfo> list = MembershipManagement.Instance.StandardGeneralRoleService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"standardGeneralRole\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<standardGeneralRoles>");

            foreach (IStandardGeneralRoleInfo item in list)
            {
                outString.Append(item.Serializable());
            }

            outString.Append("</standardGeneralRoles>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // ���ñ�׼ͨ�ý�ɫ����֯ӳ����ϵ
        // -------------------------------------------------------

        #region 属性:FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>���ұ�׼ͨ�ý�ɫ����֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public IStandardGeneralRoleMappingRelationInfo FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            return this.provider.FindOneMappingRelation(standardGeneralRoleId, organizationId);
        }
        #endregion

        #region 属性:GetMappingRelationPaging(int startIndex, int pageSize,  DataQuery query, out int rowCount)
        /// <summary>标准通用角色映射关系分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IStandardGeneralRoleMappingRelationInfo"/></returns>
        public IList<IStandardGeneralRoleMappingRelationInfo> GetMappingRelationPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetMappingRelationPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 属性:AddMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>���ӱ�׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public int AddMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            return this.AddMappingRelation(standardGeneralRoleId, organizationId, string.Empty);
        }
        #endregion

        #region 属性:AddMapping(string standardGeneralRoleId, string organizationId, string roleId)
        /// <summary>���ӱ�׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="roleId">��ɫ��ʶ</param>
        public int AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId)
        {
            if (this.HasMappingRelation(standardGeneralRoleId, organizationId))
            {
                // �Ѵ���ӳ����ϵ
                return 1;
            }

            IRoleInfo role = null;

            if (string.IsNullOrEmpty(roleId))
            {
                role = this.CreateNewRole(standardGeneralRoleId, organizationId);
            }
            else
            {
                role = MembershipManagement.Instance.RoleService.FindOne(roleId);
            }

            if (role == null)
            {
                // ���ؽ�ɫ��Ϣ������
                return 2;
            }

            return this.provider.AddMappingRelation(standardGeneralRoleId, organizationId, role.Id, role.StandardRoleId);
        }
        #endregion

        private IRoleInfo CreateNewRole(string standardGeneralRoleId, string organizationId)
        {
            IStandardGeneralRoleInfo standardGeneralRole = this.FindOne(standardGeneralRoleId);

            IOrganizationInfo organization = MembershipManagement.Instance.OrganizationService.FindOne(organizationId);

            RoleInfo role = new RoleInfo();

            role.Id = DigitalNumberContext.Generate("Key_Guid");
            role.Name = organization.GlobalName + "_" + standardGeneralRole.Name;
            role.OrganizationId = organizationId;
            role.StandardRoleId = string.Empty;
            role.Type = 65536; // �ڲ�������ɫ
            role.Locking = 0; // ������
            role.Status = 1;

            string name = role.Name;

            int count = 1;

            while (MembershipManagement.Instance.RoleService.IsExistName(role.Name))
            {
                role.Name = name + count++;

                // ����������ѭ��, ��ѭ������10��ʱ���ؿ�ֵ.
                if (count > 10) { return null; }
            }

            return MembershipManagement.Instance.RoleService.Save(role);
        }

        #region 属性:RemoveMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>�Ƴ���׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public int RemoveMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            return this.provider.RemoveMappingRelation(standardGeneralRoleId, organizationId);
        }
        #endregion

        #region 属性:HasMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>������׼ͨ�ý�ɫ��������֯�Ƿ���ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public bool HasMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            return this.provider.HasMappingRelation(standardGeneralRoleId, organizationId);
        }
        #endregion
    }
}
