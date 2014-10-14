#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
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

namespace X3Platform.Entities
{
    using System;

    /// <summary>ʵ����������Ϣ�ӿ�</summary>
    public interface IEntityClickInfo
    {
        /// <summary>ʵ������ʶ</summary>
        string EntityId { get; }

        /// <summary>ʵ��������</summary>
        string EntityClassName { get; }

        /// <summary>�ʺű�ʶ</summary>
        string AccountId { get; }

        /// <summary>�ʺ�����</summary>
        string AccountName { get; }

        /// <summary>������</summary>
        int Click { get; }

        /// <summary>��������ʱ��</summary>
        DateTime UpdateDate { get; }
    }
}
