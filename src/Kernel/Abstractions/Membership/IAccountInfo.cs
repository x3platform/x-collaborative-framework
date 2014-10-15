#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IAccountInfo.cs
//
// Description  :�ʺ���Ϣ
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>�ʺ�</summary>
    public interface IAccountInfo : IAuthorizationObject, ICacheable, IIdentity
    {
        #region 属性:Id
        /// <summary>��ʶ</summary>
        new string Id { get; set; }
        #endregion

        #region 属性:Code
        /// <summary>����</summary>
        string Code { get; set; }
        #endregion

        #region 属性:Name
        /// <summary>����</summary>
        new string Name { get; set; }
        #endregion

        #region 属性:GlobalName
        /// <summary>ȫ������</summary>
        string GlobalName { get; }
        #endregion

        #region 属性:DisplayName
        /// <summary>��ʾ����</summary>
        string DisplayName { get; set; }
        #endregion

        #region 属性:PinYin
        /// <summary>ƴ��</summary>
        string PinYin { get; set; }
        #endregion

        #region 属性:LoginName
        /// <summary>��¼��</summary>
        string LoginName { get; set; }
        #endregion

        #region 属性:IdentityCard
        /// <summary>����֤</summary>
        string IdentityCard { get; set; }
        #endregion

        #region 属性:Type
        /// <summary>�ʺ����� 0:��ͨ�ʺ� 1:�����ʺ� 2:Rtx�ʺ� 3:CRM�ʺ� 1000:��Ӧ���ʺ� 2000:�ͻ��ʺ�</summary>
        new int Type { get; set; }
        #endregion

        #region 属性:CertifiedTelephone
        /// <summary>����֤�ĵ绰</summary>
        string CertifiedTelephone { get; set; }
        #endregion

        #region 属性:CertifiedEmail
        /// <summary>����֤������</summary>
        string CertifiedEmail { get; set; }
        #endregion

        #region 属性:CertifiedAvatar
        /// <summary>����֤��ͷ��</summary>
        string CertifiedAvatar { get; set; }
        #endregion

        #region 属性:EnableExchangeEmail
        /// <summary>������ҵ����</summary>
        int EnableExchangeEmail { get; set; }
        #endregion

        #region 属性:Type
        /// <summary>������ʶ</summary>
        string OrderId { get; set; }
        #endregion

        #region 属性:IP
        /// <summary>IP��ַ</summary>
        string IP { get; set; }
        #endregion

        #region 属性:OrganizationRelations
        /// <summary>��֯��Ϣ</summary>
        IList<IAccountOrganizationRelationInfo> OrganizationRelations { get; }
        #endregion

        #region 属性:RoleRelations
        /// <summary>��ɫ����</summary>
        IList<IAccountRoleRelationInfo> RoleRelations { get; }
        #endregion

        #region 属性:GroupRelations
        /// <summary>Ⱥ�鼯��</summary>
        IList<IAccountGroupRelationInfo> GroupRelations { get; }
        #endregion

        #region 属性:LoginDate
        /// <summary>��¼ʱ��</summary>
        DateTime LoginDate { get; set; }
        #endregion

        #region 属性:DistinguishedName
        /// <summary>Ψһ����</summary>
        string DistinguishedName { get; set; }
        #endregion

        #region 属性:CreateDate
        /// <summary>����ʱ��</summary>
        DateTime CreateDate { get; set; }
        #endregion
    }
}