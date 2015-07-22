namespace X3Platform.Plugins.Forum.IDAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;
    using X3Platform.AttachmentStorage;

    using X3Platform.Plugins.Forum.Model;
    using System.Data;
  using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Forum.IDAL.IForumThreadProvider")]
    public interface IForumThreadProvider
    {
        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(ForumThreadInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ForumThreadInfo"/>详细信息</param>
        /// <returns>实例<see cref="ForumThreadInfo"/>详细信息</returns>
        ForumThreadInfo Save(ForumThreadInfo param);
        #endregion

        #region 函数:Insert(ForumThreadInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ForumThreadInfo"/>详细信息</param>
        void Insert(ForumThreadInfo param);
        #endregion

        #region 函数:Update(ForumThreadInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ForumThreadInfo"/>详细信息</param>
        void Update(ForumThreadInfo param);
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
        /// <returns>返回实例<see cref="ForumThreadInfo"/>的详细信息</returns>
        ForumThreadInfo FindOne(string id);
        #endregion

        #region 函数:FindOneByCode(string code)
        /// <summary>查询某条记录</summary>
        /// <param name="code">编号</param>
        /// <returns>返回实例<see cref="ForumThreadInfo"/>的详细信息</returns>
        ForumThreadInfo FindOneByCode(string code);
        #endregion
        
        #region 函数:FindOneByNew(string accountId)
        /// <summary>
        /// 查询用户最新发帖
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <returns>返回实例</returns>
        ForumThreadInfo FindOneByNew(string accountId);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ForumThreadInfo"/>的详细信息</returns>
        IList<ForumThreadInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllQueryObject(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有实例<see cref="ForumThreadInfo"/>的详细信息</returns>
        IList<ForumThreadQueryInfo> FindAllQueryObject(DataQuery query);
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
        /// <returns>返回一个列表实例<see cref="ForumThreadInfo"/></returns>
        IList<ForumThreadInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:GetQueryObjectPaging(int startIndex, int pageSize, string whereClause, string orderBy,out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ForumThreadInfo"/></returns>
        IList<ForumThreadQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistByCategory(string id)
        /// <summary>根据版块查询是否存在相关记录</summary>
        /// <param name="?">版块表示</param>
        /// <returns>布尔值</returns>
        bool IsExistByCategory(string id);
        #endregion

        #region 函数:GetTheadCount(string id)
        /// <summary>
        /// 根据用户查询发帖数
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>回帖数</returns>
        int GetTheadCount(string id);
        #endregion

        #region 函数:GetStorageList(string id, string className)
        /// <summary>根据实体编号实体类名查询附件信息</summary>
        /// <param name="id"></param>
        /// <param name="className"></param>
        /// <returns>附件集合</returns>
        IList<IAttachmentFileInfo> GetStorageList(string id, string className);
        #endregion

        #region 函数:SetEssential(string id, string isEssential)
        /// <summary>设置精华贴</summary>
        /// <param name="id"></param>
        /// <param name="isEssential"></param>
        int SetEssential(string id, string isEssential);
        #endregion

        #region 函数:SetTop(string id, string isTop)
        /// <summary>设置置顶</summary>
        /// <param name="id"></param>
        /// <param name="isTop"></param>
        int SetTop(string id, string isTop);
        #endregion

        #region 函数:SetUp(string id)
        /// <summary>顶一下</summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        int SetUp(string id);
        #endregion

        #region 函数:SetClick(string id)
        /// <summary>设置帖子点击数</summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        int SetClick(string id);
        #endregion

        // -------------------------------------------------------
        // 编号和存储节点管理
        // -------------------------------------------------------

        #region RebuildCode()
        /// <summary>重建编号</summary>
        int RebuildCode();
        #endregion

        #region RebuildCode()
        /// <summary>重建存储节点索引</summary>
        int RebuildStorageNodeIndex();
        #endregion
    }
}
