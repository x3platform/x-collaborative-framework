#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
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

namespace X3Platform.Membership.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IAccountGrantProvider")]
    public interface IAccountGrantProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(IAccountGrantInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IAccountGrantInfo"/>详细信息</param>
        /// <returns>实例<see cref="IAccountGrantInfo"/>详细信息</returns>
        IAccountGrantInfo Save(IAccountGrantInfo param);
        #endregion

        #region 函数:Insert(IAccountGrantInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IAccountGrantInfo"/>详细信息</param>
        void Insert(IAccountGrantInfo param);
        #endregion

        #region 函数:Update(IAccountGrantInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IAccountGrantInfo"/>详细信息</param>
        void Update(IAccountGrantInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IAccountGrantInfo"/>的详细信息</returns>
        IAccountGrantInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IAccountGrantInfo"/>的详细信息</returns>
        IList<IAccountGrantInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IAccountGrantInfo"/></returns>
        IList<IAccountGrantInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>查询是否存在相关委托人的记录.</summary>
        /// <param name="grantorId">委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <returns>布尔值</returns>
        bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo);
        #endregion

        #region 函数:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>查询是否存在相关委托人的记录.</summary>
        /// <param name="grantorId">委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <param name="ignoreIds">忽略委托标识</param>
        /// <returns>布尔值</returns>
        bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds);
        #endregion

        #region 函数:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>查询是否存在相关被委托人的记录.</summary>
        /// <param name="granteeId">被委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <returns>布尔值</returns>
        bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo);
        #endregion

        #region 函数:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>查询是否存在相关被委托人的记录.</summary>
        /// <param name="granteeId">被委托人标识</param>
        /// <param name="grantedTimeFrom">委托开始时间</param>
        /// <param name="grantedTimeTo">委托结束时间</param>
        /// <param name="ignoreIds">忽略委托标识</param>
        /// <returns>布尔值</returns>
        bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds);
        #endregion

        #region 函数:Abort(string id)
        /// <summary>中止当前委托</summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        int Abort(string id);
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(IAccountGrantInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">帐号信息</param>
        int SyncFromPackPage(IAccountGrantInfo param);
        #endregion
    }
}
