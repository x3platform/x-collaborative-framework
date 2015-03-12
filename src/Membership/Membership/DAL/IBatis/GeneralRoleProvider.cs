// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IGeneralRoleProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [DataObject]
    public class GeneralRoleProvider : IGeneralRoleProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_GeneralRole";

        #region 构造函数:GeneralRoleProvider()
        /// <summary>构造函数</summary>
        public GeneralRoleProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(GeneralRoleInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="GeneralRoleInfo"/>详细信息</param>
        /// <returns>实例<see cref="GeneralRoleInfo"/>详细信息</returns>
        public GeneralRoleInfo Save(GeneralRoleInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (GeneralRoleInfo)param;
        }
        #endregion

        #region 函数:Insert(GeneralRoleInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="GeneralRoleInfo"/>详细信息</param>
        public void Insert(GeneralRoleInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_GeneralRole_Key_Code");
            }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(GeneralRoleInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="GeneralRoleInfo"/>详细信息</param>
        public void Update(GeneralRoleInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", ids.Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="GeneralRoleInfo"/>的详细信息</returns>
        public GeneralRoleInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            GeneralRoleInfo param = this.ibatisMapper.QueryForObject<GeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="GeneralRoleInfo"/>的详细信息</returns>
        public IList<GeneralRoleInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<GeneralRoleInfo> list = this.ibatisMapper.QueryForList<GeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="groupTreeNodeId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="GeneralRoleInfo"/>的详细信息</returns>
        public IList<GeneralRoleInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId)
        {
            string whereClause = string.Format(" GroupTreeNodeId = ##{0}## ORDER BY OrderId ", groupTreeNodeId);

            return FindAll(whereClause, 0);
        }
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
        /// <returns>返回一个列表实例<see cref="GeneralRoleInfo"/></returns>
        public IList<GeneralRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

            args.Add("RowCount", 0);

            IList<GeneralRoleInfo> list = this.ibatisMapper.QueryForList<GeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

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
        /// <param name="name">通用角色名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name='{0}' ", StringHelper.ToSafeSQL(name)));

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

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Rename", tableName)), args);

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

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetGlobalName", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SyncFromPackPage(GeneralRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">通用角色信息</param>
        public int SyncFromPackPage(GeneralRoleInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion
    }
}
