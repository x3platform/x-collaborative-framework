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

namespace X3Platform.Security.VerificationCode.IBLL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Security.VerificationCode.IBLL.IVerificationCodeService")]
    public interface IVerificationCodeService
    {
        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(VerificationCodeInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="VerificationCodeInfo"/>详细信息</param>
        /// <returns>VerificationCodeInfo 实例详细信息</returns>
        VerificationCodeInfo Save(VerificationCodeInfo param);
        #endregion

        #region 属性:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
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

        #region 属性:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个 VerificationCodeInfo 列表实例</returns> 
        IList<VerificationCodeInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:Create(string objectType, string objectValue, string validationType)
        /// <summary>创建新的验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <returns>验证码对象</returns>
        VerificationCodeInfo Create(string objectType, string objectValue, string validationType);
        #endregion

        #region 函数:Create(string objectType, string objectValue, string validationType, string ip)
        /// <summary>创建新的验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="ip">IP 地址</param>
        /// <returns>验证码对象</returns>
        VerificationCodeInfo Create(string objectType, string objectValue, string validationType, string ip);
        #endregion
        
        #region 函数:Create(string objectType, string objectValue, string validationType, string ip, int length)
        /// <summary>创建新的验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="length">验证码长度</param>
        /// <returns>验证码对象</returns>
        VerificationCodeInfo Create(string objectType, string objectValue, string validationType, int length);
        #endregion

        #region 函数:Create(string objectType, string objectValue, string validationType, string ip, int length)
        /// <summary>创建新的验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="ip">IP 地址</param>
        /// <param name="length">验证码长度</param>
        /// <returns>验证码对象</returns>
        VerificationCodeInfo Create(string objectType, string objectValue, string validationType, string ip, int length);
        #endregion

        #region 函数:Validate(string objectType, string objectValue, string validationType, string code)
        /// <summary>校验验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="code">验证码</param>
        /// <returns>布尔值</returns>
        bool Validate(string objectType, string objectValue, string validationType, string code);
        #endregion

        #region 函数:Validate(string objectType, string objectValue, string validationType, string code, int availableMinutes)
        /// <summary>校验验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="code">验证码</param>
        /// <param name="availableMinutes">有效分钟数</param>
        /// <returns>布尔值</returns>
        bool Validate(string objectType, string objectValue, string validationType, string code, int availableMinutes);
        #endregion
    }
}
