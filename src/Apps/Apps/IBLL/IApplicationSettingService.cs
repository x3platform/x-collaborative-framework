namespace X3Platform.Apps.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Apps.Model;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Apps.IBLL.IApplicationSettingService")]
    public interface IApplicationSettingService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApplicationSettingInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationSettingInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationSettingInfo"/>详细信息</returns>
        ApplicationSettingInfo Save(ApplicationSettingInfo param);
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
        /// <returns>返回实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        ApplicationSettingInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        IList<ApplicationSettingInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        IList<ApplicationSettingInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        IList<ApplicationSettingInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllQueryObject(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationMenuQueryInfo"/>的详细信息</returns>
        IList<ApplicationSettingQueryInfo> FindAllQueryObject(string whereClause, int length);
        #endregion

        #region 函数:FindAllByApplicationSettingGroupId(string applicationSettingGroupId)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupId">参数分组标识</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupId(string applicationSettingGroupId);
        #endregion

        #region 函数:FindAllByApplicationSettingGroupId(string applicationSettingGroupId, string keyword)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupId">参数分组标识</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupId(string applicationSettingGroupId, string keyword);
        #endregion

        #region 函数:FindAllByApplicationSettingGroupName(string applicationSettingGroupName)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupName">参数分组名称</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupName(string applicationSettingGroupName);
        #endregion

        #region 函数:FindAllByApplicationSettingGroupName(string applicationSettingGroupName, string keyword)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupName">参数分组名称</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupName(string applicationSettingGroupName, string keyword);
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
        /// <returns>返回一个列表实例<see cref="ApplicationSettingInfo"/></returns>
        IList<ApplicationSettingInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy,out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationSettingQueryInfo"/></returns>
        IList<ApplicationSettingQueryInfo> GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetValue(string applicationId, string applicationSettingGroupName, string value)
        /// <summary>根据配置的文本获取值信息</summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationSettingGroupName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetText(string applicationId, string applicationSettingGroupName, string value);
        #endregion

        #region 函数:GetValue(string applicationId, string applicationSettingGroupName, string text)
        /// <summary>根据配置的文本获取值信息</summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationSettingGroupName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        string GetValue(string applicationId, string applicationSettingGroupName, string text);
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        ///<summary>获取需要同步的数据</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        IList<ApplicationSettingInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:SyncFromPackPage(ApplicationSettingInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用参数信息</param>
        void SyncFromPackPage(ApplicationSettingInfo param);
        #endregion
    }
}
