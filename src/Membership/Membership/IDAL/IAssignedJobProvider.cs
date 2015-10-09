// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IAssignedJobProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IAssignedJobProvider")]
    public interface IAssignedJobProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(IAssignedJobInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IAssignedJobInfo"/>详细信息</param>
        /// <returns>实例<see cref="IAssignedJobInfo"/>详细信息</returns>
        IAssignedJobInfo Save(IAssignedJobInfo param);
        #endregion

        #region 函数:Insert(IAssignedJobInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IAssignedJobInfo"/>详细信息</param>
        void Insert(IAssignedJobInfo param);
        #endregion

        #region 函数:Update(IAssignedJobInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IAssignedJobInfo"/>详细信息</param>
        void Update(IAssignedJobInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        IAssignedJobInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IAssignedJobInfo"/>的详细信息</returns>
        IList<IAssignedJobInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户的所有岗位信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        IList<IAssignedJobInfo> FindAllByAccountId(string accountId);
        #endregion

        #region 函数:FindAllByOrganizationUnitId(string organizationId)
        /// <summary>查询某个组织下面所有的岗位</summary>
        /// <param name="organizationId">组织标识</param>
        /// <returns>返回一个 IAssignedJobInfo 实例的详细信息</returns>
        IList<IAssignedJobInfo> FindAllByOrganizationUnitId(string organizationId);
        #endregion

        #region 函数:FindAllPartTimeJobsByAccountId(string accountId)
        /// <summary>查询某个用户的兼职信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        IList<IAssignedJobInfo> FindAllPartTimeJobsByAccountId(string accountId);
        #endregion

        #region 函数:FindAllByRoleId(string roleId)
        /// <summary>查询某个角色的所对应岗位信息</summary>
        /// <param name="roleId">角色标识</param>
        /// <returns>返回所有<see cref="IAssignedJobInfo"/>实例的详细信息</returns>
        IList<IAssignedJobInfo> FindAllByRoleId(string roleId);
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
        /// <returns>返回一个列表实例<see cref="IAssignedJobInfo"/></returns>
        IList<IAssignedJobInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">岗位标识</param>
        /// <param name="name">岗位名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        int Rename(string id, string name);
        #endregion

        #region 函数:SetJobId(string assignedJobId, string jobId)
        /// <summary>设置岗位与相关职位的关系</summary>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="jobId">职位标识</param>
        int SetJobId(string assignedJobId, string jobId);
        #endregion

        #region 函数:SyncFromPackPage(IAssignedJobInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">岗位信息</param>
        int SyncFromPackPage(IAssignedJobInfo param);
        #endregion

        // -------------------------------------------------------
        // 设置帐号和岗位关系
        // -------------------------------------------------------

        #region 函数:AddRelation(string accountId, string assignedJobId, bool isDefault, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关岗位的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="isDefault">是否是默认角色</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        int AddRelation(string accountId, string assignedJobId, bool isDefault, DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:ExtendRelation(string accountId, string assignedJobId, DateTime endDate)
        /// <summary>续约帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        /// <param name="endDate">新的截止时间</param>
        int ExtendRelation(string accountId, string assignedJobId, DateTime endDate);
        #endregion

        #region 函数:RemoveRelation(string accountId, string assignedJobId)
        /// <summary>移除帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        int RemoveRelation(string accountId, string assignedJobId);
        #endregion

        #region 函数:RemoveDefaultRelation(string accountId)
        /// <summary>移除帐号相关岗位的默认关系(默认岗位)</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveDefaultRelation(string accountId);
        #endregion

        #region 函数:RemoveNondefaultRelation(string accountId)
        /// <summary>移除帐号相关岗位的非默认关系(兼职岗位)</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveNondefaultRelation(string accountId);
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的岗位关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveExpiredRelation(string accountId);
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关角色的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveAllRelation(string accountId);
        #endregion

        #region 函数:HasDefaultRelation(string accountId)
        /// <summary>检测帐号的默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        bool HasDefaultRelation(string accountId);
        #endregion

        #region 函数:SetDefaultRelation(string accountId, string assignedJobId)
        /// <summary>设置帐号的默认岗位</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="assignedJobId">岗位标识</param>
        int SetDefaultRelation(string accountId, string assignedJobId);
        #endregion
    }
}
