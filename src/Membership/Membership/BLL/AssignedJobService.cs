namespace X3Platform.Membership.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    public class AssignedJobService : IAssignedJobService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IAssignedJobProvider provider = null;

        #region 构造函数:AssignedJobService()
        /// <summary>构造函数</summary>
        public AssignedJobService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IAssignedJobProvider>(typeof(IAssignedJobProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAssignedJobInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IAssignedJobInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IAssignedJobInfo"/>详细信息</param>
        /// <returns>实例<see cref="IAssignedJobInfo"/>详细信息</returns>
        public IAssignedJobInfo Save(IAssignedJobInfo param)
        {
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

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        public IAssignedJobInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        public IList<IAssignedJobInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        public IList<IAssignedJobInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        public IList<IAssignedJobInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户的所有岗位信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllByAccountId(string accountId)
        {
            return this.provider.FindAllByAccountId(accountId);
        }
        #endregion

        #region 函数:FindAllByOrganizationUnitId(string organizationId)
        /// <summary>查询某个组织下面所有的岗位</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllByOrganizationUnitId(string organizationId)
        {
            return this.FindAllByOrganizationUnitId(organizationId, 0);
        }
        #endregion

        #region 函数:FindAllByOrganizationUnitId(string organizationId, int depth)
        /// <summary>查询某个组织节点下的所有岗位信息</summary>
        /// <param name="organizationId">组织标识</param>
        /// <param name="depth">深入获取的层次，0表示只获取本层次，-1表示全部获取</param>
        /// <returns>返回所有实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllByOrganizationUnitId(string organizationId, int depth)
        {
            // 结果列表
            List<IAssignedJobInfo> list = new List<IAssignedJobInfo>();

            // -------------------------------------------------------
            // 查找组织子部门的角色信息
            // -------------------------------------------------------

            IList<IOrganizationUnitInfo> organizations = MembershipManagement.Instance.OrganizationUnitService.FindAllByParentId(organizationId);

            // -------------------------------------------------------
            // 查找角色信息
            // -------------------------------------------------------

            list.AddRange(this.provider.FindAllByOrganizationUnitId(organizationId));

            if (depth == -1)
            {
                depth = int.MaxValue;
            }

            if (organizations.Count > 0 && depth > 0)
            {
                foreach (IOrganizationUnitInfo organization in organizations)
                {
                    list.AddRange(this.FindAllByOrganizationUnitId(organization.Id, (depth - 1)));
                }
            }

            return list;
        }
        #endregion

        #region 函数:FindAllPartTimeJobsByAccountId(string accountId)
        /// <summary>查询某个用户的所有兼职信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllPartTimeJobsByAccountId(string accountId)
        {
            return this.provider.FindAllPartTimeJobsByAccountId(accountId);
        }
        #endregion

        #region 函数:FindAllByRoleId(string roleId)
        /// <summary>查询某个角色的所对应岗位信息</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        public IList<IAssignedJobInfo> FindAllByRoleId(string roleId)
        {
            return this.provider.FindAllByRoleId(roleId);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IAssignedJobInfo"/></returns>
        public IList<IAssignedJobInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return this.provider.IsExistName(name);
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">岗位标识</param>
        /// <param name="name">岗位名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            return this.provider.Rename(id, name);
        }
        #endregion

        #region 函数:SetJobId(string assignedJobId, string jobId)
        /// <summary>设置岗位与相关职位的关系</summary>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="jobId">职位标识</param>
        public int SetJobId(string assignedJobId, string jobId)
        {
            return this.provider.SetJobId(assignedJobId, jobId);
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

            IList<IAssignedJobInfo> list = MembershipManagement.Instance.AssignedJobService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"assignedJob\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<assignedJobs>");

            foreach (AssignedJobInfo item in list)
            {
                outString.Append(item.Serializable());
            }

            outString.Append("</assignedJobs>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion

        #region 函数:SyncFromPackPage(IAssignedJobInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">岗位信息</param>
        public int SyncFromPackPage(IAssignedJobInfo param)
        {
            this.provider.SyncFromPackPage(param);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 设置帐号和岗位关系
        // -------------------------------------------------------

        #region 函数:AddRelation(string accountId, string assignedJobId)
        /// <summary>添加帐号与相关岗位的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        public int AddRelation(string accountId, string assignedJobId)
        {
            return AddRelation(accountId, assignedJobId, false, DateTime.Now, DateTime.MaxValue);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string assignedJobId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关岗位的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="isDefault">是否是默认岗位</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        public int AddRelation(string accountId, string assignedJobId, bool isDefault, DateTime beginDate, DateTime endDate)
        {
            return this.provider.AddRelation(accountId, assignedJobId, isDefault, beginDate, endDate);
        }
        #endregion

        #region 函数:ExtendRelation(string accountId, string assignedJobId, DateTime endDate)
        /// <summary>续约帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="endDate">新的截止时间</param>
        public int ExtendRelation(string accountId, string assignedJobId, DateTime endDate)
        {
            return this.provider.ExtendRelation(accountId, assignedJobId, endDate);
        }
        #endregion

        #region 函数:RemoveRelation(string accountId, string assignedJobId)
        /// <summary>移除帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        public int RemoveRelation(string accountId, string assignedJobId)
        {
            return this.provider.RemoveRelation(accountId, assignedJobId);
        }
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关岗位的默认关系(默认岗位)</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveDefaultRelation(string accountId)
        {
            return this.provider.RemoveDefaultRelation(accountId);
        }
        #endregion

        #region 函数:RemoveNondefaultRelation(string accountId)
        /// <summary>移除帐号相关岗位的非默认关系(兼职岗位)</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveNondefaultRelation(string accountId)
        {
            return this.provider.RemoveNondefaultRelation(accountId);
        }
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的岗位关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveExpiredRelation(string accountId)
        {
            return this.provider.RemoveExpiredRelation(accountId);
        }
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关岗位的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveAllRelation(string accountId)
        {
            return this.provider.RemoveAllRelation(accountId);
        }
        #endregion

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号的默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        public bool HasDefaultRelation(string accountId)
        {
            return this.provider.HasDefaultRelation(accountId);
        }
        #endregion

        #region 函数:SetDefaultRelation(string accountId, string assignedJobId)
        /// <summary>设置帐号的默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        public int SetDefaultRelation(string accountId, string assignedJobId)
        {
            return this.provider.SetDefaultRelation(accountId, assignedJobId);
        }
        #endregion
    }
}
