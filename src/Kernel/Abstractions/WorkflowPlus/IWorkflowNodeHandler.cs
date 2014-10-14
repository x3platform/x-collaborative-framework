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
    /// <summary>�������ڵ㴦����</summary>
    public interface IWorkflowNodeHandler
    {
        /// <summary>ִ��</summary>
        /// <param name="methodName">��������</param>
        /// <param name="args">����</param>
        void Execute(string methodName, IWorkflowClientContext context);
    }
}