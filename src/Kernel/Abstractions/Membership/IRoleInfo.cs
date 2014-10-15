#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IRoleInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Security.Authority;

    /// <summary>��ɫ</summary>
    public interface IRoleInfo : IAuthorizationObject
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

        #region 属性:PinYin
        /// <summary>ƴ��</summary>
        string PinYin { get; set; }
        #endregion

        #region 属性:Parent
        /// <summary>������Ϣ</summary>
        IRoleInfo Parent { get; }
        #endregion

        #region 属性:OrganizationId
        /// <summary>������֯��ʶ</summary>
        string OrganizationId { get; set; }
        #endregion

        #region 属性:Organization
        /// <summary>������֯��Ϣ</summary>
        IOrganizationInfo Organization { get; }
        #endregion

        #region 属性:StandardRoleId
        /// <summary>������׼��ɫ��ʶ</summary>
        string StandardRoleId { get; set; }
        #endregion

        #region 属性:StandardRole
        /// <summary>������׼��ɫ��Ϣ</summary>
        IStandardRoleInfo StandardRole { get; }
        #endregion

        #region 属性:EnableExchangeEmail
        /// <summary>������ҵ����</summary>
        int EnableExchangeEmail { get; set; }
        #endregion

        #region 属性:FullPath
        /// <summary>������֯�ܹ�ȫ·��</summary>
        string FullPath { get; set; }
        #endregion

        #region 属性:DistinguishedName
        /// <summary>Ψһ����</summary>
        string DistinguishedName { get; set; }
        #endregion

        #region 属性:Members
        /// <summary>��Ա�б�</summary>
        IList<IAccountInfo> Members { get; }
        #endregion

        #region 属性:ExtensionInformation
        /// <summary>��ɫ����չ��Ϣ</summary>
        IExtensionInformation ExtensionInformation { get; }
        #endregion

        // -------------------------------------------------------
        // ���ó�Ա��ϵ
        // -------------------------------------------------------

        #region 属性:ResetMemberRelations(string relationText)
        /// <summary>���ó�Ա��ϵ</summary>
        /// <param name="relationText"></param>
        void ResetMemberRelations(string relationText);
        #endregion
    }
}
