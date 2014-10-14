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

using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Collections;

namespace X3Platform.WorkflowPlus
{
    /// <summary>����ʱ���������̶����ӿ�</summary>
    public interface IWorkflowObject
    {
        /// <summary>��ʶ</summary>
        string Id { get; }

        /// <summary>����</summary>
        string Name { get; }

        /// <summary>����</summary>		
        string Type { get; }

        /// <summary>������ʵ��</summary>
        IWorkflowInstanceInfo WorkflowInstance { get; }
    }
}