namespace X3Platform.Plugins.Forum.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using X3Platform.Spring;
    using X3Platform.Plugins.Forum.Model;
  using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Forum.IDAL.IForumMemberProvider")]
    public interface IForumMemberProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ForumMemberInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ForumMemberInfo"/>详细信息</param>
        /// <returns>实例<see cref="ForumMemberInfo"/>详细信息</returns>
        ForumMemberInfo Save(ForumMemberInfo param);
        #endregion

        #region 函数:Insert(ForumMemberInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ForumMemberInfo"/>详细信息</param>
        void Insert(ForumMemberInfo param);
        #endregion

        #region 函数:Update(ForumMemberInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ForumMemberInfo"/>详细信息</param>
        void Update(ForumMemberInfo param);
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
        /// <returns>返回实例<see cref="ForumMemberInfo"/>的详细信息</returns>
        ForumMemberInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByAccountId(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ForumMemberInfo"/>的详细信息</returns>
        ForumMemberInfo FindOneByAccountId(string id);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ForumMemberInfo"/>的详细信息</returns>
        IList<ForumMemberInfo> FindAll(string whereClause, int length);
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
        IList<ForumMemberInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:SetIconPath(string id)
        /// <summary>
        /// 设置头像
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns>布尔值</returns>
        void SetIconPath(string id);
        #endregion

        #region 函数:SetPoint(string id,int score)
        /// <summary>
        /// 增加积分
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="score">积分</param>
        void SetPoint(string id, int score);
        #endregion

        #region 函数:SetThreadCount(string id)
        /// <summary>
        /// 增加帖子数
        /// </summary>
        /// <param name="id">用户编号</param>
        void SetThreadCount(string id);
        #endregion

        #region 函数:SetFollowCount(string id, int value)
        /// <summary>
        /// 增加关注数
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="value">关注数值</param>
        void SetFollowCount(string id, int value);
        #endregion

        #region 函数:UpdateMemberInfo(string applicationTag)
        /// <summary>
        /// 同步论坛member信息
        /// </summary>
        /// <param name="applicationTag">论坛模块标识</param>
        /// <returns></returns>
        bool UpdateMemberInfo(string applicationTag);
        #endregion
    }
}
