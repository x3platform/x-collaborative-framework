using System;
using System.Collections.Generic;
using System.ComponentModel;

using X3Platform.IBatis.DataMapper;
using X3Platform.Util;

using X3Platform.Apps.Configuration;
using X3Platform.Apps.IDAL;
using X3Platform.Apps.Model;
using X3Platform.DigitalNumber;
using X3Platform.Membership;
using X3Platform.Membership.Scope;

namespace X3Platform.Apps.DAL.IBatis
{
    /// <summary></summary>
    [DataObject]
    public class ApplicationSettingProvider : IApplicationSettingProvider
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Application_Setting";

        #region 构造函数:ApplicationSettingProvider()
        /// <summary>构造函数</summary>
        public ApplicationSettingProvider()
        {
            configuration = AppsConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // 添加 删除 修改
        // -------------------------------------------------------

        #region 函数:Save(ApplicationSettingInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationSettingInfo"/>详细信息</returns>
        public ApplicationSettingInfo Save(ApplicationSettingInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (ApplicationSettingInfo)param;
        }
        #endregion

        #region 函数:Insert(ApplicationSettingInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingInfo"/>详细信息</param>
        public void Insert(ApplicationSettingInfo param)
        {
            param.Code = DigitalNumberContext.Generate("Table_Application_Setting_Key_Code");

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(ApplicationSettingInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="ApplicationSettingInfo"/>详细信息</param>
        public void Update(ApplicationSettingInfo param)
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
        /// <returns>返回实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        public ApplicationSettingInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ApplicationSettingInfo param = ibatisMapper.QueryForObject<ApplicationSettingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        public IList<ApplicationSettingInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ApplicationSettingInfo> list = ibatisMapper.QueryForList<ApplicationSettingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllByApplicationSettingGroupId(string applicationSettingGroupId, string keyword, string scope)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupId">参数分组标识</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        public IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupId(string applicationSettingGroupId, string keyword, string scope)
        {
            string whereClause = string.Format(@" ApplicationSettingGroupId = ##{0}## AND Text LIKE ##%{1}%## ORDER BY Text ", StringHelper.ToSafeSQL(applicationSettingGroupId), StringHelper.ToSafeSQL(keyword));

            if (string.IsNullOrEmpty(keyword))
            {
                whereClause = string.Format(@" ApplicationSettingGroupId = ##{0}## ORDER BY OrderId, Text ", StringHelper.ToSafeSQL(applicationSettingGroupId));
            }

            if (whereClause.IndexOf(scope) == -1)
            {
                whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
            }

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllByApplicationSettingGroupName(string applicationSettingGroupName, string keyword)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupName">参数分组名称</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingInfo"/>的详细信息</returns>
        public IList<ApplicationSettingInfo> FindAllByApplicationSettingGroupName(string applicationSettingGroupName, string keyword, string scope)
        {
            string whereClause = string.Format(@" ApplicationSettingGroupId = ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##{0}## ) AND Text LIKE ##%{1}%## ORDER BY Text ", StringHelper.ToSafeSQL(applicationSettingGroupName), StringHelper.ToSafeSQL(keyword));

            if (string.IsNullOrEmpty(keyword))
            {
                whereClause = string.Format(@" ApplicationSettingGroupId = ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##{0}## ) ORDER BY OrderId, Text ", StringHelper.ToSafeSQL(applicationSettingGroupName));
            }

            if (whereClause.IndexOf(scope) == -1)
            {
                whereClause = string.IsNullOrEmpty(whereClause) ? scope : scope + " AND " + whereClause;
            }

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllQueryObject(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationSettingQueryInfo"/>的详细信息</returns>
        public IList<ApplicationSettingQueryInfo> FindAllQueryObject(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<ApplicationSettingQueryInfo> list = this.ibatisMapper.QueryForList<ApplicationSettingQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAllQueryObject", tableName)), args);

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationSettingInfo"/></returns>
        public IList<ApplicationSettingInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ApplicationSettingInfo> list = ibatisMapper.QueryForList<ApplicationSettingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationSettingQueryInfo"/></returns>
        public IList<ApplicationSettingQueryInfo> GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<ApplicationSettingQueryInfo> list = this.ibatisMapper.QueryForList<ApplicationSettingQueryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetQueryObjectPaging", tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

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

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:GetText(string applicationId, string settingGroupName, string value)
        /// <summary>根据配置的值获取文本信息</summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationSettingGroupName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetText(string applicationId, string applicationSettingGroupName, string value)
        {
            string whereClause = string.Format(@"
ApplicationId = ##{0}## 
AND ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##{1}## ) 
AND Value = ##{2}## 
", StringHelper.ToSafeSQL(applicationId), StringHelper.ToSafeSQL(applicationSettingGroupName), StringHelper.ToSafeSQL(value));

            // 设置根级目录下的参数
            if (string.IsNullOrEmpty(applicationSettingGroupName))
            {
                whereClause = string.Format(@" ApplicationId = ##{0}## 
                    AND ApplicationSettingGroupId = ##00000000-0000-0000-0000-000000000000## AND Value = ##{1}## ",
                    StringHelper.ToSafeSQL(applicationId), StringHelper.ToSafeSQL(value));
            }

            IList<ApplicationSettingInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? string.Empty : list[0].Text;
        }
        #endregion

        #region 函数:GetValue(string applicationId, string applicationSettingGroupName, string text)
        /// <summary>根据配置的文本获取值信息</summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationSettingGroupName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetValue(string applicationId, string applicationSettingGroupName, string text)
        {
            string whereClause = string.Format(@"
ApplicationId = ##{0}## 
AND ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##{1}## ) 
AND Text = ##{2}## 
", StringHelper.ToSafeSQL(applicationId), StringHelper.ToSafeSQL(applicationSettingGroupName), StringHelper.ToSafeSQL(text));

            // 设置根级目录下的参数
            if (string.IsNullOrEmpty(applicationSettingGroupName))
            {
                whereClause = string.Format(@" ApplicationId = ##{0}## 
                    AND ApplicationSettingGroupId = ##00000000-0000-0000-0000-000000000000## AND Text = ##{1}## ",
                    StringHelper.ToSafeSQL(applicationId), StringHelper.ToSafeSQL(text));
            }

            IList<ApplicationSettingInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? string.Empty : list[0].Value;
        }
        #endregion

        // -------------------------------------------------------
        // 授权范围管理
        // -------------------------------------------------------

        #region 函数:HasAuthority(string entityId, string authorityName, IAccountInfo account)
        /// <summary>判断用户是否拥数据权限信息</summary>
        /// <param name="entityId">实体标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string entityId, string authorityName, IAccountInfo account)
        {
            return MembershipManagement.Instance.AuthorizationObjectService.HasAuthority(
                string.Format("{0}_Scope", this.tableName),
                entityId,
                KernelContext.ParseObjectType(typeof(ApplicationSettingInfo)),
                authorityName,
                account);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        /// <summary>配置应用的权限信息</summary>
        /// <param name="entityId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string entityId, string authorityName, string scopeText)
        {
            MembershipManagement.Instance.AuthorizationObjectService.BindAuthorizationScopeObjects(
                string.Format("{0}_Scope", this.tableName),
                entityId,
                KernelContext.ParseObjectType(typeof(ApplicationSettingInfo)),
                authorityName,
                scopeText);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>查询应用的权限信息</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName)
        {
            return MembershipManagement.Instance.AuthorizationObjectService.GetAuthorizationScopeObjects(
                string.Format("{0}_Scope", this.tableName),
                entityId,
                KernelContext.ParseObjectType(typeof(ApplicationSettingInfo)),
                authorityName);
        }
        #endregion

        // -------------------------------------------------------
        // 同步管理
        // -------------------------------------------------------

        #region 函数:SyncFromPackPage(ApplicationSettingInfo param)
        ///<summary>同步信息</summary>
        ///<param name="param">应用参数信息</param>
        public void SyncFromPackPage(ApplicationSettingInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);
        }
        #endregion
    }
}
