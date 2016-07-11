namespace X3Platform.Membership.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using X3Platform.Data;
    using X3Platform.Membership.Model;
    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IGroupService")]
    public interface IGroupService
    {
        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IGroupInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IGroupInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IGroupInfo"/>详细信息</param>
        /// <returns>实例<see cref="IGroupInfo"/>详细信息</returns>
        IGroupInfo Save(IGroupInfo param);
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IGroupInfo"/>的详细信息</returns>
        IGroupInfo FindOne(string id);
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        IList<IGroupInfo> FindAll();
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        IList<IGroupInfo> FindAll(string whereClause);
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        IList<IGroupInfo> FindAll(string whereClause, int length);
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户所在的所有群组信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IGroupInfo"/>实例的详细信息</returns>
        IList<IGroupInfo> FindAllByAccountId(string accountId);
        #endregion

        #region 函数:FindAllByCatalogItemId(string CatalogItemId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="CatalogItemId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        IList<IGroupInfo> FindAllByCatalogItemId(string CatalogItemId);
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
        /// <returns>返回一个列表实例<see cref="IGroupInfo"/></returns>
        IList<IGroupInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        bool IsExist(string id);
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">群组名称</param>
        /// <returns>布尔值</returns>
        bool IsExistName(string name);
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">群组全局名称</param>
        /// <returns>布尔值</returns>
        bool IsExistGlobalName(string globalName);
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">群组标识</param>
        /// <param name="name">群组名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        int Rename(string id, string name);
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">群组标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        int SetGlobalName(string id, string globalName);
        #endregion

        #region 函数:SetExchangeStatus(string accountId, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        int SetExchangeStatus(string accountId, int status);
        #endregion

        #region 函数:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>创建数据包</summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        string CreatePackage(DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:SyncToLDAP(IRoleInfo param)
        /// <summary>同步信息至 Active Directory</summary>
        /// <param name="param">群组信息</param>
        int SyncToLDAP(IGroupInfo param);
        #endregion

        #region 函数:SyncFromPackPage(IRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">群组信息</param>
        int SyncFromPackPage(IGroupInfo param);
        #endregion

        // -------------------------------------------------------
        // 设置帐号和群组关系
        // -------------------------------------------------------

        #region 函数:FindAllRelationByAccountId(string accountId)
        /// <summary>根据帐号查询相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>Table Columns：AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        IList<IAccountGroupRelationInfo> FindAllRelationByAccountId(string accountId);
        #endregion

        #region 函数:FindAllRelationByGroupId(string groupId)
        /// <summary>根据群组查询相关帐号的关系</summary>
        /// <param name="groupId">群组标识</param>
        /// <returns>Table Columns：AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        IList<IAccountGroupRelationInfo> FindAllRelationByGroupId(string groupId);
        #endregion

        #region 函数:AddRelation(string accountId, string groupId)
        /// <summary>添加帐号与相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        int AddRelation(string accountId, string groupId);
        #endregion

        #region 函数:AddRelation(string accountId, string groupId, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        int AddRelation(string accountId, string groupId, DateTime beginDate, DateTime endDate);
        #endregion

        #region 函数:ExtendRelation(string accountId, string groupId, DateTime endDate)
        /// <summary>续约帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        /// <param name="endDate">新的截止时间</param>
        int ExtendRelation(string accountId, string groupId, DateTime endDate);
        #endregion

        #region 函数:RemoveRelation(string accountId, string groupId)
        /// <summary>移除帐号与相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        int RemoveRelation(string accountId, string groupId);
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的群组关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveExpiredRelation(string accountId);
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号所有相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        int RemoveAllRelation(string accountId);
        #endregion

        #region 函数:ClearupRelation(string groupId)
        /// <summary>清理群组与帐号的关系</summary>
        /// <param name="groupId">群组标识</param>
        int ClearupRelation(string groupId);
        #endregion
    }
}
