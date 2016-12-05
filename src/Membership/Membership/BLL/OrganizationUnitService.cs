namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using X3Platform.CacheBuffer;
    using X3Platform.Configuration;
    using X3Platform.Data;
    using X3Platform.LDAP;
    using X3Platform.LDAP.Configuration;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    
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

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id">组织标识</param>
        /// <returns></returns>
        public IOrganizationUnitInfo this[string id]
        {
            get { return this.provider.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 属性:Save(IOrganizationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">IOrganizationInfo 实例详细信息</param>
        /// <returns>IOrganizationInfo 实例详细信息</returns>
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

            // 设置组织全路径
            param.FullPath = this.CombineFullPath(param.Name, param.ParentId);

            // 设置唯一识别名称
            param.DistinguishedName = this.CombineDistinguishedName(param.GlobalName, param.Id);

            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(int id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AccountInfo Id号</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
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

        #region 函数:FindOneByGlobalName(string globalName)
        /// <summary>查询某条记录</summary>
        /// <param name="globalName">组织的全局名称</param>
        /// <returns>返回一个<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        public IOrganizationUnitInfo FindOneByGlobalName(string globalName)
        {
            return this.provider.FindOneByGlobalName(globalName);
        }
        #endregion

        #region 函数:FindOneByRoleId(string roleId)
        /// <summary>查询某个角色所属的组织信息</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        public IOrganizationUnitInfo FindOneByRoleId(string roleId)
        {
            return this.provider.FindOneByRoleId(roleId);
        }
        #endregion

        #region 函数:FindOneByRoleId(string roleId, int level)
        /// <summary>查询某个角色所属的某一级次的组织信息</summary>
        /// <param name="roleId">角色标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        public IOrganizationUnitInfo FindOneByRoleId(string roleId, int level)
        {
            return this.provider.FindOneByRoleId(roleId, level);
        }
        #endregion

        #region 函数:FindCorporationByOrganizationUnitId(string id)
        /// <summary>查询某个组织所属的公司信息</summary>
        /// <param name="id">组织标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        public IOrganizationUnitInfo FindCorporationByOrganizationUnitId(string id)
        {
            return this.provider.FindCorporationByOrganizationUnitId(id);
        }
        #endregion

        #region 函数:FindDepartmentByOrganizationUnitId(string organizationId, int level)
        /// <summary>查询某个组织的所属某个上级部门信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        public IOrganizationUnitInfo FindDepartmentByOrganizationUnitId(string organizationId, int level)
        {
            return this.provider.FindDepartmentByOrganizationUnitId(organizationId, level);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 IOrganizationInfo 实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAll()
        {
            return this.provider.FindAll(string.Empty, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 IOrganizationInfo 实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAll(string whereClause)
        {
            return this.provider.FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 IOrganizationInfo 实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回所有实例<see cref="IOrganizationInfo"/>的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAllByParentId(string parentId)
        {
            return FindAllByParentId(parentId, 0);
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId, int depth)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <param name="depth">深入获取的层次，0表示只获取本层次，-1表示全部获取</param>
        /// <returns>返回所有实例<see cref="IOrganizationInfo"/>的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAllByParentId(string parentId, int depth)
        {
            // 结果列表
            List<IOrganizationUnitInfo> list = new List<IOrganizationUnitInfo>();

            //
            // 查找组织子部门信息
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

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAllByAccountId(string accountId)
        {
            // 过滤 非法的内容信息
            if (string.IsNullOrEmpty(accountId) || accountId == "0")
            {
                return new List<IOrganizationUnitInfo>();
            }

            return this.provider.FindAllByAccountId(accountId);
        }
        #endregion

        #region 函数:FindAllByRoleIds(string roleIds)
        /// <summary>查询某个角色的所属相关组织</summary>
        /// <param name="roleIds">角色标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
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

        #region 函数:FindAllByRoleIds(string roleIds)
        /// <summary>查询某个角色的所属相关组织</summary>
        /// <param name="roleIds">角色标识</param>
        /// <param name="level">层次</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
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

        #region 函数:FindAllByCorporationId(string corporationId)
        /// <summary>递归查询某个公司下面所有的角色</summary>
        /// <param name="corporationId">组织标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAllByCorporationId(string corporationId)
        {
            // 结果列表
            List<IOrganizationUnitInfo> list = new List<IOrganizationUnitInfo>();

            //
            // 查找部门(公司下一级组织架构)
            //
            IList<IOrganizationUnitInfo> organizations = MembershipManagement.Instance.OrganizationUnitService.FindAllByParentId(corporationId);

            foreach (IOrganizationUnitInfo organization in organizations)
            {
                list.Add(organization);

                // 获取项目团队以外的只能部门
                if (organization.Name.IndexOf("项目团队") == -1)
                {
                    list.AddRange(FindAllByParentId(organization.Id, -1));
                }
            }

            list.Add(FindOne(corporationId));

            return list;
        }
        #endregion

        #region 函数:FindAllByProjectId(string projectId)
        /// <summary>递归查询某个项目下面所有的角色</summary>
        /// <param name="projectId">组织标识</param>
        /// <returns>返回一个<see cref="IOrganizationInfo"/>实例的详细信息</returns>
        public IList<IOrganizationUnitInfo> FindAllByProjectId(string projectId)
        {
            // 
            // 项目团队的标识 和 项目标识 保存一致
            //

            string organizationId = projectId;

            IList<IOrganizationUnitInfo> list = FindAllByParentId(organizationId, 1);

            list.Add(FindOne(organizationId));

            return list;
        }
        #endregion

        #region 函数:FindCorporationsByAccountId(string accountId)
        /// <summary>查询某个帐户所属的所有公司信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IOrganizationInfo"/>实例的详细信息</returns>
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
        // 自定义功能
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

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">组织单位名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">组织单位全局名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistGlobalName(string globalName)
        {
            return this.provider.IsExistGlobalName(globalName);
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">组织标识</param>
        /// <param name="name">组织名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            // 检测是否存在对象
            if (!IsExist(id))
            {
                // 不存在对象
                return 1;
            }

            return this.provider.Rename(id, name);
        }
        #endregion

        #region 函数:CombineFullPath(string name, string parentId)
        /// <summary>组合全路径</summary>
        /// <param name="name">组织名称</param>
        /// <param name="parentId">父级对象标识</param>
        /// <returns></returns>
        public string CombineFullPath(string name, string parentId)
        {
            string path = GetOrganizationPathByOrganizationUnitId(parentId);

            return string.Format(@"{0}{1}", path, name);
        }
        #endregion

        #region 函数:GetOrganizationPathByOrganizationId(string organizationId)
        /// <summary>根据组织标识计算组织的全路径</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns></returns>
        public string GetOrganizationPathByOrganizationUnitId(string organizationId)
        {
            string path = FormatOrganizationPath(organizationId);

            return string.Format(@"{0}\", path);
        }
        #endregion

        #region 私有函数:FormatOrganizationPath(string id)
        /// <summary>格式化组织路径</summary>
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

        #region 函数:CombineDistinguishedName(string globalName, string id)
        /// <summary>组合唯一名称</summary>
        /// <param name="globalName">组织全局名称</param>
        /// <param name="id">对象标识</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string globalName, string id)
        {
            string path = this.GetLDAPOUPathByOrganizationUnitId(id);

            return string.Format("CN={0},{1}{2}", globalName, path, LDAPConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        #region 属性:GetLDAPOUPathByOrganizationUnitId(string organizationId)
        /// <summary>根据组织标识计算 Active Directory OU 路径</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns></returns>
        public string GetLDAPOUPathByOrganizationUnitId(string organizationId)
        {
            return FormatLDAPPath(organizationId);
        }
        #endregion

        #region 私有函数:FormatActiveDirectoryPath(string id)
        /// <summary>格式化 Active Directory 路径</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string FormatLDAPPath(string id)
        {
            string path = string.Empty;

            string parentId = string.Empty;

            // OU的名称
            string name = null;

            IOrganizationUnitInfo param = FindOne(id);

            if (param == null)
            {
                return string.Empty;
            }
            else
            {
                name = param.Name;

                // 组织结构的根节点OU特殊处理 默认为组织结构
                if (id == "00000000-0000-0000-0000-000000000001")
                {
                    name = LDAPConfigurationView.Instance.CorporationOrganizationUnitFolderRoot;
                }

                // 1.名称不能为空 2.父级对象标识不能为空 
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

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">帐户标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetGlobalName(string id, string globalName)
        {
            if (string.IsNullOrEmpty(globalName))
            {
                // 对象【${Id}】全局名称不能为空。
                return 1;
            }

            if (IsExistGlobalName(globalName))
            {
                return 2;
            }

            // 检测是否存在对象
            if (!IsExist(id))
            {
                // 对象【${Id}】不存在。
                return 3;
            }

            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IOrganizationUnitInfo originalObject = FindOne(id);

                if (originalObject != null)
                {
                    // 由于外部系统直接同步到人员及权限管理的数据库中，
                    // 所以 Active Directory 上不会直接创建相关对象，需要手工设置全局名称并创建相关对象。
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

        #region 函数:SetParentId(string id, string parentId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">组织标识</param>
        /// <param name="parentId">父级组织标识</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetParentId(string id, string parentId)
        {
            return this.provider.SetGlobalName(id, parentId);
        }
        #endregion

        #region 函数:SetExchangeStatus(string id, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="id">组织标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int SetExchangeStatus(string id, int status)
        {
            return this.provider.SetExchangeStatus(id, status);
        }
        #endregion

        #region 函数:GetChildNodes(string organizationId)
        /// <summary>获取组织的子成员</summary>
        /// <param name="organizationId">组织单位标识</param>
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

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
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

        #region 函数:SyncToLDAP(IOrganizationUnitInfo param)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">组织信息</param>
        public int SyncToLDAP(IOrganizationUnitInfo param)
        {
            return SyncToLDAP(param, param.Name, param.GlobalName, param.ParentId);
        }
        #endregion

        #region 函数:SyncToLDAP(IOrganizationUnitInfo param, string originalName, string originalGlobalName, string originalParentId)
        /// <summary>同步信息</summary>
        /// <param name="param">组织信息</param>
        /// <param name="originalName">原始名称</param>
        /// <param name="originalGlobalName">原始全局名称</param>
        /// <param name="originalParentId">原始父级标识</param>
        public int SyncToLDAP(IOrganizationUnitInfo param, string originalName, string originalGlobalName, string originalParentId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.GlobalName))
                {
                    // 组织【${FullPath}】全局名称为空，请配置相关信息。
                    return 1;
                }
                else if (string.IsNullOrEmpty(param.Name))
                {
                    // 组织【${FullPath}】名称为空，请配置相关信息。
                    return 2;
                }
                else
                {
                    // 1.原始的全局名称不为空。
                    // 2.Active Directory 上有相关对象。
                    if (!string.IsNullOrEmpty(originalGlobalName)
                        && LDAPManagement.Instance.Group.IsExistName(originalGlobalName))
                    {
                        if (param.Name != originalName)
                        {
                            // 组织【${GlobalName}】的名称发生改变。
                            LDAPManagement.Instance.OrganizationUnit.Rename(
                                    originalName,
                                    MembershipManagement.Instance.OrganizationUnitService.GetLDAPOUPathByOrganizationUnitId(originalParentId),
                                    param.Name);
                        }

                        if (param.GlobalName != originalGlobalName)
                        {
                            // 组织【${GlobalName}】的全局名称发生改变。
                            LDAPManagement.Instance.Group.Rename(originalGlobalName, param.GlobalName);
                        }

                        if (param.ParentId != originalParentId)
                        {
                            // 组织【${GlobalName}】的父级节点发生改变。
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

                        // 组织【${GlobalName}】创建成功。
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

        #region 函数:SyncFromPackPage(IOrganizationInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">组织信息</param>
        public int SyncFromPackPage(IOrganizationUnitInfo param)
        {
            return this.provider.SyncFromPackPage(param);
        }
        #endregion

        // -------------------------------------------------------
        // 设置帐号和组织关系
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

        #region 函数:FindAllRelationByRoleId(string organizationId)
        /// <summary>根据组织查询相关帐号的关系</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>Table Columns：AccountId, OrganizationId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountOrganizationUnitRelationInfo> FindAllRelationByRoleId(string organizationId)
        {
            return this.provider.FindAllRelationByRoleId(organizationId);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string organizationId)
        /// <summary>添加帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        public int AddRelation(string accountId, string organizationId)
        {
            return AddRelation(accountId, organizationId, false, DateTime.Now, DateTime.MaxValue);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string organizationId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="isDefault">是否是默认组织</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        public int AddRelation(string accountId, string organizationId, bool isDefault, DateTime beginDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                // 帐号标识不能为空
                return 1;
            }

            if (string.IsNullOrEmpty(organizationId))
            {
                // 组织标识不能为空
                return 2;
            }

            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService[organizationId];

                // 帐号对象、帐号的全局名称、帐号的登录名、组织对象、组织的全局名称等不能为空。
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && organization != null && !string.IsNullOrEmpty(organization.GlobalName))
                {
                    LDAPManagement.Instance.Group.AddRelation(account.LoginName, LDAPSchemaClassType.User, organization.GlobalName);
                }
            }

            return this.provider.AddRelation(accountId, organizationId, isDefault, beginDate, endDate);
        }
        #endregion

        #region 函数:AddRelationRange(string accountIds, string organizationId)
        /// <summary>添加帐号与相关组织的关系</summary>
        /// <param name="accountIds">帐号标识，多个以逗号隔开</param>
        /// <param name="organizationId">组织标识</param>
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

        #region 函数:AddParentRelations(string accountId, string organizationId)
        /// <summary>添加帐号与相关组织的父级组织关系(递归)</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        public int AddParentRelations(string accountId, string organizationId)
        {
            IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService[organizationId];

            // [容错]如果角色信息为空，中止相关组织设置
            if (organization != null && !string.IsNullOrEmpty(organization.ParentId) && organization.Parent != null)
            {
                // 添加父级对象关系
                AddRelation(accountId, organization.ParentId);

                // 递归查找父级对象关系
                AddParentRelations(accountId, organization.ParentId);
            }

            return 0;
        }
        #endregion

        #region 函数:ExtendRelation(string accountId, string organizationId, DateTime endDate)
        /// <summary>续约帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="endDate">新的截止时间</param>
        public int ExtendRelation(string accountId, string organizationId, DateTime endDate)
        {
            return this.provider.ExtendRelation(accountId, organizationId, endDate);
        }
        #endregion

        #region 函数:RemoveRelation(string accountId, string organizationId)
        /// <summary>移除帐号与相关组织的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
        public int RemoveRelation(string accountId, string organizationId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                IAccountInfo account = MembershipManagement.Instance.AccountService[accountId];

                IOrganizationUnitInfo organization = MembershipManagement.Instance.OrganizationUnitService[organizationId];

                // 帐号对象、帐号的全局名称、帐号的登录名、组织对象、组织的全局名称等不能为空。
                if (account != null && !string.IsNullOrEmpty(account.GlobalName) && !string.IsNullOrEmpty(account.LoginName)
                    && organization != null && !string.IsNullOrEmpty(organization.GlobalName))
                {
                    LDAPManagement.Instance.Group.RemoveRelation(account.LoginName, LDAPSchemaClassType.User, organization.GlobalName);
                }
            }

            return this.provider.RemoveRelation(accountId, organizationId);
        }
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关组织的默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveDefaultRelation(string accountId)
        {
            return this.provider.RemoveDefaultRelation(accountId);
        }
        #endregion

        #region 函数:RemoveNondefaultRelation(string accountId)
        /// <summary>移除帐号相关组织的非默认关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveNondefaultRelation(string accountId)
        {
            return this.provider.RemoveNondefaultRelation(accountId);
        }
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的组织关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveExpiredRelation(string accountId)
        {
            return this.provider.RemoveExpiredRelation(accountId);
        }
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关组织的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
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

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号的默认组织</summary>
        /// <param name="accountId">帐号标识</param>
        public bool HasDefaultRelation(string accountId)
        {
            return this.provider.HasDefaultRelation(accountId);
        }
        #endregion

        #region 函数:SetDefaultRelation(string accountId, string organizationId)
        /// <summary>设置帐号的默认组织</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织标识</param>
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

        #region 函数:ClearupRelation(string organizationId)
        /// <summary>清理组织与帐号的关系</summary>
        /// <param name="organizationId">组织标识</param>
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