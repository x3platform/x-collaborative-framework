// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IStandardGeneralRoleProvider.cs
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
    public class StandardGeneralRoleProvider : IStandardGeneralRoleProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_StandardGeneralRole";

        #region 构造函数:StandardGeneralRoleProvider()
        /// <summary>构造函数</summary>
        public StandardGeneralRoleProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(IStandardGeneralRoleInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IStandardGeneralRoleInfo"/>详细信息</param>
        /// <returns>实例<see cref="IStandardGeneralRoleInfo"/>详细信息</returns>
        public IStandardGeneralRoleInfo Save(IStandardGeneralRoleInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (IStandardGeneralRoleInfo)param;
        }
        #endregion

        #region 函数:Insert(IStandardGeneralRoleInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IStandardGeneralRoleInfo"/>详细信息</param>
        public void Insert(IStandardGeneralRoleInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_StandardGeneralRole_Key_Code");
            }

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(IStandardGeneralRoleInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IStandardGeneralRoleInfo"/>详细信息</param>
        public void Update(IStandardGeneralRoleInfo param)
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

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        public IStandardGeneralRoleInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            IStandardGeneralRoleInfo param = ibatisMapper.QueryForObject<IStandardGeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        public IList<IStandardGeneralRoleInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<IStandardGeneralRoleInfo> list = ibatisMapper.QueryForList<IStandardGeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 属性:FindAllByCatalogItemId(string CatalogItemId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="groupTreeNodeId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="IStandardGeneralRoleInfo"/>的详细信息</returns>
        public IList<IStandardGeneralRoleInfo> FindAllByCatalogItemId(string CatalogItemId)
        {
            string whereClause = string.Format(" CatalogItemId = ##{0}## ORDER BY OrderId ", CatalogItemId);

            return this.FindAll(whereClause, 0);
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
        /// <returns>返回一个列表实例<see cref="IStandardGeneralRoleInfo"/></returns>
        public IList<IStandardGeneralRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" OrderId, ModifiedDate DESC "));

            args.Add("RowCount", 0);

            IList<IStandardGeneralRoleInfo> list = ibatisMapper.QueryForList<IStandardGeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("实例标识不能为空。");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="name">标准通用角色名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("实例名称不能为空。");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        // -------------------------------------------------------
        // 设置标准通用角色和组织映射关系
        // -------------------------------------------------------

        #region 函数:FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>查找标准通用角色与组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        public IStandardGeneralRoleMappingRelationInfo FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StandardGeneralRoleId", StringHelper.ToSafeSQL(standardGeneralRoleId));
            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

            return this.ibatisMapper.QueryForObject<IStandardGeneralRoleMappingRelationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneMappingRelation", tableName)), args);
        }
        #endregion

        #region 函数:GetMappingRelationPaging(int startIndex, int pageSize,  DataQuery query, out int rowCount)
        /// <summary>标准通用角色映射关系分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IStandardGeneralRoleMappingRelationInfo"/></returns>
        public IList<IStandardGeneralRoleMappingRelationInfo> GetMappingRelationPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" RoleName "));

            args.Add("RowCount", 0);

            IList<IStandardGeneralRoleMappingRelationInfo> list = ibatisMapper.QueryForList<IStandardGeneralRoleMappingRelationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetMappingRelationPaging", tableName)), args);

            rowCount = Convert.ToInt32(ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetMappingRelationRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId, string standardRoleId)
        /// <summary>添加标准通用角色与相关组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        /// <param name="roleId">组织标识</param>
        /// <param name="standardRoleId">组织标识</param>
        public int AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId, string standardRoleId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StandardGeneralRoleId", StringHelper.ToSafeSQL(standardGeneralRoleId));
            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));
            args.Add("RoleId", StringHelper.ToSafeSQL(roleId));
            args.Add("StandardRoleId", StringHelper.ToSafeSQL(standardRoleId));

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_AddMappingRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:RemoveMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>移除标准通用角色与相关组织的映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        public int RemoveMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StandardGeneralRoleId", StringHelper.ToSafeSQL(standardGeneralRoleId));
            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveMappingRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 函数:HasMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>检测标准通用角色与相关组织是否有映射关系</summary>
        /// <param name="standardGeneralRoleId">标准通用角色标识</param>
        /// <param name="organizationId">组织标识</param>
        public bool HasMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StandardGeneralRoleId", StringHelper.ToSafeSQL(standardGeneralRoleId));
            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_HasMappingRelation", tableName)), args) == 0) ? false : true;
        }
        #endregion
    }
}
