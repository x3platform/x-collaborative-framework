#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
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

    /// <summary>ʵ�����ĵ���Ϣ�ӿ�</summary>
    public interface IEntityDocObjectInfo
    {
        /// <summary>�ĵ�Ψһ��ʶ</summary>
        string Id { get; }

        /// <summary>�ĵ�����</summary>
        string DocTitle { get; }

        /// <summary>�ĵ�ȫ�ֱ�ʶ(��ͬ�İ汾֮�䣬����һ�� DocToken��)</summary>
        string DocToken { get; }

        /// <summary>�ĵ��汾</summary>
        string DocVersion { get; }

        /// <summary>�ĵ�״̬</summary>
        string DocStatus { get; }

        /// <summary>��������</summary>
        DateTime UpdateDate { get; }

        /// <summary>��������</summary>
        DateTime CreateDate { get; }
    }
}
