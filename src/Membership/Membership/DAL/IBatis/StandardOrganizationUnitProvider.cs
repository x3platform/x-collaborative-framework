// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IStandardOrganizationUnitProvider.cs
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
    using X3Platform.Data;
    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Util;

    /// <summary></summary>
    [DataObject]
    public class StandardOrganizationUnitProvider : IStandardOrganizationUnitProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_StandardOrganizationUnit";

        #region 构造函数:StandardOrganizationUnitProvider()
        /// <summary>构造函数</summary>
        public StandardOrganizationUnitProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(IStandardOrganizationUnitInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IStandardOrganizationUnitInfo"/>详细信息</param>
        /// <returns>实例<see cref="IStandardOrganizationUnitInfo"/>详细信息</returns>
        public IStandardOrganizationUnitInfo Save(IStandardOrganizationUnitInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (IStandardOrganizationUnitInfo)param;
        }
        #endregion

        #region 函数:Insert(IStandardOrganizationUnitInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IStandardOrganizationUnitInfo"/>详细信息</param>
        public void Insert(IStandardOrganizationUnitInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_StandardOrganizationUnit_Key_Code");
            }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(IStandardOrganizationUnitInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IStandardOrganizationUnitInfo"/>详细信息</param>
        public void Update(IStandardOrganizationUnitInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
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

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IStandardOrganizationUnitInfo"/>的详细信息</returns>
        public IStandardOrganizationUnitInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            IStandardOrganizationUnitInfo param = this.ibatisMapper.QueryForObject<IStandardOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStandardOrganizationUnitInfo"/>的详细信息</returns>
        public IList<IStandardOrganizationUnitInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<IStandardOrganizationUnitInfo> list = this.ibatisMapper.QueryForList<IStandardOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationUnitInfo 实例的详细信息</returns>
        public IList<IStandardOrganizationUnitInfo> FindAllByParentId(string parentId)
        {
            string whereClause = string.Format(" ParentId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(parentId));

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
        /// <returns>返回一个列表实例<see cref="IStandardOrganizationUnitInfo"/></returns>
        public IList<IStandardOrganizationUnitInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" ModifiedDate DESC "));

            args.Add("RowCount", 0);

            IList<IStandardOrganizationUnitInfo> list = this.ibatisMapper.QueryForList<IStandardOrganizationUnitInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

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
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">标准组织名称</param>
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
        /// <param name="id">标准组织标识</param>
        /// <param name="name">标准组织名称</param>
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
        /// <param name="id">标准组织标识</param>
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

        #region 函数:SetParentId(string id, string parentId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标准组织标识</param>
        /// <param name="parentId">父级组织标识</param>
        /// <returns>修改成功, 返回 0, 修改失败, 返回 1.</returns>
        public int SetParentId(string id, string parentId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("ParentId", StringHelper.ToSafeSQL(parentId));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetParentId", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:SyncFromPackPage(IStandardOrganizationUnitInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">组织信息</param>
        public int SyncFromPackPage(IStandardOrganizationUnitInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion
    }
}
