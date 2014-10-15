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
        #region 属性:Code
        /// <summary>����</summary>
        string Code { get; set; }
        #endregion

        #region 属性:GlobalName
        /// <summary>ȫ������</summary>
        string GlobalName { get; }
        #endregion

        #region 属性:PinYin
        /// <summary>ƴ��</summary>
        string PinYin { get; set; }
        #endregion

        #region 属性:GroupTreeNodeId
        /// <summary>���������ڵ���ʶ</summary>
        string GroupTreeNodeId { get; set; }
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
