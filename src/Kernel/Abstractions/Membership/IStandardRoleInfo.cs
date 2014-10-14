#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IStandardRoleInfo.cs
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
    using System;
    using System.Collections.Generic;

    using X3Platform.Security.Authority;
    #endregion

    /// <summary>��׼��ɫ��Ϣ</summary>
    public interface IStandardRoleInfo : IAuthorizationObject
    {
        /// <summary>����</summary>
        string Code { get; set; }

        /// <summary>���� 0:�����ܲ� 1:�����ز���˾ 2:�����ز���Ŀ�Ŷ� 11:������ҵ��˾ 12:�����ز���Ŀ�Ŷ� 21:������ҵ��˾ 22:������ҵ��Ŀ�Ŷ� 65535:����</summary>
        int Type { get; set; }

        /// <summary>���ȼ�</summary>
        int Priority { get; set; }

        /// <summary>���ڵ���ʶ</summary>
        string ParentId { get; set; }

        /// <summary>��׼��֯��ʶ</summary>
        string StandardOrganizationId { get; set; }

        /// <summary>�Ƿ��ǹؼ���ɫ</summary>
        bool IsKey { get; set; }

        /// <summary>������ʶ</summary>
        string OrderId { get; set; }
    }
}
