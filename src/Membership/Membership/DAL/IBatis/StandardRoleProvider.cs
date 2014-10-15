// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IStandardRoleProvider.cs
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

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.DigitalNumber;

    /// <summary></summary>
    [DataObject]
    public class StandardRoleProvider : IStandardRoleProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_StandardRole";

        #region 构造函数:StandardRoleProvider()
        /// <summary>构造函数</summary>
        public StandardRoleProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(IStandardRoleInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IStandardRoleInfo"/>详细信息</param>
        /// <returns>实例<see cref="IStandardRoleInfo"/>详细信息</returns>
        public IStandardRoleInfo Save(IStandardRoleInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (IStandardRoleInfo)param;
        }
        #endregion

        #region 函数:Insert(IStandardRoleInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="IStandardRoleInfo"/>详细信息</param>
        public void Insert(IStandardRoleInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_StandardRole_Key_Code");
            }

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(IStandardRoleInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="IStandardRoleInfo"/>详细信息</param>
        public void Update(IStandardRoleInfo param)
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
        /// <returns>返回实例<see cref="IStandardRoleInfo"/>的详细信息</returns>
        public IStandardRoleInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            IStandardRoleInfo param = ibatisMapper.QueryForObject<IStandardRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStandardRoleInfo"/>的详细信息</returns>
        public IList<IStandardRoleInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<IStandardRoleInfo> list = ibatisMapper.QueryForList<IStandardRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
        public IList<IStandardRoleInfo> FindAllByParentId(string parentId)
        {
            string whereClause = string.Format(" ParentId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(parentId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByStandardOrganizationId(string standardOrganizationId)
        /// <summary>递归查询某个标准组织下面所有的标准角色</summary>
        /// <param name="standardOrganizationId">组织标识</param>
        /// <returns>返回所有<see cref="IRoleInfo"/>实例的详细信息</returns>
        public IList<IStandardRoleInfo> FindAllByStandardOrganizationId(string standardOrganizationId)
        {
            string whereClause = string.Format(" StandardOrganizationId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(standardOrganizationId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByType(int standardRoleType)
        /// <summary>根据标准角色类型查询所有相关记录</summary>
        /// <param name="standardRoleType">标准角色类型</param>
        /// <returns>返回所有实例<see cref="IStandardRoleInfo"/>的详细信息</returns>
        public IList<IStandardRoleInfo> FindAllByType(int standardRoleType)
        {
            string whereClause = string.Format(" Type = {0} ORDER BY OrderId ", standardRoleType);

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="groupTreeNodeId">分类节点标识</param>
        /// <returns>返回所有实例<see cref="GeneralRoleInfo"/>的详细信息</returns>
        public IList<IStandardRoleInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId)
        {
            string whereClause = string.Format(" GroupTreeNodeId = ##{0}## ORDER BY OrderId ", groupTreeNodeId);

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
        /// <returns>返回一个列表实例<see cref="IStandardRoleInfo"/></returns>
        public IList<IStandardRoleInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<IStandardRoleInfo> list = ibatisMapper.QueryForList<IStandardRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

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

            args.Add("WhereClause", string.Format(" Id='{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">标准角色名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("实例标识不能为空。");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标准角色标识</param>
        /// <param name="name">标准角色名称</param>
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

        #region 函数:SyncFromPackPage(IStandardRoleInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">标准角色信息</param>
        public int SyncFromPackPage(IStandardRoleInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);
            
            return 0;
        }
        #endregion

        #region 函数:GetKeyStandardRoles()
        /// <summary>获取所有关键标准角色</summary>
        /// <returns>返回一个列表实例<see cref="IStandardRoleInfo"/></returns>
        public IList<IStandardRoleInfo> GetKeyStandardRoles()
        {
            string whereClause = " IsKey = 1 ";

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:GetKeyStandardRoles(int standardRoleType)
        /// <summary>获取所有关键标准角色</summary>
        /// <param name="standardRoleType">标准角色类型</param>
        /// <returns>返回一个列表实例<see cref="IStandardRoleInfo"/></returns>
        public IList<IStandardRoleInfo> GetKeyStandardRoles(int standardRoleType)
        {
            string whereClause = string.Format(" IsKey = 1 AND Type = {0} ORDER BY OrderId ", standardRoleType);

            return FindAll(whereClause, 0);
        }
        #endregion
    }
}
