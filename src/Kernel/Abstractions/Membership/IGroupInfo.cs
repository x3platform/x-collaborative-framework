#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IGroupInfo.cs
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
    #region Using Libraries
    using System.Collections.Generic;
    #endregion

    /// <summary>Ⱥ����Ϣ</summary>
    public interface IGroupInfo : IAuthorizationObject
    {
        #region ����:Code
        /// <summary>����</summary>
        string Code { get; set; }
        #endregion

        #region ����:GlobalName
        /// <summary>ȫ������</summary>
        string GlobalName { get; }
        #endregion

        #region ����:PinYin
        /// <summary>ƴ��</summary>
        string PinYin { get; set; }
        #endregion

        #region ����:GroupTreeNodeId
        /// <summary>���������ڵ���ʶ</summary>
        string GroupTreeNodeId { get; set; }
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
