namespace X3Platform.Apps.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Spring;

    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Apps.IBLL.IApplicationPackageService")]
    public interface IApplicationPackageService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApplicationPackageInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationPackageInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationPackageInfo"/>详细信息</returns>
        ApplicationPackageInfo Save(ApplicationPackageInfo param);
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

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        IList<ApplicationPackageInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        IList<ApplicationPackageInfo> FindAll(string whereClause);
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
        /// <summary>查找最新的数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="direction">数据包的流向: In | Out</param>
        ApplicationPackageInfo GetLatestPackage(string applicationId, string direction);
        #endregion

        #region 函数:GetPackage(string applicationId, string direction, int code)
        /// <summary>查找数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="direction">数据包的流向: In | Out</param>
        /// <param name="code">数据包的编码</param>
        ApplicationPackageInfo GetPackage(string applicationId, string direction, int code);
        #endregion

        #region 函数:CreateReceivedPackage(string applicationId, string code, string path, XmlDocument doc)
        /// <summary>创建接收到的数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="code">数据包的编码</param>
        /// <param name="path">数据包的物理路径</param>
        /// <param name="doc">数据包的详细数据</param>
        int CreateReceivedPackage(string id, string applicationId, int code, string path, XmlDocument doc);
        #endregion

        #region 函数:CreateReceivedPackage(string applicationId, string code, string path, XmlDocument doc)
        /// <summary>创建接收到的数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="code">数据包的编码</param>
        /// <param name="path">数据包的物理路径</param>
        /// <param name="doc">数据包的详细数据</param>
        int CreateReceivedPackage(string applicationId, int code, string path, XmlDocument doc);
        #endregion

        #region 函数:TransformToFriendlyXml(string id)
        /// <summary>转换为友好的Xml格式</summary>
        /// <param name="id">数据包的标识</param>
        XmlDocument TransformToFriendlyXml(string id);
        #endregion

        #region 函数:TransformToFriendlyHtml(string id)
        /// <summary>转换为友好的Html格式</summary>
        /// <param name="id">数据包的标识</param>
        string TransformToFriendlyHtml(string id);
        #endregion
    }
}
