// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IApplicationFeatureProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Apps.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Security.Authority;
    using X3Platform.Util;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;

    /// <summary></summary>
    [DataObject]
    public class ApplicationFeatureProvider : IApplicationFeatureProvider
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Application_Feature";

        #region 构造函数:ApplicationFeatureProvider()
        /// <summary>构造函数</summary>
        public ApplicationFeatureProvider()
        {
            configuration = AppsConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(ApplicationFeatureInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationFeatureInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationFeatureInfo"/>详细信息</returns>
        public ApplicationFeatureInfo Save(ApplicationFeatureInfo param)
        {
            if (!IsExist(param.Id))
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

        #region 函数:Insert(ApplicationFeatureInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationFeatureInfo"/>详细信息</param>
        public void Insert(ApplicationFeatureInfo param)
        {
            param.Code = DigitalNumberContext.Generate("Table_Application_Feature_Key_Code");

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ApplicationFeatureInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationFeatureInfo"/>详细信息</param>
        public void Update(ApplicationFeatureInfo param)
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
        /// <returns>返回实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public ApplicationFeatureInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ApplicationFeatureInfo param = ibatisMapper.QueryForObject<ApplicationFeatureInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ApplicationFeatureInfo> list = ibatisMapper.QueryForList<ApplicationFeatureInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="parentId">父级对象的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAllByParentId(string parentId)
        {
            string whereClause = string.Format(@" ParentId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(parentId));

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByApplicationId(string applicationId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="applicationId">应用系统的标识</param>
        /// <returns>返回所有实例<see cref="ApplicationFeatureInfo"/>的详细信息</returns>
        public IList<ApplicationFeatureInfo> FindAllByApplicationId(string applicationId)
        {
            string whereClause = string.Format(@" ApplicationId = ##{0}## ORDER BY OrderId ", StringHelper.ToSafeSQL(applicationId));

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
        /// <returns>返回一个列表实例<see cref="ApplicationFeatureInfo"/></returns>
        public IList<ApplicationFeatureInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ApplicationFeatureInfo> list = ibatisMapper.QueryForList<ApplicationFeatureInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="name">应用名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 函数:Authorize(string name, string accountId)
        /// <summary>检测用户是否拥有某一功能权限</summary>
        /// <param name="name">应用功能名称</param>
        /// <param name="accountId">用户帐号标识</param>
        /// <returns>布尔值</returns>
        public bool Authorize(string name, string accountId)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("实例名称不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", StringHelper.ToSafeSQL(name, true));
            args.Add("AccountId", StringHelper.ToSafeSQL(accountId, true));

            return ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_Authorize", tableName)), args) == 0) ? false : true;
        }
        #endregion

        // -------------------------------------------------------
        // 授权范围管理
        // -------------------------------------------------------

        #region 函数:GetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId)
        /// <summary>获取某一个应用系统功能的授权范围的记录.</summary>
        /// <param name="applicationId">所属应用标识</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        public DataTable GetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ApplicationId", StringHelper.ToSafeSQL(applicationId));

            args.Add("AuthorizationObjectType", StringHelper.ToSafeSQL(authorizationObjectType));
            args.Add("AuthorizationObjectId", StringHelper.ToSafeSQL(authorizationObjectId));

            return ibatisMapper.QueryForDataTable(StringHelper.ToProcedurePrefix(string.Format("{0}_Scope_GetRelations", tableName)), args);
        }
        #endregion

        #region 函数:SetApplicationFeatureScope(string authorizationObjectType, string authorizationObjectId, string applicationFeatureIds)
        /// <summary>设置某一个应用系统功能的授权范围的记录.</summary>
        /// <param name="applicationId"></param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <param name="applicationFeatureIds">应用功能标识, 多个以逗号隔开。</param>
        public void SetApplicationFeatureScope(string applicationId, string authorizationObjectType, string authorizationObjectId, string applicationFeatureIds)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object>();

                string[] keys = applicationFeatureIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                AuthorityInfo authority = AuthorityContext.Instance.AuthorityService["应用_通用_查看权限"];

                if (authority == null)
                {
                    throw new NullReferenceException("[应用_通用_查看权限]未设置权限配置。");
                }

                args.Add("ApplicationId", StringHelper.ToSafeSQL(applicationId));
                args.Add("EntityId", string.Empty);
                args.Add("EntityClassName", KernelContext.ParseObjectType(typeof(ApplicationFeatureInfo)));
                args.Add("AuthorityId", authority.Id);
                args.Add("AuthorizationObjectType", StringHelper.ToSafeSQL(authorizationObjectType));
                args.Add("AuthorizationObjectId", StringHelper.ToSafeSQL(authorizationObjectId));

                ibatisMapper.BeginTransaction();

                ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Scope_RemoveRelation", tableName)), args);

                foreach (string key in keys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        args["EntityId"] = StringHelper.ToSafeSQL(key);

                        ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Scope_AddRelation", tableName)), args);
                    }
                }

                ibatisMapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                ibatisMapper.RollBackTransaction();

                throw ex;
            }
        }
        #endregion

        #region 函数:HasAuthority(string applicationFeatureId, string authorizationObjectType, string authorizationObjectId)
        /// <summary>判断权限对象是否拥应用有此功能项权限信息</summary>
        /// <param name="entityId">应用功能的标识</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string entityId, string authorityName, string authorizationObjectType, string authorizationObjectId)
        {
            return MembershipManagement.Instance.AuthorizationObjectService.HasAuthority(
                "tb_Application_Feature_Scope",
                entityId,
                KernelContext.ParseObjectType(typeof(ApplicationFeatureInfo)),
                authorityName,
                authorizationObjectType,
                authorizationObjectId);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>配置应用的权限信息</summary>
        /// <param name="entityId">数据标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        {
            MembershipManagement.Instance.AuthorizationObjectService.BindAuthorizationScopeObjects(
                  "tb_Application_Feature_Scope",
                  entityId,
                  KernelContext.ParseObjectType(typeof(ApplicationFeatureInfo)),
                  authorityName,
                  scopeText);

            if (authorityName == "应用_通用_查看权限")
            {
                // RefreshAuthorizationReadScopeObjectText
            }
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string applicationId, string authorityName)
        /// <summary>查询应用功能的权限信息</summary>
        /// <param name="entityId">应用功能标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            return MembershipManagement.Instance.AuthorizationObjectService.GetAuthorizationScopeObjects(
                "tb_Application_Feature_Scope",
                entityId,
                KernelContext.ParseObjectType(typeof(ApplicationFeatureInfo)),
                authorityName);
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(ApplicationFeatureInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用功能信息</param>
        public void SyncFromPackPage(ApplicationFeatureInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);
        }
        #endregion
    }
}
