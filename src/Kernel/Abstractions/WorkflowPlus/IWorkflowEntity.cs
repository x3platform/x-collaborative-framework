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
    /// <summary>������ʵ�����ӿ�</summary>
    public interface IWorkflowEntity
    {
        /// <summary>ʵ�����ı��Ż���ʶ</summary>
        string Id { get; }

        /// <summary>ʵ������ȫ��</summary>
        string ClassName { get; }

        /// <summary>����ʵ����</summary>
        /// <param name="context">�����Ļ����ӿ�</param>
        /// <returns></returns>
        void Load(IWorkflowClientContext context);

        /// <summary>����</summary>
        void Save();

        /// <summary>ɾ��</summary>
        void Delete();
    }
}