// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :IWorkflowActorInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace X3Platform.WorkflowPlus
{
    /// <summary>������ִ����</summary>
    public interface IWorkflowActorInfo
    {
        /// <summary>��ʶ</summary>
        string Id { get; set; }

        /// <summary>����</summary>
        string Name { get; set; }

        /// <summary>��¼��</summary>
        string LoginName { get; set; }
    }
}