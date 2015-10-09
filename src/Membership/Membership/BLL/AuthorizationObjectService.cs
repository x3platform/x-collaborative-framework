#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

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

    /// <summary>��Ȩ��������</summary>
    public class AuthorizationObjectService : IAuthorizationObjectService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IAuthorizationObjectProvider provider = null;

        #region ���캯��:AuthorizationObjectService()
        /// <summary>���캯��</summary>
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

        #region 属性:this[string authorizationObjectType, string authorizationObjectId]
        /// <summary>����</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <returns></returns>
        public IAuthorizationObject this[string authorizationObjectType, string authorizationObjectId]
        {
            get { return this.FindOne(authorizationObjectType, authorizationObjectId); }
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string authorizationObjectType, string authorizationObjectId)
        /// <summary>��ѯĳ����Ȩ������Ϣ</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <returns>����һ��<see cref="IAuthorizationObject"/>ʵ������ϸ��Ϣ</returns>
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
                    throw new Exception(string.Format("δ�ҵ����ص���Ȩ�������ͣ�{0}��", authorizationObjectType));
            }

            return authorizationObject;
        }
        #endregion

        #region 函数:Filter(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>查询授权对象信息</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns>
        public DataTable Filter(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.Filter(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 属性:IsExistName(string authorizationObjectName)
        /// <summary>�����Ƿ��������صļ�¼, ���Ʋ����ظ�</summary>
        /// <param name="authorizationObjectName">��Ȩ��������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string authorizationObjectName)
        {
            return IsExistName(string.Empty, authorizationObjectName);
        }
        #endregion

        #region 属性:IsExistName(string authorizationObjectType, string authorizationObjectName)
        /// <summary>�����Ƿ��������صļ�¼, ���Ʋ����ظ�</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectName">��Ȩ��������</param>
        /// <returns>����ֵ</returns>
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
                // ���������ظ�����Ϣ����true
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

        #region 属性:GetInstantiatedAuthorizationObjects(string corporationId, IList<MembershipAuthorizationScopeObject> authorizationScopeObjects)
        /// <summary>��ȡʵ��������Ȩ����</summary>
        /// <param name="corporationId">��˾��ʶ</param>
        /// <param name="authorizationScopeObjects">��Ȩ��Χ����</param>
        /// <returns></returns>
        public IList<IAuthorizationObject> GetInstantiatedAuthorizationObjects(string corporationId, IList<MembershipAuthorizationScopeObject> authorizationScopeObjects)
        {
            IList<IAuthorizationObject> list = new List<IAuthorizationObject>();

            IList<IRoleInfo> roles = MembershipManagement.Instance.RoleService.FindAllByCorporationId(corporationId);

            foreach (MembershipAuthorizationScopeObject authorizationScope in authorizationScopeObjects)
            {
                switch (authorizationScope.AuthorizationObjectType)
                {
                    // ͨ�ø�λ
                    case "GeneralRole":

                        MembershipUitily.GetIntersectionRoles(MembershipManagement.Instance.RoleService.FindAllByGeneralRoleId(authorizationScope.AuthorizationObjectId), roles)
                            .ToList()
                            .ForEach(item => list.Add((IAuthorizationObject)item));

                        break;

                    // ��׼��ɫ
                    case "StandardRole":

                        MembershipUitily.GetIntersectionRoles(MembershipManagement.Instance.RoleService.FindAllByStandardRoleId(authorizationScope.AuthorizationObjectId), roles)
                            .ToList()
                            .ForEach(item => list.Add((IAuthorizationObject)item));

                        break;

                    // ��׼����
                    case "StandardOrganizationUnit":

                        MembershipUitily.GetIntersectionRoles(MembershipManagement.Instance.RoleService.FindAllByStandardOrganizationUnitId(authorizationScope.AuthorizationObjectId), roles)
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

        #region 属性:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>�ж���Ȩ�����Ƿ�ӵ��ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <returns>����ֵ</returns>
        public bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        {
            return provider.HasAuthority(scopeTableName, entityId, entityClassName, authorityName, account);
        }
        #endregion

        #region 属性:HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>�ж���Ȩ�����Ƿ�ӵ��ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <returns>����ֵ</returns>
        public bool HasAuthority(string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        {
            return provider.HasAuthority(scopeTableName, entityId, entityClassName, authorityName, authorizationObjectType, authorizationObjectId);
        }
        #endregion

        #region 属性:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        /// <summary>�ж���Ȩ�����Ƿ�ӵ��ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="account">�ʺ���Ϣ</param>
        /// <returns>����ֵ</returns>
        public bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, IAccountInfo account)
        {
            return provider.HasAuthority(command, scopeTableName, entityId, entityClassName, authorityName, account);
        }
        #endregion

        #region 属性:HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        /// <summary>�ж���Ȩ�����Ƿ�ӵ��ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <returns>����ֵ</returns>
        public bool HasAuthority(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string authorizationObjectType, string authorizationObjectId)
        {
            return provider.HasAuthority(command, scopeTableName, entityId, entityClassName, authorityName, authorizationObjectType, authorizationObjectId);
        }
        #endregion

        #region 属性:AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>����ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
        public void AddAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            provider.AddAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName, scopeText);
        }
        #endregion

        #region 属性:AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>����ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
        public void AddAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            provider.AddAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName, scopeText);
        }
        #endregion

        #region 属性:RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>�Ƴ�ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        public void RemoveAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            provider.RemoveAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);
        }
        #endregion

        #region 属性:RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>�Ƴ�ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        public void RemoveAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            provider.RemoveAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName);
        }
        #endregion

        #region 属性:BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>����ʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
        public void BindAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            provider.BindAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName, scopeText);
        }
        #endregion

        #region 属性:BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        /// <summary>����ʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <param name="scopeText">Ȩ�޷�Χ���ı�</param>
        public void BindAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName, string scopeText)
        {
            provider.BindAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName, scopeText);
        }
        #endregion

        #region 属性:GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>��ѯʵ��������Ȩ����Ϣ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            return provider.GetAuthorizationScopeObjects(scopeTableName, entityId, entityClassName, authorityName);
        }
        #endregion

        #region 属性:GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>��ѯʵ��������Ȩ����Ϣ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <returns></returns>
        public IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        {
            return provider.GetAuthorizationScopeObjects(command, scopeTableName, entityId, entityClassName, authorityName);
        }
        #endregion

        #region 属性:GetAuthorizationScopeObjectText(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>��ѯʵ��������Ȩ�޷�Χ���ı�</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
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

        #region 属性:GetAuthorizationScopeObjectText(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>��ѯʵ��������Ȩ�޷�Χ���ı�</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
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

        #region 属性:GetAuthorizationScopeObjectView(string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>��ѯʵ��������Ȩ�޷�Χ����ͼ</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
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

        #region 属性:GetAuthorizationScopeObjectView(GenericSqlCommand command, string scopeTableName, string entityId, string entityClassName, string authorityName)
        /// <summary>��ѯʵ��������Ȩ�޷�Χ����ͼ</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="authorityName">Ȩ������</param>
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

        #region 属性:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds)
        /// <summary>��ȡʵ��������ʶSQL����</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="accountId">�û���ʶ</param>
        /// <param name="contactType">��ϵ�˶���</param>
        /// <param name="authorityIds">Ȩ�ޱ�ʶ</param>
        /// <returns></returns>
        public string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds)
        {
            return this.GetAuthorizationScopeEntitySQL(scopeTableName, accountId, contactType, authorityIds, "EntityId");
        }
        #endregion

        #region 属性:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName)
        /// <summary>��ȡʵ��������ʶSQL����</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="accountId">�û���ʶ</param>
        /// <param name="contactType">��ϵ�˶���</param>
        /// <param name="authorityIds">Ȩ�ޱ�ʶ</param>
        /// <param name="entityIdDataColumnName">ʵ�����ݱ�ʶ</param>
        /// <returns></returns>
        public string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName)
        {
            return this.GetAuthorizationScopeEntitySQL(scopeTableName, accountId, contactType, authorityIds, entityIdDataColumnName, string.Empty, string.Empty);
        }
        #endregion

        #region 属性:GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        /// <summary>��ȡʵ��������ʶSQL����</summary>
        /// <param name="scopeTableName">���ݱ�������</param>
        /// <param name="accountId">�û���ʶ</param>
        /// <param name="contactType">��ϵ�˶���</param>
        /// <param name="authorityIds">Ȩ�ޱ�ʶ</param>
        /// <param name="entityIdDataColumnName">ʵ������ʶ����������</param>
        /// <param name="entityClassNameDataColumnName">ʵ�������Ƶ���������</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns></returns>
        public string GetAuthorizationScopeEntitySQL(string scopeTableName, string accountId, ContactType contactType, string authorityIds, string entityIdDataColumnName, string entityClassNameDataColumnName, string entityClassName)
        {
            return this.provider.GetAuthorizationScopeEntitySQL(scopeTableName, accountId, contactType, authorityIds, entityIdDataColumnName, entityClassNameDataColumnName, entityClassName);
        }
        #endregion
    }
}