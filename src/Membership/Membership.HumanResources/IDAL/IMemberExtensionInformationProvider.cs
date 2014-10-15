using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;
using X3Platform.Membership.HumanResources.Model;

namespace X3Platform.Membership.HumanResources.IDAL
{
    [SpringObject("X3Platform.Membership.HumanResources.IDAL.IMemberExtensionInformationProvider")]
    public interface IMemberExtensionInformationProvider
    { 
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(MemberExtensionInformation param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="MemberExtensionInformation"/>详细信息</param>
        /// <returns>实例<see cref="MemberExtensionInformation"/>详细信息</returns>
        MemberExtensionInformation Save(MemberExtensionInformation param);
        #endregion

        #region 函数:Insert(MemberExtensionInformation param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="MemberExtensionInformation"/>详细信息</param>
        void Insert(MemberExtensionInformation param);
        #endregion

        #region 函数:Update(MemberExtensionInformation param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="MemberExtensionInformation"/>详细信息</param>
        void Update(MemberExtensionInformation param);
        #endregion

        #region 函数:Delete(string ids)
        ///<summary>删除记录</summary>
        ///<param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        ///<summary>查询某条记录</summary>
        ///<param name="accountId">标识</param>
        ///<returns>返回实例<see cref="MemberExtensionInformation"/>的详细信息</returns>
        MemberExtensionInformation FindOneByAccountId(string accountId);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="whereClause">SQL 查询条件</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有实例<see cref="MemberExtensionInformation"/>的详细信息</returns>
        IList<MemberExtensionInformation> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例<see cref="MemberExtensionInformation"/></returns>
        IList<MemberExtensionInformation> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        ///<summary>查询是否存在相关的记录.</summary>
        ///<param name="id">标识</param>
        ///<returns>布尔值</returns>
        bool IsExist(string id);
        #endregion
    }
}

