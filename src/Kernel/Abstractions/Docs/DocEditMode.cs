#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :DocEditMode.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Docs
{
    using System;

    /// <summary>文档编辑模式</summary>
    [Flags]
    public enum DocEditMode
    {
        /// <summary>新建</summary>
        New = 1,
        /// <summary>从草稿箱提交流程</summary>
        Draft = 2,
        /// <summary>编辑</summary>
        Edit = 4,
        /// <summary>管理员编辑</summary>
        ForcedEdit = 8,
        /// <summary>驳回</summary>
        Rejected = 16,
        /// <summary>校稿</summary>
        Proofread = 32,
        /// <summary>新版本</summary>
        NewVersion = 64,
        /// <summary>只读</summary>
        ReadOnly = 32768,
        /// <summary>未知状态</summary>
        Unkown = 65536,
    }
}
