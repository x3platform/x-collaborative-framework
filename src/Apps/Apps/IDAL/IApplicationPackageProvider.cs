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
    [SpringObject("X3Platform.Apps.IDAL.IApplicationPackageProvider")]
    public interface IApplicationPackageProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationPackageInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationPackageInfo"/>详细信息</returns>
        ApplicationPackageInfo Save(ApplicationPackageInfo param);
        #endregion

        #region 函数:Insert(ApplicationPackageInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageInfo"/>详细信息</param>
        void Insert(ApplicationPackageInfo param);
        #endregion

        #region 函数:Update(ApplicationPackageInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageInfo"/>详细信息</param>
        void Update(ApplicationPackageInfo param);
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        void Delete(string ids);
        #endregion

        #region 函数:DeleteAll()
        /// <summary>删除所有输出同步数据包记录</summary>
        void DeleteAll();
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        ApplicationPackageInfo FindOne(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        IList<ApplicationPackageInfo> FindAll(string whereClause, int length);
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
        /// <returns>返回一个列表实例<see cref="ApplicationPackageInfo"/></returns>
        IList<ApplicationPackageInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:GetLatestPackage(string applicationId, string direction)
        /// <summary>创建接收到的数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="direction">数据包的编码</param>
        ApplicationPackageInfo GetLatestPackage(string applicationId, string direction);
        #endregion  
        
        #region 函数:GetPackage(string applicationId, string direction, int code)
        /// <summary>查找数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="direction">数据包的流向: In | Out</param>
        /// <param name="code">数据包的编码</param>
        ApplicationPackageInfo GetPackage(string applicationId, string direction, int code);
        #endregion
    }
}
