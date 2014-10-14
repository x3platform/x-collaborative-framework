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
        #region ����:Id
        /// <summary>��ʶ</summary>
        new string Id { get; set; }
        #endregion

        #region ����:Code
        /// <summary>����</summary>
        string Code { get; set; }
        #endregion

        #region ����:Name
        /// <summary>����</summary>
        new string Name { get; set; }
        #endregion

        #region ����:GlobalName
        /// <summary>ȫ������</summary>
        string GlobalName { get; }
        #endregion

        #region ����:PinYin
        /// <summary>ƴ��</summary>
        string PinYin { get; set; }
        #endregion

        #region ����:Parent
        /// <summary>������Ϣ</summary>
        IRoleInfo Parent { get; }
        #endregion

        #region ����:OrganizationId
        /// <summary>������֯��ʶ</summary>
        string OrganizationId { get; set; }
        #endregion

        #region ����:Organization
        /// <summary>������֯��Ϣ</summary>
        IOrganizationInfo Organization { get; }
        #endregion

        #region ����:StandardRoleId
        /// <summary>������׼��ɫ��ʶ</summary>
        string StandardRoleId { get; set; }
        #endregion

        #region ����:StandardRole
        /// <summary>������׼��ɫ��Ϣ</summary>
        IStandardRoleInfo StandardRole { get; }
        #endregion

        #region ����:EnableExchangeEmail
        /// <summary>������ҵ����</summary>
        int EnableExchangeEmail { get; set; }
        #endregion

        #region ����:FullPath
        /// <summary>������֯�ܹ�ȫ·��</summary>
        string FullPath { get; set; }
        #endregion

        #region ����:DistinguishedName
        /// <summary>Ψһ����</summary>
        string DistinguishedName { get; set; }
        #endregion

        #region ����:Members
        /// <summary>��Ա�б�</summary>
        IList<IAccountInfo> Members { get; }
        #endregion

        #region ����:ExtensionInformation
        /// <summary>��ɫ����չ��Ϣ</summary>
        IExtensionInformation ExtensionInformation { get; }
        #endregion

        // -------------------------------------------------------
        // ���ó�Ա��ϵ
        // -------------------------------------------------------

        #region ����:ResetMemberRelations(string relationText)
        /// <summary>���ó�Ա��ϵ</summary>
        /// <param name="relationText"></param>
        void ResetMemberRelations(string relationText);
        #endregion
    }
}
