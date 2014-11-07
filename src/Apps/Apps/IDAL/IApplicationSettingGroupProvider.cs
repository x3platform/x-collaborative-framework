namespace X3Platform.Apps.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Apps.IDAL.IApplicationSettingGroupProvider")]
    public interface IApplicationSettingGroupProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationSettingGroupInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingGroupInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationSettingGroupInfo"/>详细信息</returns>
        ApplicationSettingGroupInfo Save(ApplicationSettingGroupInfo param);
        #endregion

        #region 函数:Insert(ApplicationSettingGroupInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingGroupInfo"/>详细信息</param>
        void Insert(ApplicationSettingGroupInfo param);
        #endregion

        #region 函数:Update(ApplicationSettingGroupInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingGroupInfo"/>详细信息</param>
        void Update(ApplicationSettingGroupInfo param);
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
        /// <returns>返回实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        ApplicationSettingGroupInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        IList<ApplicationSettingGroupInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="parentId">父级对象的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        IList<ApplicationSettingGroupInfo> FindAllByParentId(string parentId);
        #endregion

        #region 函数:FindAllByApplicationId(string applicationId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingGroupInfo"/>的详细信息</returns>
        IList<ApplicationSettingGroupInfo> FindAllByApplicationId(string applicationId);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationSettingGroupInfo"/></returns>
        IList<ApplicationSettingGroupInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">应用名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(ApplicationSettingGroupInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用参数分组信息</param>
        void SyncFromPackPage(ApplicationSettingGroupInfo param);
        #endregion
    }
}
