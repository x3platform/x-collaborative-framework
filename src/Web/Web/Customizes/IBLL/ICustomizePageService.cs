namespace X3Platform.Web.Customizes.IBLL
{
    #region Using Libraries
  using System.Collections.Generic;
  using X3Platform.Data;
  using X3Platform.Spring;
  using X3Platform.Web.Customizes.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Web.Customizes.IBLL.ICustomizePageService")]
    public interface ICustomizePageService
    {
        #region 索引:this[string index]
        /// <summary>索引</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        CustomizePageInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(CustomizePageInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">CustomizePageInfo 实例详细信息</param>
        /// <returns>CustomizePageInfo 实例详细信息</returns>
        CustomizePageInfo Save(CustomizePageInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="keys">标识,多个以逗号隔开</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">CustomizePageInfo Id号</param>
        /// <returns>返回一个 CustomizePageInfo 实例的详细信息</returns>
        CustomizePageInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>查询某条记录</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="name">页面名称</param>
        /// <returns>返回一个实例<see cref="CustomizePageInfo"/>的详细信息</returns>
        CustomizePageInfo FindOneByName(string authorizationObjectType, string authorizationObjectId, string name);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 CustomizePageInfo 实例的详细信息</returns>
        IList<CustomizePageInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有 CustomizePageInfo 实例的详细信息</returns>
        IList<CustomizePageInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 CustomizePageInfo 实例的详细信息</returns>
        IList<CustomizePageInfo> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例</returns> 
        IList<CustomizePageInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion
        
        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">CustomizePageInfo 实例详细信息</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="name">页面名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string authorizationObjectType, string authorizationObjectId, string name);
        #endregion

        #region 函数:GetHtml(string name)
        /// <summary>获取Html文本</summary>
        /// <param name="name">页面名称</param>
        /// <returns>Html文本</returns>
        string GetHtml(string name);
        #endregion

        #region 函数:GetHtml(string name, string authorizationObjectType, string authorizationObjectId)
        /// <summary>获取Html文本</summary>
        /// <param name="name">页面名称</param>
        /// <param name="authorizationObjectType">授权对象类别</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>返回一个实例<see cref="CustomizePageInfo"/>的详细信息</returns>
        string GetHtml(string name, string authorizationObjectType, string authorizationObjectId);
        #endregion
    }
}
