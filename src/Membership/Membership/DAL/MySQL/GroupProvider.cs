// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IGroupProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.DAL.MySQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Util;

    [DataObject]
    public class GroupProvider : IGroupProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Group";

        #region 构造函数:GroupProvider()
        /// <summary>构造函数</summary>
        public GroupProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(IGroupInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IGroupInfo"/>详细信息</param>
        /// <returns>实例<see cref="IGroupInfo"/>详细信息</returns>
        public IGroupInfo Save(IGroupInfo param)
        {
            if (string.IsNullOrEmpty(param.Id) || !IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region 函数:Insert(IGroupInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IGroupInfo"/>详细信息</param>
        public void Insert(IGroupInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_Group_Key_Code");
            }

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(IGroupInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IGroupInfo"/>详细信息</param>
        public void Update(IGroupInfo param)
        {
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", ids.Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IGroupInfo"/>的详细信息</returns>
        public IGroupInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            IGroupInfo param = ibatisMapper.QueryForObject<IGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        public IList<IGroupInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<IGroupInfo> list = ibatisMapper.QueryForList<IGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByAccountId(string accountId)
        /// <summary>查询某个用户所在的所有群组信息</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>返回所有<see cref="IGroupInfo"/>实例的详细信息</returns>
        public IList<IGroupInfo> FindAllByAccountId(string accountId)
        {
            string whereClause = string.Format(" T.Id IN ( SELECT GroupId FROM tb_Account_Group WHERE AccountId = ##{0}## ) ORDER BY OrderId ", StringHelper.ToSafeSQL(accountId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="groupTreeNodeId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="IGroupInfo"/>的详细信息</returns>
        public IList<IGroupInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId)
        {
            string whereClause = string.Format(" GroupTreeNodeId = ##{0}##", groupTreeNodeId);

            return FindAll(whereClause, 0);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IGroupInfo"/></returns>
        public IList<IGroupInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<IGroupInfo> list = ibatisMapper.QueryForList<IGroupInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">群组名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">组织全局名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistGlobalName(string globalName)
        {
            if (string.IsNullOrEmpty(globalName)) { throw new Exception("实例全局名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" GlobalName = '{0}' ", StringHelper.ToSafeSQL(globalName)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">群组标识</param>
        /// <param name="name">群组名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("Name", StringHelper.ToSafeSQL(name));

            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Rename", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">通用角色标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        public int SetGlobalName(string id, string globalName)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("GlobalName", StringHelper.ToSafeSQL(globalName));

            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetGlobalName", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SetExchangeStatus(string accountId, int status)
        /// <summary>设置企业邮箱状态</summary>
        /// <param name="accountId">帐户标识</param>
        /// <param name="status">状态标识, 1:启用, 0:禁用</param>
        /// <returns>0 设置成功, 1 设置失败.</returns>
        public int SetExchangeStatus(string accountId, int status)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(accountId));
            args.Add("EnableExchangeEmail", status);

            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetExchangeStatus", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SyncFromPackPage(IRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">群组信息</param>
        public int SyncFromPackPage(IGroupInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // 设置帐号和群组关系
        // -------------------------------------------------------

        #region 私有函数:FindAllRelation(string whereClause)
        /// <summary>查询帐号与群组的关系</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>Table Columns：AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        private IList<IAccountGroupRelationInfo> FindAllRelation(string whereClause)
        {
            if (string.IsNullOrEmpty(whereClause))
                return null;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

            return ibatisMapper.QueryForList<IAccountGroupRelationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllRelation", tableName)), args);
        }
        #endregion

        #region 函数:FindAllRelationByAccountId(string accountId)
        /// <summary>根据帐号查询相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <returns>Table Columns：AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountGroupRelationInfo> FindAllRelationByAccountId(string accountId)
        {
            string whereClause = string.Format(" AccountId = ##{0}## ", accountId);

            return FindAllRelation(whereClause);
        }
        #endregion

        #region 函数:FindAllRelationByGroupId(string groupId)
        /// <summary>根据群组查询相关帐号的关系</summary>
        /// <param name="groupId">群组标识</param>
        /// <returns>Table Columns：AccountId, GroupId, IsDefault, BeginDate, EndDate</returns>
        public IList<IAccountGroupRelationInfo> FindAllRelationByGroupId(string groupId)
        {
            string whereClause = string.Format(" GroupId = ##{0}## ", groupId);

            return FindAllRelation(whereClause);
        }
        #endregion

        #region 函数:AddRelation(string accountId, string groupId, DateTime beginDate, DateTime endDate)
        /// <summary>添加帐号与相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        /// <param name="beginDate">启用时间</param>
        /// <param name="endDate">停用时间</param>
        public int AddRelation(string accountId, string groupId, DateTime beginDate, DateTime endDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);
            args.Add("GroupId", groupId);
            args.Add("BeginDate", beginDate);
            args.Add("EndDate", endDate);

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_AddRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:ExtendRelation(string accountId, string groupId, DateTime endDate)
        /// <summary>续约帐号与相关群组的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        /// <param name="endDate">新的截止时间</param>
        public int ExtendRelation(string accountId, string groupId, DateTime endDate)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", accountId);
            args.Add("GroupId", groupId);
            args.Add("EndDate", endDate);

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_ExtendRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 私有函数:RemoveAllRelation(string whereClause)
        /// <summary>移除帐号相关群组的关系</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        private int RemoveRelation(string whereClause)
        {
            if (string.IsNullOrEmpty(whereClause))
                return -1;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:RemoveRelation(string accountId, string groupId)
        /// <summary>移除帐号与相关角色的关系</summary>
        /// <param name="accountId">帐号标识</param>
        /// <param name="groupId">群组标识</param>
        public int RemoveRelation(string accountId, string groupId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND GroupId = ##{1}## ) ", accountId, groupId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveExpiredRelation(string accountId)
        /// <summary>移除帐号已过期的群组关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveExpiredRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## AND EndDate < ##{1}## ) ", accountId, DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:RemoveAllRelation(string accountId)
        /// <summary>移除帐号相关群组的所有关系</summary>
        /// <param name="accountId">帐号标识</param>
        public int RemoveAllRelation(string accountId)
        {
            string whereClause = string.Format(" ( AccountId = ##{0}## ) ", accountId);

            return RemoveRelation(whereClause);
        }
        #endregion

        #region 函数:ClearupRelation(string groupId)
        /// <summary>清理群组与帐号的关系</summary>
        /// <param name="groupId">群组标识</param>
        public int ClearupRelation(string groupId)
        {
            string whereClause = string.Format(" ( GroupId = ##{0}## ) ", groupId);

            return RemoveRelation(whereClause);
        }
        #endregion
    }
}
