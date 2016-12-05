namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using X3Platform.Spring;
    using X3Platform.Data;
    using X3Platform.DigitalNumber;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
   
    /// <summary></summary>
    public class StandardGeneralRoleService : IStandardGeneralRoleService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IStandardGeneralRoleProvider provider = null;

        #region 构造函数:StandardGeneralRoleService()
        /// <summary>构造函数</summary>
        public StandardGeneralRoleService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IStandardGeneralRoleProvider>(typeof(IStandardGeneralRoleProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStandardGeneralRoleInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IStandardGeneralRoleInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IStandardGeneralRoleInfo"/>详细信息</param>
        /// <returns>实例<see cref="IStandardGeneralRoleInfo"/>详细信息</returns>
        public IStandardGeneralRoleInfo Save(IStandardGeneralRoleInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        public IStandardGeneralRoleInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        public IList<IStandardGeneralRoleInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        public IList<IStandardGeneralRoleInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        public IList<IStandardGeneralRoleInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByCatalogItemId(string CatalogItemId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="catalogItemId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        public IList<IStandardGeneralRoleInfo> FindAllByCatalogItemId(string catalogItemId)
        {
            return provider.FindAllByCatalogItemId(catalogItemId);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
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

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">标准通用角色名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 函数:GetMappingTable(string standardGeneralRoleId, string organizationId)
        /// <summary>查找所属组织下的角色和标准通用角色的映射关系</summary>
        /// <param name="standardGeneralRoleId">开始时间</param>
        /// <param name="organizationId">所属的标准</param>
        public DataTable GetMappingTable(string standardGeneralRoleId, string organizationId)
        {
            DataTable table = new DataTable();

            IStandardGeneralRoleMappingRelationInfo relation = this.FindOneMappingRelation(standardGeneralRoleId, organizationId);

            IList<IRoleInfo> roles = MembershipManagement.Instance.RoleService.FindAllByOrganizationUnitId(organizationId);

            table.Columns.Add("StandardGeneralRoleId");
            table.Columns.Add("OrganizationUnitId");
            table.Columns.Add("RoleId");
            table.Columns.Add("RoleName");
            table.Columns.Add("IsMapping");

            for (int i = 0; i < roles.Count; i++)
            {
                DataRow row = table.NewRow();

                row["StandardGeneralRoleId"] = standardGeneralRoleId;
                row["OrganizationUnitId"] = organizationId;
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

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        public string CreatePackage(DateTime beginDate, DateTime endDate)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" ModifiedDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

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
        // 设置标准通用角色和组织映射关系
        // -------------------------------------------------------

        #region 函数:FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>查找标准通用角色与组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
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

        #region 函数:AddMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>添加标准通用角色与相关组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        public int AddMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            return this.AddMappingRelation(standardGeneralRoleId, organizationId, string.Empty);
        }
        #endregion

        #region 函数:AddMapping(string standardGeneralRoleId, string organizationId, string roleId)
        /// <summary>添加标准通用角色与相关组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="roleId">角色标识</param>
        public int AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId)
        {
            if (this.HasMappingRelation(standardGeneralRoleId, organizationId))
            {
                // 已存在映射关系
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
                // 相关角色信息不存在
                return 2;
            }

            return this.provider.AddMappingRelation(standardGeneralRoleId, organizationId, role.Id, role.StandardRoleId);
        }
        #endregion

        private IRoleInfo CreateNewRole(string standardGeneralRoleId, string organizationId)
        {
            IStandardGeneralRoleInfo standardGeneralRole = this.FindOne(standardGeneralRoleId);

            IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService.FindOne(organizationId);

            RoleInfo role = new RoleInfo();

            role.Id = DigitalNumberContext.Generate("Key_Guid");
            role.Name = organization.GlobalName + "_" + standardGeneralRole.Name;
            role.OrganizationUnitId = organizationId;
            role.StandardRoleId = string.Empty;
            role.Type = 65536; // 内部虚拟角色
            role.Locking = 0; // 非锁定
            role.Status = 1;

            string name = role.Name;

            int count = 1;

            while (MembershipManagement.Instance.RoleService.IsExistName(role.Name))
            {
                role.Name = name + count++;

                // 避免陷入死循环, 当循环超过10次时返回空值.
                if (count > 10) { return null; }
            }

            return MembershipManagement.Instance.RoleService.Save(role);
        }

        #region 函数:RemoveMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>移除标准通用角色与相关组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        public int RemoveMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            return this.provider.RemoveMappingRelation(standardGeneralRoleId, organizationId);
        }
        #endregion

        #region 函数:HasMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>检测标准通用角色与相关组织是否有映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        public bool HasMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            return this.provider.HasMappingRelation(standardGeneralRoleId, organizationId);
        }
        #endregion
    }
}
