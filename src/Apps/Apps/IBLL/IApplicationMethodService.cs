namespace X3Platform.Apps.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.Web;

    using X3Platform.Spring;

    using X3Platform.Apps.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Apps.IBLL.IApplicationMethodService")]
    public interface IApplicationMethodService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApplicationMethodInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationMethodInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationMethodInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationMethodInfo"/>详细信息</returns>
        ApplicationMethodInfo Save(ApplicationMethodInfo param);
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
        /// <returns>返回实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        ApplicationMethodInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByName(string name)
        /// <summary>查询某条记录</summary>
        /// <param name="name">名称</param>
        /// <returns>返回实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        ApplicationMethodInfo FindOneByName(string name);
        #endregion

        #region 函数:FindAllByApplicationId(string applicationId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationId">应用唯一标识</param>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        IList<ApplicationMethodInfo> FindAllByApplicationId(string applicationId);
        #endregion

        #region 函数:FindAllByApplicationName(string applicationName)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationName">应用名称</param>
        /// <returns>返回所有实例<see cref="ApplicationMethodInfo"/>的详细信息</returns>
        IList<ApplicationMethodInfo> FindAllByApplicationName(string applicationName);
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
        /// <returns>返回一个列表实例<see cref="ApplicationMethodInfo"/></returns>
        IList<ApplicationMethodInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistCode(string code)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="code">代码</param>
        /// <returns>布尔值</returns>
        bool IsExistCode(string code);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:FetchNeededSyncData(DateTime beginDate, DateTime endDate)
        ///<summary>获取需要同步的数据</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        IList<ApplicationMethodInfo> FetchNeededSyncData(DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:SyncFromPackPage(ApplicationSettingGroupInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用请求路由信息</param>
        void SyncFromPackPage(ApplicationMethodInfo param);
        #endregion
    }
}
