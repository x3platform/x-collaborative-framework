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

namespace X3Platform.WorkflowPlus
{
    /// <summary>����ʱ������ʵ��</summary>
    public interface IWorkflowInstanceInfo
    {
        #region 属性:Id
        /// <summary>��ʶ</summary>
        string Id { get; }
        #endregion

        #region 属性:WorkflowEntity
        /// <summary>����������ʵ����Ϣ</summary>
        IWorkflowEntity WorkflowEntity { get; }
        #endregion
    }
}