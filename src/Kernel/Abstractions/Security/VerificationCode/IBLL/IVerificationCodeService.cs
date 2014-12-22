#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

#region Using Libraries
using System.Collections.Generic;

using X3Platform.Data;
using X3Platform.Spring;
#endregion

namespace X3Platform.Security.Authority.IBLL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Security.Authority.IBLL.IAuthorityService")]
    public interface IAuthorityService
    {
        #region 索引:this[string name]
        /// <summary>索引</summary>
        /// <param name="name">权限名称</param>
        /// <returns></returns>
        AuthorityInfo this[string name] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(AuthorityInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="AuthorityInfo"/>详细信息</param>
        /// <returns>AuthorityInfo 实例详细信息</returns>
        AuthorityInfo Save(AuthorityInfo param);
        #endregion

        #region 属性:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AuthorityInfo id号</param>
        /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
        AuthorityInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">权限名称</param>
        /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
        AuthorityInfo FindOneByName(string name);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 AuthorityInfo 实例的详细信息</returns>
        IList<AuthorityInfo> FindAll();
        #endregion

        #region 属性:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 AuthorityInfo 实例的详细信息</returns>
        IList<AuthorityInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 属性:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个 AuthorityInfo 列表实例</returns> 
        IList<AuthorityInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
