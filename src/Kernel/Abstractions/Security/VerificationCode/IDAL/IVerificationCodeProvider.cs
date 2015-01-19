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
using System;
using System.Collections.Generic;

using X3Platform.Spring;
using X3Platform.Security.VerificationCode.Configuration;
using X3Platform.Data;
#endregion

namespace X3Platform.Security.VerificationCode.IDAL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Security.VerificationCode.IDAL.IVerificationCodeProvider")]
    public interface IVerificationCodeProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(VerificationCodeInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="VerificationCodeInfo"/>详细信息</param>
        /// <returns>VerificationCodeInfo 实例详细信息</returns>
        VerificationCodeInfo Save(VerificationCodeInfo param);
        #endregion

        #region 函数:Insert(VerificationCodeInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">VerificationCodeInfo 实例的详细信息</param>
        void Insert(VerificationCodeInfo param);
        #endregion

        #region 函数:Update(VerificationCodeInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">VerificationCodeInfo 实例的详细信息</param>
        void Update(VerificationCodeInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识信息,多个以逗号分开.</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string objectType, string objectValue, string validationType)
        /// <summary>查询某条记录</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <returns>返回一个<see cref="VerificationCodeInfo"/>实例的详细信息</returns>
        VerificationCodeInfo FindOne(string objectType, string objectValue, string validationType);
        #endregion

        #region 属性:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 VerificationCodeInfo 实例的详细信息</returns>
        IList<VerificationCodeInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 属性:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件.</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
        IList<VerificationCodeInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}
