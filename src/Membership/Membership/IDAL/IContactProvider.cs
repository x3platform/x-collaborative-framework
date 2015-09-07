using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Membership.Model;
using X3Platform.Data;

namespace X3Platform.Membership.IDAL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IContactProvider")]
    public interface IContactProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ContactInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ContactInfo"/>详细信息</param>
        /// <returns>实例<see cref="ContactInfo"/>详细信息</returns>
        ContactInfo Save(ContactInfo param);
        #endregion

        #region 函数:Insert(ContactInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ContactInfo"/>详细信息</param>
        void Insert(ContactInfo param);
        #endregion

        #region 函数:Update(ContactInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ContactInfo"/>详细信息</param>
        void Update(ContactInfo param);
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
        /// <returns>返回实例<see cref="ContactInfo"/>的详细信息</returns>
        ContactInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ContactInfo"/>的详细信息</returns>
        IList<ContactInfo> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例<see cref="ContactInfo"/></returns>
        IList<ContactInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
