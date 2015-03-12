// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :INonstandardRoleService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Model;
    using System.Data;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IStandardGeneralRoleService")]
    public interface IStandardGeneralRoleService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IStandardGeneralRoleInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 属性:Save(IStandardGeneralRoleInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</returns>
        IStandardGeneralRoleInfo Save(IStandardGeneralRoleInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        IStandardGeneralRoleInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        IList<IStandardGeneralRoleInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        IList<IStandardGeneralRoleInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        IList<IStandardGeneralRoleInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="groupTreeNodeId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        IList<IStandardGeneralRoleInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId);
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
        /// <returns>返回一个列表<see cref="IStandardGeneralRoleInfo"/></returns>
        IList<IStandardGeneralRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">标准通用角色名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:GetMappingTable(string standardGeneralRoleId, string organizationId)
        /// <summary>查找所属组织下的角色和标准通用角色的映射关系</summary>
        /// <param name="standardGeneralRoleId">开始时间</param>
        /// <param name="organizationId">所属的标准</param>
        DataTable GetMappingTable(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        string CreatePackage(DateTime beginDate, DateTime endDate);
        #endregion

        // -------------------------------------------------------
        // 设置标准通用角色和组织映射关系
        // -------------------------------------------------------

        #region 函数:FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>查找标准通用角色与组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        IStandardGeneralRoleMappingRelationInfo FindOneMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 函数:GetMappingRelationPaging(int startIndex, int pageSize,  DataQuery query, out int rowCount)
        /// <summary>标准通用角色映射关系分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IStandardGeneralRoleMappingRelationInfo"/></returns>
        IList<IStandardGeneralRoleMappingRelationInfo> GetMappingRelationPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:AddMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>添加标准通用角色与相关组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        int AddMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 函数:AddMapping(string standardGeneralRoleId, string organizationId, string roleId)
        /// <summary>添加标准通用角色与相关组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="roleId">角色标识</param>
        int AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId);
        #endregion

        #region 函数:RemoveMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>移除标准通用角色与相关组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        int RemoveMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 函数:HasMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>检测标准通用角色与相关组织是否有映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        bool HasMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion
    }
}
