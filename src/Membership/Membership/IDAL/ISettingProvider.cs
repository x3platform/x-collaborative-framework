// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ISettingProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.IDAL
{
    using System.Collections.Generic;

    using X3Platform.Membership.Model;
    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.ISettingProvider")]
    public interface ISettingProvider
    {
        //-------------------------------------------------------
        // 保存 添加 修改 删除
        //-------------------------------------------------------

        #region 函数:Save(SettingInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="SettingInfo"/>详细信息</param>
        /// <returns>实例<see cref="SettingInfo"/>详细信息</returns>
        SettingInfo Save(SettingInfo param);
        #endregion

        #region 函数:Insert(SettingInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="SettingInfo"/>详细信息</param>
        void Insert(SettingInfo param);
        #endregion

        #region 函数:Update(SettingInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="SettingInfo"/>详细信息</param>
        void Update(SettingInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="SettingInfo"/>的详细信息</returns>
        SettingInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        IList<SettingInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllBySettingGroupId(string applicationSettingGroupId, string keyword)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupId">参数分组标识</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        IList<SettingInfo> FindAllBySettingGroupId(string applicationSettingGroupId, string keyword);
        #endregion

        #region 函数:FindAllBySettingGroupName(string applicationSettingGroupName, string keyword)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupName">参数分组名称</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        IList<SettingInfo> FindAllBySettingGroupName(string applicationSettingGroupName, string keyword);
        #endregion
        
        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="SettingInfo"/></returns>
        IList<SettingInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetValue(string applicationId, string applicationSettingGroupName, string value)
        /// <summary>根据配置的文本获取值信息</summary>
        /// <param name="applicationSettingGroupName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetText(string applicationSettingGroupName, string value);
        #endregion

        #region 函数:GetValue(string applicationId, string applicationSettingGroupName, string text)
        /// <summary>根据配置的文本获取值信息</summary>
        /// <param name="applicationSettingGroupName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        string GetValue(string applicationSettingGroupName, string text);
        #endregion
    }
}
