namespace X3Platform.Membership.IBLL
{
    using System;
    using System.Collections.Generic;

    using X3Platform;
    using X3Platform.Spring;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IMemberService")]
    public interface IMemberService
    {
        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="id">会员标识</param>
        /// <returns></returns>
        IMemberInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IAccount param)
        /// <summary>保存记录</summary>
        /// <param name="param">IAccount 实例详细信息</param>
        /// <returns>IAccount 实例详细信息</returns>
        IMemberInfo Save(IMemberInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号分开</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">会员标识</param>
        /// <returns>返回一个 MemberInfo 实例的详细信息</returns>
        IMemberInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByAccountId(string accountId)
        /// <summary>查询某条记录</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回一个 MemberInfo 实例的详细信息</returns>
        IMemberInfo FindOneByAccountId(string accountId);
        #endregion

        #region 函数:FindOneByLoginName(string loginName)
        /// <summary>查询某条记录</summary>
        /// <param name="loginName">登录名</param>
        /// <returns>返回一个 MemberInfo 实例的详细信息</returns>
        IMemberInfo FindOneByLoginName(string loginName);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        IList<IMemberInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        IList<IMemberInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        IList<IMemberInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllWithoutDefaultOrganizationUnit(int length)
        /// <summary>返回所有没有默认组织的成员信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        IList<IMemberInfo> FindAllWithoutDefaultOrganizationUnit(int length);
        #endregion

        #region 函数:FindAllWithoutDefaultJob(int length)
        /// <summary>返回所有没有默认职位的成员信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        IList<IMemberInfo> FindAllWithoutDefaultJob(int length);
        #endregion

        #region 函数:FindAllWithoutDefaultAssignedJob(int length)
        /// <summary>返回所有没有默认岗位的成员信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        IList<IMemberInfo> FindAllWithoutDefaultAssignedJob(int length);
        #endregion

        #region 函数:FindAllWithoutDefaultRole(int length)
        /// <summary>返回所有没有默认角色的成员信息</summary>
        /// <param name="length">条数, 0表示全部</param>
        /// <returns>返回所有<see cref="IMemberInfo" />实例的详细信息</returns>
        IList<IMemberInfo> FindAllWithoutDefaultRole(int length);
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
        /// <returns>返回一个列表实例<see cref="IMemberInfo"/></returns>
        IList<IMemberInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">人员标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:CreateEmptyMember(string accountId)
        /// <summary>创建空的人员信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns></returns>
        IMemberInfo CreateEmptyMember(string accountId);
        #endregion

        #region 函数:CombineFullPath(string name, string organizationId)
        /// <summary>成员全路径</summary>
        /// <param name="name">姓名</param>
        /// <param name="organizationId">所属组织标识</param>
        /// <returns></returns>
        string CombineFullPath(string name, string organizationId);
        #endregion

        #region 函数:SetContactCard(string accountId, Dictionary<string,string> contactItems);
        /// <summary>设置联系卡信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="contactItems">联系项字典</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        int SetContactCard(string accountId, Dictionary<string,string> contactItems);
        #endregion

        #region 函数:SetDefaultCorporationAndDepartments(string accountId, string organizationIds)
        /// <summary>设置默认组织单位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationIds">组织单位标识，[0]公司标识，[1]一级部门标识，[2]二级部门标识，[3]三级部门标识。</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        int SetDefaultCorporationAndDepartments(string accountId, string organizationIds);
        #endregion

        #region 函数:SetDefaultOrganizationUnit(string accountId, string organizationId)
        /// <summary>设置默认组织单位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="organizationId">组织单位标识</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        int SetDefaultOrganizationUnit(string accountId, string organizationId);
        #endregion

        #region 函数:SetDefaultRole(string accountId, string roleId)
        /// <summary>设置默认角色</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="roleId">角色标识</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        int SetDefaultRole(string accountId, string roleId);
        #endregion

        #region 函数:SetDefaultJob(string accountId, string jobId)
        /// <summary>设置默认职位信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="jobId">职位信息</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        int SetDefaultJob(string accountId, string jobId);
        #endregion

        #region 函数:SetDefaultAssignedJob(string accountId, string assignedJobId)
        /// <summary>设置默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        int SetDefaultAssignedJob(string accountId, string assignedJobId);
        #endregion

        #region 函数:SetDefaultJobGrade(string accountId, string jobGradeId)
        /// <summary>设置默认职级信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="jobGradeId">职级标识</param>
        /// <returns>修改成功,返回 0, 修改失败,返回 1.</returns>
        int SetDefaultJobGrade(string accountId, string jobGradeId);
        #endregion

        #region 函数:SyncFromPackPage(IMemberInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">成员信息</param>
        int SyncFromPackPage(IMemberInfo param);
        #endregion
    }
}
