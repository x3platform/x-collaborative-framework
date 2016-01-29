namespace X3Platform.Membership.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Model;
    using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IAccountLogService")]
    public interface IAccountLogService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountLogInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(AccountLogInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="AccountLogInfo"/>详细信息</param>
        /// <returns>实例<see cref="AccountLogInfo"/>详细信息</returns>
        AccountLogInfo Save(AccountLogInfo param);
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
        /// <returns>返回实例<see cref="AccountLogInfo"/>的详细信息</returns>
        AccountLogInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="AccountLogInfo"/>的详细信息</returns>
        IList<AccountLogInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="AccountLogInfo"/>的详细信息</returns>
        IList<AccountLogInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="AccountLogInfo"/>的详细信息</returns>
        IList<AccountLogInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有实例<see cref="AccountLogInfo"/>的详细信息</returns>
        IList<AccountLogInfo> FindAllByAccountId(string accountId);
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
        /// <returns>返回一个列表实例<see cref="AccountLogInfo"/></returns>
        IList<AccountLogInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:Log(string accountId, string optionName, string description)
        /// <summary>保存帐号操作日志信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="optionName">操作名称: 登录</param>
        /// <param name="description">描述信息</param>
        /// <returns>0 保存成功 | 1 保存失败</returns>
        int Log(string accountId, string optionName, string description);
        #endregion

        #region 函数:Log(string accountId, string optionName, string description, string optionAccountId)
        /// <summary>保存帐号操作日志信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="optionName">操作名称: 查看</param>
        /// <param name="description">描述信息</param>
        /// <param name="optionAccountId">操作人的帐号标识</param>
        /// <returns>0 保存成功 | 1 保存失败</returns>
        int Log(string accountId, string optionName, string description, string optionAccountId);
        #endregion

        #region 函数:Log(string accountId, string optionName, IAccountInfo originalObject, string description, string optionAccountId)
        /// <summary>保存帐号日志信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="optionName">操作名称: 创建 编辑 删除 设置状态 第三方同步</param>
        /// <param name="originalObject">原始的帐号信息</param>
        /// <param name="description">描述信息</param>
        /// <param name="optionAccountId">操作人的帐号标识</param>
        /// <returns>0 保存成功 | 1 保存失败</returns>
        int Log(string accountId, string optionName, IAccountInfo originalObject, string description, string optionAccountId);
        #endregion
    }
}
