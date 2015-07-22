using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using X3Platform.Spring;

using X3Platform.Plugins.Forum.Model;
using X3Platform.Data;

namespace X3Platform.Plugins.Forum.IBLL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Forum.IBLL.IForumFollowService")]
    public interface IForumFollowService
    {
        // -------------------------------------------------------
        // 添加 删除
        // -------------------------------------------------------
        #region 函数:Insert(string,accountId,string followAccountId)
        /// <summary>
        /// 添加关注用户
        /// </summary>
        /// <param name="accountId">主标识</param>
        /// <param name="followAccountId">添加标识</param>
        void Insert(string accountId, string followAccountId);
        #endregion

        #region 函数:Delete(string,accountId,string followAccountId)
        /// <summary>
        /// 删除关注用户
        /// </summary>
        /// <param name="accountId">主标识</param>
        /// <param name="followAccountId">添加标识</param>
        void Delete(string accountId, string followAccountId);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ForumMemberInfo"/></returns>
        IList<ForumFollowInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ForumFollowQueryInfo"/></returns>
        IList<ForumFollowQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string accountId,string followAccountId)
        /// <summary>
        /// 判断是否已经关注用户
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="followAccountId"></param>
        /// <returns></returns>
        bool IsExist(string accountId, string followAccountId);
        #endregion

        #region 函数:IsMutual(string FollowAccountId)
        /// <summary>
        /// 查看是否相互关注
        /// </summary>
        /// <param name="FollowAccountId">被关注人标识</param>
        /// <returns></returns>
        bool IsMutual(string FollowAccountId);
        #endregion

        #region 函数:GetFollowCount(string accountId)
        /// <summary>
        /// 根据用户标识查询用户人气
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        int GetFollowCount(string accountId);
        #endregion
    }
}
