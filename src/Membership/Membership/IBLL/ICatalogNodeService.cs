using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Membership.Model;
using X3Platform.Data;

namespace X3Platform.Membership.IBLL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.ICatalogItemService")]
    public interface ICatalogItemService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CatalogItemInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(CatalogItemInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="CatalogItemInfo"/>详细信息</param>
        /// <returns>实例<see cref="CatalogItemInfo"/>详细信息</returns>
        CatalogItemInfo Save(CatalogItemInfo param);
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
        /// <returns>返回实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        CatalogItemInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        IList<CatalogItemInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        IList<CatalogItemInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        IList<CatalogItemInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="parentId">父节点标识</param>
        /// <returns>返回所有实例<see cref="CatalogItemInfo"/>的详细信息</returns>
        IList<CatalogItemInfo> FindAllByParentId(string parentId);
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
        IList<CatalogItemInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetCatalogItemPathByCatalogItemId(string CatalogItemId)
        /// <summary>根据分组类别节点标识计算类别的全路径</summary>
        /// <param name="CatalogItemId">分组类别节点标识</param>
        /// <returns></returns>
        string GetCatalogItemPathByCatalogItemId(string CatalogItemId);
        #endregion

        #region 函数:GetLDAPOUPathByCatalogItemId(string CatalogItemId)
        /// <summary>根据分组类别节点标识计算 Active Directory OU 路径</summary>
        /// <param name="CatalogItemId">分组类别节点标识</param>
        /// <returns></returns>
        string GetLDAPOUPathByCatalogItemId(string CatalogItemId);
        #endregion

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        string CreatePackage(DateTime beginDate, DateTime endDate);
        #endregion
    }
}
