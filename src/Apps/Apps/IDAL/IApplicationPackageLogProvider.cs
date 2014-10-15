// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IApplicationPackageLogProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Apps.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Apps.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Apps.IDAL.IApplicationPackageLogProvider")]
    public interface IApplicationPackageLogProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationPackageLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationPackageLogInfo"/>详细信息</returns>
        ApplicationPackageLogInfo Save(ApplicationPackageLogInfo param);
        #endregion

        #region 函数:Insert(ApplicationPackageLogInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageLogInfo"/>详细信息</param>
        void Insert(ApplicationPackageLogInfo param);
        #endregion

        #region 函数:Update(ApplicationPackageLogInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageLogInfo"/>详细信息</param>
        void Update(ApplicationPackageLogInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
        #endregion

        #region 函数:DeleteByApplicationId(string applicationId)
        /// <summary>删除某个应用的同步数据包发送记录</summary>
        /// <param name="applicationId">应用标识</param>
        void DeleteByApplicationId(string applicationId);
        #endregion

        #region 函数:DeleteAll()
        /// <summary>删除全部应用的同步数据包发送记录</summary>
        void DeleteAll();
        #endregion
        
        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        ApplicationPackageLogInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        IList<ApplicationPackageLogInfo> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例<see cref="ApplicationPackageLogInfo"/></returns>
        IList<ApplicationPackageLogInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetLatestPackageLogByApplicationId(string applicationId)
        /// <summary>根据应用标识查询最新的应用包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageLogInfo"/>的详细信息</returns>
        ApplicationPackageLogInfo GetLatestPackageLogByApplicationId(string applicationId);
        #endregion
    }
}
