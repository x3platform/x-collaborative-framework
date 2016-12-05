namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.LDAP;
    using X3Platform.LDAP.Configuration;
    using X3Platform.Configuration;
    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    public class CatalogItemService : ICatalogItemService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private ICatalogItemProvider provider = null;

        #region 构造函数:CatalogItemService()
        /// <summary>构造函数</summary>
        public CatalogItemService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ICatalogItemProvider>(typeof(ICatalogItemProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatalogItemInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(CatalogItemInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="CatalogItemInfo"/>详细信息</param>
        /// <returns>实例<see cref="CatalogItemInfo"/>详细信息</returns>
        public CatalogItemInfo Save(CatalogItemInfo param)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                CatalogItemInfo originalObject = FindOne(param.Id);

                if (originalObject == null)
                    originalObject = param;

                SyncToLDAP(param, originalObject.Name, originalObject.ParentId);
            }

            // 设置唯一识别名称
            param.DistinguishedName = this.CombineDistinguishedName(param.Name, param.ParentId);

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
        /// <returns>返回实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        public CatalogItemInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        public IList<CatalogItemInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        public IList<CatalogItemInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        public IList<CatalogItemInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="parentId">父节点标识</param>
        /// <returns>返回所有实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        public IList<CatalogItemInfo> FindAllByParentId(string parentId)
        {
            return provider.FindAllByParentId(parentId);
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
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="CatalogItemInfo"/></returns>
        public IList<CatalogItemInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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

        #region 函数:GetCatalogItemPathByCatalogItemId(string catalogItemId)
        /// <summary>根据分组类别节点标识计算类别的全路径</summary>
        /// <param name="catalogItemId">分组类别节点标识</param>
        /// <returns></returns>
        public string GetCatalogItemPathByCatalogItemId(string catalogItemId)
        {
            string path = FormatCatalogItemPath(catalogItemId);

            return string.Format(@"{0}\", path);
        }
        #endregion

        #region 私有函数:FormatTreeNodePath(string id)
        /// <summary>格式化组织路径</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string FormatCatalogItemPath(string id)
        {
            string path = string.Empty;

            string parentId = string.Empty;

            string name = null;

            CatalogItemInfo param = FindOne(id);

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
                        path = FormatCatalogItemPath(parentId);
                    }

                    path = string.IsNullOrEmpty(path) ? name : string.Format(@"{0}\{1}", path, name);

                    return path;
                }
            }

            return string.Empty;
        }
        #endregion

        #region 函数:CombineDistinguishedName(string name, string parentId)
        /// <summary>组合唯一名称</summary>
        /// <param name="name">组织全局名称</param>
        /// <param name="parentId">父级对象标识</param>
        /// <returns></returns>
        public string CombineDistinguishedName(string name, string parentId)
        {
            string path = GetLDAPOUPathByCatalogItemId(parentId);

            return string.Format("OU={0},{1}{2}", name, path, LDAPConfigurationView.Instance.SuffixDistinguishedName);
        }
        #endregion

        #region 函数:GetLDAPOUPathByCatalogItemId(string catalogItemId)
        /// <summary>根据分组类别节点标识计算 Active Directory OU 路径</summary>
        /// <param name="groupTreeNodeId">分组类别节点标识</param>
        /// <returns></returns>
        public string GetLDAPOUPathByCatalogItemId(string catalogItemId)
        {
            return FormatLDAPPath(catalogItemId);
        }
        #endregion

        #region 私有函数:FormatLDAPPath(string id)
        /// <summary>格式化 Active Directory 路径</summary>
        /// <param name="groupTreeNodeId"></param>
        /// <returns></returns>
        private string FormatLDAPPath(string catalogItemId)
        {
            string path = string.Empty;

            string parentId = string.Empty;

            // OU的名称
            string name = null;

            CatalogItemInfo param = FindOne(catalogItemId);

            if (param == null)
            {
                return string.Empty;
            }
            else
            {
                name = param.Name;

                // 组织结构的根节点OU特殊处理 默认为组织结构
                //if (CatalogItemId == tree.RootTreeNodeId)
                //{
                //    name = tree.Name;
                //}

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

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        public string CreatePackage(DateTime beginDate, DateTime endDate)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" ModifiedDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            IList<CatalogItemInfo> list = MembershipManagement.Instance.CatalogItemService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"Catalog\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<catalog>");

            foreach (CatalogItemInfo item in list)
            {
                outString.Append(item.Serializable());
            }

            outString.Append("</catalog>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        #region 函数:SyncToLDAP(IRoleInfo param)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">角色信息</param>
        public int SyncToLDAP(CatalogItemInfo param)
        {
            return SyncToLDAP(param, param.Name, param.ParentId);
        }
        #endregion

        #region 函数:SyncToLDAP(IRoleInfo param, string originalName, string originalParentId)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">分组类别信息</param>
        /// <param name="originalName">原始的名称</param>
        /// <param name="originalParentId">原始的父级标识</param>
        public int SyncToLDAP(CatalogItemInfo param, string originalName, string originalParentId)
        {
            if (LDAPConfigurationView.Instance.IntegratedMode == "ON")
            {
                if (string.IsNullOrEmpty(param.Name))
                {
                    // 角色【${Name}】名称为空，请配置相关信息。
                    return 1;
                }
                else
                {
                    string parentPath = this.GetLDAPOUPathByCatalogItemId(originalParentId);

                    // 1.原始的全局名称不为空。
                    // 2.Active Directory 上有相关对象。
                    if (!string.IsNullOrEmpty(originalName)
                        && LDAPManagement.Instance.OrganizationUnit.IsExistName(originalName, parentPath))
                    {
                        if (param.Name != originalName)
                        {
                            // 分组类别【${Name}】的名称发生改变。
                            LDAPManagement.Instance.OrganizationUnit.Rename(
                                    originalName,
                                    this.GetLDAPOUPathByCatalogItemId(originalParentId),
                                    param.Name);
                        }

                        if (param.ParentId != originalParentId)
                        {
                            // 分组类别【${Name}】的父级节点发生改变。
                            LDAPManagement.Instance.OrganizationUnit.MoveTo(
                                this.GetLDAPOUPathByCatalogItemId(param.Id),
                                this.GetLDAPOUPathByCatalogItemId(param.ParentId));
                        }

                        return 0;
                    }
                    else
                    {
                        LDAPManagement.Instance.OrganizationUnit.Add(param.Name, this.GetLDAPOUPathByCatalogItemId(param.ParentId));

                        // 分组类别【${Name}】创建成功。
                        return 0;
                    }
                }
            }

            return 0;
        }
        #endregion

    }
}
