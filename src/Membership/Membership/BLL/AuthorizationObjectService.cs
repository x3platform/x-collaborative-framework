namespace X3Platform.Membership.BLL
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Membership.Model;
    using X3Platform.Data;
    #endregion

    /// <summary>授权对象服务</summary>
    public class AuthorizationObjectService : IAuthorizationObjectService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IAuthorizationObjectProvider provider = null;

        #region 构造函数:AuthorizationObjectService()
        /// <summary>构造函数</summary>
        public AuthorizationObjectService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IAuthorizationObjectProvider>(typeof(IAuthorizationObjectProvider));
        }
        #endregion

        #region 索引:this[string authorizationObjectType, string authorizationObjectId]
        /// <summary>索引</summary>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns></returns>
        public IAuthorizationObject this[string authorizationObjectType, string authorizationObjectId]
        {
            get { return this.FindOne(authorizationObjectType, authorizationObjectId); }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string authorizationObjectType, string authorizationObjectId)
        /// <summary>查询某条授权对象信息</summary>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>返回一个<see cref="IAuthorizationObject"/>实例的详细信息</returns>
        public IAuthorizationObject FindOne(string authorizationObjectType, string authorizationObjectId)
        {
            IAuthorizationObject authorizationObject = null;

            switch (authorizationObjectType.ToLower())
            {
                case "account":
                    authorizationObject = MembershipManagement.Instance.AccountService[authorizationObjectId];
                    break;
                case "role":
                    authorizationObject = MembershipManagement.Instance.RoleService[authorizationObjectId];
                    break;
                case "organization":
                    authorizationObject = MembershipManagement.Instance.OrganizationUnitService[authorizationObjectId];
                    break;
                case "group":
                    authorizationObject = MembershipManagement.Instance.GroupService[authorizationObjectId];
                    break;
                case "generalrole":
                    authorizationObject = MembershipManagement.Instance.GeneralRoleService[authorizationObjectId];
                    break;
                case "standardorganization":
                    authorizationObject = MembershipManagement.Instance.StandardOrganizationUnitService[authorizationObjectId];
                    break;
                case "standardrole":
                    authorizationObject = MembershipManagement.Instance.StandardRoleService[authorizationObjectId];
                    break;
                default:
                    throw new Exception(string.Format("未找到相关的授权对象类型：{0}。", authorizationObjectType));
            }

            return authorizationObject;
        }
        #endregion

        #region 函数:Filter(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>查询授权对象信息</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        public DataTable Filter(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.Filter(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExistName(string authorizationObjectName)
        /// <summary>检测是否存在相关的记录, 名称不能重复</summary>
        /// <param name="authorizationObjectName">授权对象名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string authorizationObjectName)
        {
            return IsExistName(string.Empty, authorizationObjectName);
        }
        #endregion

        #region 函数:IsExistName(string authorizationObjectType, string authorizationObjectName)
        /// <summary>检测是否存在相关的记录, 名称不能重复</summary>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectName">授权对象名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string authorizationObjectType, string authorizationObjectName)
        {
            bool isExist = false;

            string[] authorizationObjectTypes = new string[] {
                "account", 
                "role",
                "organization",
                "group",
                "generalrole",
                "standardorganization",
                "standardrole",
                "standardgeneralrole",
            };

            foreach (string authorizationObjectTypeValue in authorizationObjectTypes)
            {
                // 如果存在重复的信息返回true
                if (isExist)
                    return true;

                if (string.IsNullOrEmpty(authorizationObjectType) || authorizationObjectType == authorizationObjectTypeValue)
                {
                    switch (authorizationObjectTypeValue.ToLower())
                    {
                        case "account":
                            isExist = MembershipManagement.Instance.AccountService.IsExistName(authorizationObjectName);
                            break;
                        case "role":
                            isExist = MembershipManagement.Instance.RoleService.IsExistName(authorizationObjectName);
                            break;
                        case "organization":
                            isExist = MembershipManagement.Instance.OrganizationUnitService.IsExistName(authorizationObjectName);
                            break;
                        case "group":
                            isExist = MembershipManagement.Instance.GroupService.IsExistName(authorizationObjectName);
                            break;
                        case "generalrole":
                            isExist = MembershipManagement.Instance.GeneralRoleService.IsExistName(authorizationObjectName);
                            break;
                        case "standardorganization":
                            isExist = MembershipManagement.Instance.StandardOrganizationUnitService.IsExistName(authorizationObjectName);
                            break;
                        case "standardrole":
                            isExist = MembershipManagement.Instance.StandardRoleService.IsExistName(authorizationObjectName);
                            break;
                        default:
                            break;
                    }
                }
            }

            return isExist;
        }
        #endregion

        #region 函数:GetInstantiatedAuthorizationObjects(string corporationId, IList<MembershipAuthorizationScopeObject> authorizationScopeObjects)
        /// <summary>获取实例化的授权对象</summary>
        /// <param name="corporationId">公司标识</param>
        /// <param name="authorizationScopeObjects">授权范围对象</param>
        /// <returns></returns>
        public IList<IAuthorizationObject> GetInstantiatedAuthorizationObjects(string corporationId, IList<MembershipAuthorizationScopeObject> authorizationScopeObjects)
        {
            IList<IAuthorizationObject> list = new List<IAuthorizationObject>();

            IList<IRoleInfo> roles = MembershipManagement.Instance.RoleService.FindAllByCorporationId(corporationId);

            foreach (MembershipAuthorizationScopeObject authorizationScope in authorizationScopeObjects)
            {
                switch (authorizationScope.AuthorizationObjectType)
                {
                    // 通用岗位
                    case "GeneralRole":

                        MembershipUtil.GetIntersectionRoles(MembershipManagement.Instance.RoleService.FindAllByGeneralRoleId(authorizationScope.AuthorizationObjectId), roles)
                            .ToList()
                            .ForEach(item => list.Add((IAuthorizationObject)item));

                        break;

                    // 标准角色
                    case "StandardRole":

                        MembershipUtil.GetIntersectionRoles(MembershipManagement.Instance.RoleService.FindAllByStandardRoleId(authorizationScope.AuthorizationObjectId), roles)
                            .ToList()
                            .ForEach(item => list.Add((IAuthorizationObject)item));

                        break;

                    // 标准部门
                    case "StandardOrganizationUnit":

                        MembershipUtil.GetIntersectionRoles(MembershipManagement.Instance.RoleService.FindAllByStandardOrganizationUnitId(authorizationScope.AuthorizationObjectId), roles)
                            .ToList()
                            .ForEach(item => list.Add((IAuthorizationObject)item));

                        break;

                    default:
                        list.Add(authorizationScope.GetAuthorizationObject());
                        break;
                }
            }

            return list;
        }
        #endregion

        #region 函数:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号信息</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        {
            return provider.HasAuthority(scopeTableName, entityId, entityClassName, authorityName, account);
        }
        #endregion

        #region 函数:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        {
            return provider.HasAuthority(scopeTableName, entityId, entityClassName, authorityName, authorizationObjectType, authorizationObjectId);
        }
        #endregion

        #region 函数:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="account">帐号信息</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        {
            return provider.HasAuthority(command, scopeTableName, entityId, entityClassName, authorityName, account);
        }
        #endregion

        #region 函数:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>判断授权对象是否拥有实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="authorizationObjectType">授权对象类型</param>
        /// <param name="authorizationObjectId">授权对象标识</param>
        /// <returns>布尔值</returns>
        public bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        {
            return provider.HasAuthority(command, scopeTableName, entityId, entityClassName, authorityName, authorizationObjectType, authorizationObjectId);
        }
        #endregion

        #region 函数:AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>新增实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            provider.AddAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName, scopeText);
        }
        #endregion

        #region 函数:AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>新增实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            provider.AddAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName, scopeText);
        }
        #endregion

        #region 函数:RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>移除实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        public void RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            provider.RemoveAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);
        }
        #endregion

        #region 函数:RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>移除实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        public void RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            provider.RemoveAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>配置实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            provider.BindAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName, scopeText);
        }
        #endregion

        #region 函数:BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>配置实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="scopeText">权限范围的文本</param>
        public void BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            provider.BindAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName, scopeText);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限信息</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            return provider.GetAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限信息</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            return provider.GetAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName);
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjectText(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限范围的文本</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public string GetAuthorizationScopeObjectText(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            StringBuilder outString = new StringBuilder();

            IList<MembershipAuthorizationScopeObject> authorizationScopeObjects = this.GetAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);

            foreach (MembershipAuthorizationScopeObject authorizationScopeObject in authorizationScopeObjects)
            {
                if (!string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectType)
                    && !string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectId))
                {
                    outString.AppendFormat("{0}#{1}#{2};",
                        authorizationScopeObject.AuthorizationObjectType.ToLower(),
                        authorizationScopeObject.AuthorizationObjectId,
                        authorizationScopeObject.AuthorizationObjectDescription);
                }
            }

            return outString.ToString();
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjectText(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限范围的文本</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public string GetAuthorizationScopeObjectText(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            StringBuilder outString = new StringBuilder();

            IList<MembershipAuthorizationScopeObject> authorizationScopeObjects = this.GetAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName);

            foreach (MembershipAuthorizationScopeObject authorizationScopeObject in authorizationScopeObjects)
            {
                if (!string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectType)
                    && !string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectId))
                {
                    outString.AppendFormat("{0}#{1}#{2};",
                        authorizationScopeObject.AuthorizationObjectType.ToLower(),
                        authorizationScopeObject.AuthorizationObjectId,
                        authorizationScopeObject.AuthorizationObjectDescription);
                }
            }

            return outString.ToString();
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjectView(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限范围的视图</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public string GetAuthorizationScopeObjectView(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            StringBuilder outString = new StringBuilder();

            IList<MembershipAuthorizationScopeObject> authorizationScopeObjects = GetAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);

            foreach (MembershipAuthorizationScopeObject authorizationScopeObject in authorizationScopeObjects)
            {
                if (!string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectType)
                    && !string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectId))
                {
                    if (string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectDescription))
                    {
                        outString.AppendFormat("{0}(ID:{1});", authorizationScopeObject.AuthorizationObjectType, authorizationScopeObject.AuthorizationObjectId);
                    }
                    else
                    {
                        outString.AppendFormat("{0};", authorizationScopeObject.AuthorizationObjectDescription);
                    }
                }
            }

            return outString.ToString();
        }
        #endregion

        #region 函数:GetAuthorizationScopeObjectView(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>查询实体对象的权限范围的视图</summary>
        /// <param name="command">通用SQL命令对象</param>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="authorityName">权限名称</param>
        /// <returns></returns>
        public string GetAuthorizationScopeObjectView(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            StringBuilder outString = new StringBuilder();

            IList<MembershipAuthorizationScopeObject> authorizationScopeObjects = this.GetAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName);

            foreach (MembershipAuthorizationScopeObject authorizationScopeObject in authorizationScopeObjects)
            {
                if (!string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectType)
                    && !string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectId))
                {
                    if (string.IsNullOrEmpty(authorizationScopeObject.AuthorizationObjectDescription))
                    {
                        outString.AppendFormat("{0}(ID:{1});", authorizationScopeObject.AuthorizationObjectType, authorizationScopeObject.AuthorizationObjectId);
                    }
                    else
                    {
                        outString.AppendFormat("{0};", authorizationScopeObject.AuthorizationObjectDescription);
                    }
                }
            }

            return outString.ToString();
        }
        #endregion

        #region 函数:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds)
        /// <summary>获取实体对象标识SQL语句</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="accountId">用户标识</param>
        /// <param name="contactType">联系人对象</param>
        /// <param name="authorityIds">权限标识</param>
        /// <returns></returns>
        public string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds)
        {
            return this.GetAuthorizationScopeEntitySQL(scopeTableName, accountId, contactType, authorityIds, "EntityId");
        }
        #endregion

        #region 函数:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName)
        /// <summary>获取实体对象标识SQL语句</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="accountId">用户标识</param>
        /// <param name="contactType">联系人对象</param>
        /// <param name="authorityIds">权限标识</param>
        /// <param name="entityIdDataColumnName">实体数据标识</param>
        /// <returns></returns>
        public string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName)
        {
            return this.GetAuthorizationScopeEntitySQL(scopeTableName, accountId, contactType, authorityIds, entityIdDataColumnName, string.Empty, string.Empty);
        }
        #endregion

        #region 函数:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        /// <summary>获取实体对象标识SQL语句</summary>
        /// <param name="scopeTableName">数据表的名称</param>
        /// <param name="accountId">用户标识</param>
        /// <param name="contactType">联系人对象</param>
        /// <param name="authorityIds">权限标识</param>
        /// <param name="entityIdDataColumnName">实体类标识的数据列名</param>
        /// <param name="entityClassNameDataColumnName">实体类名称的数据列名</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns></returns>
        public string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        {
            return this.provider.GetAuthorizationScopeEntitySQL(scopeTableName, accountId, contactType, authorityIds, entityIdDataColumnName, entityClassNameDataColumnName, entityClassName);
        }
        #endregion
    }
}